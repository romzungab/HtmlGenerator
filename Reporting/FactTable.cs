namespace HtmlGenerator
{
    public class FactTable: ViewSource
    {
        public Dimension[] Dimensions { get; set; }
        public Measure[] Measures { get; set; }
    }

    public class Measure: Column
    {
        public string Aggregation { get; set; }
    }

    public class MeasureValue
    {
        public Measure Measure { get; set; }
        public object Value { get; set; }
    }
}