using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using Newtonsoft.Json;
using Plugin.Connectivity;
using Semver;
using Xamarin.Forms;
using strings = HGMF2018.Core.Constants;

namespace HGMF2018.Core
{
    public class App : Application
    {
        string _RootUrl = String.Empty;

        const string _FestivalUpdatesUrl = "https://www.duluthhomegrown.org/festival-alerts";

        const string _iOSAppStoreUrl = "https://itunes.apple.com/us/app/duluth-homegrown-2018/id1371299649";
        const string _AndroidAppStoreUrl = "https://play.google.com/store/apps/details?id=org.duluthhomegrown.hgmf2018";

        const string _HGMF2018AppVersionApiBase = "https://hgmf2018.azurewebsites.net/api/";
        const string _HGMF2018AppVersionApiIosPath = "CurrentiOSVersion";
        const string _HGMF2018AppVersionApiAndroidPath = "CurrentAndroidVersion";

        static string _CurrentUrl = "";

        static int _NavCount;
        static bool _IsFirstNav = true;
        static bool _IsBackNav;

        HttpClient _HttpClient => new HttpClient();

        IUserDialogService _UserDialogService;
        IVersionRetrievalService _VersionRetrievalService;
        IUberService _UberService;
        ILyftService _LyftService;
        INotificationNavigationService _NotificationNavigationService;

        IEnumerable<string> _BannedDomains = new List<string>();

        WebView _WebView;
        BackButtonPage _BackButtonPage;
        NavigationPage _NavPage;
        ActivityIndicator _ActivityIndicator;
        bool _NewVersionAvailable;
        bool IsBusy;

        public App()
        {
            ResolveServices();

            SetupViewHierarchy();

            SetupEvents();
        }

        /// <summary>
        /// Resolves the services.
        /// </summary>
        void ResolveServices()
        {
            _UserDialogService = DependencyService.Get<IUserDialogService>();
            _VersionRetrievalService = DependencyService.Get<IVersionRetrievalService>();
            _UberService = DependencyService.Get<IUberService>();
            _LyftService = DependencyService.Get<ILyftService>();
            _NotificationNavigationService = DependencyService.Get<INotificationNavigationService>();
        }

        /// <summary>
        /// Setup the view hierarchy.
        /// </summary>
        void SetupViewHierarchy()
        {
            _ActivityIndicator = new ActivityIndicator()
            {
                HorizontalOptions = LayoutOptions.FillAndExpand,
                VerticalOptions = LayoutOptions.FillAndExpand,
                Color = Color.White,
                BackgroundColor = Color.FromRgba(0, 0, 0, 0.7)
            };

            var _ActivityIndicatorContainer = new ContentView();

            _WebView = new WebView()
            {
                VerticalOptions = LayoutOptions.FillAndExpand,
                HorizontalOptions = LayoutOptions.FillAndExpand,
                BackgroundColor = Color.Black,
            };

            var versionLabel = new Label()
            {
                Text = $"v{_VersionRetrievalService.Version}",
                FontSize = Device.GetNamedSize(NamedSize.Micro, typeof(Label)),
                TextColor = Color.FromRgb(50, 50, 50),
                BackgroundColor = Color.Black,
                HorizontalTextAlignment = TextAlignment.End
            };

            var labelContent = new ContentView()
            {
                Content = versionLabel,
                BackgroundColor = Color.Black,
                HorizontalOptions = LayoutOptions.FillAndExpand,
                VerticalOptions = LayoutOptions.Start,
                Margin = new Thickness(17, 0)
            };

            var stackLayout = new StackLayout() { Spacing = 0, BackgroundColor = Color.Black };
            stackLayout.Children.Add(labelContent);
            stackLayout.Children.Add(_WebView);

            var layout = new RelativeLayout();

            layout.Children.Add(
                stackLayout,
                xConstraint: Constraint.RelativeToParent(p => p.X),
                yConstraint: Constraint.RelativeToParent(p => p.Y),
                widthConstraint: Constraint.RelativeToParent(p => p.Width),
                heightConstraint: Constraint.RelativeToParent(p => p.Height));

            layout.Children.Add(
                _ActivityIndicator,
                xConstraint: Constraint.RelativeToParent(p => (p.Width - (p.Width * 0.2)) / 2),
                yConstraint: Constraint.RelativeToParent(p => (p.Height - (p.Width * 0.2)) / 2),
                widthConstraint: Constraint.RelativeToParent(p => p.Width * 0.2),
                heightConstraint: Constraint.RelativeToParent(p => p.Width * 0.2)
            );

            _BackButtonPage = new BackButtonPage()
            {
                Content = layout,
                CustomBackButtonAction = new Action(() =>
                {
                    _IsBackNav = true;
                    _WebView.GoBack();
                })
            };

            _NavPage = new NavigationPage(_BackButtonPage) { BarBackgroundColor = Color.Black, BarTextColor = Color.White };

            MainPage = _NavPage;

            SetupToolbarItemsForiOS();
        }

        /// <summary>
        /// Setup event handlers.
        /// </summary>
        void SetupEvents()
        {
            _WebView.Navigating += async (object sender, WebNavigatingEventArgs e) =>
            {
                if (!(await PromptIfNotConnected()))
                {
                    e.Cancel = true;
                    return;
                }

                if (Device.RuntimePlatform == Device.Android)
                {
                    var host = new Uri(e.Url)?.Host;

                    if (!String.IsNullOrWhiteSpace(host))
                    {
                        if (_BannedDomains == null || _BannedDomains?.Count() < 1)
                            _BannedDomains = await FetchBannedDomainsAsync();

                        foreach (var banned in _BannedDomains)
                        {
                            if (host.Contains(banned))
                            {
                                e.Cancel = true;
                                await _UserDialogService.ShowAlert($"{banned} not allowed", $"Sorry, but to comply with Google Play regulations, we've blocked access to {banned} on this page.", strings.OK);
                                return;
                            }
                        }
                    }
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

                if (new Uri((e.Source as UrlWebViewSource).Url).Equals(_RootUrl))
                    _NavCount = 0;

                SetBackButton();
            };

            _NotificationNavigationService.NotificationReceived += (sender, e) =>
            {
                _WebView.Source = _FestivalUpdatesUrl;

                _NavCount = 0;
            };
        }

        /// <summary>
        /// Sets the back button.
        /// </summary>
        void SetBackButton()
        {
            if (_NavCount >= 1)
                _BackButtonPage.EnableBackButtonOverride = true;
            else
                _BackButtonPage.EnableBackButtonOverride = false;
        }

        protected override async void OnStart()
        {
            base.OnStart();

            if (!(await PromptIfNotConnected()))
                return;

            // When the app starts, fetch remote values, and display activity indicator.
            await FetchValues(true);
        }

        protected override async void OnResume()
        {
            base.OnResume();

            if (!(await PromptIfNotConnected()))
                return;

            // When the app resumes, fetch remote values.
            await FetchValues();
        }

        /// <summary>
        /// Prompts if not connected.
        /// </summary>
        /// <returns>A Task of bool</returns>
        async Task<bool> PromptIfNotConnected()
        {
            if (CrossConnectivity.IsSupported && !CrossConnectivity.Current.IsConnected)
            {
                await _UserDialogService.ShowAlert("No Internet!", "Please check your internet connection and try again.", "OK");
                return false;
            }
            return true;
        }

        /// <summary>
        /// Shows the activity indicator.
        /// </summary>
        void ShowActivityIndicator()
        {
            _ActivityIndicator.IsEnabled = true;
            _ActivityIndicator.IsVisible = true;
            _ActivityIndicator.IsRunning = true;
            _WebView.IsEnabled = false;
        }

        /// <summary>
        /// Hides the activity indicator.
        /// </summary>
        void HideActivityIndicator()
        {
            _ActivityIndicator.IsEnabled = false;
            _ActivityIndicator.IsVisible = false;
            _ActivityIndicator.IsRunning = false;
            _WebView.IsEnabled = true;
        }

        /// <summary>
        /// Fetches the app values that we can set remotely.
        /// </summary>
        /// <returns>A Task.</returns>
        /// <param name="showActivityIndicator">If set to <c>true</c>, show activity indicator.</param>
        async Task FetchValues(bool showActivityIndicator = false)
        {
            if (IsBusy)
                return;

            IsBusy = true;

            SetupToolbarItemsForAndroid();

            if (showActivityIndicator)
                ShowActivityIndicator();

            try
            {
                var tasks = new List<Task>()
                {
                    Task.Run(() =>
                    {
                        Device.BeginInvokeOnMainThread(async () => { _BackButtonPage.Title = await FetchNavBarTextAsync(); });
                    }),
                    Task.Run(async () =>
                    {
                        _BannedDomains = await FetchBannedDomainsAsync();
                    }),
                    Task.Run(async () =>
                    {
                        _NewVersionAvailable = await FetchNewerVersionIsAvailable();
                    }),
                    Task.Run(async () =>
                    {
                        if (_WebView.Source == null)
                        {
                            if (String.IsNullOrWhiteSpace(_RootUrl))
                                _RootUrl = await FetchRootUrlAsync();
                            Device.BeginInvokeOnMainThread(() => { _WebView.Source = _RootUrl; });
                        }
                    })
                };

                await Task.WhenAll(tasks);
            }
            catch (Exception ex)
            {
                ex.ReportError();
            }
            finally
            {
                IsBusy = false;
                HideActivityIndicator();
            }

            if (_NewVersionAvailable)
            {
                _NewVersionAvailable = false;

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

        /// <summary>
        /// Setups the toolbar items for iOS.
        /// </summary>
        void SetupToolbarItemsForiOS()
        {
            if (Device.RuntimePlatform != Device.iOS)
                return;

            _NavPage.ToolbarItems.Clear();
            _NavPage.ToolbarItems.Add(new ToolbarItem("Lyft", "LyftToolbar", () => _LyftService.Open()));
            _NavPage.ToolbarItems.Add(new ToolbarItem("Uber", "UberToolbar", () => _UberService.Open()));
        }

        /// <summary>
        /// Setups the toolbar items for android.
        /// </summary>
        void SetupToolbarItemsForAndroid()
        {
            if (Device.RuntimePlatform != Device.Android)
                return;

            _NavPage.ToolbarItems.Clear();

            if (_LyftService.IsInstalled)
                // don't show Lyft button on Android unless the app is already installed
                _NavPage.ToolbarItems.Add(new ToolbarItem("Lyft", "LyftToolbar", () => _LyftService.Open()));
            if (_UberService.IsInstalled)
                // don't show Uber button on Android unless the app is already installed
                _NavPage.ToolbarItems.Add(new ToolbarItem("Uber", "UberToolbar", () => _UberService.Open()));
        }

        /// <summary>
        /// Fetches a bool indicating whether or not there is a newer version of the app available.
        /// </summary>
        /// <returns>The bool indicating whether or not there is a newer version of the app available.</returns>
        async Task<bool> FetchNewerVersionIsAvailable()
        {
            var availableVersionString = await FetchAvailableVersionStringAsync();

            var currentVersionString = DependencyService.Get<IVersionRetrievalService>().Version;

            if (String.IsNullOrWhiteSpace(availableVersionString) || String.IsNullOrWhiteSpace(currentVersionString))
                return false;

            SemVersion.TryParse(availableVersionString, out var availableVersion);
            SemVersion.TryParse(currentVersionString, out var currentVersion);

            return (availableVersion > currentVersion);
        }

        /// <summary>
        /// Fetches the nav bar text;
        /// </summary>
        /// <returns>The nav bar text.</returns>
        async Task<string> FetchNavBarTextAsync()
        {
            var result = "#HGMF2018";

            try
            {
                result = await _HttpClient.GetStringAsync($"https://hgmf2018.azurewebsites.net/api/NavBarText?code={Settings.AzureFunctionNavBarTextApiKey}");
            }
            catch (Exception ex)
            {
                ex.ReportError();
            }

            return result;
        }

        /// <summary>
        /// Fetches the banned domains.
        /// </summary>
        /// <returns>The banned domains.</returns>
        async Task<IEnumerable<string>> FetchBannedDomainsAsync()
        {
            var result = new List<string>();

            try
            {
                var json = await _HttpClient.GetStringAsync($"https://hgmf2018.azurewebsites.net/api/BannedDomains?code={Settings.AzureFunctionBannedDomainsApiKey}");

                result = JsonConvert.DeserializeObject<IEnumerable<string>>(json).ToList();
            }
            catch (Exception ex)
            {
                ex.ReportError();
            }

            return result;
        }

        /// <summary>
        /// Fetches the root URL.
        /// </summary>
        /// <returns>The root URL.</returns>
        async Task<string> FetchRootUrlAsync()
        {
            var result = "https://www.duluthhomegrown.org/";

            try
            {
                result = await _HttpClient.GetStringAsync($"https://hgmf2018.azurewebsites.net/api/RootUrl?code={Settings.AzureFunctionRootUrlApiKey}");
            }
            catch (Exception ex)
            {
                ex.ReportError();
            }

            return result;
        }

        /// <summary>
        /// Fetches the available version string.
        /// </summary>
        /// <returns>The available version string.</returns>
        async Task<string> FetchAvailableVersionStringAsync()
        {
            var result = "0.0.0";

            try
            {
                var azureFunctionAppVersionApiKey = String.Empty;
                var versionApiPath = String.Empty;

                switch (Device.RuntimePlatform)
                {
                    case Device.iOS:
                        azureFunctionAppVersionApiKey = Settings.AzureFunctioniOSVersionApiKey;
                        versionApiPath = _HGMF2018AppVersionApiIosPath;
                        break;
                    case Device.Android:
                        azureFunctionAppVersionApiKey = Settings.AzureFunctionAndroidVersionApiKey;
                        versionApiPath = _HGMF2018AppVersionApiAndroidPath;
                        break;
                }

                if (String.IsNullOrWhiteSpace(azureFunctionAppVersionApiKey))
                    throw new Exception("azureFunctionAppVersionApiKey is null or whitespace");

                result = await _HttpClient.GetStringAsync($"{_HGMF2018AppVersionApiBase}{versionApiPath}?code={azureFunctionAppVersionApiKey}");
            }
            catch (Exception ex)
            {
                ex.ReportError();
            }

            return result;
        }
    }
}
