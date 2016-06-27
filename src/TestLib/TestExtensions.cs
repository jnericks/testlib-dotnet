using System;
using NSubstitute;

namespace jnericks.TestLib
{
    public static class TestExtensions
    {
        public static object Default(this Type type)
        {
            return typeof(GenericHelper).GetMethod(nameof(GenericHelper.GetDefaultGeneric))
                                        .MakeGenericMethod(type)
                                        .Invoke(new GenericHelper(), null);
        }

        public static object Fake(this Type type)
        {
            return Substitute.For(new[] { type }, null);
        }

        class GenericHelper
        {
            public static T GetDefaultGeneric<T>()
            {
                return default(T);
            }
        }
    }
}