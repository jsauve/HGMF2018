using System.IO;
using Foundation;

namespace HGMF2018.iOS
{
	public class LocalBundleFileService : ILocalBundleFileService
	{
		public string ReadFileFromBundleAsString(string fileName)
		{
			//string ext = Path.GetExtension(fileName);
			//string filenameNoExt = fileName.Substring(0, fileName.Length - ext.Length);
			//var resourcePathname = NSBundle.MainBundle.PathForResource(filenameNoExt, ext.Substring(1, ext.Length - 1));
			//var fStream = new FileStream(resourcePathname, FileMode.Open, FileAccess.Read);

			var filePath = Path.Combine(NSBundle.MainBundle.ResourcePath, fileName);


			var text = File.ReadAllText(filePath);

			return text;

		}
	}
}
