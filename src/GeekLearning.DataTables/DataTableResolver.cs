namespace GeekLearning.DataTables
{
    using System;

    [AttributeUsage(AttributeTargets.Class)]
	public class DataTableResolver : Attribute
	{
		public string SearchableColumn { get; set; }

		public string[] OrderableColumns { get; set; }
	}
}
