using System;

namespace MyNUnitAttributes
{
    /// <summary>
    /// Attribute of method that executes before all tests in the class
    /// </summary>
    [AttributeUsage(AttributeTargets.Method, Inherited = false)]
    public class BeforeClassAttribute : Attribute
    {
    }
}
