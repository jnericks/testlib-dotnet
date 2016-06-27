namespace jnericks.TestLib.Tests.TestObjects
{
    public class SystemWithPrimitive : SystemForTest
    {
        public int Integer { get; set; }
        public string String { get; set; }

        public SystemWithPrimitive(IDependencyA dependencyA, IDependencyB dependencyB, int integer, string @string)
            : base(dependencyA, dependencyB)
        {
            Integer = integer;
            String = @string;
        }
    }
}