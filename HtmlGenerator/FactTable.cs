using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HtmlGenerator
{
    public class FactTable: Dimension
    {
        public Dimension[] Dimensions { get; set; }
        public string[] Columns { get; set; }

    }

    public class Measure
    {
        public string Name { get; set; }
        public string Expression { get; set; }
        public string Aggregation { get; set; }

    }

    public class MeasureValue
    {
        public Measure Measure { get; set; }
        public object Value { get; set; }
    }
}