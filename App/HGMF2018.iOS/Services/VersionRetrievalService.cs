using System;
using HGMF2018.iOS;
using Foundation;

[assembly: Xamarin.Forms.Dependency(typeof(VersionRetrievalService))]
namespace HGMF2018.iOS
{
	public class VersionRetrievalService : IVersionRetrievalService
	{
		public string Version
		{
			get
			{
				NSObject ver = NSBundle.MainBundle.InfoDictionary["CFBundleShortVersionString"];
                return ver.ToString();
			}
		}
	}
}
