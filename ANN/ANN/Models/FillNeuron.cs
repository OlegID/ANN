using Npgsql;
using NpgsqlTypes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ANN.Models
{
    public static class FillNeuron
    {
        private static ANNDBEntities1 db = new ANNDBEntities1();

        public static void Filling()
        {
            NpgsqlConnection conn = new NpgsqlConnection("Server=127.0.0.1;Port=5435;User Id=postgres;Password=4784;Database=ANNDB");
            conn.Open();
            Random rand = new Random();
            foreach (var neur in db.neuron)
            {
                db.neuron.Remove(neur);
            }
            db.SaveChanges();
            double[] weight = new double[125];
            
            for(int i = 0; i < 4; i++)
            {
                for(int j = 1; j <= 125; j++)
                {
                    for (int l = 0; l < 125; l++)
                    {
                        weight[l] = rand.Next(1, 1000) * 0.01;
                    }
                    NpgsqlCommand cmd = new NpgsqlCommand("INSERT INTO neuron(neuron_id, weight, layer) VALUES(@id, @weight, @layer)", conn);
                    cmd.Parameters.Add(new NpgsqlParameter("id", i * 125 + j));
                    cmd.Parameters.Add(new NpgsqlParameter
                    {
                        ParameterName = "weight",
                        NpgsqlDbType = NpgsqlDbType.Array | NpgsqlDbType.Double,
                        Value = weight
                    });
                    cmd.Parameters.Add(new NpgsqlParameter("layer", i + 1));
                    cmd.ExecuteNonQuery();
                }
            }
            db.Database.ExecuteSqlCommand("INSERT INTO neuron (neuron_id, weight, layer) VALUES (@id, @weight, @layer)",
                                                new object[] {
                                                    new NpgsqlParameter("id", 626),
                                                   new NpgsqlParameter {ParameterName = "weight",
                                                                        NpgsqlDbType = NpgsqlDbType.Array | NpgsqlDbType.Double,
                                                                        Value = weight },
                                                    new NpgsqlParameter("layer", 5)});
            db.SaveChanges();
        }
    }
}