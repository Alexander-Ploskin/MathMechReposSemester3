using System;

namespace MyNUnitAttributes
{
    /// <summary>
    /// Attribute of method that executes before every single test
    /// </summary>
    [AttributeUsage(AttributeTargets.Method, Inherited = false)]
    public class BeforeAttribute : Attribute
    {
    }
}
