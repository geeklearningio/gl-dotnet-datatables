namespace GeekLearning.DataTables
{
	using System;
	using System.Collections.Generic;
	using System.Linq;
	using System.Threading.Tasks;

	/// <summary>
	/// The parameters sent by jQuery DataTables in AJAX queries.
	/// </summary>
	public interface IDataTableContext<TData> where TData : class
	{
		bool IsPaginationActivated { get; }

		DataTableParameters Parameters { get; set; }
		
		DataTableResolver Resolver { get; set; }

		IQueryable<TData> Query { get; set; }

              IQueryable<TData> OrderedQuery { get; set; }

              IQueryable<TData> FilteredQuery { get; set; }

		IQueryable<TData> PaginatedQuery { get; set; }

        Task<DataTableResult<TDataViewModel>> CreateResultAsync<TDataViewModel>(IEnumerable<TDataViewModel> viewModels);

        IDataTableContext<TData> Configure(Type resolverType);
	}
}
