using System.Threading.Tasks;
using Android.App;
using Android.Content;
using Android.Graphics;
using Android.OS;
using Android.Support.V7.App;
using Android.Widget;

namespace HGMF2018.Droid
{
	[Activity(Theme = "@style/MyTheme.Splash", MainLauncher = true, NoHistory = true)]
	public class SplashActivity : AppCompatActivity
	{
		public override void OnCreate(Bundle savedInstanceState, PersistableBundle persistentState)
		{
			base.OnCreate(savedInstanceState, persistentState);
		}

		// Launches the startup task
		protected async override void OnResume()
		{
			base.OnResume();

            StartActivity(new Intent(Application.Context, typeof(MainActivity)));
		}

		public override void OnBackPressed() { }
	}
}