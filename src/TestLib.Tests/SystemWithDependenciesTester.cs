using System;
using FluentAssertions;
using jnericks.TestLib.Tests.TestObjects;
using NSubstitute;
using Xunit;

namespace jnericks.TestLib.Tests
{
    public class SystemWithDependenciesTester : BaseUnitTester<SystemForTest>
    {
        [Fact]
        public void Should_be_able_to_create_sut()
        {
            Sut.Should().BeAssignableTo<SystemForTest>();
        }

        [Fact]
        public void Should_have_sut_be_a_singleton()
        {
            Sut.Should().BeSameAs(Sut);
        }

        [Fact]
        public void Should_have_dependencyA()
        {
            Dependency<IDependencyA>().Should().BeAssignableTo<IDependencyA>();
        }

        [Fact]
        public void Should_have_dependencyB()
        {
            Dependency<IDependencyB>().Should().BeAssignableTo<IDependencyB>();
        }

        [Fact]
        public void Should_be_able_to_do_A_stuff()
        {
            Sut.DoAStuff();

            Dependency<IDependencyA>().Received().AStuff();
        }

        [Fact]
        public void Should_be_able_to_do_B_stuff()
        {
            Sut.DoBStuff();

            Dependency<IDependencyB>().Received().BStuff();
        }

        [Fact]
        public void Should_be_able_to_stub_A_method()
        {
            var objectPassedToSut = new object();
            var objectReturnedFromDependencyA = new object();

            Dependency<IDependencyA>()
                .DoSomething(objectPassedToSut)
                .Returns(objectReturnedFromDependencyA);

            var actual = Sut.PassToDependencyA(objectPassedToSut);

            actual.Should().BeSameAs(objectReturnedFromDependencyA);
        }

        [Fact]
        public void Should_be_able_to_retrieve_injected_substitute()
        {
            var myDependencyA = Substitute.For<IDependencyA>();

            ForDependency<IDependencyA>().Use(myDependencyA);

            Dependency<IDependencyA>().Should().BeSameAs(myDependencyA);
        }

        [Fact]
        public void Should_be_able_to_assert_on_injected_substitute()
        {
            var myDependencyA = Substitute.For<IDependencyA>();

            ForDependency<IDependencyA>().Use(myDependencyA);

            Sut.DoAStuff();

            myDependencyA.Received().AStuff();
        }

        [Fact]
        public void Should_throw_exception_when_configuring_an_object_that_is_NOT_a_dependency()
        {
            Assert.Throws<InvalidOperationException>(() => ForDependency<INotADependency>().Use(Substitute.For<INotADependency>()));
        }

        [Fact]
        public void Should_throw_exception_when_retrieving_object_that_is_NOT_a_dependency()
        {
            Assert.Throws<InvalidOperationException>(() => Dependency<INotADependency>());
        }

        [Fact]
        public void Should_throw_exception_when_trying_to_retrieve_Sut_from_method()
        {
            Assert.Throws<InvalidOperationException>(() => Dependency<SystemForTest>());
        }

        [Fact]
        public void Should_be_able_to_add_a_post_processor()
        {
            var wasCalled = false;
            AfterSutCreated(sut => wasCalled = true);

            CreateSut();

            wasCalled.Should().BeTrue();
        }

        [Fact]
        public void Should_be_able_to_add_a_post_processor_to_custom_sut_creation()
        {
            var wasCalled = false;

            AfterSutCreated(sut => wasCalled = true);
            CreateSutUsing(() => new SystemForTest(null));

            CreateSut();

            wasCalled.Should().BeTrue();
        }

        [Fact]
        public void Should_be_able_to_swap_in_a_concrete_impl_of_a_dependency()
        {
            var aStuffWasCalled = false;
            var expectedObject = new object();

            var impl = new DependencyImplA(() => aStuffWasCalled = true, x => expectedObject);

            ForDependency<IDependencyA>().Use(impl);

            Sut.DoAStuff();
            aStuffWasCalled.Should().BeTrue();

            var actual = Sut.PassToDependencyA(null);
            
            actual.Should().BeSameAs(expectedObject);
        }
    }
}