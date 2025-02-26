using RAFSimulation.Models;
using RAFSimulation.Services.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace RAFSimulation.Services
{
    public class CatalyzedReactionGraphGenerator
    {
        /// <summary>
        /// The random number generator.
        /// </summary>
        private static readonly Random random = new Random();

        /// <summary>
        /// The maximum polymer length.
        /// </summary>
        private readonly int _maximumLength;

        /// <summary>
        /// The alphabet for each polymer.
        /// </summary>
        private PolymerAlphabet _polymerAlphabet;

        /// <summary>
        /// The total set of polymers in the graph.
        /// </summary>
        private readonly List<string> _polymerSet;

        /// <summary>
        /// The total set of reactions possible between the polymers.
        /// </summary>
        private readonly List<Reaction> _reactionSet;

        /// <summary>
        /// Sets up a catalyzed reaction graph generator.
        /// </summary>
        /// <param name="alphabet">The alphabet to be used to generate polymers.</param>
        /// <param name="maximumLength">The maximum length of a polymer.</param>
        public CatalyzedReactionGraphGenerator(
            PolymerAlphabet alphabet,
            int maximumLength)
        {
            // Set up the polymer generator.
            PolymerGenerator polymerGenerator = new PolymerGenerator(maximumLength, alphabet);
            _maximumLength = maximumLength;
            _polymerAlphabet = alphabet;
            _polymerSet = polymerGenerator.GeneratePolymers();

            // Set up the reaction set.
            ReactionGenerator reactionGenerator = new ReactionGenerator(_polymerSet);
            _reactionSet = reactionGenerator.GenerateReactions();
        }

        /// <summary>
        /// Gets a collection of catalyzed reactions given a probability of acatalyzation and a food set..
        /// </summary>
        /// <param name="getProbability">A function that takes in a polymer and a reaction, and decides if the polymer catalyzes the reaction.</param>
        /// <param name="foodSet">The set of food for the graph, a subset of the total polymer set.</param>
        /// <returns></returns>
        public CatalyzedReactionGraph GenerateCatalyzedReactionGraph(Func<string, Reaction, double> getProbability, List<string> foodSet)
        {
            if (getProbability == null)
            {
                throw new ArgumentNullException(nameof(getProbability));
            }

            if (!foodSet.All(x => _polymerSet.Contains(x)))
            {
                throw new ArgumentException("The food set must be a subset of the reaction set.", nameof(foodSet));
            }

            foreach (Reaction reaction in _reactionSet)
            {
                foreach (string polymer in _polymerSet)
                {
                    double chance = random.NextDouble();
                    if (chance < getProbability(polymer, reaction))
                    {
                        reaction.AddCatalyst(polymer);
                    }
                }
            }
            return new CatalyzedReactionGraph(_polymerSet, foodSet, _reactionSet);
        }
    }
}
