using System.Collections;
using System.Collections.Generic;
using System.Reflection;
using Rebec.Interfaces;

namespace Rebec.Builders
{
    public interface ITableBuilderContext
    {
        ICollection Objects { get; }
        ICollection<string> Columns { get; }
        ICollection<PropertyInfo> Properties { get; }
        IBuilderStyle Style { get; }
    }
}