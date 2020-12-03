using System;
namespace MyNUnit.Attributes
{
    /// <summary>
    /// Attribute of method that executes after all tests in the class
    /// </summary>
    [AttributeUsage(AttributeTargets.Method, Inherited = false)]
    public class AfterClassAttribute : Attribute
    {
    }
}
