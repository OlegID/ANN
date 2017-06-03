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

        private bool state;

        public NeuronNetwork(double coof)
        {
            this.coof = coof;
            state = false;
            while(!state)
            {
                Load();
            }
        }

        private void Load()
        {
            layersList = new List<Layer>();
            for(int i = 1; i <= 4; i++)
            {
                layersList.Add(new Layer(i));
                while(layersList[i - 1].State != 2)
                {
                    if(layersList[i - 1].State == 1)
                    {
                        return;
                    }
                }
            }
            outputLayer = new OutputLayer(5);
            while(outputLayer.State != 2)
            {
                if(outputLayer.State == 1)
                {
                    return;
                }
            }
            state = true;
        }

        public double Calculate(double[] input)
        {
            this.input = input;
            foreach(Layer layer in layersList)
            {
                layer.Calculate(this.input);
                this.input = layer.Output;
            }
            outputLayer.Calculate(this.input);
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
            outputLayer.Learning(misstake, coof);
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