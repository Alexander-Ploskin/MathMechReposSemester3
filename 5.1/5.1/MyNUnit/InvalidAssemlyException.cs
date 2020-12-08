using System;
using System.Collections.Generic;

namespace MyNUnit
{
    /// <summary>
    /// Throws when test assembly is not clear
    /// </summary>
    public class InvalidAssemlyException : SystemException
    {
        /// <summary>
        /// Basic constructor
        /// </summary>
        /// <param name="invalidClasses">Invalid class reports for all invalid test classes</param>
        public InvalidAssemlyException(IEnumerable<InvalidTestClassReport> invalidClasses)
        {
            this.invalidClasses = invalidClasses;
        }

        public readonly IEnumerable<InvalidTestClassReport> invalidClasses;
    }
}
