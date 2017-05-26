using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

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
            for (int i = 0; i < 5; i++)
            {
                sum += weights[i] * input[i];
            }
            return ((Math.Exp(2 * sum) - 1) / (Math.Exp(2 * sum) + 1));
        }

        public void Learning(double misstake, double coof)
        {
            for (int i = 0; i < 5; i++)
            {
                weights[i] += weights[i] * misstake * coof;
            }
        }
    }
}