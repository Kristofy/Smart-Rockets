using System;
using System.Collections.Generic;
using System.Collections;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Vector;
using System.Drawing;

namespace SmartRockets
{
    class Population
    {
        public int LeftLifetime { get; set; } = DNA.lifetime;
        public static int PopulationSize { get; set; } = 100;
        Rocket[] rockets;
        public static Vector2 target { get; set; } = new Vector2();
        private static float mutationRate = 0.0038f; 
        private static Random rnd = new Random();
        private int count;

        public Population()
        {
            count = 0;
            rockets = new Rocket[PopulationSize];
            for (int i = 0; i < PopulationSize; i++)
            {
                rockets[i] = new Rocket((Vector2)Form1.Spawner.Clone());
            }
        }

        public void Render(Graphics g)
        {
            for (int i = 0; i < PopulationSize; i++)
            {
                rockets[i].Render(g);
            }
        }

        public void Update()
        {
            if (count < DNA.lifetime)
            {
                Parallel.ForEach(rockets, (rocket) =>
                {
                    if (rocket.state == State.Alive)
                    {
                        rocket.Run();
                    }
                });
                //for (int i = 0; i < PopulationSize; i++)
                //{
                //    if (rockets[i].state == State.Alive)
                //    {
                //        rockets[i].Run();
                //    }
                //}
                count++;
                LeftLifetime--;
                return;
            }
            else
            {
                newGeneration();
                count = 0;
                Form1.Gen++;
                LeftLifetime = DNA.lifetime;
                return;
            }
            throw new DoneException("END");
        }

        public void newGeneration()
        {
            // Selection
            List<Rocket> pool = new List<Rocket>();
            List<float> fitnesses = new List<float>();
            float max = 0;
            Array.ForEach(rockets,(r)=> {
                r.calculateFitness();
                if(r.fitness > max)
                {
                    max = r.fitness;
                }
            });

            for (int i = 0; i < PopulationSize; i++)
            {
                
                int n = (int)(rockets[i].fitness/max * PopulationSize);
                for (int j = 0; j < n; j++)
                {
                    pool.Add(rockets[i]);
                }
                if (rockets[i].record <  Form1.Best)
                {
                    Form1.Best = rockets[i].record;
                }
            }
            // Reproduction
            for (int i = 0; i < PopulationSize; i++)
            {
                int a = rnd.Next(0, pool.Count);
                int b = rnd.Next(0, pool.Count);
                DNA parentA = pool[a].dna;
                DNA parentB = pool[b].dna;

                DNA child = parentA.Crossover(parentB);
                child.mutate(mutationRate);

                rockets[i] = new Rocket((Vector2)Form1.Spawner.Clone());
                rockets[i].dna = child;
            }
        }

    }
}
