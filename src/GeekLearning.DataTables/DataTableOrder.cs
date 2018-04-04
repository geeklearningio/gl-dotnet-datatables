// https://www.echosteg.com/jquery-datatables-asp.net-mvc5-server-side
namespace GeekLearning.DataTables
{
	using Newtonsoft.Json;
	using Newtonsoft.Json.Converters;

	/// <summary>
	/// An order, as sent by jQuery DataTables when doing AJAX queries.
	/// </summary>
	public class DataTableOrder
	{
		/// <summary>
		/// Column to which ordering should be applied.
		/// This is an index reference to the columns array of information that is also submitted to the server.
		/// </summary>
		public int Column { get; set; }

		/// <summary>
		/// Ordering direction for this column.
		/// It will be dt-string asc or dt-string desc to indicate ascending ordering or descending ordering, respectively.
		/// </summary>
		[JsonProperty(PropertyName = "dir")]
		[JsonConverter(typeof(StringEnumConverter))]
		public DataTableOrderDirection Direction { get; set; }
	}
}
