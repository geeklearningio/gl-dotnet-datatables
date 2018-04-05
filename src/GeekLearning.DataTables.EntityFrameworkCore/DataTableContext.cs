namespace GeekLearning.DataTables.EntityFrameworkCore
{
	using Microsoft.EntityFrameworkCore;
	using System;
	using System.Collections.Generic;
	using System.Linq;
	using System.Threading.Tasks;

	/// <summary>
	/// The parameters sent by jQuery DataTables in AJAX queries.
	/// </summary>
	public class DataTableContext<TData> : IDataTableContext<TData> where TData : class
	{
		public DataTableContext()
		{		
		}

		public DataTableContext(DataTableParameters parameters)
		{
			this.Parameters = parameters;
		}

		public bool IsPaginationActivated { get; private set; } = true;

		public DataTableParameters Parameters { get; set; }
		
		public DataTableResolver Resolver { get; set; }

		public IQueryable<TData> Query { get; set; }

		public IQueryable<TData> FilteredQuery { get; set; }

		public IQueryable<TData> PaginatedQuery { get; set; }

		public async Task<DataTableResult<TDataViewModel>> CreateResultAsync<TDataViewModel>(IEnumerable<TDataViewModel> viewModels)
		{
            var data = viewModels.ToList();

            return new DataTableResult<TDataViewModel>
			{
				Data = data,
				RecordsTotal = Query != null ? await Query.CountAsync() : data.Count,
				RecordsFiltered = FilteredQuery != null ? await FilteredQuery.CountAsync() : data.Count,
				Draw = Parameters.Draw
			};
		}

		public IDataTableContext<TData> Configure(Type resolverType)
		{
			this.Resolver = Attribute.GetCustomAttribute(resolverType, typeof(DataTableResolver)) as DataTableResolver;

			return this;
		}

		public static IDataTableContext<TData> TakeAll
		{
			get
			{
				return new DataTableContext<TData>
				{
					IsPaginationActivated = false
				};
			}
		}
	}
}
