namespace jnericks.TestLib.Tests.TestObjects
{
    public class SystemForTest
    {
        readonly IDependencyA _dependencyA;
        readonly IDependencyB _dependencyB;

        public SystemForTest(IDependencyB dependencyB)
        {
            _dependencyB = dependencyB;
        }

        public SystemForTest(IDependencyA dependencyA, IDependencyB dependencyB)
        {
            _dependencyA = dependencyA;
            _dependencyB = dependencyB;
        }

        public void DoAStuff()
        {
            _dependencyA.AStuff();
        }

        public object PassToDependencyA(object o)
        {
            return _dependencyA.DoSomething(o);
        }

        public void DoBStuff()
        {
            _dependencyB.BStuff();
        }
    }
}