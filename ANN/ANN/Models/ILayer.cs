using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ANN.Models
{
    public interface ILayer
    {
        void Calculate(double[] input);

        void Learning(double misstake, double coof);

        void SaveWeights();
    }
}