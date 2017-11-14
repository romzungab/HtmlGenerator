namespace HtmlGenerator
{
    public class Dimension
    {
        public string Name { get; set; }
        public string[] Values { get; set; }
    }

    public class Grouping
    {
        public Dimension Dimension { get; set; }
        public bool Group { get; set; }
    }
}
