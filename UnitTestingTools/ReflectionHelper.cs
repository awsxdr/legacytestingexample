// ReSharper disable PossibleNullReferenceException
namespace UnitTestingTools
{
    using System.Linq;
    using System.Reflection;

    /// <summary>
    /// Contains helper methods for using reflection in unit tests
    /// </summary>
    public static class ReflectionHelper
    {
        /// <summary>
        /// Sets the value of a private field.
        /// </summary>
        /// <typeparam name="TTarget">The type of the target.</typeparam>
        /// <param name="fieldName">Name of the field.</param>
        /// <param name="setOn">The object to set the value on.</param>
        /// <param name="value">The value.</param>
        public static void SetPrivateField<TTarget>(
            string fieldName,
            TTarget setOn,
            object value)
            =>
                typeof(TTarget)
                    .GetField(fieldName, BindingFlags.NonPublic | BindingFlags.Instance | BindingFlags.Static)
                    .SetValue(setOn, value);

        /// <summary>
        /// Invokes a private method.
        /// </summary>
        /// <typeparam name="TTarget">The type of the target.</typeparam>
        /// <param name="methodName">Name of the method to invoke.</param>
        /// <param name="invokeOn">The object to invoke the method on.</param>
        /// <param name="parameters">The parameters.</param>
        /// <returns>The return value of the method.</returns>
        public static object InvokePrivateMethod<TTarget>(
            string methodName,
            TTarget invokeOn,
            params object[] parameters)
            =>
                typeof(TTarget)
                    .GetMethod(
                        methodName,
                        BindingFlags.NonPublic | BindingFlags.Instance | BindingFlags.Static,
                        null,
                        parameters.Select(x => x.GetType()).ToArray(),
                        null)
                    .Invoke(invokeOn, parameters);
    }
}