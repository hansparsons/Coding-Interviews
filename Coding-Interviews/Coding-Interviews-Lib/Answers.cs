using System;
using System.Collections.Generic;
using System.Linq;

namespace Coding_Interviews_Lib
{
    public class Answers
    {

        //****************************************************
        //
        // Write a method that would reverse a string
        //
        //****************************************************

        public static string ReverseString(string inputstring)
        {
            char[] charArray = inputstring.ToCharArray();
            Array.Reverse(charArray);
            return new string(charArray);

        }

        //****************************************************
        //
        // Write a method that would reverse a string 
        // without using any of the C# collections
        //
        //****************************************************

        public static string ReverseStringWithoutCollections(string inputString)
        {
            string reversed = string.Empty;
            for (int i = inputString.Length - 1; i >= 0; i--)
            {
                reversed += inputString[i].ToString();
            }

            return reversed;
        }

        //****************************************************
        //
        // Write a method that would reverse the words in  
        // a string
        //****************************************************

        public static string ReverseWords(string inputString)
        {
            string[] reversedString = inputString.Split(' ');
            Array.Reverse(reversedString);

            return string.Join(" ", reversedString);

        }

        //****************************************************
        //
        // Write a method that would reverse the words in 
        // a string without using any of the C# collections
        // and handle punctuation correctly
        //
        //****************************************************

        public static string ReverseWordsWithoutCollections(string inputString)
        {
            string reversedString = String.Empty;
            string word = String.Empty;
            string punctuation = String.Empty;

            List<string> stringList = new List<string>();

            for (int i = 0; i <= inputString.Length - 1; i++)
            {
                // if the char equal space, we have the end of a word - add that word to the list
                if (inputString[i] == ' ')
                {
                    stringList.Add(word);
                    word = String.Empty;
                }
                //if the char equal punctuation, add to punctuation
                else if (inputString[i] == '!' || inputString[i] == '.' || inputString[i] == '?')
                {
                    if (word.Length != 0) //this will handle the case where a sentence has more then one puctuation mark e.g. !!!
                    {
                        stringList.Add(word);
                        word = String.Empty; // we need to empty the word so that we only add the last word once
                    }
                    punctuation += inputString[i];
                }
                //we have a char that we need to add to build the word
                else
                {
                    word += inputString[i];
                }
            }

            stringList.Reverse();

            reversedString = string.Join(" ", stringList) + punctuation;

            return reversedString;

        }

        //****************************************************
        //
        // Write a method that would reverse the words in  
        // a string and handle puctuation correctly
        //****************************************************

        public static string ReverseWords2(string inputString)
        {
            //create string with only the words 
            List<string> onlyWords = new List<string>();
            List<string> onlyPuctuation = new List<string>();
            //string reversedString = String.Empty;
            char[] stringSeperator = { ',', '.', '!', '?', ';', ':', ' ' };
            string[] stringArray = inputString.Split(stringSeperator);
            inputString.Split();

            for (int i = 0; i <= stringArray.Count() - 1; i++)
            {
                if (stringArray[i].Contains('!') || stringArray[i].Contains('.') || stringArray[i].Contains('?'))
                {
                    onlyPuctuation.Add(stringArray[i]);
                }
                else
                {
                    onlyWords.Add(stringArray[i]);

                }

            }
            onlyWords.Reverse();

            string reversedString = String.Concat(onlyWords, onlyPuctuation);

            return reversedString;

        }

        //****************************************************
        //
        // Write a method that will find all the sum combinations  
        // in an int array that that equal to a given sum
        //
        //****************************************************

        public static void FindSum(int[] array, int sum)
        {
            for (int i = 0; i <= array.Count() - 1; i++)
                if (array.Contains(sum - array[i]))
                    Console.WriteLine("{0}, {1}", array[i], sum - array[i]);
        }

        //****************************************************
        //
        // Write a method that will find all the sum combinations  
        // in an int array that that equal to a given sum
        // without using any c# collections
        //
        //****************************************************

        public static void FindSumWithoutCollections(int[] intarray, int sum)
        {
            for (int i = 0; i <= intarray.Count() - 1; i++)
            {
                for (int j = 0; j <= intarray.Count() - 1; j++)
                {
                    if ((intarray[i] + intarray[j] == sum) && (i != j))
                    {
                        Console.WriteLine("{0}, {1}", intarray[i], intarray[j]);
                    }

                }

            }

        }

        //****************************************************
        //
        // Write a method that will add up all the integers in 
        // matrix array
        //
        //****************************************************

        public static int SumMatrix(int[,] matrix)
        {
            int sum = 0;

            for (int i = 0; i < matrix.GetLength(0); i++)
            {
                for (int j = 0; j < matrix.GetLength(1); j++)
                {
                    sum += matrix[i, j];
                }
            }
            return sum;
        }

        //****************************************************
        //
        // Write a method that will find all duplicate integers 
        // in two arrays
        //
        //****************************************************

        public static void FindDuplicateIntegers()
        {
            int[] firstSet = { 1, 2, 3, 4, 5 };
            int[] secondSet = { 3, 4, 7, 8 };

            foreach (int var1 in firstSet)
            {
                foreach (int var2 in secondSet)
                {
                    if (var1 == var2)
                    {
                        Console.WriteLine("{0}", var1);
                    }
                }
            }
        }

        //****************************************************
        //
        // Write a method that will find all duplicate strings 
        // in two arrays
        //
        //****************************************************

        public static void FindDuplicateStrings()
        {
            HashSet<string> firstHashSet = new HashSet<string> { };
            HashSet<string> secondHashSet = new HashSet<string> { };

            firstHashSet.Add("Hello");
            firstHashSet.Add("GoodBye");

            secondHashSet.Add("Hello");
            secondHashSet.Add("Sam");

            foreach (string var3 in firstHashSet)
            {
                if (secondHashSet.Contains(var3))
                    Console.WriteLine("{0}", var3);
            }
        }

        //****************************************************
        //
        // Write a method that will remove all duplicate  
        // integers in an integer array
        //
        //****************************************************

        public static int[] RemoveDuplicateIntegers(int[] intArray)
        {

            var result = new List<int>();
            foreach (var item in intArray)
            {
                bool found = false;
                foreach (var resultItem in result)
                {
                    if (resultItem == item) found = true;
                }

                if (!found)
                {
                    result.Add(item);
                }
            }
            return result.ToArray();
        }


    }
}
