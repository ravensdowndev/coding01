using System;
using System.Linq;

namespace SpreadyMcSpreader
{
  class Program
  {
    static void Main(string[] args)
    {
      //
      // I skipped lots of validation here
      //
      var target = args[0];
      var maps = target?.Split('|');

      if (maps?.Length != 3)
      {
        Console.WriteLine("Invalid input format. Expected 3 sections separated by '|'.");
        return;
      }

      var startingAmount = int.Parse(maps[1]);
      var plannedMap = maps[0].Split(";")
        .SelectMany(str => str.Select(p => int.Parse(p.ToString()))).ToArray();
      var endResult = maps[2].Split(";")
        .SelectMany(str => Enumerable.Range(0, str.Length / 2).Select(i => int.Parse(str.Substring(i * 2, 2))))
        .ToArray();

      //
      // ???
      //
      var totalCells = plannedMap.Length;
      var overSpreadAmount = 10;
      var underSpreadAmount = 3;

      var incorrectCount = 0;
      var overSpreadCount = 0;
      var underSpreadCount = 0;

      for (var i = 0; i < plannedMap.Length; i++)
      {
        var isUnplannedCell = plannedMap[i] == 0;
        var remainingAmount = endResult[i];
        var spentAmount = startingAmount - remainingAmount;
        if (isUnplannedCell)
        {
          if (spentAmount > 0)
            incorrectCount++;
        }
        else
        {
          if (spentAmount >= overSpreadAmount)
            overSpreadCount++;

          if (spentAmount <= underSpreadAmount)
            underSpreadCount++;
        }
      }

      var accuracy = 100 * (double)(totalCells - incorrectCount) / totalCells;
      Console.Write($"{incorrectCount}|{overSpreadCount}|{underSpreadCount}|{accuracy}");
    }
  }
}