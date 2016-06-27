using FluentAssertions;
using NSubstitute;
using Xunit;

namespace jnericks.TestLib.Tests
{
    public class TestExtensionsTester : BaseUnitTester
    {
        [Fact]
        public void should_be_able_to_get_default_of_primitive()
        {
            typeof(int).Default().Should().Be(default(int));
            typeof(decimal).Default().Should().Be(default(decimal));
        }

        [Fact]
        public void should_be_able_to_get_default_of_string()
        {
            typeof(string).Default().Should().BeNull();
        }

        [Fact]
        public void should_be_able_to_get_default_of_object()
        {
            typeof(TestExtensionsTester).Default().Should().BeNull();
        }

        [Fact]
        public void should_be_able_to_fake()
        {
            var message = "some message";
            var fake = (IForTest)typeof(IForTest).Fake();
            fake.Do().Returns(message);
            fake.Do().Should().Be(message);
        }
    }

    public interface IForTest
    {
        string Do();
    }
}