using NUnit.Framework;
using Coding_Interviews_Lib;

namespace UnitTests
{
    public class Tests
    {
        [TestFixture]
        public class UnitTests
        {
            [Test]
            public void MatrixTest()
            {
                int[,] matrix = { { 1, 2, 3 }, { 4, 5, 6 }, { 7, 8, 9 }, { 10, 11, 12 } };
                Assert.AreEqual(Answers.SumMatrix(matrix), 78);
            }
        }
    }
}
