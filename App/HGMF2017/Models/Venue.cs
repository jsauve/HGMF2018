using System.Collections.Generic;
using MvvmHelpers;

namespace HGMF2018
{
	public class Venue : ObservableObject
	{
		string _Name;
		public string Name 
		{
			get { return _Name; }
			set { SetProperty(ref _Name, value); }
		}

		List<Performance> _Performances;
		public List<Performance> Performances 
		{ 
			get { return _Performances; }
			set { SetProperty(ref _Performances, value); }
		}
	}
	
}
