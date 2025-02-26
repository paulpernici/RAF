using RAFSimulation.Services.Models;
using System;
using System.Collections.Generic;
using System.Linq;

namespace RAFSimulation.Services
{
    public class ReactionGenerator
    {
        /// <summary>
        /// The set of polymers to generate reactions for.
        /// </summary>
        private readonly List<string> _polymerSet;

        /// <summary>
        /// The maximum length polymer in the set.
        /// </summary>
        private readonly int _maximumLength;

        /// <summary>
        /// Generates all possible reactions between the polymers in the set.
        /// </summary>
        /// <param name="polymerSet">The set of polymers to generate reactions for.</param>
        public ReactionGenerator(List<string> polymerSet) 
        {
            if (polymerSet == null || polymerSet.Count == 0)
            {
                throw new ArgumentException("The polymer set cannot be null or empty.", nameof(polymerSet));
            }

            _polymerSet = polymerSet;
            _maximumLength = polymerSet.Max(x => x.Length);
        }

        /// <summary>
        /// Generates a collection of all the reactions possible with the polymer set provided.
        /// </summary>
        /// <returns></returns>
        public List<Reaction> GenerateReactions()
        {
            // Starting with the maximum length.
            // For each polymer of a given length, find every combination of polymers that can generate it.
            // For each combination, add a reaction with the larger polymer as resultant and as reactant.

            List<Reaction> reactions = new List<Reaction>();

            // Start with the maximum length.
            int length = _maximumLength;

            // While the polymer is not a unit, find the breakdown.
            while (length > 1)
            {
                // For each polymer of the given length.
                foreach (string polymer in _polymerSet.Where(x => x.Length == length))
                {
                    string start, end;
                    int spliceIndex = 1;

                    // Iterate through the polymer, generating reactions forward and backwards.
                    while (spliceIndex < polymer.Length)
                    { 
                        start = polymer[..spliceIndex];
                        end = polymer[spliceIndex..];
                        List<string> parts = new List<string>{ start, end };
                        List<string> whole = new List<string> { polymer };
                        reactions.Add(new Reaction(parts, whole));
                        reactions.Add(new Reaction(whole, parts));
                        ++spliceIndex;
                    }
                }

                // Decrease the length.
                length--;
            }

            return reactions;
        }
    }
}
