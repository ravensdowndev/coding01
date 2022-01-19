using SpreadyMcSpreader.Abstractions;
using SpreadyMcSpreader.Models;
using System;
using System.Linq;

namespace SpreadyMcSpreader.Concrete
{
    /// <summary>
    /// Concrete class that recevies a string containing fertilizer data and determines how much is overspread and underspread.
    /// Results from the calculator is then delegated to the iResultDisplayer.
    /// </summary>
    public class McSpreaderCalculator : IMcSpreaderCalculator
    {
        public const int CommandLineInputSections = 3;
        public const string CommandLineInputSectionsSeparator = "|";
        public const string CommandLineInputRowSeparator = ";";

        private readonly IResultDisplayer _resultDisplayer;

        public McSpreaderCalculator(IResultDisplayer resultDisplayer)
        {
            _resultDisplayer = resultDisplayer;
        }

        /// <summary>
        /// Receives the command line input string and calculates the overspread and underspread. Results are passed onto the IResultDisplayer.
        /// </summary>
        /// <param name="commandLineInputs"></param>
        public void CalculateAndDisplay(string commandLineInputs)
        {
            var fertilizerSpreaderModel = ConvertInputs(commandLineInputs);

            var layout = fertilizerSpreaderModel.Layout;
            var remainingFertilizer = fertilizerSpreaderModel.RemainingFertilizer;
            var initialFertilizer = fertilizerSpreaderModel.InitialFertilizer;
            var previousRemaining = initialFertilizer;

            var overSpreadCells = 0;
            var underSpeadCells = 0;
            var layoutRows = layout.GetLength(0);
            var layoutCols = layout.GetLength(1);

            for (int row = 0; row < layoutRows; row++)
            {
                for (int col = 0; col < layoutCols; col++)
                {
                    var currentRemaining = remainingFertilizer[row, col];
                    var expectedFertilizerUsed = layout[row, col];
                    var fertilizerused = previousRemaining - currentRemaining;

                    if (fertilizerused < 0)
                        throw new ArgumentException("Invalid remaining fertilizer entries");

                    if (fertilizerused > expectedFertilizerUsed)
                        overSpreadCells++;
                    else if (fertilizerused < expectedFertilizerUsed)
                        underSpeadCells++;

                    previousRemaining = currentRemaining;
                }
            }

            _resultDisplayer.Display(new FertilizerCalculatorModel(layoutRows * layoutCols, overSpreadCells, underSpeadCells));
        }

        /// <summary>
        /// Validates the command line inputs and transforms it into an object
        /// </summary>
        /// <param name="commandLineInputs">A string composed of layout info, initial fertilizer, spread of fertilizer on layout</param>
        /// <returns>An object containing fertilizer layout and spread information</returns>
        private FertilizerSpreaderModel ConvertInputs(string commandLineInputs)
        {
            if (string.IsNullOrWhiteSpace(commandLineInputs))
                throw new ArgumentNullException("Inputs must be provided");

            var sections = commandLineInputs.Split(CommandLineInputSectionsSeparator, StringSplitOptions.TrimEntries);
            if (sections.Length != CommandLineInputSections)
                throw new ArgumentException($"Must provide {CommandLineInputSections} sections separated by '{CommandLineInputSectionsSeparator}'");

            var initialFertilizer = Convert.ToInt32(sections[1]);
            if (initialFertilizer < 0)
                throw new ArgumentException("Cannot accept negative fertilizer");

            var layout = ConvertLayoutInputs(sections[0]);
            var remainingFertilizer = ConvertRemainingFertInputs(sections[2]);
            if (layout.Length != remainingFertilizer.Length)
                throw new ArgumentException("size of layout must match size of remaining fertilizer");

            return new FertilizerSpreaderModel(layout, initialFertilizer, remainingFertilizer);
        }

        /// <summary>
        /// Validates the input string and transforms it into a 2D array
        /// </summary>
        /// <param name="layout">A string of 0 and 1s corresponding expected fertilizer distribution</param>
        /// <returns>2D array of expected fertilizer distribution</returns>
        private int[,] ConvertLayoutInputs(string layout)
        {
            if (string.IsNullOrWhiteSpace(layout))
                throw new ArgumentNullException("Cannot read empty layout");

            var layoutRows = layout.Split(CommandLineInputRowSeparator, StringSplitOptions.TrimEntries);
            var layoutSize = layoutRows.Length;

            var layout2DMatrix = new int[layoutSize, layoutSize];
            foreach (var item in layoutRows.Select((layoutRow, row) => (layoutRow, row)))
            {
                var layoutRow = item.layoutRow;
                var row = item.row;

                if (layoutRow.Length != layoutSize)
                    throw new ArgumentException("First section must be a square map with consistent rows and columns");

                for (var col = 0; col < layoutSize; col++)
                {
                    layout2DMatrix[row, col] = (int)Char.GetNumericValue(layoutRow[col]);
                }
            }

            return layout2DMatrix;
        }

        /// <summary>
        /// Validates the input string and transforms it into a 2D array
        /// </summary>
        /// <param name="layout">A string of numerical data that corresponds to the spread of the fertilizer</param>
        /// <returns>2D array of fertilizer spread</returns>
        private int[,] ConvertRemainingFertInputs(string remainingFert)
        {
            if (string.IsNullOrWhiteSpace(remainingFert))
                throw new ArgumentNullException("Cannot read empty remaining fertilizer");

            var remainingFertRows = remainingFert.Split(CommandLineInputRowSeparator, StringSplitOptions.TrimEntries);
            var remainingFertSize = remainingFertRows.Length;

            var remainingFert2DMatrix = new int[remainingFertSize, remainingFertSize];
            var rowSize = remainingFertRows[0].Length;

            // Remaining fertilizer data is formatted as such: "09090906",
            // this could be interpreted as "0,9,0,9,0,9,0,6" or "09,09,09,06" or "0909,0906".
            // To figure this out, we divide the number of characters in the row by the size of the layout.
            // So for a 4x4 layout, the row above would be read as "09,09,09,06" so every 2 characters represents a cell distribution.
            var numOfDigits = Math.DivRem(rowSize, remainingFertSize, out int remainder);
            if (remainder != 0)
                throw new ArgumentException("Invalid entries for remaining fertilizer");

            foreach (var item in remainingFertRows.Select((remainingFertRow, row) => (remainingFertRow, row)))
            {
                var remainingFertRow = item.remainingFertRow;
                var row = item.row;

                if (remainingFertRow.Length != rowSize)
                    throw new ArgumentException("Invalid entries for remaining fertilizer");

                for (int col = 0; col < remainingFertSize; col++)
                {
                    remainingFert2DMatrix[row, col] = Convert.ToInt32(remainingFertRow.Substring(col * numOfDigits, numOfDigits));
                }
            }

            return remainingFert2DMatrix;
        }
    }
}
