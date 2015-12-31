using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace jnericks.TestLib
{
    public class SystemUnderTestFactory<TSystemUnderTest> : SystemUnderTestFactory<TSystemUnderTest, TSystemUnderTest> where TSystemUnderTest : class { }

    public class SystemUnderTestFactory<TContract, TSystemUnderTest> where TSystemUnderTest : class, TContract
    {
        Func<TSystemUnderTest> _factory;
        Action _preProcess = delegate { };
        Action<TContract> _postProcess = delegate { };

        ConstructorInfo _ctor;
        IList<Tuple<Type, object>> _parameters = new List<Tuple<Type, object>>();

        TContract _sut;
        public TContract Sut
        {
            get
            {
                CreateSut();
                return _sut;
            }
        }

        public SystemUnderTestFactory()
        {
            _ctor = GetGreediestCtor<TSystemUnderTest>();
            _factory = DefaultFactory;
            InjectDefaultDependencies();
        }

        void InjectDefaultDependencies()
        {
            foreach (var type in _ctor.GetParameters().Select(parameterInfo => parameterInfo.ParameterType))
            {
                if (type == typeof(string) || type.IsValueType)
                    _parameters.Add(new Tuple<Type, object>(type, type.Default()));
                else
                    _parameters.Add(new Tuple<Type, object>(type, type.Mock()));
            }
        }

        TSystemUnderTest DefaultFactory()
        {
            return (TSystemUnderTest)_ctor.Invoke(_parameters.Select(x => x.Item2).ToArray());
        }

        ConstructorInfo GetGreediestCtor<T>()
        {
            return typeof(T).GetConstructors()
                            .OrderByDescending(x => x.GetParameters().Count())
                            .First();
        }

        bool DependencyExistsFor<TDependency>()
        {
            return _parameters.Any(x => x.Item1 == typeof(TDependency));
        }

        public void CreateSut()
        {
            if (_sut != null) return;

            _preProcess();
            _sut = _factory();
            _postProcess(_sut);
        }

        public void CreateSutUsing(Func<TSystemUnderTest> factory)
        {
            _factory = factory;
        }

        public void BeforeSutCreated(Action sutPreProcessor)
        {
            if (_sut != null)
                throw new InvalidOperationException(@"Sut has already been created.");

            _preProcess = sutPreProcessor;
        }

        public void AfterSutCreated(Action<TContract> sutPostProcessor)
        {
            if (_sut != null)
                throw new InvalidOperationException(@"Sut has already been created.");

            _postProcess = sutPostProcessor;
        }

        public TDependency Dependency<TDependency>()
        {
            if (typeof(TDependency) == typeof(TSystemUnderTest))
                throw new InvalidOperationException(@"Access Sut through property.");

            if (!DependencyExistsFor<TDependency>())
                throw new InvalidOperationException($@"{typeof(TDependency).Name} is not a depedency of {typeof(TSystemUnderTest).Name}");

            return (TDependency)_parameters.First(x => x.Item1 == typeof(TDependency)).Item2;
        }

        public DoForDependency<TDependency> ForDependency<TDependency>()
        {
            if (!DependencyExistsFor<TDependency>())
                throw new InvalidOperationException($@"{typeof(TDependency).Name} is not a depedency of {typeof(TSystemUnderTest).Name}");

            return new DoForDependency<TDependency>(_parameters);
        }

        public class DoForDependency<TDependency>
        {
            readonly IList<Tuple<Type, object>> _parameters;

            public DoForDependency(IList<Tuple<Type, object>> parameters)
            {
                _parameters = parameters;
            }

            public void Use(TDependency dependency)
            {
                var tuple = _parameters.First(x => x.Item1 == typeof(TDependency));
                var index = _parameters.IndexOf(tuple);
                _parameters.RemoveAt(index);
                _parameters.Insert(index, new Tuple<Type, object>(tuple.Item1, dependency));
            }
        }
    }
}