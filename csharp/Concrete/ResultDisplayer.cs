using SpreadyMcSpreader.Abstractions;
using SpreadyMcSpreader.Models;
using System;

namespace SpreadyMcSpreader.Concrete
{
    /// <summary>
    /// Concrete class for displaying results from calculator. Writes results to the console.
    /// </summary>
    public class ResultDisplayer : IResultDisplayer
    {
        public const char ResultSeparator = '|';

        public void Display(FertilizerCalculatorModel fertilizerCalculatorModel)
        {
            var incorrectCells = fertilizerCalculatorModel.TotalIncorrectCells;
            var overSpreadCells = fertilizerCalculatorModel.TotalOverspreadCells;
            var underSpreadCells = fertilizerCalculatorModel.TotalUnderspreadCells;
            var percentageAccuracy = fertilizerCalculatorModel.PercentCorrectlySpread;

            Console.WriteLine($"{incorrectCells}{ResultSeparator}{overSpreadCells}{ResultSeparator}{underSpreadCells}{ResultSeparator}{percentageAccuracy}");
        }
    }
}
