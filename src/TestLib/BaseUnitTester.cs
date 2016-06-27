using System;
using NSubstitute;

namespace jnericks.TestLib
{
    public abstract class BaseUnitTester
    {
        protected const int Once = 1;

        protected T Fake<T>(params object[] constructorArguments) where T : class
        {
            return Substitute.For<T>(constructorArguments);
        }

        protected T1 Fake<T1, T2>(params object[] constructorArguments) where T1 : class where T2 : class
        {
            return Substitute.For<T1, T2>(constructorArguments);
        }

        protected T1 Fake<T1, T2, T3>(params object[] constructorArguments) where T1 : class where T2 : class where T3 : class
        {
            return Substitute.For<T1, T2, T3>(constructorArguments);
        }

        protected object Fake(Type[] typesToProxy, object[] constructorArguments)
        {
            return Substitute.For(typesToProxy, constructorArguments);
        }
    }

    public abstract class BaseUnitTester<TContract, TSystemUnderTest> : BaseUnitTester where TSystemUnderTest : class, TContract
    {
        SystemUnderTestFactory<TContract, TSystemUnderTest> SutFactory { get; }

        protected TContract Sut => SutFactory.Sut;

        protected BaseUnitTester()
        {
            SutFactory = new SystemUnderTestFactory<TContract, TSystemUnderTest>();
        }

        protected void CreateSut()
        {
            SutFactory.CreateSut();
        }

        protected void CreateSutUsing(Func<TSystemUnderTest> factory)
        {
            SutFactory.CreateSutUsing(factory);
        }

        public void BeforeSutCreated(Action sutPreProcessor)
        {
            SutFactory.BeforeSutCreated(sutPreProcessor);
        }

        protected void AfterSutCreated(Action<TContract> sutPostProcessor)
        {
            SutFactory.AfterSutCreated(sutPostProcessor);
        }

        public TDependency Dependency<TDependency>()
        {
            return SutFactory.Dependency<TDependency>();
        }

        public DoForDependency<TDependency> ForDependency<TDependency>()
        {
            return SutFactory.ForDependency<TDependency>();
        }
    }

    public abstract class BaseUnitTester<TSystemUnderTest> : BaseUnitTester<TSystemUnderTest, TSystemUnderTest> where TSystemUnderTest : class { }
}