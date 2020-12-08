namespace MyNUnit
{
    /// <summary>
    /// Report of an invalid test in assembly
    /// </summary>
    public class InvalidTest
    {
        /// <summary>
        /// Basic constructor
        /// </summary>
        /// <param name="name">Name of the test</param>
        /// <param name="error">Errir in the test</param>
        public InvalidTest(string name, string error)
        {
            Error = error;
        }

        public string Name { get; }

        public string Error { get; }
    }
}
