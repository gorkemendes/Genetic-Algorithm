using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GeneticAlgorithm
{
    class PasswordCracking : IProblem<string,char,string>
    {
        public GeneticAlgorithm<char> geneticAlgorithm { get; set; }
        public string target { get; set; }
        public string validGenes { get; set; }
        private Random random;

        public PasswordCracking(string target, string validGenes, Random random)
        {
            this.target = target;
            this.validGenes = validGenes;
            this.random = random;           
        }

        public float FitnessFunction(int index)
        {            

            float score = 0;

            Chromosome<char> chromosome = geneticAlgorithm.population[index];

            for (int i = 0; i < chromosome.genes.Length; i++)
            {
                if (chromosome.genes[i] == target[i])
                {
                    score += 1;
                }
            }
            return score;
        }

        public char GetRandomGene()
        {
            int i = random.Next(2);
            return validGenes[i];
        }
    }
}
