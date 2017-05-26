using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using ANN;
using ANN.Models;

namespace ANN.Models
{
    public class NeuronNetwork
    {
        

        private List<Layer> layersList;

        private OutputLayer outputLayer;

        private double[] input;

        public double Output { get; private set; }

        private double misstake;

        private double coof;

        public NeuronNetwork(double coof)
        {
            layersList = new List<Layer>();
            this.coof = coof;
            for(int i = 1; i <= 4; i++)
            {
                layersList.Add(new Layer(i));
            }
            outputLayer = new OutputLayer(5);
        }

        public double Calculate(double[] input)
        {
            this.input = input;
            foreach(Layer layer in layersList)
            {
                layer.Calculate(this.input);
                input = layer.Output;
            }
            Output = outputLayer.Output;
            return Output;
        }
        
        public void Learning(double trueCount)
        {
            misstake = trueCount - Output;
            foreach (var layer in layersList)
            {
                layer.Learning(misstake,coof);
            }
        }

        public void SaveWeights()
        {
            foreach (var layer in layersList)
            {
                layer.SaveWeights();
            }
            outputLayer.SaveWeights();
        }
    }

    
}