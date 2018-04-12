using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using LinqToTwitter;
using MvvmHelpers;
using Plugin.Connectivity;
using Xamarin.Forms;

namespace HGMF2018
{
	public class TweetsViewModel : BaseNavigationViewModel
	{
		public TweetsViewModel()
		{
			CanLoadMore = true;
		}

		HttpClient _HttpClient = new HttpClient();

		ulong? _LowestTweetId = null;

		string _TwitterSearchQuery;

		public int SelectedPosition { get; set; }

		public event EventHandler NoNetworkDetected;

		protected virtual void RaiseNoNetworkDetectedEvent()
		{
			EventHandler handler = NoNetworkDetected;

			if (handler != null)
				handler(this, new EventArgs());
		}

		public event EventHandler OnError;

		protected virtual void RaiseOnErrorEvent()
		{
			EventHandler handler = OnError;

			if (handler != null)
				handler(this, new EventArgs());
		}

		public bool IsInitialized { get { return Tweets.Count > 0; } }

		ObservableRangeCollection<TweetWrapper> _Tweets;
		public ObservableRangeCollection<TweetWrapper> Tweets
		{
			get { return _Tweets ?? (_Tweets = new ObservableRangeCollection<TweetWrapper>()); }
			set
			{
				_Tweets = value;
				OnPropertyChanged("Tweets");
			}
		}

		public IEnumerable<TweetWrapper> TweetsWithImages => Tweets.Where(x => x?.Status?.Entities?.MediaEntities?.Count(y => y.Type == "photo") > 0);

		Command _LoadTweetsCommand;
		public Command LoadTweetsCommand
		{
			get { return _LoadTweetsCommand ?? (_LoadTweetsCommand = new Command(async () => await ExecuteLoadTweetsCommand())); }
		}

		public async Task ExecuteLoadTweetsCommand()
		{
			LoadTweetsCommand.ChangeCanExecute();

			await FetchTweets();

			LoadTweetsCommand.ChangeCanExecute();
		}

		Command _RefreshTweetsCommand;
		public Command RefreshTweetsCommand
		{
			get { return _RefreshTweetsCommand ?? (_RefreshTweetsCommand = new Command(async () => await ExecuteRefreshTweetsCommand())); }
		}

		async Task ExecuteRefreshTweetsCommand()
		{
			RefreshTweetsCommand.ChangeCanExecute();

			Tweets.Clear();

			_LowestTweetId = null;

			CanLoadMore = true;

			await FetchTweets();

			RefreshTweetsCommand.ChangeCanExecute();
		}

		Command<string> _ImageTapCommand;
		public Command<string> ImageTapCommand
		{
			get { return _ImageTapCommand ?? (_ImageTapCommand = new Command<string>(async imageUrl => await ImageTapCommandCommand(imageUrl))); }
		}

		public async Task ImageTapCommandCommand(string imageUrl)
		{
			var tweetCount = Tweets.Count;

			ImageTapCommand.ChangeCanExecute();

			SelectedPosition = TweetsWithImages.Select(x => x.ImageUrl).ToList().IndexOf(imageUrl);

			var tweetDetailPage = new TweetImageDetailPage() { BindingContext = this };
			var tweetDetailNavPage = new NavigationPage(tweetDetailPage) { BarBackgroundColor = Color.Black };
			var backToolBarItem = new ToolbarItem("Back", null, async () => { await PopModalAsync(); });
			tweetDetailNavPage.ToolbarItems.Add(backToolBarItem);
			await PushModalAsync(tweetDetailNavPage);

			ImageTapCommand.ChangeCanExecute();
		}

		async Task FetchTweets()
		{
			if (!CrossConnectivity.Current.IsConnected)
			{
				RaiseNoNetworkDetectedEvent();
				return;
			}

			if (IsBusy)
				return;

			IsBusy = true;

			try
			{
				//only grab the twitter search query once per instantiation of the view model, otherwise the web service will get hit too often
				if (string.IsNullOrWhiteSpace(_TwitterSearchQuery))
				{
					// the query string coming from the web service looks similar to this: "#hgmf17 OR @dhgmf OR from:dhgmf -filter:retweets"
					_TwitterSearchQuery = await _HttpClient.GetStringAsync($"https://duluthhomegrown2017.azurewebsites.net/api/TwitterSearchQueryProvider?code={Settings.AZURE_FUNCTION_TWITTERSEARCHQUERY_API_KEY}");
				}

				var statuses = await SearchTweets(_TwitterSearchQuery);

				if (statuses.Count > 0)
				{
					var tweetWrappers = new List<TweetWrapper>();

					foreach (var s in statuses)
					{
						tweetWrappers.Add(new TweetWrapper(s));
					}

					Tweets.AddRange(tweetWrappers);

					_LowestTweetId = Tweets.Min(x => x.Status.StatusID);
				}
			}
			catch (Exception ex)
			{
				ex.ReportError();
				RaiseOnErrorEvent();
			}
			finally
			{
				IsBusy = false;
			}
		}

		async Task<List<Status>> SearchTweets(string query)
		{
			var result = new List<Status>();

			var auth = new ApplicationOnlyAuthorizer
			{
				CredentialStore = new SingleUserInMemoryCredentialStore()
				{
					ConsumerKey = Settings.TWITTER_API_CONSUMER_KEY,
					ConsumerSecret = Settings.TWITTER_API_CONSUMER_SECRET
				}
			};

			await auth.AuthorizeAsync();

			var twitterContext = new TwitterContext(auth);

			var tweets = await GetTweets(twitterContext, query);

			return tweets;
		}

		async Task<List<Status>> GetTweets(TwitterContext twitterContext, string query)
		{
			int count = 100;

			if (!_LowestTweetId.HasValue)
			{
				var firstresults = (await
					(from search in twitterContext.Search
					 where
					 search.Type == SearchType.Search &&
					 search.Count == count &&
					 search.ResultType == ResultType.Mixed &&
					 search.IncludeEntities == true &&
					 search.Query == query
					 select search)
					 .SingleOrDefaultAsync())?.Statuses;

				if (firstresults.Count < count)
					CanLoadMore = false;

				return firstresults;
			}

			var subsequentResults = (await
				(from search in twitterContext.Search
				 where
				 search.Type == SearchType.Search &&
				 search.Count == count &&
				 search.ResultType == ResultType.Mixed &&
				 search.IncludeEntities == true &&
				 search.Query == query &&
				 (long)search.MaxID == (long)_LowestTweetId.Value - 1 // must cast these `ulong` values to `long`, otherwise Xamarin.iOS' equality comparer freaks out and throws an invalid cast exception
				 select search)
				 .SingleOrDefaultAsync())?.Statuses;

			if (subsequentResults.Count < count)
				CanLoadMore = false;

			return subsequentResults;
		}
	}
}
