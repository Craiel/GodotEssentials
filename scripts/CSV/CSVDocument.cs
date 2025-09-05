using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace Craiel.Essentials.CSV
{
    public sealed class CSVDocument : IEnumerable<CSVRow>
    {
        public const char DefaultDelimiter = ',';
        public const char SemicolonDelimiter = ';';
        public const char TabDelimiter = '\t';
        public const char PipeDelimiter = '|';

        private readonly List<object> columnDefinitions = new();
        private readonly List<CSVRow> rows = new();

        // -------------------------------------------------------------------
        // Constructor
        // -------------------------------------------------------------------
        public CSVDocument(char delimiter = DefaultDelimiter)
        {
            this.Delimiter = delimiter;
        }

        // -------------------------------------------------------------------
        // Public
        // -------------------------------------------------------------------
        public char Delimiter { get; }
        public int ColumnCount => this.columnDefinitions.Count;
        public int RowCount => this.rows.Count;

        public CSVDocument AddColumn<T>(CSVColumn<T> column)
        {
            if (this.columnDefinitions.Any(c => GetColumnKey(c) == column.Key))
            {
                throw new ArgumentException($"Column with key '{column.Key}' already exists");
            }

            this.columnDefinitions.Add(column);
            return this;
        }

        public CSVRow AddRow()
        {
            if (this.columnDefinitions.Count == 0)
            {
                throw new InvalidOperationException("Cannot add rows without columns. Add columns first.");
            }

            var row = new CSVRow(this.columnDefinitions);
            this.rows.Add(row);
            return row;
        }

        public void Clear()
        {
            this.rows.Clear();
        }

        public void ClearAll()
        {
            this.rows.Clear();
            this.columnDefinitions.Clear();
        }

        public string ToCSV()
        {
            var csvBuilder = new StringBuilder();

            if (this.columnDefinitions.Count == 0)
            {
                return string.Empty;
            }

            var headers = this.columnDefinitions.Select(GetColumnDisplayName);
            csvBuilder.AppendLine(string.Join(this.Delimiter, headers));

            var columnKeys = this.columnDefinitions.Select(GetColumnKey).ToList();

            foreach (var row in this.rows)
            {
                var values = columnKeys.Select(key => row.GetValueAsString(key, this.Delimiter));
                csvBuilder.AppendLine(string.Join(this.Delimiter, values));
            }

            return csvBuilder.ToString();
        }

        public void SaveToFile(string filePath)
        {
            var csvContent = this.ToCSV();
            using var writer = new StreamWriter(filePath, false, Encoding.UTF8);
            writer.Write(csvContent);
        }

        public static CSVDocument LoadFromFile(string filePath, char? delimiter = null)
        {
            if (!File.Exists(filePath))
            {
                throw new FileNotFoundException($"CSV file not found: {filePath}");
            }

            var lines = File.ReadAllLines(filePath);
            return LoadFromLines(lines, delimiter);
        }

        public static CSVDocument LoadFromLines(string[] lines, char? delimiter = null)
        {
            if (lines.Length == 0)
            {
                return new CSVDocument();
            }

            char detectedDelimiter = delimiter ?? DetectDelimiter(lines[0]);
            var document = new CSVDocument(detectedDelimiter);
            
            var headerLine = lines[0];
            var headers = ParseCSVLine(headerLine, detectedDelimiter);
            
            foreach (var header in headers)
            {
                document.AddColumn(CSVColumn.String(header, header));
            }

            for (int i = 1; i < lines.Length; i++)
            {
                var line = lines[i].Trim();
                if (string.IsNullOrEmpty(line))
                {
                    continue;
                }

                var values = ParseCSVLine(line, detectedDelimiter);
                var row = document.AddRow();

                for (int j = 0; j < Math.Min(values.Length, headers.Length); j++)
                {
                    var columnKey = headers[j];
                    var value = values[j];
                    row.SetValueFromString(columnKey, value);
                }
            }

            return document;
        }

        public IEnumerator<CSVRow> GetEnumerator()
        {
            return this.rows.GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return this.GetEnumerator();
        }

        // -------------------------------------------------------------------
        // Private
        // -------------------------------------------------------------------
        private static string GetColumnKey(object column)
        {
            var keyProperty = column.GetType().GetProperty("Key");
            return keyProperty?.GetValue(column) as string ?? throw new ArgumentException("Invalid column definition");
        }

        private static string GetColumnDisplayName(object column)
        {
            var displayNameProperty = column.GetType().GetProperty("DisplayName");
            return displayNameProperty?.GetValue(column) as string ?? throw new ArgumentException("Invalid column definition");
        }

        private static char DetectDelimiter(string headerLine)
        {
            var delimiters = new[] { DefaultDelimiter, SemicolonDelimiter, TabDelimiter, PipeDelimiter };
            
            foreach (var delimiter in delimiters)
            {
                if (headerLine.Contains(delimiter))
                {
                    return delimiter;
                }
            }
            
            return DefaultDelimiter;
        }

        public static string[] ParseCSVLine(string line, char delimiter)
        {
            var result = new List<string>();
            var currentField = new StringBuilder();
            bool insideQuotes = false;

            for (int i = 0; i < line.Length; i++)
            {
                var c = line[i];

                switch (c)
                {
                    case '"':
                        if (insideQuotes)
                        {
                            if (i + 1 < line.Length && line[i + 1] == '"')
                            {
                                currentField.Append('"');
                                i++;
                            }
                            else
                            {
                                insideQuotes = false;
                            }
                        }
                        else
                        {
                            insideQuotes = true;
                        }
                        break;

                    default:
                        if (c == delimiter && !insideQuotes)
                        {
                            result.Add(currentField.ToString());
                            currentField.Clear();
                        }
                        else
                        {
                            currentField.Append(c);
                        }
                        break;
                }
            }

            result.Add(currentField.ToString());
            return result.ToArray();
        }
    }
}