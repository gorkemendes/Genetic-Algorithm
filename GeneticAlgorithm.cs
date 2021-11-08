using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GeneticAlgorithm
{
    class GeneticAlgorithm<T>
    {
        public List<Chromosome<T>> population { get; private set; }
        public int generation { get; private set; }
        public float mutationRate { get; private set; }
        public float bestFitness { get; private set; }
        public T[] BestGenes { get; private set; }
        private List<Chromosome<T>> newPopulation;
        public int selectionMethod { get; set; }
        public int crossOverMethod { get; set; }
        public int elitisim;
        private Random random;
        private float fitnessSum;

        public GeneticAlgorithm( int populationSize, int chromosomeSize, Func<T> getRandomGene, Func<int, float> fitnessFunction, Random random,
            int elitisim, float mutationRate, int selectionMethod, int crossOverMethod)
        {
            generation = 0;
            this.mutationRate = mutationRate;
            this.random = random;
            this.elitisim = elitisim;
            this.selectionMethod = selectionMethod;
            this.crossOverMethod = crossOverMethod;
            population = new List<Chromosome<T>>(populationSize);
            newPopulation = new List<Chromosome<T>>(populationSize);
            
            BestGenes = new T[chromosomeSize];

            for (int i = 0; i < populationSize; i++)
            {
                population.Add(new Chromosome<T>(chromosomeSize, random, getRandomGene, fitnessFunction, shouldInitGenes: true));
            }
        }

        public void MakeNewGeneration()
        {
            if (population.Count <= 0)
            {
                return;
            }

            CalculateFitness();
            population.Sort(compareChromosome);
            newPopulation.Clear();

            for (int i = 0; i < population.Count; i++)
            {
                if (i < elitisim)
                {
                    newPopulation.Add(population[i]);
                }

                Chromosome<T> parent1;
                Chromosome<T> parent2;
                Chromosome<T> child;

                //Alternate Selection Methods
                if (selectionMethod == 0)
                {                  
                    parent2 = RouletteSelection();
                    parent1 = RouletteSelection();
                }
                else
                {
                    parent1 = TournamentSelection(3);
                    parent2 = TournamentSelection(3);                    
                }

                //Alternate CrossOver Methods
                if (crossOverMethod == 0)
                {
                    child = parent1.UniformCrossOver(parent1, parent2);
                }
                else if (crossOverMethod == 1)
                {
                    child = parent1.OnePointCrossOver(parent1, parent2);
                }
                else
                {
                    child = parent1.TwoPointCrossOver(parent1, parent2);
                }

                child.Mutate(mutationRate);
                newPopulation.Add(child);
            }

            List<Chromosome<T>> tempList = population;
            population = newPopulation;
            newPopulation = tempList;
            generation++;
        }

        public void CalculateFitness()
        {
            fitnessSum = 0;
            Chromosome<T> best = population[0];

            for (int i = 0; i < population.Count; i++)
            {
                fitnessSum += population[i].CalculateFitness(i);

                if (population[i].fitness > best.fitness)
                {
                    best = population[i];
                }
            }

            bestFitness = best.fitness;
            best.genes.CopyTo(BestGenes, 0);
        }

        private Chromosome<T> RouletteSelection()
        {
            double randomNum = random.NextDouble() * fitnessSum;

            for (int i = 0; i < population.Count; i++)
            {
                if (randomNum < population[i].fitness)
                {
                    return population[i];
                }

                randomNum -= population[i].fitness;
            }
            return population[0];
        }

        private Chromosome<T> TournamentSelection(int k)
        {
            Chromosome<T> best = null;

            for (int i = 0; i < k; i++)
            {
                Chromosome<T> ind = population[random.Next(0, population.Count)];
                
                if (best == null || ind.fitness > best.fitness)
                {
                    best = ind;
                }
            }
            return best;
        }

        public int compareChromosome(Chromosome<T> c1, Chromosome<T> c2)
        {
            if (c1.fitness > c2.fitness)
            {
                return -1;

            }
            else if (c1.fitness < c2.fitness)
            {
                return 1;
            }
            else
            {
                return 0;
            }
        }

        public void toString()
        {

            for (int i = 0; i < population[0].genes.Length; i++)
            {
                Console.Write(BestGenes[i]);
            }
        }
    }
}
