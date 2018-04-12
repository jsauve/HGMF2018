using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using HGMF2018;
using Newtonsoft.Json;
using Xamarin.Forms;

// If you want to use this local filesystem data source, uncomment the following line, and then comment the corresponding line in AzureFunctionDayDataSource.cs
//[assembly: Dependency(typeof(FilesystemOnlyDayDataSource))] 
namespace HGMF2018
{
	/// <summary>
	/// This implementation of IDataSource<Day> is intended for loading the 
	/// schedule from JSON that is stored in the app as an Asset in Android,
	/// and a Resource in iOS.
	/// 
	/// *** NOT ACTIVELY USED IN THE PRODUCTION VERSIONS OF HGMF2017 ***
	/// 
	/// </summary>
	public class FilesystemOnlyDayDataSource : IDataSource<Day>
	{
		const string _FileName = "Schedule.json";

		bool _IsInitialized;

		List<Day> _Days;

		#region IDataSource implementation

		public async Task<IEnumerable<Day>> GetItems()
		{
			var result = new List<Day>();

			await EnsureInitialized().ConfigureAwait(false);

			return await Task.FromResult(_Days).ConfigureAwait(false);
		}

		#endregion

		#region supporting methods

		async Task Initialize()
		{
			_Days = DeserializeSchedule();

			_IsInitialized = true;
		}

		async Task EnsureInitialized()
		{
			if (!_IsInitialized)
				await Initialize().ConfigureAwait(false);
		}

		List<Day> DeserializeSchedule()
		{
			var json = DependencyService.Get<ILocalBundleFileService>().ReadFileFromBundleAsString(_FileName);

			var days = JsonConvert.DeserializeObject<List<Day>>(json);

			return days;
		}

		#endregion
	}
}
