using System;
using System.Threading.Tasks;
using MvvmHelpers;
using Xamarin.Forms;
using Plugin.Connectivity;

namespace HGMF2018
{
	public class ScheduleMasterViewModel : BaseNavigationViewModel
	{
		public ScheduleMasterViewModel()
		{
			SetDataSource();
		}

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

		IDataSource<Day> _DataSource;

		public bool IsInitialized { get { return Days.Count > 0; } }

		void SetDataSource()
		{
			_DataSource = DependencyService.Get<IDataSource<Day>>();

			//_DataSource.OnError += DataSource_OnError;
		}

		void DataSource_OnError(object sender, EventArgs e)
		{
			OnError(sender, e);
		}

		ObservableRangeCollection<Day> _Days;
		public ObservableRangeCollection<Day> Days
		{
			get { return _Days ?? (_Days = new ObservableRangeCollection<Day>()); }
			set
			{
				_Days = value;
				OnPropertyChanged("Days");
			}
		}

		Command _LoadDaysCommand;
		public Command LoadDaysCommand
		{
			get { return _LoadDaysCommand ?? (_LoadDaysCommand = new Command(async () => await ExecuteLoadDaysCommand())); }
		}

		public async Task ExecuteLoadDaysCommand()
		{
			LoadDaysCommand.ChangeCanExecute();

			await FetchDays();

			LoadDaysCommand.ChangeCanExecute();
		}

		Command _RefreshDaysCommand;
		public Command RefreshDaysCommand
		{
			get { return _RefreshDaysCommand ?? (_RefreshDaysCommand = new Command(async () => await ExecuteRefreshDaysCommand())); }
		}

		async Task ExecuteRefreshDaysCommand()
		{
			RefreshDaysCommand.ChangeCanExecute();

			await FetchDays();

			RefreshDaysCommand.ChangeCanExecute();
		}

		async Task FetchDays()
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
				Days = new ObservableRangeCollection<Day>(await _DataSource.GetItems());
			}
			catch (Exception ex)
			{
				RaiseOnErrorEvent();
				ex.ReportError();
			}
			finally
			{
				IsBusy = false;
			}
		}
	}
}
