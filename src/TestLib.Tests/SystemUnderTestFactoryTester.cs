using System;
using FluentAssertions;
using jnericks.TestLib.Tests.TestObjects;
using NSubstitute;
using Ploeh.AutoFixture;
using Xunit;

namespace jnericks.TestLib.Tests
{
    public class SystemUnderTestFactoryTester
    {
        private Fixture myFixture = new Fixture();

        [Fact]
        public void Should_be_able_to_create_sut()
        {
            new SystemUnderTestFactory<SystemForTest>().Sut
                                                .Should().BeAssignableTo<SystemForTest>();
        }

        [Fact]
        public void Should_have_sut_be_a_singleton()
        {
            var sutFactory = new SystemUnderTestFactory<SystemForTest>();
            var reference1 = sutFactory.Sut;
            var reference2 = sutFactory.Sut;
            reference2.Should().BeSameAs(reference1);
        }

        [Fact]
        public void Should_have_dependencyA()
        {
            new SystemUnderTestFactory<SystemForTest>()
                .Dependency<IDependencyA>()
                .Should().BeAssignableTo<IDependencyA>();
        }

        [Fact]
        public void Should_have_dependencyB()
        {
            new SystemUnderTestFactory<SystemForTest>()
                .Dependency<IDependencyB>()
                .Should().BeAssignableTo<IDependencyB>();
        }

        [Fact]
        public void Should_be_able_to_do_A_stuff()
        {
            var sutFactory = new SystemUnderTestFactory<SystemForTest>();
            sutFactory.Sut.DoAStuff();
            sutFactory.Dependency<IDependencyA>().Received().AStuff();
        }

        [Fact]
        public void Should_be_able_to_do_B_stuff()
        {
            var sutFactory = new SystemUnderTestFactory<SystemForTest>();
            sutFactory.Sut.DoBStuff();
            sutFactory.Dependency<IDependencyB>().Received().BStuff();
        }

        [Fact]
        public void Should_be_able_to_stub_A_method()
        {
            var objectPassedToSut = new object();
            var objectReturnedFromDependencyA = new object();

            var sutFactory = new SystemUnderTestFactory<SystemForTest>();

            sutFactory.Dependency<IDependencyA>()
                      .DoSomething(objectPassedToSut)
                      .Returns(objectReturnedFromDependencyA);

            var actual = sutFactory.Sut.PassToDependencyA(objectPassedToSut);

            actual.Should().BeSameAs(objectReturnedFromDependencyA);
        }

        [Fact]
        public void Should_be_able_to_retrieve_injected_substitute()
        {
            var myDependencyA = Substitute.For<IDependencyA>();

            var sutFactory = new SystemUnderTestFactory<SystemForTest>();
            sutFactory.ForDependency<IDependencyA>().Use(myDependencyA);

            sutFactory.Dependency<IDependencyA>().Should().BeSameAs(myDependencyA);
        }

        [Fact]
        public void Should_be_able_to_assert_on_injected_substitute()
        {
            var myDependencyA = Substitute.For<IDependencyA>();

            var sutFactory = new SystemUnderTestFactory<SystemForTest>();
            sutFactory.ForDependency<IDependencyA>().Use(myDependencyA);
            sutFactory.Sut.DoAStuff();

            myDependencyA.Received().AStuff();
        }

        [Fact]
        public void Should_throw_exception_when_configuring_an_object_that_is_NOT_a_dependency()
        {
            var sutFactory = new SystemUnderTestFactory<SystemForTest>();
            Assert.Throws<InvalidOperationException>(() => sutFactory.ForDependency<INotADependency>().Use(Substitute.For<INotADependency>()));
        }

        [Fact]
        public void Should_throw_exception_when_retrieving_object_that_is_NOT_a_dependency()
        {
            var sutFactory = new SystemUnderTestFactory<SystemForTest>();
            Assert.Throws<InvalidOperationException>(() => sutFactory.Dependency<INotADependency>());
        }

        [Fact]
        public void Should_throw_exception_when_trying_to_retrieve_Sut_from_method()
        {
            var sutFactory = new SystemUnderTestFactory<SystemForTest>();
            Assert.Throws<InvalidOperationException>(() => sutFactory.Dependency<SystemForTest>());
        }

        [Fact]
        public void Should_be_able_to_add_a_post_processor()
        {
            var wasCalled = false;
            var sutFactory = new SystemUnderTestFactory<SystemForTest>();
            sutFactory.AfterSutCreated(sut => wasCalled = true);
            sutFactory.CreateSut();
            wasCalled.Should().BeTrue();
        }

        [Fact]
        public void Should_be_able_to_add_a_post_processor_to_custom_sut_creation()
        {
            var wasCalled = false;
            var sutFactory = new SystemUnderTestFactory<SystemForTest>();
            sutFactory.AfterSutCreated(sut => wasCalled = true);
            sutFactory.CreateSutUsing(() => new SystemForTest(null));
            sutFactory.CreateSut();
            wasCalled.Should().BeTrue();
        }

        [Fact]
        public void Should_be_able_to_swap_in_a_concrete_impl_of_a_dependency()
        {
            var aStuffWasCalled = false;
            var expectedObject = new object();

            var impl = new DependencyImplA(() => aStuffWasCalled = true, x => expectedObject);
            var sutFactory = new SystemUnderTestFactory<SystemForTest>();
            sutFactory.ForDependency<IDependencyA>().Use(impl);

            sutFactory.Sut.DoAStuff();
            aStuffWasCalled.Should().BeTrue();

            var actual = sutFactory.Sut.PassToDependencyA(null);
            actual.Should().BeSameAs(expectedObject);
        }

        [Fact]
        public void Should_be_able_to_handle_primitive()
        {
            var theInt = myFixture.Create<int>();

            var sutFactory = new SystemUnderTestFactory<SystemWithPrimitive>();

            sutFactory.ForDependency<int>().Use(theInt);

            sutFactory.Sut.Integer.Should().Be(theInt);
        }

        [Fact]
        public void Should_be_able_to_handle_string()
        {
            var theString = myFixture.Create<string>();

            var sutFactory = new SystemUnderTestFactory<SystemWithString>();

            sutFactory.ForDependency<string>().Use(theString);

            sutFactory.Sut.String.Should().BeSameAs(theString);
        }

    }
}