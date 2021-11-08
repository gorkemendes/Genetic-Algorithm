using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace GeneticAlgorithm
{
    class KnapSack : IProblem<int[], int, int>
    {
        public GeneticAlgorithm<int> geneticAlgorithm { get; set; }
        public Chromosome<int> chromosome { get; set; }
        public int target { get; set; }
        public int[] validGenes { get; set; }
        public int[] weights { get; set; }
        public int[] values { get; set; }
        public int sackCapacity { get; set; }
        public float sackPercentage { get; set; }
        private Random random;

        public KnapSack(int target, int[] validGenes, int[] weights, int[] values, Random random, float percentage)
        {
            this.target = target;
            this.validGenes = validGenes;
            this.weights = weights;
            this.values = values;
            //this.sackCapacity = capacity;
            this.random = random;
            this.sackPercentage = percentage;

            this.sackCapacity = (int)Math.Round(weights.Sum() * percentage);

            if (percentage < 0.5)
            {
                Chromosome<int>.tempFlag = true;
            }
        }

        public float FitnessFunction(int index)
        {
            float score = 0;
            int totalWeight = 0;
           
            this.chromosome = geneticAlgorithm.population[index];            

            for (int i = 0; i < chromosome.genes.Length; i++)
            {
                if (chromosome.genes[i] == 1)
                {
                    totalWeight += weights[i];
                    score += values[i];
                }
            }

            if (totalWeight <= sackCapacity)
            {
                return score;               
            }
            else                
                return 0;
        }

        public int GetTotalWeight() 
        {
            int totalWeight = 0;
            for (int i = 0; i < geneticAlgorithm.BestGenes.Length; i++)
            {
                if (geneticAlgorithm.BestGenes[i] == 1)
                    totalWeight += weights[i];
            }
            return totalWeight;
        }

        public int GetRandomGene()
        {
            int i = random.Next(validGenes.Length);
            return validGenes[i];
        }
    }
}
