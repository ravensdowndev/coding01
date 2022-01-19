using FluentAssertions;
using NUnit.Framework;
using SpreadyMcSpreader.Models;

namespace SpreadyMcSpreaderTests.Models
{
    [TestFixture]
    public class FertilizerCalculatorModelTests
    {
        [TestCase(25, 6, 8, 14, 44)]
        [TestCase(100, 10, 20, 30, 70)]
        [TestCase(9, 6, 3, 9, 0)]
        [TestCase(16, 0, 0, 0, 100)]
        public void FertilizerCalculatorModel_ProvidesCorrectTotalAndPercentages(int totalCells, int overspreadCells, int underspreadCells, int totalIncorrectCells, int percentageOfCorrectCells)
        {
            var fertilizerCalculatorModel = new FertilizerCalculatorModel(totalCells, overspreadCells, underspreadCells);

            fertilizerCalculatorModel.TotalIncorrectCells.Should().Be(totalIncorrectCells);
            fertilizerCalculatorModel.PercentCorrectlySpread.Should().Be(percentageOfCorrectCells);
        }
    }
}
