using ExpectedObjects;
using Moq;
using NUnit.Framework;
using SpreadyMcSpreader.Abstractions;
using SpreadyMcSpreader.Concrete;
using SpreadyMcSpreader.Models;
using System;
using System.Collections.Generic;

namespace SpreadyMcSpreaderTests.Concrete
{
    [TestFixture]
    public class McSpreaderCalculatorTests
    {
        private McSpreaderCalculator _mcSpreaderCalculator;
        private Mock<IResultDisplayer> _resultDisplayerMock;

        [SetUp]
        public void SetUp()
        {
            _resultDisplayerMock = new Mock<IResultDisplayer>();
            _mcSpreaderCalculator = new McSpreaderCalculator(_resultDisplayerMock.Object);
        }

        [TestCase("0111;0101;1111;0011|12|12111009;09090906;06060504;04040302", 16, 1, 3)]
        [TestCase("1111;0000;1111;0000|20|19181716;16161616;15141312;12121212", 16, 0, 0)]
        [TestCase("11;10|9|0604;0301", 4, 3, 0)]
        [TestCase("11;10|9|64;31", 4, 3, 0)]
        [TestCase("101;010;110|30|302929;282828;282720", 9, 3, 4)]
        [TestCase("101;010;110|30|003000290029;002800280028;002800270020", 9, 3, 4)]
        [TestCase("1|30|29", 1, 0, 0)]
        [TestCase("1|30|30", 1, 0, 1)]
        public void WhenValidInputsAreProvided_CalculateAndDisplay_CalculatesExpectedOverspreadAndUnderspreadCellsAndDisplaysResult(string input, int expectedTotalCells, int expectedTotalOverspreadCells, int expectedTotalUnderspreadCells)
        {
            _mcSpreaderCalculator.CalculateAndDisplay(input);

            var expectedResult = new FertilizerCalculatorModel(expectedTotalCells, expectedTotalOverspreadCells, expectedTotalUnderspreadCells);

            _resultDisplayerMock.Verify(x => x.Display(It.Is<FertilizerCalculatorModel>(y => expectedResult.ToExpectedObject().Equals(y))), Times.Once());
        }

        private static IEnumerable<TestCaseData> invalidInputsTestData
        {
            get
            {
                yield return new TestCaseData("", typeof(ArgumentNullException));
                yield return new TestCaseData("1|1|1|1|1|1", typeof(ArgumentException));
                yield return new TestCaseData("1|1", typeof(ArgumentException));
                yield return new TestCaseData("1b|30|30", typeof(ArgumentException));
                yield return new TestCaseData("1|red|30", typeof(FormatException));
                yield return new TestCaseData("1|30|30;blue", typeof(ArgumentException));
                yield return new TestCaseData("01111;0101;1111;0011|12|12111009;09090906;06060504;04040302", typeof(ArgumentException));
                yield return new TestCaseData("0111;0101;1111;0011|12|12111009;09090906;06060504;0404030201", typeof(ArgumentException));
                yield return new TestCaseData("0111;0101;1111;0011|12|12131009;09090906;06060504;04040302", typeof(ArgumentException));
                yield return new TestCaseData("0111;0101;0011|12|12111009;09090906;06060504;0404030201", typeof(ArgumentException));
                yield return new TestCaseData("0111;0101;1111;0011|12|12111009;09090906;06060504;0404", typeof(ArgumentException));
                yield return new TestCaseData("0111;0101;1111;0011|5|12111009;09090906;06060504;04040302", typeof(ArgumentException));
                yield return new TestCaseData("0111;0101;1111;0011|12|121110;090909;060605", typeof(ArgumentException));
                yield return new TestCaseData("|12|121110;090909;060605", typeof(ArgumentNullException));
                yield return new TestCaseData("0111;0101;1111;0011|12|", typeof(ArgumentNullException));
            }
        }

        [TestCaseSource(nameof(invalidInputsTestData))]
        public void WhenInvalidInputsAreProvided_CalculateAndDisplay_ThrowsAnException(string input, Type expectedException)
        {
            Assert.Throws(Is.TypeOf(expectedException),
                          () => _mcSpreaderCalculator.CalculateAndDisplay(input));
        }
    }
}
