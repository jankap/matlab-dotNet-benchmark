
using System;
using System.Collections.Generic;
using System.Runtime.Serialization.Formatters.Binary;
using System.IO;
using System.Diagnostics;

namespace myNamespace.Events
{

    public class DataEventArgs : EventArgs
    {
        public double[] Time;
        public double[,] Data;
        public int[] Idx;

        #region Constructor
        public DataEventArgs()
        {
          
        }
                
        public void AddData(double[] time, double[,] data, int[] idx)
        {
            Time = time;
            Data = data;
            Idx = idx;
        }
        #endregion Constructor
    }
}
