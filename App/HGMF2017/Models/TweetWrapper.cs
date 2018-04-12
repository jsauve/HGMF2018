using System.Collections.Generic;
using System.Text;
using LinqToTwitter;
using MvvmHelpers;
using System.Linq;

namespace HGMF2018
{
	public class TweetWrapper : ObservableObject
	{
		public TweetWrapper(Status status)
		{
			status.Text = System.Net.WebUtility.HtmlDecode(status.Text);
			Status = status;
		}

		public Status Status { get; private set; }

		public string ImageUrl => Status?.Entities?.MediaEntities?.FirstOrDefault(x => x.Type == "photo")?.MediaUrl;

		public string StatusUrl => $"https://twitter.com/{Status.User.ScreenNameResponse}/status/{Status.StatusID}";

		public bool HasImage => !string.IsNullOrWhiteSpace(ImageUrl);

		public bool HasUrl => !string.IsNullOrWhiteSpace(StatusUrl);

		public string Handle => $"@{Status.User.ScreenNameResponse}";

		public string Text => Status.Text;
	}
}
