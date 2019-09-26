using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using myNamespace;
using myNamespace.Events;
using System.Diagnostics;
using System.Runtime.CompilerServices;


namespace TestProgram
{

    // this program benchmarks polling vs events in C#. Same test should be done in Matlab.
    class Program
    {
        static Stopwatch stopwatch = new Stopwatch();
        static int cnt = 0;

        static void Main(string[] args)
        {
            // first benchmark calling the GetSingleDataset as fast as possible
            var class1 = new dotNetClass();
            stopwatch.Reset();
            stopwatch.Start();

            while (stopwatch.ElapsedMilliseconds < 10000) // 10 seconds
            {
                var e = class1.GetSingleDataset();
                extractAndDoSomethingWithData(e);
            }

            stopwatch.Stop();
            
            var sw = stopwatch.ElapsedMilliseconds;
            double thousandElementsPerSec = ((double)cnt / (double)sw);
            Console.WriteLine("polling speed: " + thousandElementsPerSec.ToString() + " kPcs/s");


            // now benchmark the event
            var class2 = new dotNetClass();
            class2.DataIsReady += DataIsReady; // assing event

            cnt = 0; // reset counter
            stopwatch.Reset();
            stopwatch.Start();
            class2.StartDataGenerationEvent(); // blocking for 10 seconds -> no need to stop here

            stopwatch.Stop();

            sw = stopwatch.ElapsedMilliseconds;
            thousandElementsPerSec = ((double)cnt / (double)sw);
            Console.WriteLine("event speed: " + thousandElementsPerSec.ToString() + " kPcs/s");
            Console.ReadLine();
        }

        [MethodImpl(MethodImplOptions.NoOptimization | MethodImplOptions.NoInlining)]
        static void DataIsReady(object sender, DataEventArgs e)
        {
            // extract the event data
            extractAndDoSomethingWithData(e);
        }

        private static void extractAndDoSomethingWithData(DataEventArgs e)
        {
            cnt++;

            double[] Time = new double[8];
            double[,] Data = new double[8, 4];
            int[] Idx = new int[8];

            for (int i = 0; i < 8; i++)
            {
                for (int j = 0; j < 4; j++)
                {
                    Data[i, j] = e.Data[i, j];
                }
                Time[i] = e.Time[i];
                Idx[i] = e.Idx[i];
            }
        }
    }
}
