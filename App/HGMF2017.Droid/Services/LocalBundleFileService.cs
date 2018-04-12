using Android.Content.Res;
using System.IO;
using Plugin.CurrentActivity;

namespace HGMF2018.Droid
{
	public class LocalBundleFileService : ILocalBundleFileService
	{
		public string ReadFileFromBundleAsString(string fileName)
		{
			// Read the contents of our asset
			string content;
			AssetManager assets = CrossCurrentActivity.Current.Activity.Assets;
			using (StreamReader sr = new StreamReader(assets.Open(fileName)))
			{
				content = sr.ReadToEnd();
			}
			return content;
		}
	}
}
