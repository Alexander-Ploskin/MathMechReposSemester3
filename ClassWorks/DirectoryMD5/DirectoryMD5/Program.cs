using System;
using System.Collections.Generic;

namespace DirectoryMD5
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("Enter the path of directory to calculate checksum");
            var path = Console.ReadLine();
            Console.WriteLine(BitConverter.ToString(HashCalculator.CalculateCheckSum(path)));
            Console.WriteLine(BitConverter.ToString(HashCalculator.CalculateCheckSumParralel(path)));
            var paths = new List<string>();
            paths.Add("../../../../TestDirectory");
            var expectedTimeOfSequentallyCalculation = EfficiencyAnalyzer.CalculateExpectedTime(HashCalculator.CalculateCheckSum, paths);
            var expectedTimeOfConcurentallyCalculation = EfficiencyAnalyzer.CalculateExpectedTime(HashCalculator.CalculateCheckSumParralel, paths);
            var averageDeviationOfSequentallyCalculation = EfficiencyAnalyzer.CalculateAverageDeviation(HashCalculator.CalculateCheckSum,
                paths, expectedTimeOfSequentallyCalculation);
            var averageDeviationOfConcurrentallyCalculation = EfficiencyAnalyzer.CalculateAverageDeviation(HashCalculator.CalculateCheckSum,
                paths, expectedTimeOfConcurentallyCalculation);
            Console.WriteLine($"Expected time of sequentally calculation: {expectedTimeOfSequentallyCalculation}");
            Console.WriteLine($"Expected time of concutntally calculation: {expectedTimeOfConcurentallyCalculation}");
            Console.WriteLine($"Average deviation of sequentally calculation: {averageDeviationOfSequentallyCalculation}");
            Console.WriteLine($"Average deviation of concurentally calculation: {averageDeviationOfConcurrentallyCalculation}");
        }
    }
}
