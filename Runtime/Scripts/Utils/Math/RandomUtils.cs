using System.Collections.Generic;
using System.Linq;

namespace SakyoGame.Lib.Utils.Math {

    /**
     * <summary>
     *  Class for utility methods related to randomization.
     * </summary>
     */
    public static class RandomUtils {

        /**
         * <summary>
         *  Shuffles the list randomly using the Fisher-Yates algorithm.
         * </summary>
         * <typeparam name="TEnumerable">The type of the list elements.</typeparam>
         * <param name="enumerable">The enumerable collection to shuffle (will be shuffled in place).</param>
         */
        public static IEnumerable<TEnumerable> Shuffle<TEnumerable>(IEnumerable<TEnumerable> enumerable) {

            // Convert the IEnumerable to a List to avoid multiple enumerations
            List<TEnumerable> list = enumerable.ToList();

            // Initialize the random number generator
            System.Random rng = new System.Random();

            int n = list.Count; // Count the number of elements

            /*
             * Loop through the list and swap each element with a random element
             */
            while(n > 1) {

                n--; // Decrement the count
                int k = rng.Next(n + 1); // Generate a random number between 0 and n

                // Deconstruct the current and random elements into temporary variables
                (list[k], list[n]) = (list[n], list[k]);  // Swap using deconstruction
            }

            return list; // Return the shuffled list
        }
    }
}