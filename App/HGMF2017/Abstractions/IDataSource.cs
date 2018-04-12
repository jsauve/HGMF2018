using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace HGMF2018
{
	/// <summary>
	/// Defines a conract for data source that exposes CRUD operations for a specific type.
	/// </summary>
	public interface IDataSource<T>
	{
		/// <summary>
		/// Gets all the items.
		/// </summary>
		/// <returns>All the items.</returns>
		Task<IEnumerable<T>> GetItems();
	}
}
