namespace HGMF2018
{
	/// <summary>
	/// An interface for getting the string contents of a file that is in an app bundle.
	/// </summary>
	public interface ILocalBundleFileService
	{
		string ReadFileFromBundleAsString(string fileName);
	}
}
