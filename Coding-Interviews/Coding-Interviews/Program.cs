using Coding_Interviews_Lib;

namespace Coding_Interviews
{
    class Program
    {
        static void Main(string[] args)
        {


            string ls = Answers.ReverseString("home");
            string hj = Answers.ReverseStringWithoutCollections("home");

            string ab = Answers.ReverseWords("This is my home!!!");

            string xy = Answers.ReverseWords2("This is my home!!!");

            string iop = Answers.ReverseWordsWithoutCollections("This is my home??");

            int[] tempIntArray = new int[] { 1, 3, 4, 7, 9 };
            int tempInt = 6;

            Answers.FindSum(tempIntArray, tempInt);
            Answers.FindSumWithoutCollections(tempIntArray, tempInt);

            int[,] matrix = { { 1, 2, 3 }, { 4, 5, 6 }, { 7, 8, 9 }, { 10, 11, 12 } };
            int sum = Answers.SumMatrix(matrix);

            Answers.FindDuplicateIntegers();
            Answers.FindDuplicateStrings();

            int[] dupArray = { 1, 2, 3, 3, 4, 5, };
            var array = Answers.RemoveDuplicateIntegers(dupArray);


        }

    }
}
