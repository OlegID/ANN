using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Web;
using System.Numerics;

namespace ANN.Models
{
    public class Neuron
    {
        public double[] weights;

        public Neuron(double[] w)
        {
            weights = w;
        }

        public virtual double Calculate(double[] input)
        {
            double sum = 0;
            for (int i = 0; i < 125; i++)
            {
                sum += weights[i] * input[i];
            }
            if (double.IsInfinity((Math.Exp(-1 * sum))) || double.IsNaN((Math.Exp(-1 * sum))))
            {
                return 0;
            }
            else if ((Math.Exp(-1 * sum)) == 0)
            {
                return 1;
            }
            else
            {
                double result = 1 / (Math.Exp(-1 * sum) + 1);
                if (double.IsNaN(result))
                {
                    new object();
                }
                return result;
            }
        }

        public void Learning(double misstake, double coof)
        {
            for (int i = 0; i < 125; i++)
            {
                weights[i] += weights[i] * misstake * coof;
            }
        }
    }
}