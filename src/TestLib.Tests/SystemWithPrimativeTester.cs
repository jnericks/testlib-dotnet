using FluentAssertions;
using jnericks.TestLib.Tests.TestObjects;
using Xunit;

namespace jnericks.TestLib.Tests
{
    public class SystemWithPrimativeTester : BaseUnitTester<SystemWithPrimitive>
    {
        [Fact]
        public void Should_be_able_to_handle_primitive()
        {
            var i = 12;

            ForDependency<int>().Use(i);

            Sut.Integer.Should().Be(i);
        }

        [Fact]
        public void Should_be_able_to_handle_string()
        {
            var str = "jnericks";

            ForDependency<string>().Use(str);

            Sut.String.Should().BeSameAs(str);
        }
    }
}