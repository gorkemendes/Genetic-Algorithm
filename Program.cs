using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GeneticAlgorithm
{
    class Program
    {
        static public GeneticAlgorithm<int> geneticAlgorithm;      
        static Random random;       
        static KnapSack knapSack;

        static void Main(string[] args)
        {
            random = new Random();
            int populationSize = 100;
            int elitisim = 20;
            float mutationRate = 0.25f;            
            int target = 30;

            int[] weights = Enumerable
            .Repeat(0, populationSize)
            .Select(i => random.Next(1, 100))
            .ToArray();

            int[] values = Enumerable
            .Repeat(0, populationSize)
            .Select(i => random.Next(1, 100))
            .ToArray();

            int[] validGenes = new int[2] { 0, 1 };
            float sackPercentage = 0.50f;

            // Roulette Wheel = 0, Tournement = 1
            int selection = 0;

            // Uniform = 0, One Point = 1, Two Point = 2
            int crossover = 0;
            
            knapSack = new KnapSack(target, validGenes, weights, values ,random, sackPercentage);           
            geneticAlgorithm = new GeneticAlgorithm<int>(populationSize, knapSack.weights.Length, knapSack.GetRandomGene, 
                                                         knapSack.FitnessFunction, random, elitisim, mutationRate,selection,crossover);
            knapSack.geneticAlgorithm = geneticAlgorithm;
           
            double execTimeMean = 0;
            var watch = System.Diagnostics.Stopwatch.StartNew();
                
            for (int j = 0; j < target; j++)
            {
                geneticAlgorithm.MakeNewGeneration();
                geneticAlgorithm.toString();
                Console.WriteLine("   Generation:" + geneticAlgorithm.generation + " Fitness:" + geneticAlgorithm.bestFitness + ""+ "     total weight:  " +knapSack.GetTotalWeight()+"  Sack Capacity: "+knapSack.sackCapacity);
            }
            watch.Stop();
            execTimeMean += watch.Elapsed.TotalMilliseconds;
            Console.WriteLine("İşlem Süresi: " + watch.Elapsed.TotalSeconds + " s\n");
        Console.ReadKey();
        }       
    }
}



