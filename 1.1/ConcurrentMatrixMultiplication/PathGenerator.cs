using System;

namespace ConcurrentMatrixMultiplication
{
    public static class PathGenerator
    {
        public static string GetPathByName(string name) => Environment.CurrentDirectory.TrimEnd(@"\bin\Debug\netcoreapp3.1".ToCharArray()) + $@"ication\{name}";
    }
}
