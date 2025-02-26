using RAFSimulation.Models;
using RAFSimulation.Services;
using RAFSimulation.Services.Models;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading;

namespace RAFSimulation
{
    class Program
    {
        static void Main(string[] args)
        {
            PolymerAlphabet alphabet = new PolymerAlphabet('A', 'B');

            using (StreamWriter writer = new StreamWriter("RAFData.csv"))
            {
                writer.WriteLine("M_Polymer,P_Catalysis,P_Closure,Mean_Reactions");
                for (int m = 2; m <= 6; ++m)
                {
                    Console.WriteLine($"m = {m}");
                    int runs = 1000;

                    double p = 0.00001;
                    while (p <= 0.15)
                    {
                        Console.WriteLine($"p = {p}");
                        int closures = 0;
                        int totalReactions = 0;
                        for (int k = 0; k < runs; ++k)
                        {
                            CatalyzedReactionGraphGenerator generator = new CatalyzedReactionGraphGenerator(alphabet, m);
                            CatalyzedReactionGraph graph = generator.GenerateCatalyzedReactionGraph((string polymer, Reaction reaction) => p, new List<string> { "A", "B", "AA", "BB", "BA", "AB" });
                            var closure = graph.RAF();

                            if (closure.Count > 0)
                            {
                                ++closures;
                            }
                            totalReactions += closure.Count;
                        }
                        writer.WriteLine($"{m},{p},{(double)closures / (double)runs},{(double)totalReactions / (double)runs}");
                        p += 0.001;
                    }
                }
            }
        }
    }
}
