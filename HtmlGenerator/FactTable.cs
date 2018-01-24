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
}