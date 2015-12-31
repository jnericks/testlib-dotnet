using System;

namespace jnericks.TestLib.Tests.TestObjects
{
    public class DependencyImplA : IDependencyA
    {
        Action _aStuff;
        Func<object, object> _doSomething;

        public DependencyImplA(Action aStuff = null, Func<object, object> doSomething = null)
        {
            _aStuff = aStuff ?? delegate { };
            _doSomething = doSomething ?? delegate { return null; };
        }

        public void AStuff() => _aStuff();
        public object DoSomething(object o) => _doSomething(o);
    }
}