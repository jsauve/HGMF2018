using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using Semver;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using strings = HGMF2018.Core.Constants;

[assembly: XamlCompilation(XamlCompilationOptions.Compile)]

namespace HGMF2018.Core
{
    public partial class App : Application
    {
        const string _RootAddress = "https://www.duluthhomegrown.org/";

        const string _iOSAppStoreUrl = "https://itunes.apple.com/us/app/duluth-homegrown-2018/id1371299649";
        const string _AndroidAppStoreUrl = "https://play.google.com/store/apps/details?id=org.duluthhomegrown.hgmf2018";

        const string _HGMF2018AppVersionApiBase = "https://hgmf2018.azurewebsites.net/api/";
        const string _HGMF2018AppVersionApiIosPath = "CurrentiOSVersion";
        const string _HGMF2018AppVersionApiAndroidPath = "CurrentAndroidVersion";

        static string _CurrentUrl = "";

        static int _NavCount = 0;
        static bool _IsFirstNav = true;
        static bool _IsBackNav;

        HttpClient _HttpClient => new HttpClient();

        IUserDialogService _UserDialogService;
        IVersionRetrievalService _VersionRetrievalService;
        IUberService _UberService;
        ILyftService _LyftService;

        IEnumerable<string> _BannedDomains;

        WebView _WebView;
        BackButtonPage _BackButtonPage;
        NavigationPage _NavPage;

        public App()
        {
            InitializeComponent();

            _BannedDomains = new List<string>()
            {
                "youtube.com"
            };

            _UserDialogService = DependencyService.Get<IUserDialogService>();
            _VersionRetrievalService = DependencyService.Get<IVersionRetrievalService>();
            _UberService = DependencyService.Get<IUberService>();
            _LyftService = DependencyService.Get<ILyftService>();

            _WebView = new WebView()
            {
                Source = _RootAddress,
                VerticalOptions = LayoutOptions.FillAndExpand,
                HorizontalOptions = LayoutOptions.FillAndExpand,
            };

            _BackButtonPage = new BackButtonPage()
            {
                Title = "#HGMF2018",
                Content = _WebView,
                CustomBackButtonAction = new Action(() =>
                {
                    _IsBackNav = true;
                    _WebView.GoBack();
                })
            };

            _WebView.Navigating += async (object sender, WebNavigatingEventArgs e) =>
            {
                if (_BannedDomains.ToList().Contains(e.Url))
                {
                    e.Cancel = true;
                    await _UserDialogService.ShowAlert("Not allowed!", $"Sorry, but to comply with Google Play regulations, we've blocked access to {new Uri(e.Url).Host} in this app.", strings.OK);
                    return;
                }

                if (!_IsBackNav && _CurrentUrl.Equals(e.Url, StringComparison.Ordinal))
                    e.Cancel = true;
            };

            _WebView.Navigated += (sender, e) =>
            {
                _CurrentUrl = e.Url;

                if (_IsFirstNav)
                {
                    _IsFirstNav = false;
                    return;
                }

                if (_IsBackNav)
                {
                    _IsBackNav = false;
                    _NavCount--;
                }
                else
                {
                    _NavCount++;
                }

                if (_NavCount >= 1)
                    _BackButtonPage.EnableBackButtonOverride = true;
                else
                    _BackButtonPage.EnableBackButtonOverride = false;
            };

            _NavPage = new NavigationPage(_BackButtonPage) { BarBackgroundColor = Color.Black, BarTextColor = Color.White };

            MainPage = _NavPage;
        }

        protected override async void OnStart()
        {
            base.OnStart();

            SetupRideShareButtons();

            await CheckForNewVersionAsync();
        }

        protected override async void OnResume()
        {
            base.OnResume();

            SetupRideShareButtons();

            await CheckForNewVersionAsync();
        }

        async Task CheckForNewVersionAsync()
        {
            if (await NewerVersionIsAvailable())
            {
                await _UserDialogService.ShowConfirmOrCancelDialog(
                    strings.NEW_VERSION_AVAILABLE_TITLE,
                    strings.NEW_VERSION_AVAILABLE_MESSAGE,
                    strings.OK,
                    strings.CANCEL,
                    () =>
                    {
                        var appStoreUrl = String.Empty;

                        switch (Device.RuntimePlatform)
                        {
                            case Device.iOS:
                                appStoreUrl = _iOSAppStoreUrl;
                                break;
                            case Device.Android:
                                appStoreUrl = _AndroidAppStoreUrl;
                                break;
                        }

                        if (String.IsNullOrWhiteSpace(appStoreUrl))
                            return;

                        Device.OpenUri(new Uri(appStoreUrl));
                    });
            }
        }

        void SetupRideShareButtons()
        {
            _NavPage.ToolbarItems.Clear();

            switch (Device.RuntimePlatform)
            {
                case Device.Android:
                    if (_LyftService.IsInstalled)
                        // don't show Lyft button on Android unless the app is already installed
                        _NavPage.ToolbarItems.Add(new ToolbarItem("Lyft", "LyftToolbar", () => _LyftService.Open()));
                    if (_UberService.IsInstalled)
                        // don't show Uber button on Android unless the app is already installed
                        _NavPage.ToolbarItems.Add(new ToolbarItem("Uber", "UberToolbar", () => _UberService.Open()));
                    break;
                case Device.iOS:
                    _NavPage.ToolbarItems.Add(new ToolbarItem("Lyft", "LyftToolbar", () => _LyftService.Open()));
                    _NavPage.ToolbarItems.Add(new ToolbarItem("Uber", "UberToolbar", () => _UberService.Open()));
                    break;
            }
        }

        async Task<bool> NewerVersionIsAvailable()
        {
            bool result = false;

            try
            {
                var azureFunctionAppVersionApiKey = String.Empty;
                var versionApiPath = String.Empty;

                switch (Device.RuntimePlatform)
                {
                    case Device.iOS:
                        azureFunctionAppVersionApiKey = Settings.AZURE_FUNCTION_IOSVERSION_API_KEY;
                        versionApiPath = _HGMF2018AppVersionApiIosPath;
                        break;
                    case Device.Android:
                        azureFunctionAppVersionApiKey = Settings.AZURE_FUNCTION_ANDROIDVERSION_API_KEY;
                        versionApiPath = _HGMF2018AppVersionApiAndroidPath;
                        break;
                }

                if (String.IsNullOrWhiteSpace(azureFunctionAppVersionApiKey))
                    return false;

                var availableVersionString = await _HttpClient.GetStringAsync($"{_HGMF2018AppVersionApiBase}{versionApiPath}?code={azureFunctionAppVersionApiKey}");
                var currentVersionString = DependencyService.Get<IVersionRetrievalService>().Version;

                if (String.IsNullOrWhiteSpace(availableVersionString) || String.IsNullOrWhiteSpace(currentVersionString))
                    return false;

                SemVersion.TryParse(availableVersionString, out var availableVersion);
                SemVersion.TryParse(currentVersionString, out var currentVersion);

                return (availableVersion > currentVersion);
            }
            catch (Exception ex)
            {
                ex.ReportError();
            }

            return result;
        }
    }
}
