namespace UnitTestingTools
{
    using System;

    public static class FunctionalExtensionMethods
    {
        public static TOut Map<TIn, TOut>(this TIn @this, Func<TIn, TOut> func) =>
            func(@this);
    }
}