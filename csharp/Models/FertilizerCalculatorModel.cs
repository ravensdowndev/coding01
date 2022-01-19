using System;

namespace SpreadyMcSpreader.Models
{
    public class FertilizerCalculatorModel
    {
        public FertilizerCalculatorModel(int totalCells, int totalOverspreadCells, int totalUnderspeadCells)
        {
            TotalCells = totalCells;
            TotalOverspreadCells = totalOverspreadCells;
            TotalUnderspreadCells = totalUnderspeadCells;
        }

        public int TotalCells { get; }
        public int TotalOverspreadCells { get; }
        public int TotalUnderspreadCells { get; }
        public int TotalIncorrectCells => TotalOverspreadCells + TotalUnderspreadCells;
        public int PercentCorrectlySpread => (int)Math.Round(100f * (TotalCells - TotalIncorrectCells) / TotalCells);
    }
}
