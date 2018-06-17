using System;
using System.Collections.Generic;
using System.Data;
using System.Diagnostics;
using System.IO;
using System.Linq;
using Reporting;

namespace Reporting
{
    public class TableBuilder
    {
        private readonly Report _report;
        private readonly string[] _rows;
        private readonly string[] _nonPivoted;
        private readonly Pivot _pivot;
        private readonly IGrouping<IDictionary<string, string>, IDictionary<string, string>>[] _grouped;
        private readonly string[] _empty;
        private readonly int _pivotedHeadersCount;

        public TableBuilder(Report report, DataTable data)
            : this(report, data.Rows.Cast<DataRow>().Select(row => data.Columns.Cast<DataColumn>().ToDictionary(c => c.ColumnName, c => Convert.ToString(row[c]))))
        {
        }

        public TableBuilder(Report report, IEnumerable<IDictionary<string, string>> data)
        {
            _report = report;

            _rows = report.Rows.Select(r => r.Name).ToArray();
            _nonPivoted = report.Columns.Where(c => !c.Pivot).Select(c => c.Attribute.Name).ToArray();
            _pivot = new Pivot(report.Columns.Where(c => c.Pivot).Select(c => c.Attribute.Name));

            _grouped = data
                .GroupBy(row => _rows.Concat(_nonPivoted).ToDictionary(c => c, c => row[c]), DictionaryComparer.Default)
                .ToArray();

            foreach (var group in _grouped)
            foreach (var row in group)
            {
                _pivot.Add(
                    group.Key,
                    _pivot.Groups.Select(g => row[g]).ToArray(),
                    report.Measures.Select(m => row[m.Name]).ToArray());
            }

            _empty = new string[report.Measures.Length];
            _pivotedHeadersCount = _pivot.GetHeaderCount();
        }

        public string Build(bool measureHeader)
        {
            var sw = new StringWriter();
            sw.WriteLine("<table border><tbody>");

            var rowHeaders = new string[_rows.Length];
            var wroteHeadersOnce = false;

            foreach (var row in _grouped)
            {
                if (WriteRowHeaders(sw, row, rowHeaders) || !wroteHeadersOnce)
                {
                    wroteHeadersOnce = true;
                    WriteColumnHeaders(sw, measureHeader);
                }

                sw.WriteLine("<tr>");

                foreach (var column in _nonPivoted)
                    sw.WriteLine($"<td>{row.Key[column]}</td>");

                foreach (var values in _pivot.GetValues(row.Key))
                foreach (var value in values ?? _empty)
                    sw.WriteLine($"<td>{value}</td>");

                sw.WriteLine("</tr>");
            }

            sw.WriteLine("</tbody></table>");
            return sw.ToString();
        }

        private bool WriteRowHeaders(TextWriter tw, IGrouping<IDictionary<string, string>, IDictionary<string, string>> row, IList<string> rowHeaders)
        {
            var shouldOutputHeader = false;
            for (var i = 0; i < _rows.Length; i++)
            {
                var header = row.Key[_rows[i]];
                if (shouldOutputHeader || !Equals(header, rowHeaders[i]))
                {
                    rowHeaders[i] = header;
                    tw.WriteLine($"<tr><th colspan={_pivotedHeadersCount + _nonPivoted.Length}>{header}</th></tr>");
                    shouldOutputHeader = true;
                }
            }

            return shouldOutputHeader;
        }

        private void WriteColumnHeaders(TextWriter tw, bool measureHeader)
        {
            tw.WriteLine("<tr>");
            foreach (var column in _nonPivoted)
                tw.WriteLine($"<th rowspan={_pivot.Groups.Count + (measureHeader ? 1 : 0)}>{column}</th>");

            var rowOpened = true;

            foreach (var group in _pivot.Groups)
            {
                if (!rowOpened) tw.WriteLine("<tr>");

                foreach (var header in _pivot.GetHeaders(group))
                    tw.WriteLine($"<th colspan={header.Span}>{header.Title}</th>");

                tw.WriteLine("</tr>");
                rowOpened = false;
            }

            if (!measureHeader)
            {
                if (rowOpened) tw.WriteLine("</tr>");
                return;
            }

            if (!rowOpened) tw.WriteLine("<tr>");

            for (var i = 0; i < _pivotedHeadersCount; i++)
                foreach (var measure in _report.Measures)
                    tw.WriteLine($"<th>{measure.Name}</th>");

            tw.WriteLine("</tr>");
        }

        private class Pivot
        {
            private readonly string[] _groups;
            private readonly Node _root = new Node(null);
            private readonly Dictionary<object, Dictionary<Node, IEnumerable<string>>> _rows = new Dictionary<object, Dictionary<Node, IEnumerable<string>>>();

            public Pivot(IEnumerable<string> groups)
            {
                _groups = groups.ToArray();
            }

            internal void Add(object rowKey, IReadOnlyCollection<string> groupValues, IEnumerable<string> values)
            {
                if (groupValues.Count != _groups.Length)
                    throw new ArgumentException();

                var node = _root;
                foreach (var groupValue in groupValues)
                    node = Ensure(node, groupValue, () => new Node(groupValue));

                Ensure(_rows, rowKey).Add(node, values);
            }

            public IReadOnlyList<string> Groups => _groups;

            public IEnumerable<Header> GetHeaders(string group)
            {
                var level = Array.IndexOf(_groups, group) + 1;
                return GetNodes(level).Select(node => new Header
                {
                    Title = node.Value,
                    Span = GetNodes(node, _groups.Length - level).Count,
                });
            }

            public IEnumerable<IEnumerable<string>> GetValues(object rowKey)
            {
                var row = _rows[rowKey];

                foreach (var node in GetNodes(_groups.Length))
                {
                    row.TryGetValue(node, out var values);
                    yield return values;
                }
            }

            public int GetHeaderCount() => GetNodes(_groups.Length).Count;

            private IList<Node> GetNodes(int level)
            {
                return GetNodes(_root, level);
            }

            private static IList<Node> GetNodes(Node root, int level)
            {
                var leaves = new List<Node>();
                Traverse(root, 0);
                return leaves;

                void Traverse(Node node, int lvl)
                {
                    if (lvl == level)
                    {
                        leaves.Add(node);
                    }
                    else
                    {
                        foreach (var subNode in node.Values)
                            Traverse(subNode, lvl + 1);
                    }
                }
            }

            public struct Header
            {
                public string Title { get; set; }
                public int Span { get; set; }
            }

            [DebuggerDisplay("{" + nameof(Value) + "} (Count = {" + nameof(Count) + "})")]
            private class Node : Dictionary<string, Node>
            {
                public Node(string value)
                {
                    Value = value;
                }

                public string Value { get; }
            }

            private static TV Ensure<TK, TV>(IDictionary<TK, TV> dic, TK key) where TV : new()
                => Ensure(dic, key, () => new TV());

            private static TV Ensure<TK, TV>(IDictionary<TK, TV> dic, TK key, Func<TV> createValue)
            {
                if (!dic.TryGetValue(key, out var value))
                {
                    value = createValue();
                    dic.Add(key, value);
                }

                return value;
            }
        }

        private class DictionaryComparer : IEqualityComparer<IDictionary<string, string>>
        {
            public static readonly DictionaryComparer Default = new DictionaryComparer();

            public bool Equals(IDictionary<string, string> x, IDictionary<string, string> y)
            {
                if (x == y) return true;
                if (x == null || y == null) return false;

                if (!x.Keys.SequenceEqual(y.Keys))
                    return false;

                return x.All(p => Equals(p.Value, y[p.Key]));
            }

            public int GetHashCode(IDictionary<string, string> dic)
            {
                return dic.Values.Aggregate(0, (cur, value) => (cur * 397) ^ (value?.GetHashCode() ?? 0));
            }
        }
    }
}
