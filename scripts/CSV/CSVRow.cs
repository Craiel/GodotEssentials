using System;
using System.Collections.Generic;
using System.Globalization;

namespace Craiel.Essentials.CSV
{
    public sealed class CSVRow
    {
        private readonly Dictionary<string, object> values = new();
        private readonly Dictionary<string, Type> columnTypes = new();

        // -------------------------------------------------------------------
        // Constructor
        // -------------------------------------------------------------------
        internal CSVRow(IEnumerable<object> columnDefinitions)
        {
            foreach (var column in columnDefinitions)
            {
                var key = GetColumnKey(column);
                var type = GetColumnType(column);
                this.columnTypes[key] = type;
            }
        }

        // -------------------------------------------------------------------
        // Public
        // -------------------------------------------------------------------
        public CSVRow Set<T>(CSVColumn<T> column, T value)
        {
            if (!this.columnTypes.ContainsKey(column.Key))
            {
                throw new ArgumentException($"Column '{column.Key}' is not defined in this row");
            }

            this.values[column.Key] = value;
            return this;
        }

        public T Get<T>(CSVColumn<T> column)
        {
            if (!this.columnTypes.ContainsKey(column.Key))
            {
                throw new ArgumentException($"Column '{column.Key}' is not defined in this row");
            }

            if (!this.values.TryGetValue(column.Key, out var value))
            {
                return default(T);
            }

            if (value is T typedValue)
            {
                return typedValue;
            }

            return default(T);
        }

        public bool HasValue<T>(CSVColumn<T> column)
        {
            return this.values.ContainsKey(column.Key);
        }

        public IEnumerable<string> GetColumnKeys()
        {
            return this.columnTypes.Keys;
        }

        internal string GetValueAsString(string columnKey, char delimiter)
        {
            if (!this.values.TryGetValue(columnKey, out var value))
            {
                return string.Empty;
            }

            if (value == null)
            {
                return string.Empty;
            }

            return FormatValue(value, delimiter);
        }

        internal void SetValueFromString(string columnKey, string stringValue)
        {
            if (!this.columnTypes.TryGetValue(columnKey, out var columnType))
            {
                throw new ArgumentException($"Unknown column key: {columnKey}");
            }

            if (string.IsNullOrEmpty(stringValue))
            {
                this.values[columnKey] = GetDefaultValue(columnType);
                return;
            }

            var convertedValue = ConvertFromString(stringValue, columnType);
            this.values[columnKey] = convertedValue;
        }

        // -------------------------------------------------------------------
        // Private
        // -------------------------------------------------------------------
        private static string GetColumnKey(object column)
        {
            var keyProperty = column.GetType().GetProperty("Key");
            return keyProperty?.GetValue(column) as string ?? throw new ArgumentException("Invalid column definition");
        }

        private static Type GetColumnType(object column)
        {
            var typeProperty = column.GetType().GetProperty("ValueType");
            return typeProperty?.GetValue(column) as Type ?? throw new ArgumentException("Invalid column definition");
        }

        private static string FormatValue(object value, char delimiter)
        {
            var formattedValue = value switch
            {
                float f => f.ToString("F1", CultureInfo.InvariantCulture),
                double d => d.ToString("F1", CultureInfo.InvariantCulture),
                string s => s,
                _ => value.ToString()
            };

            return EscapeCSVValue(formattedValue, delimiter);
        }

        private static string EscapeCSVValue(string value, char delimiter)
        {
            if (string.IsNullOrEmpty(value))
            {
                return value;
            }

            bool needsQuoting = value.Contains(delimiter) || value.Contains('"') || value.Contains('\n') || value.Contains('\r');
            
            if (!needsQuoting)
            {
                return value;
            }

            var escaped = value.Replace("\"", "\"\"");
            return $"\"{escaped}\"";
        }

        private static object ConvertFromString(string value, Type targetType)
        {
            if (targetType == typeof(string))
            {
                return UnescapeCSVValue(value);
            }

            if (targetType == typeof(int))
            {
                return int.Parse(value, CultureInfo.InvariantCulture);
            }

            if (targetType == typeof(long))
            {
                return long.Parse(value, CultureInfo.InvariantCulture);
            }

            if (targetType == typeof(float))
            {
                return float.Parse(value, CultureInfo.InvariantCulture);
            }

            if (targetType == typeof(double))
            {
                return double.Parse(value, CultureInfo.InvariantCulture);
            }

            if (targetType == typeof(bool))
            {
                return bool.Parse(value);
            }

            if (targetType == typeof(ushort))
            {
                return ushort.Parse(value, CultureInfo.InvariantCulture);
            }

            if (targetType == typeof(ulong))
            {
                return ulong.Parse(value, CultureInfo.InvariantCulture);
            }

            throw new ArgumentException($"Unsupported type conversion: {targetType.Name}");
        }

        private static string UnescapeCSVValue(string value)
        {
            if (string.IsNullOrEmpty(value))
            {
                return value;
            }

            if (value.StartsWith("\"") && value.EndsWith("\""))
            {
                value = value.Substring(1, value.Length - 2);
                value = value.Replace("\"\"", "\"");
            }

            return value;
        }

        private static object GetDefaultValue(Type type)
        {
            if (type.IsValueType)
            {
                return Activator.CreateInstance(type);
            }

            return null;
        }
    }
}