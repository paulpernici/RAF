using RAFSimulation.Services.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace RAFSimulation.Services
{
    public  class PolymerGenerator
    {
        /// <summary>
        /// The maximum size.
        /// </summary>
        private readonly int _maximumSize;

        /// <summary>
        /// The alphabet.
        /// </summary>
        private readonly PolymerAlphabet _polymerAlphabet;

        /// <summary>
        /// Creates a new polymer generator from the alphabet and maximum size.
        /// </summary>
        /// <param name="maximumSize">The largest polymer this generator can create.</param>
        /// <param name="alphabet">The alphabet used to construct polymers.</param>
        /// <exception cref="ArgumentOutOfRangeException">If the maximum size <= 0.</exception>
        /// <exception cref="ArgumentNullException">If the alphabet is null.</exception>
        public PolymerGenerator(int maximumSize, PolymerAlphabet alphabet)
        {
            if (maximumSize <= 0)
            {
                throw new ArgumentOutOfRangeException(nameof(maximumSize), "The maximum size must be greater than 0.");
            }

            if (alphabet == null)
            {
                throw new ArgumentNullException(nameof(alphabet));
            }

            _maximumSize = maximumSize;
            _polymerAlphabet = alphabet;
        }

        /// <summary>
        /// Generates a collection of all polymers in the set.
        /// </summary>
        /// <returns></returns>
        public List<string> GeneratePolymers()
        {
            List<string> combinations = new List<string>();
            for (int i = 1; i <= _maximumSize; ++i)
            {
                combinations.AddRange(GetCombinations(i, _polymerAlphabet.Letters));
            }
            return combinations;
        }

        /// <summary>
        /// Gets all the possible combinations of letters in an alphabet of a given length.
        /// </summary>
        /// <param name="size">The size of the string.</param>
        /// <param name="alphabet">The alphabet to use.</param>
        /// <returns></returns>
        private List<string> GetCombinations(int size, IEnumerable<char> alphabet)
        {
            if (alphabet == null || !alphabet.Any())
            {
                return new List<string>();
            }

            List<char> localAlphabet = alphabet.ToList();
            List<string> combinations = new List<string>();

            // First combination containing nothing but the first letter.
            StringBuilder current = new StringBuilder();
            for (int i = 0; i < size; ++i)
            {
                current.Append(localAlphabet[0]);
            }

            // Add first combination to the array.
            combinations.Add(current.ToString());

            // Start with last index.
            int indexToMutate = size - 1;

            // Loop to generate combinations.
            while (true)
            {
                // Get the current alphabet index so we don't have to track another variable.
                int indexOfLetter = (localAlphabet.IndexOf(current[indexToMutate]) + 1) % localAlphabet.Count;

                // Get index where we left off.
                int leaveOffPoint = indexToMutate;

                // If we've gotten back to the first letter, keep going down until we find one that hasn't gone through the whole alphabet.
                while (indexOfLetter == 0)
                {
                    // Change the index to mutate back to the first letter.
                    current[indexToMutate] = localAlphabet[indexOfLetter];
                    indexToMutate -= 1;

                    // If the index to mutate is < 0, we've exhausted all the combinations.
                    if (indexToMutate < 0)
                    {
                        return combinations;
                    }

                    indexOfLetter = (localAlphabet.IndexOf(current[indexToMutate]) + 1) % localAlphabet.Count;
                }

                // Mutate the current index by going up by one and add the combination.
                current[indexToMutate] = localAlphabet[indexOfLetter];
                combinations.Add(current.ToString());

                // Back to theend to restart?
                indexToMutate = leaveOffPoint;
            }
        }
    }
}
