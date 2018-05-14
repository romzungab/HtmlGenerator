using System.Data.SqlClient;
using System.Dynamic;

namespace HtmlGenerator
{
    public class ViewSource
    {
        public string Table { get; set; }
        public string PrimaryKey { get; set; }
        public Column[] Columns { get; set; }
    }
    public class Column
    {
        public string Name { get; set; }
        public string Expression { get; set; }
        public string[] GroupBys { get; set; }
    }

    public class Dimension : ViewSource
    {
        
    }
    
    public class ColumnValue
    {
        public Column Column { get; set; }
        public object Value { get; set; }
    }
}

