using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WindowsFormsApp1
{
    public class Individ
    {
        public double x { get; set; }
        public double y { get; set; }

        private double fitness;

        public Individ()
        {
        }

        public double Fitness_func()
        {
            fitness = Math.Pow((x - 2), 2) + Math.Pow((y - 1), 2);
            return fitness;
        }
    }
}
