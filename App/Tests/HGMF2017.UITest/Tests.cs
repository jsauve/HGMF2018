using System.Threading;
using NUnit.Framework;
using Xamarin.UITest;

namespace HGMF2018.UITest
{
	[TestFixture(Platform.Android)]
	[TestFixture(Platform.iOS)]
	public class Tests
	{
		IApp app;
		Platform platform;

		public Tests(Platform platform)
		{
			this.platform = platform;
		}

		[SetUp]
		public void BeforeEachTest()
		{
			app = AppInitializer.StartApp(platform);
		}

		[Test]
		public void AppLaunches()
		{
			Thread.Sleep(5000);
			app.Screenshot("Schedule main page loaded");
		}

		[Test]
		public void NavigateSchedule()
		{
			AppLaunches();

			app.ScrollTo("Sat, May 6");
			app.Tap(x => x.Marked("Sat, May 6"));
			Thread.Sleep(2000);
			app.Screenshot("Navigated to Sat, May 6 in Venue view");

			app.Tap(x => x.Marked("By Time"));
			Thread.Sleep(2000);
			app.Screenshot("Navigated to Sat, May 6 in Time view");

			app.SwipeRightToLeft((arg) => { return arg.Marked("Homegrown Kickball Classic"); }, .9, 2000);
			Thread.Sleep(2000);
			app.Screenshot("Swiped to Sun, May 7 in Time view");

			app.Tap(x => x.Marked("By Venue"));
			Thread.Sleep(2000);
			app.Screenshot("Navigated to Sun, May 7 in Venue view");

			app.SwipeLeftToRight((arg) => { return arg.Marked("Jacob Mahon"); }, .9, 2000);
			Thread.Sleep(2000);
			app.Screenshot("Swiped to Sat, May 6 in Venue view");
		}

		[Test]
		public void NavigateToTweets()
		{
			AppLaunches();

			app.Tap(x => x.Marked("Tweets"));
			Thread.Sleep(2000);
			app.Screenshot("Navigated to Tweets");
		}

		[Test]
		public void NavigateToMore()
		{
			AppLaunches();

			app.Tap(x => x.Marked("More"));
			Thread.Sleep(2000);
			app.Screenshot("Navigated to More");
		}

		[Test]
		public void NavigatePhotoGallery()
		{
			NavigateToTweets();

			app.Tap(x => x.Marked("Photos"));
			Thread.Sleep(2000);
			app.Screenshot("Navigated to Photo gallery");

			app.SwipeRightToLeft(.9, 2000);
			Thread.Sleep(2000);
			app.Screenshot("Swiped to 2nd image in gallery");
		}
	}
}
