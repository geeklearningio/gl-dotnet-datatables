namespace System.Linq
{
       using GeekLearning.DataTables;

       public static class DataTableExtensions
       {
              public static IQueryable<TData> Paginate<TData>(this IQueryable<TData> query, IDataTableContext<TData> context)
                  where TData : class
              {
                     if (context.Resolver == null)
                     {
                            throw new InvalidOperationException("A DataTable resolver is required to paginate.");
                     }
                     context.Query = query;
                     context.OrderedQuery = context.Query
                            .Order(context.Parameters.Order, context.Resolver);
                     context.FilteredQuery = context.OrderedQuery
                         .Search(context.Parameters.Search, context.Resolver);
                     context.PaginatedQuery = context.FilteredQuery
                         .Skip(context.Parameters.Start)
                         .Take(context.Parameters.Length);

                     return context.PaginatedQuery;
              }

              public static IQueryable<TData> Order<TData>(this IQueryable<TData> query, DataTableOrder[] orderParameters, DataTableResolver resolver)
                  where TData : class
              {
                     if (resolver == null)
                     {
                            throw new InvalidOperationException("A DataTable resolver is required to order.");
                     }

                     for (int i = 0; i < orderParameters.Length; i++)
                     {
                            bool isDesc = orderParameters[i].Direction == DataTableOrderDirection.DESC;

                            var orderParameter = orderParameters[i];

                            var orderableColumn = string.IsNullOrEmpty(orderParameter.ColumnName)
                                ? resolver.OrderableColumns[orderParameters[i].Column]
                                : resolver.OrderableColumns.FirstOrDefault(oc => string.Compare(oc, orderParameter.ColumnName, true) == 0);

                            if (orderableColumn != null)
                            {
                                   query = i == 0 ? query.DynamicOrderBy(orderableColumn, isDesc) : query.DynamicThenBy(orderableColumn, isDesc);
                            }
                     }

                     return query;
              }

              public static IQueryable<TData> Search<TData>(this IQueryable<TData> query, DataTableSearch searchParameters, DataTableResolver resolver)
                  where TData : class
              {
                     if (resolver == null)
                     {
                            throw new InvalidOperationException("A DataTable resolver is required to search.");
                     }

                     if (!string.IsNullOrWhiteSpace(searchParameters.Value))
                     {
                            return query.DynamicWhere(resolver.SearchableColumns, searchParameters.Value);
                     }

                     return query;
              }
       }
}