using System;
using System.Collections.Generic;
using System.Linq;

namespace jnericks.TestLib
{
    public class DoForDependency<TDependency>
    {
        readonly IList<Tuple<Type, object>> _parameters;

        public DoForDependency(IList<Tuple<Type, object>> parameters)
        {
            _parameters = parameters;
        }

        public void Use(TDependency dependency)
        {
            var tuple = _parameters.First(x => x.Item1 == typeof(TDependency));
            var index = _parameters.IndexOf(tuple);
            _parameters.RemoveAt(index);
            _parameters.Insert(index, new Tuple<Type, object>(tuple.Item1, dependency));
        }
    }
}