using System;
using System.Diagnostics;
using System.Collections.Generic;
using System.Linq;
using ClosedXML.Excel;

namespace APCSP_BinaryVLinear_BigO
{
    class Program
    {
        public static Stopwatch sw = new Stopwatch();
        public static Random rnd = new Random();
        static void Main(string[] args)
        {
            long[] sizes = new long[] { 9, 10, 20, 30, 40, 50, 60, 70, 80, 90, 100, 1000, 10000, 100000, 1000000, 10000000, 20000000, 30000000, 40000000, 50000000, 60000000, 70000000, 80000000, 90000000, 100000000 };
            long[] bubbleSizes = new long[] { 9, 10, 20, 30, 40, 50, 60, 70, 80, 90, 100, 200, 300, 400, 500, 600, 700, 800, 900, 1000, 2000, 3000, 4000, 5000, 6000, 7000, 8000, 9000, 10000, 20000, 30000, 40000, 50000, 60000, 70000, 80000, 90000 };
            Dictionary<long, double> BinTimes = new Dictionary<long, double>();
            Dictionary<long, double> LinTimes = new Dictionary<long, double>();
            Dictionary<long, double> BubTimes = new Dictionary<long, double>();
            Dictionary<string, Dictionary<long, double>> Times = new Dictionary<string, Dictionary<long, double>> { { "Bin", BinTimes }, { "Lin", LinTimes }, { "Bub", BubTimes } };
            for (long i = 0; i < Math.Max(sizes.Length,bubbleSizes.Length); i++)
            {
                // Binary/Linear Search
                if (i < sizes.Length)
                {
                    // Array to search through
                    long[] numbers = CreateArr(sizes[i]);
                    for (int j = 0; j < 2; j++)
                    {
                        // Binary on first loop, Linear on second
                        if (j == 0)
                        {
                            BinTimes.Add(numbers.Length, GetAverageTime("Binary", Bin, numbers));
                        }
                        if (j > 0)
                        {
                            LinTimes.Add(numbers.Length, GetAverageTime("Linear", Lin, numbers));
                        }
                        
                    }
                }
                // Bubble Sort
                if (i < bubbleSizes.Length)
                {
                    // Array to sort
                    long[] randNums = RevArr(bubbleSizes[i]);
                    BubTimes.Add(randNums.Length, GetAverageTime("Bubble", Bub, randNums));
                }
            }
            XLWorkbook workbook = new XLWorkbook();
            foreach (KeyValuePair<string,Dictionary<long,double>> type in Times)
            {
                IXLWorksheet worksheet = workbook.Worksheets.Add($"{type.Key} Sheet");
                worksheet.Cell("A1").Value = "Inputs";
                worksheet.Cell("B1").Value = "Times";
                worksheet.Cell("A2").Value = 0;
                worksheet.Cell("B2").Value = 0;
                int num = 3;
                foreach (KeyValuePair<long,double> times in type.Value)
                {
                    worksheet.Cell($"A{num}").Value = times.Key;
                    worksheet.Cell($"B{num}").Value = times.Value;
                    num++;
                }
            }
            workbook.SaveAs("Times.xlsx");
            //Console.WriteLine("Done");
            //Console.ReadLine();
        }

        // Populates an array of size
        public static long[] CreateArr(long size)
        {
            long[] arr = new long[size];
            for (long i = 0; i < arr.Length; i++)
            {
                arr[i] = i;
            }
            return arr;
        }

        // Populates then reverses an array of sizes
        public static long[] RevArr(long size)
        {
            return CreateArr(size).Reverse().ToArray();
            //return Enumerable.Range(0, size).OrderBy(r => rnd.Next()).ToArray();
        }

        // Binary search action
        public static void Bin(long[] arr)
        {
            long element = arr[arr.Length - 1];
            sw.Restart();
            Array.BinarySearch(arr, element);
            sw.Stop();
        }

        // Linear search action
        public static void Lin(long[] arr)
        {
            long element = arr[arr.Length - 1];
            sw.Restart();
            Array.IndexOf(arr, element);
            sw.Stop();
        }

        // Bubble sort action
        public static void Bub(long[] arr)
        {
            sw.Restart();
            long stopLim = arr.Length;
            for (long i = 0; i < arr.Length - 1; i++)
            {
                bool swap = false;
                for (long j = 1; j < stopLim; j++)
                {
                    if (arr[j - 1] > arr[j])
                    {
                        swap = true;
                        long tmp = arr[j];
                        arr[j] = arr[j - 1];
                        arr[j - 1] = tmp;
                    }
                    if (j == (stopLim - 1))
                    {
                        stopLim--;
                    }
                }
                if (!swap)
                {
                    break;
                }
            }
            sw.Stop();
        }
        // Iterates through the provided action the specified number of times, and prints the average number of Ticks
        public static double GetAverageTime(string printPrefix, Action<long[]> iterate, long[] arr)
        {
            long[] times = new long[100];
            for (int k = 0; k < times.Length; k++)
            {
                iterate(arr);
                times[k] = sw.ElapsedTicks;
            }
            return times.Average();
            // Print the type of search/sort, number of elements, average time
            //Console.WriteLine($"{printPrefix}\n\tElements: {arr.Length}\n\tTicks: {times.Average()}");
        }
    }
}