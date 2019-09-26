
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading;
using myNamespace.Events;

/*
 * 1) instantiate class
 * 2.1) call either GetSingleDataset() from Matlab and from C# to show performance differences with direct function calls ...
 * 2.2) ... or call StartDataGenerationEvent() from Matlab and from C# to show performance differences with events
 */

namespace myNamespace
{

    public class dotNetClass
    {
        Stopwatch sw = new Stopwatch();
        int currIdx = 0;
        Random rnd = new Random();

        public event EventHandler<DataEventArgs> DataIsReady = delegate { };


        public dotNetClass()
        {
            // do nothing
        }

        public DataEventArgs GetSingleDataset()
        {
            return (generateSingleDataset());
        }

        // generates data for 10 seconds and throws events
        public void StartDataGenerationEvent()
        {
            sw.Start();
            while (sw.ElapsedMilliseconds < 10000)
            {
                OnDataReady(generateSingleDataset());
            }
            sw.Stop();
        }

        protected virtual void OnDataReady(DataEventArgs e)
        {
            DataIsReady(this, e);
        }

        // this function generates just some data, time (8x1 array), idx (8x1 array) and data (8x4) array
        private DataEventArgs generateSingleDataset()
        {
            var lastIdx = currIdx;
            var nextIdx = 8;

            double[] Time = new double[8];
            double[,] Data = new double[8, 4];
            int[] Idx = new int[8];

            //for (int i = 0; i < matrix.GetLength(0); i++)
            //    for (int j = 0; j < matrix.GetLength(1); j++)
            //        matrix[i, j] = i * 3 + j;

            for (int i = 0; i < 8; i++)
            {
                for (int j = 0; j < 4; j++)
                {
                    Data[i, j] = rnd.NextDouble();
                }
                Time[i] = lastIdx + i;
                Idx[i] = lastIdx + i; // not needed?
            }
            currIdx = lastIdx + nextIdx;

            var evnt = new DataEventArgs();
            evnt.AddData(Time, Data, Idx);
            return evnt;
        }

    }
}
