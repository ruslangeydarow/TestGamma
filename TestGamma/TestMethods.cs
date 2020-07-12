using System;
using System.Collections.Generic;
using System.Linq;

namespace TestGamma
{
    partial class Program
    {
        static void TestExtendRight()
        {
            //Assign
            int[] prices1 = { 10, 8, 5, 4, 3, 2, 1 };
            int[] prices2 = { 4, 8, 10, 20, 50 };
            int[] prices3 = { 1, 2, 3, 4, 5, 4, 3, 2, 1, 2, 10, 11, 11, 11, 11, 12, 13, 1 };
            int[] prices4 = { 20, 5, 3, 2, 8, 10, 17, 42, 3, 8, 8, 5, 5, 4, 2, 1, 1 };
            int[] prices5 = { 10, 10, 10, 10, 10, 10, 20, 10, 10, 10, 20, 20, 20, 20, 20, 10, 10, 10 };
            int[][] arrays = { prices1, prices2, prices3, prices4, prices5 };
            bool[] resultArray = new bool[arrays.Length];
            bool[] expectedArray = { false, false, true, true, false };

            //Act
            for (int k = 0; k < arrays.Length; k++)
            {
                Console.WriteLine("Test {0}", k + 1);
                List<Model[]> Branches = GetAllBranches(arrays[k]);
                ParseBigBranches(Branches);
                ShowBranches(Branches);
                while (ExtendRight(Branches))
                {
                    resultArray[k] = true;
                    ParseBigBranches(Branches);
                    ShowBranches(Branches);
                }
                Console.WriteLine();
            }

            //Assert
            Console.WriteLine("Test results:");
            for (int i = 0; i < resultArray.Length; i++)
                Console.WriteLine("Expected: {0}; Observed: {1}", expectedArray[i], resultArray[i]);
        }

        static void TestExtendLeft()
        {
            //Assign
            int[] prices1 = { 10, 8, 5, 4, 3, 2, 1 };
            int[] prices2 = { 4, 8, 10, 20, 50 };
            int[] prices3 = { 1, 2, 3, 4, 5, 4, 3, 2, 1, 2, 10, 11, 11, 11, 11, 12, 13, 1 };
            int[] prices4 = { 20, 5, 3, 2, 8, 10, 17, 42, 3, 8, 8, 5, 5, 4, 2, 1, 1 };
            int[] prices5 = { 10, 10, 10, 10, 10, 10, 20, 10, 10, 10, 20, 20, 20, 20, 20, 10, 10, 10 };
            int[][] arrays = { prices1, prices2, prices3, prices4, prices5 };
            bool[] resultArray = new bool[arrays.Length];
            bool[] expectedArray = { false, false, true, true, true };
           
            //Act
            for (int k = 0; k < arrays.Length; k++)
            {
                Console.WriteLine("Test {0}", k+1);
                List<Model[]> Branches = GetAllBranches(arrays[k]);
                ParseBigBranches(Branches);
                ShowBranches(Branches);
                while(ExtendLeft(Branches))
                {
                    resultArray[k] = true;
                    ParseBigBranches(Branches);
                    ShowBranches(Branches);
                }
                Console.WriteLine();
            }

            //Assert
            Console.WriteLine("Test results:");
            for (int i = 0; i < resultArray.Length; i++)
                Console.WriteLine("Expected: {0}; Observed: {1}", expectedArray[i], resultArray[i]);
        }

        static void TestFindBranchWithMaxLeftMinRight()
        {
            //Assign
            int[] prices1 = { 10, 8, 5, 4, 3, 2, 1 };
            int[] prices2 = { 4, 8, 10, 20, 50 };
            int[] prices3 = { 1, 2, 3, 4, 5, 4, 3, 2, 1, 2, 10, 11, 11, 11, 11, 12, 13, 1 };
            int[] prices4 = { 20, 5, 3, 2, 8, 10, 17, 42, 3, 8, 8, 5, 5, 4, 2, 1, 1 };
            int[] prices5 = { 10, 10, 10, 10, 10, 10, 20, 10, 10, 10, 20, 20, 20, 20, 20, 10, 10, 10 };
            int[][] arrays = { prices1, prices2, prices3, prices4, prices5 };
            int[] resultArrayMaxLeft = new int[arrays.Length];
            int[] expectedArrayMaxLeft = { -1, -1, 6, 3, 4 };
            int[] resultArrayMinRight = new int[arrays.Length];
            int[] expectedArrayMinRight = { -1, -1, -1, 2, -1 };

            //Act
            for (int k = 0; k < arrays.Length; k++)
            {
                List<Model[]> Branches = GetAllBranches(arrays[k]);
                ParseBigBranches(Branches);
                ShowBranches(Branches);
                resultArrayMaxLeft[k] = FindBranchWithMaxLeft(Branches, FindSingleMin(Branches));
                Branches = GetAllBranches(arrays[k]);
                ParseBigBranches(Branches);
                int singleMax = FindSingleMax(Branches);
                resultArrayMinRight[k] = FindBranchWithMinRight(Branches, ref singleMax);
            }

            //Assert
            for (int i = 0; i < resultArrayMaxLeft.Length; i++)
            {
                Console.WriteLine("Test {0}", i + 1);
                Console.WriteLine("Position of Minimal Single expected: {0}; observed: {1}", expectedArrayMaxLeft[i], resultArrayMaxLeft[i]);
                Console.WriteLine("Position of Maximal Single expected: {0}; observed: {1}", expectedArrayMinRight[i], resultArrayMinRight[i]);
            }
        }

        static void TestFindBranchWithMinRight()//Additional test of recursive method to check states of sent by ref parameter
        {
            int[] prices1 = { 5, 4 , 3 ,10, 15, 6 };
            int[] prices2 = { 8, 6, 9, 10, 9 };
            int[] prices3 = { 100, 20, 30, 40, 50 };
            int[] prices4 = { 5, 2, 1, 4, 5, 4, 3, 2, 1, 2, 10, 11, 11, 11, 11, 12, 13, 4 };
            int[] prices5 = { 5, 2, 2, 4, 5, 4, 3, 2, 1, 2, 10, 11, 11, 11, 11, 12, 13, 6 };
            int[][] arrays = { prices1, prices2, prices3, prices4, prices5 };
            int[] resultArray = new int[arrays.Length];
            int[] expectedArray = { -1, -1, -1, 2, -1 };
            int[] singleRights = new int[arrays.Length];

            //Act
            for (int k = 0; k < arrays.Length; k++)
            {
                List<Model[]> Branches = GetAllBranches(arrays[k]);
                ParseBigBranches(Branches);
                ShowBranches(Branches);
                singleRights[k] = FindSingleMax(Branches);
                resultArray[k] = FindBranchWithMinRight(Branches, ref singleRights[k]);
            }

            //Assert
            for (int i = 0; i < resultArray.Length; i++)
            {
                Console.WriteLine("Test {0}", i + 1);
                Console.WriteLine("Position of Maximal Single expected: {0}; observed: {1}", expectedArray[i], resultArray[i]);
                Console.WriteLine("Parameter: {0}", singleRights[i]);
                Console.WriteLine();
            }


        }

        static void TestFindSingleMinMax()
        {
            //Assign
            int[] prices1 = { 10, 8, 5, 4, 3, 2, 1 };
            int[] prices2 = { 4, 8, 10, 20, 50 };
            int[] prices3 = { 1, 2, 3, 4, 5, 4, 3, 2, 1, 2, 10, 11, 11, 11, 11, 12, 13, 1 };
            int[] prices4 = { 20, 17, 16, 15, 8, 10, 17, 42, 3, 8, 8, 5, 5, 4, 2, 1, 1 };
            int[] prices5 = { 10, 10, 10, 10, 10, 10, 20, 10, 10, 10, 10, 20, 20, 20, 20, 10, 10, 10 };
            int[][] arrays = { prices1, prices2, prices3, prices4, prices5 };
            int[] resultArrayMin = new int[arrays.Length];
            int[] expectedArrayMin = { 6, 0, 0, 3, 0};
            int[] resultArrayMax = new int[arrays.Length];
            int[] expectedArrayMax = { 0, 0, 11, 9, 8 };

            //Act
            for (int k = 0; k < arrays.Length; k++)
            {
                List<Model[]> Branches = GetAllBranches(arrays[k]);
                ParseBigBranches(Branches);
                ShowBranches(Branches);
                resultArrayMin[k] = FindSingleMin(Branches);
                resultArrayMax[k] = FindSingleMax(Branches);
            }

            //Assert
            for (int i = 0; i < resultArrayMin.Length; i++)
            {
                Console.WriteLine("Test {0}", i+1);
                Console.WriteLine("Position of Minimal Single expected: {0}; observed: {1}", expectedArrayMin[i], resultArrayMin[i]);
                Console.WriteLine("Position of Maximal Single expected: {0}; observed: {1}", expectedArrayMax[i], resultArrayMax[i]);
            }
        }

        static void TestParsingBigBrancshes()
        {
            //Assign
            int[] prices1 = { 10, 8, 5, 4, 3, 2, 1 };
            int[] prices2 = { 4, 8, 10, 20, 50 };
            int[] prices3 = { 1, 2, 3, 4, 5, 4, 3, 2, 1, 2, 10, 11, 11, 11, 11, 12, 13, 1 };
            int[] prices4 = { 20, 17, 16, 15, 8, 10, 17, 42, 3, 8, 8, 5, 5, 4, 2, 1, 1 };
            int[] prices5 = { 10, 10, 10, 10, 10, 10, 20, 10, 10, 10, 10, 20, 20, 20, 20, 10, 10, 10 };
            int[][] arrays = { prices1, prices2, prices3, prices4, prices5 };



            //Act&ASsert
            for (int k = 0; k < arrays.Length; k++)
            {
                Console.WriteLine("Test {0}", k + 1);
                List<Model[]> Branches = GetAllBranches(arrays[k]);
                ShowBranches(Branches);
                Console.WriteLine();
                Console.WriteLine("Was parsed: {0}", ParseBigBranches(Branches));
                ShowBranches(Branches);
                Console.WriteLine();

                Console.WriteLine("Return positions");
                Model[] returnPositions = BranchesToArray(Branches);
                returnPositions = returnPositions.OrderBy(x => x.Index).ToArray();
                foreach(var m in returnPositions)
                    Console.Write("{0} ", m.Value);
                Console.WriteLine();
                foreach (var m in returnPositions)
                    Console.Write("{0} ", m.Index);
                Console.WriteLine();

                Console.WriteLine();
                Console.WriteLine("Compare Arrays");
                bool sameLength = arrays[k].Length == returnPositions.Length;
                Console.WriteLine("Same length? {0}", sameLength);
                if (sameLength)
                {
                    for (int i = 0; i < arrays[k].Length; i++)
                    {
                        if (arrays[k][i] != returnPositions[i].Value)
                            sameLength = false;
                    }
                    Console.WriteLine("Same Values? {0}", sameLength);
                    Console.WriteLine();
                }
                Console.WriteLine();
            }
        }

        static void TestGettingAllBranches()
        {
            //Assign
            int[] prices1 = { 10, 8, 5, 4, 3, 2, 1 };
            int[] prices2 = { 4, 8, 10, 20, 50 };
            int[] prices3 = { 1, 2, 3, 4, 5, 4, 3, 2, 1, 2, 10, 11, 11, 11, 11, 12, 13, 1 };
            int[] prices4 = { 20, 17, 16, 15, 8, 10, 17, 42, 3, 8, 8, 5, 5, 4, 2, 1, 1 };
            int[] prices5 = { 10, 10, 10, 10, 10, 10, 20, 10, 10, 10, 10, 20, 20, 20, 20, 10, 10, 10 };
            int[][] arrays = { prices1, prices2, prices3, prices4, prices5 };
            int[] expectables = { 7, 1, 6, 10, 3 };

            //Act&Assert
            for(int k = 0; k< expectables.Length;k++)
            {
                Console.WriteLine();
                Console.WriteLine("Test {0}", k+1);
                foreach (int i in arrays[k])
                    Console.Write("{0} ", i);
                Console.WriteLine();
                List<Model[]> Branches = GetAllBranches(arrays[k]);
                ShowBranches(Branches);
                Console.WriteLine("Length Expected: {0}; Observed: {1}",expectables[k], Branches.Count);
            }
        }
    }
}
