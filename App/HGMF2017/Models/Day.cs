using System;
using System.Linq;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using Newtonsoft.Json;
using MvvmHelpers;

namespace HGMF2018
{

	public class Day : ObservableObject
	{
		string _Date;
		public string Date
		{
			get { return _Date; }
			set { SetProperty(ref _Date, value); }
		}

		List<Venue> _Venues;
		public List<Venue> Venues
		{
			get { return _Venues; }
			set { SetProperty(ref _Venues, value); }
		}

        [JsonIgnore]
		public ObservableCollection<Grouping<string, Performance>> PerformancesGroupedByVenue
		{
			get
			{
				var groupedPerformances = new ObservableCollection<Grouping<string, Performance>>();

				foreach (var v in Venues)
				{
					groupedPerformances.Add(new Grouping<string, Performance>(v.Name, v.Performances));
				}

				return groupedPerformances;
			}
		}

        [JsonIgnore]
		public ObservableCollection<Grouping<DateTime, Performance>> PerformancesGroupedByTime
		{
			get
			{
				var flattenedList = new List<Tuple<DateTime, Performance>>();

				var groupedPerformances = new ObservableCollection<Grouping<DateTime, Performance>>();

				foreach (var v in Venues)
				{
					foreach (var p in v.Performances)
					{
						p.VenueName = v.Name;
						flattenedList.Add(new Tuple<DateTime, Performance>(p.Time, p));
					}
				}

				var distinctTimes = flattenedList.OrderBy(x => x.Item1).GroupBy(x => x.Item1).Select(x => x.First()).Select(x => x.Item1);

				flattenedList = flattenedList.OrderBy(x => x.Item1).ToList();

				foreach (var dt in distinctTimes)
				{
					groupedPerformances.Add(new Grouping<DateTime, Performance>(dt, flattenedList.Where(x => x.Item1 == dt).Select(x => x.Item2)));
				}

				return groupedPerformances;
			}
		}
	}
	
}
