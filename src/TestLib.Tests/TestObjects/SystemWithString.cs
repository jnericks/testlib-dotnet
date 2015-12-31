namespace jnericks.TestLib.Tests.TestObjects
{
    public class SystemWithString : SystemForTest
    {
        public string String { get; set; }

        public SystemWithString(IDependencyA dependencyA, IDependencyB dependencyB, string @string)
            : base(dependencyA, dependencyB)
        {
            String = @string;
        }
    }
}