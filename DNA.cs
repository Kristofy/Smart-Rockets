using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Vector;

namespace SmartRockets
{
    class DNA
    {
        public static int lifetime = 800;
        public Vector2[] genes;
        private static Random rnd = new Random();
        public static float maxforce = 0.1f;

        public DNA()
        {
            genes = new Vector2[lifetime];
            for (int i = 0; i < lifetime; i++)
            {
                genes[i] = Vector2.Random();
                genes[i].MultiplyRandom(0, maxforce);

            }
        }

        public DNA Crossover(DNA other)
        {
            DNA child = new DNA();
            int midpoint = rnd.Next(0, lifetime);
            for (int i = 0; i < lifetime; i++)
            {
                if (i < midpoint)
                {
                    child.genes[i] = genes[i];
                }
                else
                {
                    child.genes[i] = other.genes[i];
                }
            }

            return child;
        }

        public void mutate(float mutationRate)
        {
            for (int i = 0; i < lifetime; i++)
            {
                if ((rnd.Next(0, 10000) / 10000.0f) < mutationRate)
                {
                    genes[i] = Vector2.Random();
                    genes[i].MultiplyRandom(0, maxforce);
                }
            }
        }

    }
}
