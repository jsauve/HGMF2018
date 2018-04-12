using System.Collections.ObjectModel;
using System.Collections.Generic;

namespace HGMF2018
{
	public class ScheduleDetailViewModel : BaseNavigationViewModel
	{
		public Day SelectedDay { set; get; }

		public ObservableCollection<Day> Days { private set; get; }

		public ScheduleDetailViewModel(IEnumerable<Day> days, Day selectedDay)
		{
			SelectedDay = selectedDay;

			Days = new ObservableCollection<Day>(days);
		}
	}
}
