using System;
using Newtonsoft.Json;
using MvvmHelpers;

namespace HGMF2018
{
	public class Performance : ObservableObject
	{
		string _Name;
		public string Name
		{
			get { return _Name; }
			set { SetProperty(ref _Name, value); }
		}

		DateTime _Time;
		public DateTime Time
		{
			get { return _Time; }
			set { SetProperty(ref _Time, value); }
		}

		bool _TwentyOnePlus;
		public bool TwentyOnePlus
		{
			get { return _TwentyOnePlus; }
			set { SetProperty(ref _TwentyOnePlus, value); }
		}

		bool _WristbandRequired;
		public bool WristbandRequired
		{
			get { return _WristbandRequired; }
			set { SetProperty(ref _WristbandRequired, value); }
		}

		bool _WeeklongWristbandRequired;
		public bool WeeklongWristbandRequired
		{
			get { return _WeeklongWristbandRequired; }
			set { SetProperty(ref _WeeklongWristbandRequired, value); }
		}

		string _VenueName;
		[JsonIgnore]
		public string VenueName
		{
			get { return _VenueName; }
			set { SetProperty(ref _VenueName, value); }
		}
	}

}
