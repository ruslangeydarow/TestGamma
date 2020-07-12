//#define Test
#define Program
using System;
using System.Collections.Generic;
using System.Linq;

namespace TestGamma
{
    partial class Program
    {
        static void Main()
        {
#if Program
            Console.WriteLine("Use random array [Press R] or enter own [Press O] ?");
            char answer = Console.ReadKey().KeyChar;
            Console.WriteLine();
            if (answer != 'r' && answer != 'o')
            {
                Console.WriteLine("Invalid input. Random array choosed.");
                answer = 'r';
            }
                                    
            while (true)
            {
                try
                {
                    int[] prices = null;
                    if (answer == 'r')
                    {
                        int length = 0;
                        if (!int.TryParse(Console.ReadLine(), out length) || length < 1)
                            throw new IncorrectInputException("You've entered incorrect length");
                        prices = GenerateRandomArray(length);
                    }
                    if(answer == 'o')
                    {
                        Console.WriteLine("Enter array using space [' '] as separator between numbers");
                        prices = StringArrayToInt(Console.ReadLine());
                    }
                    
                    Console.WriteLine("\n-----------");
                    string tactics = GetTactics(prices);
                    Console.WriteLine("For prices:");
                    ShowBranches(GetAllBranches(prices));
                    Console.WriteLine("Tactic is:");
                    Console.WriteLine(tactics);
                    Console.WriteLine();
                    break;
                }
                catch (IncorrectInputException ex)
                {
                    Console.WriteLine("You've entered wrong data. More:\n{0}", ex.Message);
                    Console.WriteLine();
                    Console.WriteLine("To try again press <Y> and enter correct length, otherwise press <N>");
                    answer = Console.ReadKey().KeyChar;
                    Console.WriteLine();
                    if (answer == 'y')
                        continue;
                    else if (answer == 'n')
                        break;
                    else
                    {
                        Console.WriteLine("Invalid input. Application is stopped.");
                        Console.WriteLine();
                        break;
                    }
                }
                catch(FormatException ex)
                {
                    Console.WriteLine("Invalid input. Please, try again.");
                    Console.WriteLine();
                }
                catch(Exception ex)
                {
                    Console.WriteLine("A serious error corrupted. More info: {0}", ex.Message);
                    Console.WriteLine();
                }
            }


#endif

#if Tests
            //TestGettingAllBranches(); //ready
            //TestParsingBigBrancshes(); //ready
            //TestFindSingleMinMax(); //ready
            //TestFindBranchWithMaxLeftMinRight(); //ready
            //TestFindBranchWithMinRight(); //ready
            //TestExtendLeft(); //ready
            //TestExtendRight(); //ready
            
#endif
            //End Of Main
        }


        static int[] BranchesOfSinglesFromListToArray(List<Model[]> Branches)
        {
            int[] result = new int[Branches.Count];
            for (int i = 0; i < result.Length; i++)
            {
                result[i] = Branches[i][0].Value;
            }
            return result;
        }

        static List<Model> BranchesToList(List<Model[]> Branches)
        {
            List<Model> result = new List<Model>();
            foreach(var b in Branches)
                foreach(var m in b)
                {
                    result.Add(m);
                }
            return result;
        }

        static Model[] BranchesToArray(List<Model[]> Branches)
        {
            int length = 0;
            int k = 0;
            foreach (var b in Branches)
                foreach (var m in b)
                    length++;
            Model[] result = new Model[length];
            for (int i = 0; i < Branches.Count; i++)
            {
                for (int j = 0; j < Branches[i].Length; j++)
                {
                    if (k < length)
                        result[k++] = Branches[i][j];
                    else break;
                }
            }
            return result;
        }

        static int CountSingles(List<Model[]> Branches)
        {
            int summ = 0;
            for (int i = 0; i < Branches.Count; i++)
            {
                if (Branches[i].Length == 1)
                    summ++;
            }
            return summ;
        }

        /// <summary>
        /// Попробовать расширить слева
        /// </summary>
        /// <param name="array"></param>
        /// <param name="branch"></param>
        static bool ExtendLeft(List<Model[]> Branches)
        {
            int singleMinPosition = FindSingleMin(Branches);
            int branchPosition;
            if (singleMinPosition >= 0)
            {
                branchPosition = FindBranchWithMaxLeft(Branches, singleMinPosition);
                if (branchPosition >= 0)
                {
                    Branches[branchPosition] =
                        (new Model[] {  Branches[singleMinPosition][0] }).Concat<Model>(Branches[branchPosition]).ToArray<Model>();
                    Branches.RemoveAt(singleMinPosition);
                    return true;
                }
            }
            return false;
        }

        /// <summary>
        /// Попробовать расширить справа
        /// </summary>
        /// <param name="array"></param>
        /// <param name="branch"></param>
        static bool ExtendRight(List<Model[]> Branches)
        {
            int singleMaxPosition = FindSingleMax(Branches);
            int branchPosition;
            if (singleMaxPosition > 0)
            {
                branchPosition = FindBranchWithMinRight(Branches, ref singleMaxPosition);
                if (branchPosition >= 0)
                {
                    Branches[branchPosition] =
                        Branches[branchPosition].Concat<Model>(new Model[] { Branches[singleMaxPosition][0]}).ToArray<Model>();
                    Branches.RemoveAt(singleMaxPosition);
                    return true;
                }
            }
            return false;
        }

        static int FindBranchWithMaxLeft(List<Model[]> Branches, int startPosition)
        {
            int max = Branches[startPosition][0].Value;
            int maxPosition = -1;            
            for (int i = startPosition; i < Branches.Count; i++)
            {
                if (Branches[i].Length > 0 && Branches[i].Length <3 && Branches[i][0].Value > max)
                {
                    max = Branches[i][0].Value;
                    maxPosition = i;
                }
            }
            return maxPosition;
        }

        static int FindBranchWithMinRight(List<Model[]> Branches, ref int startPosition)
        {
            if (Branches[startPosition].Length > 1) return -1;
            int min = Branches[startPosition][0].Value;
            int minPosition = -1;
            for (int i = startPosition; i >= 0; i--)
            {
                if (Branches[i].Length > 0 && Branches[i].Length < 3)
                {
                    if (Branches[i].Length == 2)
                    {
                        if (Branches[i][1].Value < min)
                        {
                            min = Branches[i][1].Value;
                            minPosition = i;
                        }
                    }
                    else if (Branches[i].Length == 1)
                        if (Branches[i][0].Value < min)
                        {
                            min = Branches[i][0].Value;
                            minPosition = i;
                        }
                }
            }
            while (startPosition > 0)
                if (minPosition == -1)
                {
                    startPosition--;
                    minPosition = FindBranchWithMinRight(Branches, ref startPosition);
                }
                else break;
            return minPosition;
        }

        static int FindSingleMax(List<Model[]> Branches)
        {
            int max = -1;
            int maxPosition = -1;
            for (int i = Branches.Count-1; i >= 0; i--)
            {
                if (Branches[i].Length == 1 && Branches[i][Branches[i].Length - 1].Value > max)
                {
                    max = Branches[i][0].Value;
                    maxPosition = i;
                }
                if (Branches[i].Length > 1 && maxPosition > -1) break;
            }
            return maxPosition;
        }

        /// <summary>
        /// Находит минимальный элемент среди единичных веток до первой ветки из двух элементов.Если сначала идут ветки из двух элементов, то они пропускаются.
        /// </summary>
        /// <param name="Branches"></param>
        /// <returns></returns>
        static int FindSingleMin(List<Model[]> Branches)
        {
            int min = -1;
            int minPosition = -1;
            int i;
            for (i = 0; i < Branches.Count; i++)
                if (Branches[i].Length == 1)
                {
                    min = Branches[i][0].Value;
                    minPosition = i;
                    break;
                }
            for (i = i; i < Branches.Count; i++)
            {
                if (Branches[i].Length == 1 && Branches[i][0].Value < min)
                {
                    min = Branches[i][0].Value;
                    minPosition = i;
                }
                if (Branches[i].Length > 1 && minPosition > -1) break;
            }
            return minPosition;
        }

        /// <summary>
        /// Принимает Branches, внутри которых только двумерные массивы, и формирует для соотвутствующих значений стратегию 
        /// </summary>
        /// <param name="Branches"></param>
        /// <param name="baseArray"></param>
        /// <returns></returns>
        static string FormStringResultFromDoubles(List<Model> Branches)
        {
            Branches = Branches.OrderBy(x => x.Index).ToList();
            string result = String.Empty;
            foreach (Model b in Branches)
                {
                    result += String.Format("{0}; ", b.Action);
                }
            return result;
        }

        static int[] GenerateRandomArray(int length)
        {
            int[] resultArray = new int[length];
            Random random = new Random();
            for (int i = 0; i < length; i++)
            {
                resultArray[i] = random.Next(1, 15);
            }
            foreach (var a in resultArray)
                Console.Write("{0} ", a);
            return resultArray;
        }

        /// <summary>
        /// Получить все ветки первоначального массива
        /// </summary>
        /// <param name="array"></param>
        /// <returns></returns>
        static List<Model[]> GetAllBranches(int[] array)
        {
            List<Model[]> Branches = new List<Model[]>();
            int i = 0;
            while (i + 1 < array.Length)
            {
                while (array[i] > array[i + 1])
                {
                    Branches.Add(new Model[] { new Model(array[i], i) });
                    i++;
                    if (i == array.Length - 1)
                    {
                        break;
                    }
                }

                List<Model> temp = new List<Model>();
                do
                {
                    temp.Add(new Model(array[i],i));
                    if (i == array.Length - 1)
                    {
                        i++;
                        break;
                    }
                }
                while (array[i] <= array[i++ + 1]);
                Branches.Add(temp.ToArray<Model>());
            }
            if (i == array.Length - 1)
            {
                Branches.Add(new Model[] { new Model(array[i], i) });
            }

            return Branches;
        }

        /// <summary>
        /// Метод возвращает тактику действий на фондовой бирже, основываясь на полученных данных по котировкам
        /// </summary>
        /// <param name="prices">Цены на котировки в единицу времени</param>
        /// <returns></returns>
        static string GetTactics(int[] prices)
        {
            
            List<Model[]> Branches = GetAllBranches(prices);

            ShowBranches(Branches);
            ParseBigBranches(Branches);
            ShowBranches(Branches);
            while (ExtendRight(Branches))
            {
                ParseBigBranches(Branches);
                ShowBranches(Branches);
            }
            while (ExtendLeft(Branches))
            {
                ParseBigBranches(Branches);
                ShowBranches(Branches);
            }

            MarkBranches(Branches);

            if (CountSingles(Branches) > 1)
            {                
                int[] temp =
                    BranchesOfSinglesFromListToArray(Branches.Where(x => x.Length == 1).Select(x => x).ToList());
                if (ExtendLeft(GetAllBranches(temp)))
                    MarkBranches(Branches);
            }

            return FormStringResultFromDoubles(BranchesToList(Branches));
        }

        static void MarkBranches(List<Model[]> Branches)
        {
            foreach (Model[] b in Branches)
                for (int i = 0; i < b.Length; i++)
                {
                    if (b.Length == 2)
                    {
                        b[i++].Action = Actions.Buy;
                        b[i].Action = Actions.Sell;
                    }
                    if (b.Length == 1)
                        b[i].Action= Actions.Wait;
                }
        }

        static bool ParseBigBranches(List<Model[]> Branches)
        {
            for (int i = 0; i < Branches.Count; i++)
            {
                if (Branches[i].Length > 2)
                {
                    Model[] temp = Branches[i];
                    Branches.Insert(i + 1, new Model[]
                    {
                        new Model(temp[0].Value, temp[0].Index),
                        new Model(temp[temp.Length - 1].Value, temp[temp.Length - 1].Index)
                    });
                    Model[] buf = new Model[temp.Length - 2];
                    Array.ConstrainedCopy(temp, 1, buf, 0, temp.Length - 2);
                    Branches[i] = buf;
                }
                if (Branches[i].Length > 2) i--;
            }
            return true;
        }

        static void ShowBranches(List<Model[]> Branches)
        {
            Console.Write("[");
            for (int i = 0; i < Branches.Count; i++)
            {
                Console.Write("[");
                for (int j = 0; j < Branches[i].Length; j++)
                {
                    if (j == Branches[i].Length - 1) { Console.Write("{0}", Branches[i][j].Value); break; }
                    Console.Write("{0} ", Branches[i][j].Value);
                }
                if (i == Branches.Count - 1) { Console.Write("]"); break; }
                Console.Write("] ");
            }
            Console.Write("]");
            Console.WriteLine();
            
            foreach (Model[] arr in Branches)
                foreach (Model a in arr)
                    Console.Write("{0} ", a.Index);
            Console.WriteLine();
        }
        
        static int[] StringArrayToInt(string array)
        {
            string[] temp = array.Split(' ');
            int[] result = new int[temp.Length];
            for(int i = 0; i< temp.Length; i++)
            {
                result[i] = int.Parse(temp[i]);
            }
            return result;
        }

    }
}
