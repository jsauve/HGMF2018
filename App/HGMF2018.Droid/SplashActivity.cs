using Android.App;
using Android.Content;
using Android.OS;
using Android.Support.V7.App;

namespace HGMF2018.Droid
{
	[Activity(Theme = "@style/MyTheme.Splash", MainLauncher = true, NoHistory = true)]
	public class SplashActivity : AppCompatActivity
	{
		public override void OnCreate(Bundle savedInstanceState, PersistableBundle persistentState)
		{
			base.OnCreate(savedInstanceState, persistentState);
		}

		protected override void OnResume()
		{
			base.OnResume();

            var mainIntent = new Intent(Application.Context, typeof(MainActivity));
            if (Intent?.Extras?.Size() > 0)
                mainIntent.PutExtras(Intent.Extras);
            StartActivity(mainIntent);
		}

		public override void OnBackPressed() { }
	}
}