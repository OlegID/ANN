using Npgsql;
using NpgsqlTypes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ANN.Models
{
    public class OutputLayer
    {
        private static ANNDBEntities1 db = new ANNDBEntities1();

        private int number;

        private OutputNeuron neuronOfLayer;

        public double Output { get; private set; }

        public int State { get; private set; }

        public OutputLayer(int n)
        {
            State = 0;
            number = n;
            var list = db.neuron.Where(a => a.layer == number).ToList();
            if (list.Count() != 1)
            {
                State = 1;
                FillNeuron.Filling();
            }
            State = 2;
            list = db.neuron.Where(a => a.layer == number).ToList();
            var weights = db.Database.SqlQuery<double>("SELECT unnest(weight) FROM neuron WHERE neuron.neuron_id = @id", new NpgsqlParameter("@id", list[0].neuron_id)).ToArray();
            neuronOfLayer = new OutputNeuron(weights);
        }

        public void Calculate(double[] input)
        {
            Output = neuronOfLayer.Calculate(input);
        }

        public void Learning(double misstake, double coof)
        {
            neuronOfLayer.Learning(misstake, coof);
        }

        public void SaveWeights()
        { 
            db.Database.ExecuteSqlCommand("UPDATE neuron SET weight = @weight, layer = @layer WHERE neuron_id = @id",
                                                    new object[] {
                                                    new NpgsqlParameter("id",(number - 1) * 125 + 1),
                                                    new NpgsqlParameter {ParameterName = "weight",
                                                                        NpgsqlDbType = NpgsqlDbType.Array | NpgsqlDbType.Double,
                                                                        Value = neuronOfLayer.weights },
                                                    new NpgsqlParameter("layer", number)});
        }
    }
}