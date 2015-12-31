namespace jnericks.TestLib.Tests.TestObjects
{
    public class SystemWithPrimitive : SystemForTest
    {
        public int Integer { get; set; }

        public SystemWithPrimitive(IDependencyA dependencyA, IDependencyB dependencyB, int integer)
            : base(dependencyA, dependencyB)
        {
            Integer = integer;
        }
    }
}