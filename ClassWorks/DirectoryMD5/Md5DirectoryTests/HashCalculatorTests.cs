using NUnit.Framework;
using DirectoryMD5;

namespace Md5DirectoryTests
{
    public class Tests
    {
        private const string path = "../../../../TestDirectory";

        [Test]
        public void ParralelingTest()
        {
            var hash1 = HashCalculator.CalculateCheckSum(path);
            var hash2 = HashCalculator.CalculateCheckSumParralel(path);
            Assert.AreEqual(hash1, hash2);
        }

        [Test]
        public void IndepedentByRelativePathTest()
        {
            var hash1 = HashCalculator.CalculateCheckSum(path);
            var hash2 = HashCalculator.CalculateCheckSum("../../../../HelpDir1/TestDirectory");
            Assert.AreEqual(hash1, hash2);
        }

        [Test]
        public void DependsOnDirNameTest()
        {
            var hash1 = HashCalculator.CalculateCheckSum(path);
            var hash2 = HashCalculator.CalculateCheckSum("../../../../HelpDir4/TestDirectory1");
            Assert.AreNotEqual(hash1, hash2);
        }

    }
}