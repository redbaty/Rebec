using System;
using System.Reflection;

namespace Rebec.Representations
{
    public class ColumnRepresentation
    {
        public Func<object, string> ComputedAction { get; }

        public bool IsComputed { get; }

        public string Name { get; }

        public PropertyInfo RefersTo { get; }

        public ColumnRepresentation(string name, PropertyInfo refersTo)
        {
            Name = name;
            RefersTo = refersTo;
        }

        public ColumnRepresentation(string name, Func<object, string> action)
        {
            Name = name;
            ComputedAction = action;
            IsComputed = true;
        }

        public override string ToString()
        {
            return Name;
        }
    }
}