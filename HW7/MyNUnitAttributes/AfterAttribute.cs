﻿using System;

namespace MyNUnitAttributes
{
    /// <summary>
    /// Attribute of method that executes after every single test
    /// </summary>
    [AttributeUsage(AttributeTargets.Method, Inherited = false)]
    public class AfterAttribute : Attribute
    {
    }
}
