using System;

namespace MyNUnit.Attributes
{
    [AttributeUsage(AttributeTargets.Method, Inherited = false)]
    class TestAttribute: Attribute
    {
        public Type Expected { get; }

        public string Ignore { get; }
    }
}
