using System;
using System.Collections.Generic;

namespace SpreadyMcSpreader
{
    class Program
    {
        static void Main(string[] args)
        {
            try
            {
                if (args.Length > 0)
                {
                    string[] sections = args[0].Split('|');

                    if (sections.Length == 3)
                    {
                        string[] mapRows = sections[0].Split(';');
                        int.TryParse(sections[1], out int startingFert);
                        int remainingFert = startingFert;
                        string[] remainingFertMap = sections[2].Split(';');
                        List<int> fertCellMap = new List<int>();
                        List<int> fertFinalCellMap = new List<int>();
                        int numRows = mapRows.Length;
                        int numCellsInRow = numRows > 0 ? mapRows[0].Length : 0;
                        int incorrectCells = 0;
                        int overspreadCells = 0;
                        int underspreadCells = 0;
                        int correctSpreadPercentage = 0;
                        int totalCells = numCellsInRow * mapRows.Length;
                        int maxRows = 9;

                        if (numCellsInRow == numRows)
                        {
                            if (numCellsInRow > 0 && numCellsInRow <= maxRows)
                            {
                                foreach (string row in mapRows)
                                {
                                    if (row.Length == numCellsInRow && row.Length <= maxRows)
                                    {
                                        foreach (char cell in row)
                                        {
                                            int.TryParse(cell.ToString(), out int cellFert);
                                            if (cellFert == 0 || cellFert == 1)
                                            {
                                                fertCellMap.Add(cellFert);
                                            }
                                            else
                                            {
                                                throw new Exception("Starting map cells must contain either a 0 or 1.");
                                            }
                                        }
                                    }
                                    else
                                    {
                                        throw new Exception(string.Format("Map rows are not equal in length or row length is greater than max length ({0}).", maxRows));
                                    }
                                }
                            }
                            else
                            {
                                throw new Exception(string.Format("Row length is zero or greater than max number of rows ({0}).", maxRows));
                            }
                        }
                        else
                        {
                            throw new Exception("Map is not square.");
                        }

                        int cellCount = 0;

                        if (remainingFertMap.Length == mapRows.Length)
                        {
                            foreach (string row in remainingFertMap)
                            {
                                if (row.Length % 2 == 0 && row.Length / 2 == numCellsInRow)
                                {
                                    for (int i = 0; i < row.Length; i += 2)
                                    {
                                        string remCell = row.Substring(i, 2);
                                        int.TryParse(remCell, out int remCellFert);

                                        if (remCellFert > 0 && remCellFert <= remainingFert)
                                        {
                                            int expectedRemFert = (fertCellMap[cellCount] == 1) ? remainingFert - 1 : remainingFert;

                                            if (remCellFert < expectedRemFert)
                                            {
                                                overspreadCells++;
                                                incorrectCells++;
                                            }
                                            else if (remCellFert > expectedRemFert)
                                            {
                                                underspreadCells++;
                                                incorrectCells++;
                                            }

                                            remainingFert = remCellFert;
                                            cellCount++;
                                        }
                                        else
                                        {
                                            throw new Exception("Invalid remaining cell fertliser amount.");
                                        }
                                    }
                                }
                                else
                                {
                                    throw new Exception("Remaining row length is incorrect or is not an even number.");
                                }
                            }

                            correctSpreadPercentage = (int)Math.Ceiling((double)((double)(totalCells - incorrectCells) / (double)totalCells) * 100);

                            Console.WriteLine(string.Format("Incorrectly spread cells = {0}", incorrectCells));
                            Console.WriteLine("Over spread cells = {0}", overspreadCells);
                            Console.WriteLine("Under spread cells = {0}", underspreadCells);
                            Console.WriteLine("Percentage accuracy % = {0}", correctSpreadPercentage);
                        }
                        else
                        {
                            throw new Exception("Remaining fertiliser rows do not equal map rows.");
                        }
                    }
                    else
                    {
                        throw new ArgumentException();
                    }
                }
            }
            catch (ArgumentException)
            {
                Console.WriteLine("Usage: [Fertliser map]|[Starting fertliser units]|[Remaining fertliser units]\n");
                Console.WriteLine("E.g.: 0111;0101;1111;0011|12|12111009;09090906;06060504;04040302");
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
            }
        }
    }
}
