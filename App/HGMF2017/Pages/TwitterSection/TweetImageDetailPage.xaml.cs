using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using Xamarin.Forms;

namespace HGMF2018
{
	public partial class TweetImageDetailPage : ContentPage
	{
		protected TweetsViewModel ViewModel => BindingContext as TweetsViewModel;

		public TweetImageDetailPage()
		{
			InitializeComponent();
		}
	}
}
