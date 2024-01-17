using System;
using System.Collections.Generic;
using System.Linq;

namespace WindowsFormsApp1
{
    public class Generic
    {
        List<List<Individ>> populations = new List<List<Individ>>();
        public Generic()
        {
        }

        private bool Satisf_limits(Individ individ)
        {
            double x = individ.x;
            double y = individ.y;

            if ((x - 2 * y >= 1) && (x + y <= 3))
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        private List<Individ> Selection(List<Individ> individs)
        {
            individs = individs.OrderBy(i => i.Fitness_func()).ToList();
            int count = individs.Count;
            List<Individ> to_delete = new List<Individ>();
            for (int i = 0; i < count; i++)
            {
                if (!Satisf_limits(individs[i]))
                {
                    individs.Add(individs[i]);
                    to_delete.Add(individs[i]);
                }
            }

            foreach (var i in to_delete)
            {
                individs.Remove(i);
            }

            return individs;
        }

        private List<Individ> Crossover(List<Individ> parents, int num_offsprings)
        {
            List<Individ> offsprings = new List<Individ>();
            Random rnd = new Random();
            for (int i = 0; i < num_offsprings; i++)
            {
                Individ parent1 = parents[rnd.Next(parents.Count)];
                Individ parent2 = parents[rnd.Next(parents.Count)];

                Individ offspring1 = new Individ()
                {
                    x = parent1.x,
                    y = parent2.y
                };
                offsprings.Add(offspring1);

                Individ offspring2 = new Individ()
                {
                    x = parent2.x,
                    y = parent1.y
                };
                offsprings.Add(offspring2);
            }

            return offsprings;
        }

        private List<Individ> Mutation(List<Individ> offsprings, double mutation_Rate, double low_bound, double up_bound, double up_gen, double low_gen)
        {
            Random rnd_mut_chance = new Random();
            foreach (var off in offsprings)
            {
                if (rnd_mut_chance.NextDouble() < mutation_Rate)
                {
                    Random rnd_mut_type = new Random();
                    int mut_type = rnd_mut_type.Next() * 10;

                    Random rnd_stat = new Random();
                    switch (mut_type)
                    {
                        case 0:
                            {
                                off.x += rnd_stat.NextDouble() * (up_bound - low_bound) + low_bound;
                                off.y += rnd_stat.NextDouble() * (up_bound - low_bound) + low_bound;
                                break;
                            }

                        case 1:
                            {
                                off.x += rnd_stat.NextDouble() * (up_bound - low_bound) + low_bound;
                                off.y -= rnd_stat.NextDouble() * (up_bound - low_bound) + low_bound;
                                break;
                            }

                        case 2:
                            {
                                off.x -= rnd_stat.NextDouble() * (up_bound - low_bound) + low_bound;
                                off.y += rnd_stat.NextDouble() * (up_bound - low_bound) + low_bound;
                                break;
                            }

                        case 3:
                            {
                                off.x -= rnd_stat.NextDouble() * (up_bound - low_bound) + low_bound;
                                off.y -= rnd_stat.NextDouble() * (up_bound - low_bound) + low_bound;
                                break;
                            }

                        case 4:
                            {
                                off.x *= rnd_stat.NextDouble() * (up_bound - low_bound) + low_bound;
                                off.y *= rnd_stat.NextDouble() * (up_bound - low_bound) + low_bound;
                                break;
                            }

                        case 5:
                            {
                                off.x *= rnd_stat.NextDouble() * (up_bound - low_bound) + low_bound;
                                off.y /= rnd_stat.NextDouble() * (up_bound - low_bound) + low_bound;
                                break;
                            }

                        case 6:
                            {
                                off.x /= rnd_stat.NextDouble() * (up_bound - low_bound) + low_bound;
                                off.y *= rnd_stat.NextDouble() * (up_bound - low_bound) + low_bound;
                                break;
                            }

                        case 7:
                            {
                                off.x /= rnd_stat.NextDouble() * (up_bound - low_bound) + low_bound;
                                off.y /= rnd_stat.NextDouble() * (up_bound - low_bound) + low_bound;
                                break;
                            }

                        case 8:
                            {
                                off.x = rnd_stat.NextDouble() * (up_gen - low_gen) + low_gen;
                                off.y = rnd_stat.NextDouble() * (up_gen - low_gen) + low_gen;
                                break;
                            }

                        case 9:
                            {
                                off.x = off.x * off.x;
                                break;
                            }

                        case 10:
                            {
                                off.y = off.y * off.y;
                                break;
                            }
                    }
                }
            }

            return offsprings;
        }

        public double[] Count()
        {
            double[] result = new double[2];
            List<Individ> population = new List<Individ>();
            Random random = new Random();
            for (int i = 0; i < 1000; i++)
            {
                Individ individ = new Individ()
                {
                    x = random.NextDouble() * 3,
                    y = random.NextDouble() * 3
                };

                population.Add(individ);
            }

            populations.Add(population);

            for (int i = 0; i < 4; i++)
            {
                population = Selection(population);
                population.RemoveRange(100, population.Count - 100);
                List<Individ> offsprings = Crossover(population, 400);
                offsprings = Mutation(offsprings, 0.65, 0.7, 2, 1, 3);
                population.AddRange(offsprings);

                var temp_population = population;
                temp_population.RemoveRange(100, population.Count - 100);
                populations.Add(temp_population);
            }
            result[0] = Math.Round(population[0].x, 2);
            result[1] = Math.Round(population[0].y, 2);

            return result;
        }

        public List<List<Individ>> Return_populations()
        {
            return populations;
        }
    }
}
