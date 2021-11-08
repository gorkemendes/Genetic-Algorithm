using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GeneticAlgorithm
{
    class Chromosome<T>
    {
        public T[] genes { get; private set; }
        public T[] validGenes { get; private set; }       
        public float fitness { get; private set; }
        private Random random;
        private Func<T> getRandomGene;
        Func<int, float> fitnessFunction;

        //Temporary flag for knapsack at %25 capacity
        public static bool tempFlag { get; set; }
        public Chromosome(int size, Random random, Func<T> getRandomGene, Func<int, float> fitnessFunction, bool shouldInitGenes = true)
        {
            this.genes = new T[size];
            this.random = random;
            this.getRandomGene = getRandomGene;
            this.fitnessFunction = fitnessFunction;

            if (shouldInitGenes)
            {
                int genesLenght = genes.Length;

                //When sack capacity below %50, only half of the chromosome should be filled with random genes.
                if (tempFlag)
                {
                    genesLenght = genes.Length / 2;
                }
                
                for (int i = 0; i < genesLenght; i++)
                {
                    genes[i] = getRandomGene();
                }
            }          
        }

        public float CalculateFitness(int index) 
        {
            fitness = fitnessFunction(index);
            return fitness;
        }

        public Chromosome<T> UniformCrossOver(Chromosome<T> parent1, Chromosome<T> parent2)
        {
            Chromosome<T> child = new Chromosome<T>(genes.Length, random, getRandomGene, fitnessFunction, shouldInitGenes : false);
            
            for (int i = 0; i < genes.Length; i++)
            {
                child.genes[i] = random.NextDouble() < 0.5 ? parent1.genes[i] : parent2.genes[i];               
            }
            return child;
        }

        public Chromosome<T> OnePointCrossOver(Chromosome<T> parent1, Chromosome<T> parent2)
        {
            Chromosome<T> child = new Chromosome<T>(genes.Length, random, getRandomGene, fitnessFunction, shouldInitGenes: false);

            int halfPoint = (int) genes.Length / 2;

            T[] firstGeneBlock = parent1.genes.Take(halfPoint).ToArray();
            T[] secondGeneBlock = parent2.genes.Skip(halfPoint).ToArray();

            child.genes = firstGeneBlock.Concat(secondGeneBlock).ToArray();
            return child;
        }

        public Chromosome<T> TwoPointCrossOver(Chromosome<T> parent1, Chromosome<T> parent2)
        {
            Chromosome<T> child = new Chromosome<T>(genes.Length, random, getRandomGene, fitnessFunction, shouldInitGenes: false);

            int firstPoint = (genes.Length / 4);
            int secondPoint = (firstPoint * 2)+1;
            
            List<T> firstGeneBlock = new List<T>(parent1.genes).GetRange(firstPoint, secondPoint);

            List<T> secondGeneBlock = new List<T>(parent2.genes).GetRange(0, firstPoint);    
            
            T[] thirdGeneBlock = parent2.genes.Skip((parent2.genes.Length - secondGeneBlock.Count)+1).ToArray();

            child.genes = (secondGeneBlock.Concat(firstGeneBlock)).Concat(thirdGeneBlock).ToArray();

            return child;
        }


        public void Mutate(float mutationRate) 
        {
            for (int i = 0; i < genes.Length; i++)
            {
                if (random.NextDouble() < mutationRate)
                {
                    genes[i] = getRandomGene();
                }
            }
        }
      
    }
}
