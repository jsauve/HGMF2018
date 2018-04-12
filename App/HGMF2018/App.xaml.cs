using System;
using System.Threading.Tasks;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

[assembly: XamlCompilation(XamlCompilationOptions.Compile)]

namespace HGMF2018
{
	public partial class App : Application
	{
		public static readonly string iOSAppStoreUrl = "https://itunes.apple.com/us/app/hgmf2017-unofficial/id1229131015?mt=8";
		public static readonly string AndroidAppStoreUrl = "https://play.google.com/store/apps/details?id=com.joesauve.duluthhomegrown2017";

        const string _RootAddress = "https://www.duluthhomegrown.org/";

        static string _CurrentUrl = "";

        static int _NavCount = 0;
        static bool _IsFirstNav = true;
        static bool _IsBackNav;

		public App()
		{
			InitializeComponent();

            var webView = new WebView()
            {
                Source = _RootAddress,
                VerticalOptions = LayoutOptions.FillAndExpand,
                HorizontalOptions = LayoutOptions.FillAndExpand,
            };

            var page = new CustomBackButtonPage()
            {
                Title = "#HGMF2018",
                Content = webView,
                CustomBackButtonAction = new Action(() => 
                {
                    _IsBackNav = true;
                    webView.GoBack();
                })
            };

            webView.Navigating += (sender, e) => {
                
                if (!_IsBackNav && _CurrentUrl.Equals(e.Url, StringComparison.Ordinal))
                    e.Cancel = true;
            };

            webView.Navigated += (sender, e) => 
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
                    page.EnableBackButtonOverride = true;
                else
                    page.EnableBackButtonOverride = false;
            };

            var navigationPage = new NavigationPage(page) { BarBackgroundColor = Color.Black, BarTextColor = Color.White };

            navigationPage.ToolbarItems.Add(new ToolbarItem("Lyft", "LyftToolbar", () => DependencyService.Get<ILyftService>().OpenLyft()));
            navigationPage.ToolbarItems.Add(new ToolbarItem("Uber", "UberToolbar", () => DependencyService.Get<IUberService>().OpenUber()));

            MainPage = navigationPage;
		}
	}
}
