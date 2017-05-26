using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ANN.Models
{
    public class OutputNeuron : Neuron
    {
        public OutputNeuron(double[] w) : base(w) { }
        

        public override double Calculate(double[] input)
        {
            double sum = 0;
            for (int i = 0; i < 5; i++)
            {
                sum += weights[i] * input[i];
            }
            return sum;
        }
    }
}