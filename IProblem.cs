using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GeneticAlgorithm
{
    public interface IProblem<T,X,Z>
    {
        T validGenes { get; set; }
        Z target { get; set; }
        float FitnessFunction(int index);
        X GetRandomGene();
        
    }
}
