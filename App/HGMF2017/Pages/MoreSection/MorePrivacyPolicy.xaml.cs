using System;
using System.Collections.Generic;
using System.Net.Http;
using ViewMarkdown.Forms.Plugin.Abstractions;
using Xamarin.Forms;

namespace HGMF2018
{
	public partial class MorePrivacyPolicy : ContentPage
	{
		HttpClient _HttpClient => new HttpClient();

		public MorePrivacyPolicy()
		{
			InitializeComponent();

			Content = new MarkdownView(LinkRenderingOption.Underline);

		}

		protected override async void OnAppearing()
		{
			base.OnAppearing();

			((MarkdownView)Content).Stylesheet = "body{font-family:Helvetica;}";
			((MarkdownView)Content).Markdown = await _HttpClient.GetStringAsync("https://raw.githubusercontent.com/jsauve/HGMF2017/master/PrivacyPolicy.md");
		}
	}
}
