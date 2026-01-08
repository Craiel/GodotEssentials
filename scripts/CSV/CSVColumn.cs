using System;
using Craiel.Essentials.Mathematics;

namespace Craiel.Essentials.CSV
{
    public sealed class CSVColumn<T>
    {
        // -------------------------------------------------------------------
        // Constructor
        // -------------------------------------------------------------------
        public CSVColumn(string key, string displayName)
        {
            this.Key = key ?? throw new ArgumentNullException(nameof(key));
            this.DisplayName = displayName ?? throw new ArgumentNullException(nameof(displayName));
            this.ValueType = typeof(T);
        }

        // -------------------------------------------------------------------
        // Public
        // -------------------------------------------------------------------
        public string Key { get; }
        public string DisplayName { get; }
        public Type ValueType { get; }

        public override string ToString()
        {
            return this.DisplayName;
        }

        public override bool Equals(object obj)
        {
            if (obj is CSVColumn<T> other)
            {
                return this.Key == other.Key;
            }
            return false;
        }

        public override int GetHashCode()
        {
            return this.Key.GetHashCode();
        }
    }

    public static class CSVColumn
    {
        // -------------------------------------------------------------------
        // Public
        // -------------------------------------------------------------------
        public static CSVColumn<string> String(string key, string displayName = null)
        {
            return new CSVColumn<string>(key, displayName ?? key);
        }

        public static CSVColumn<int> Int(string key, string displayName = null)
        {
            return new CSVColumn<int>(key, displayName ?? key);
        }

        public static CSVColumn<long> Long(string key, string displayName = null)
        {
            return new CSVColumn<long>(key, displayName ?? key);
        }

        public static CSVColumn<float> Float(string key, string displayName = null)
        {
            return new CSVColumn<float>(key, displayName ?? key);
        }

        public static CSVColumn<double> Double(string key, string displayName = null)
        {
            return new CSVColumn<double>(key, displayName ?? key);
        }

        public static CSVColumn<bool> Bool(string key, string displayName = null)
        {
            return new CSVColumn<bool>(key, displayName ?? key);
        }

        public static CSVColumn<ushort> UShort(string key, string displayName = null)
        {
            return new CSVColumn<ushort>(key, displayName ?? key);
        }

        public static CSVColumn<ulong> ULong(string key, string displayName = null)
        {
            return new CSVColumn<ulong>(key, displayName ?? key);
        }

        public static CSVColumn<Magnum> Magnum(string key, string displayName = null)
        {
            return new CSVColumn<Magnum>(key, displayName ?? key);
        }
    }
}