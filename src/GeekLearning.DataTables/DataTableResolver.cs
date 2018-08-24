namespace GeekLearning.DataTables
{
    using System;
    using System.Linq;

    [AttributeUsage(AttributeTargets.Class)]
	public class DataTableResolver : Attribute
	{
        public string SearchableColumn
        {
            get { return this.SearchableColumns?.FirstOrDefault(); }
            set
            {
                if (SearchableColumns == null)
                {
                    SearchableColumns = new string[1];
                }
                SearchableColumns[0] = value;
            }
        }
        public string[] SearchableColumns { get; set; }
        public string[] OrderableColumns { get; set; }
	}
}
