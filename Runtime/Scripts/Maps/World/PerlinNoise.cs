using UnityEngine;
using Random = System.Random;

namespace SakyoGame.Lib.Maps.World {

    /**
     * <summary>
     *  Class for managing primitive procedural textures, including gradient noise
     * </summary>
     */
    public static class PerlinNoise  {

        /****************************/
        /****** PUBLIC METHODS ******/
        /****************************/

        /**
         * <summary>
         *  Generates a 2D Perlin noise map with specified parameters for terrain generation.
         * </summary>
         * <param name="mapWidth">Width of the generated noise map.</param>
         * <param name="mapHeight">Height of the generated noise map.</param>
         * <param name="scale">Scale factor to control the zoom level of the noise.</param>
         * <param name="octaves">Number of layers of Perlin noise to add (higher values create more detail).</param>
         * <param name="persistances">The persistence factor controls how much each successive octave's influence diminishes.</param>
         * <param name="lacuranity">The lacunarity controls how the frequency of each successive octave changes.</param>
         * <param name="seed">Seed for random number generation to ensure consistent results.</param>
         * <returns>A 2D array representing the generated noise map.</returns>
         */
        public static float[,] GenerateNoiseMap2D(int mapWidth, int mapHeight, float scale, int octaves, float persistances, float lacuranity, int seed) {

            if(scale <= 0) scale = 0.001f; // Ensure scale is not zero.

            Random random = new Random(seed); // Create a random number generator.
            Vector2[] octaveOffsets = GenerateOctaveOffsets2D(octaves, random); // Generate offsets for each octave.

            float[,] noiseMap = new float[mapWidth, mapHeight]; // Create a 2D array to store the noise map.
            float maxNoiseHeight = float.MinValue; // Initialize the maximum noise height.
            float minNoiseHeight = float.MaxValue; // Initialize the minimum noise height.

            /*
             * Loop through each point in the noise map and calculate the noise height.
             */
            for(int y = 0; y < mapHeight; y++) {
                for(int x = 0; x < mapWidth; x++) {

                    // Calculate the noise height for the current point.
                    float noiseHeight = CalculateNoiseHeight2D(x, y, mapWidth, mapHeight, scale, octaves, persistances, lacuranity, octaveOffsets);

                    noiseMap[x, y] = noiseHeight; // Assign the noise height to the noise map.
                    if(noiseHeight > maxNoiseHeight) maxNoiseHeight = noiseHeight; // Update the maximum noise height.
                    if(noiseHeight < minNoiseHeight) minNoiseHeight = noiseHeight; // Update the minimum noise height.
                }
            }

            // Normalize the noise map values to a range of 0-1 for consistent scaling.
            NormalizeNoiseMap2D(noiseMap, mapWidth, mapHeight, minNoiseHeight, maxNoiseHeight);
            return noiseMap; // Return the generated noise map.
        }

        /**
         * <summary>
         *  Generates a 3D Perlin noise map with specified parameters for terrain generation.
         * </summary>
         * <param name="mapWidth">Width of the generated noise map.</param>
         * <param name="mapHeight">Height of the generated noise map.</param>
         * <param name="mapDepth">Depth of the generated noise map.</param>
         * <param name="scale">Scale factor to control the zoom level of the noise.</param>
         * <param name="octaves">Number of layers of Perlin noise to add (higher values create more detail).</param>
         * <param name="persistances">The persistence factor controls how much each successive octave's influence diminishes.</param>
         * <param name="lacuranity">The lacunarity controls how the frequency of each successive octave changes.</param>
         * <param name="seed">Seed for random number generation to ensure consistent results.</param>
         * <returns>A 3D array representing the generated noise map.</returns>
         */
        public static float[,,] GenerateNoiseMap3D(int mapWidth, int mapHeight, int mapDepth, float scale, int octaves, float persistances,
            float lacuranity, int seed) {

                if(scale <= 0) scale = 0.001f; // Ensure scale is not zero.

                Random random = new Random(seed); // Create a random number generator.
                Vector3[] octaveOffsets = GenerateOctaveOffsets3D(octaves, random); // Generate offsets for each octave.

                float[,,] noiseMap = new float[mapWidth, mapHeight, mapDepth]; // Create a 3D array to store the noise map.
                float maxNoiseHeight = float.MinValue; // Initialize the maximum noise height.
                float minNoiseHeight = float.MaxValue; // Initialize the minimum noise height.

                /*
                 * Loop through each point in the noise map and calculate the noise height.
                 */
                for(int z = 0; z < mapDepth; z++) {
                    for(int y = 0; y < mapHeight; y++) {
                        for(int x = 0; x < mapWidth; x++) {

                            // Calculate the noise height for the current point.
                            float noiseHeight = CalculateNoiseHeight3D(x, y, z, mapWidth, mapHeight, mapDepth, scale, octaves, persistances, lacuranity, octaveOffsets);

                            noiseMap[x, y, z] = noiseHeight; // Assign the noise height to the noise map.
                            if(noiseHeight > maxNoiseHeight) maxNoiseHeight = noiseHeight; // Update the maximum noise height.
                            if(noiseHeight < minNoiseHeight) minNoiseHeight = noiseHeight; // Update the minimum noise height.
                        }
                    }
                }

                // Normalize the noise map values to a range of 0-1 for consistent scaling.
                NormalizeNoiseMap3D(noiseMap, mapWidth, mapHeight, mapDepth, minNoiseHeight, maxNoiseHeight);
                return noiseMap; // Return the generated noise map.
        }

        /**
         * <summary>
         *  Generates random offsets for each octave to create varied noise layers in 2D.
         * </summary>
         * <param name="octaves">Number of layers of Perlin noise to add.</param>
         * <param name="random">Random number generator.</param>
         * <returns>An array of Vector2 representing the offsets for each octave.</returns>
         */
        private static Vector2[] GenerateOctaveOffsets2D(int octaves, Random random) { Vector2[] octaveOffsets = new Vector2[octaves];

            /*
             * Generate random offsets for each octave to create varied noise layers.
             */
            for(int i = 0; i < octaves; i++) {

                float offsetX = random.Next(-1000, 1000); // Generate a random offset for the X coordinate.
                float offsetY = random.Next(-1000, 1000); // Generate a random offset for the Y coordinate.
                octaveOffsets[i] = new Vector2(offsetX, offsetY); // Assign the offsets to the octave array.
            }

            return octaveOffsets; // Return the array of offsets for each octave.
        }

        /**
         * <summary>
         *  Generates random offsets for each octave to create varied noise layers in 3D.
         * </summary>
         * <param name="octaves">Number of layers of Perlin noise to add.</param>
         * <param name="random">Random number generator.</param>
         * <returns>An array of Vector3 representing the offsets for each octave.</returns>
         */
        private static Vector3[] GenerateOctaveOffsets3D(int octaves, Random random) {

            Vector3[] octaveOffsets = new Vector3[octaves]; // Create an array to store the offsets for each octave.

            /*
             * Generate random offsets for each octave to create varied noise layers.
             */
            for(int i = 0; i < octaves; i++) {

                float offsetX = random.Next(-1000, 1000); // Generate a random offset for the X coordinate.
                float offsetY = random.Next(-1000, 1000); // Generate a random offset for the Y coordinate.
                float offsetZ = random.Next(-1000, 1000); // Generate a random offset for the Z coordinate.
                octaveOffsets[i] = new Vector3(offsetX, offsetY, offsetZ); // Assign the offsets to the octave array.
            }

            return octaveOffsets; // Return the array of offsets for each octave.
        }

        /**
         * <summary>
         *  Calculates the noise height for a given point in 2D.
         * </summary>
         * <param name="x">X coordinate of the point.</param>
         * <param name="y">Y coordinate of the point.</param>
         * <param name="mapWidth">Width of the generated noise map.</param>
         * <param name="mapHeight">Height of the generated noise map.</param>
         * <param name="scale">Scale factor to control the zoom level of the noise.</param>
         * <param name="octaves">Number of layers of Perlin noise to add.</param>
         * <param name="persistances">The persistence factor controls how much each successive octave's influence diminishes.</param>
         * <param name="lacuranity">The lacunarity controls how the frequency of each successive octave changes.</param>
         * <param name="octaveOffsets">Array of offsets for each octave.</param>
         * <returns>The calculated noise height for the given point.</returns>
         */
        private static float CalculateNoiseHeight2D(int x, int y, int mapWidth, int mapHeight, float scale, int octaves, float persistances,
            float lacuranity, Vector2[] octaveOffsets) {

                float amplitude = 1; // Initialize the amplitude.
                float frequency = 1; // Initialize the frequency.
                float noiseHeight = 0; // Initialize the noise height.

                /*
                 * Loop through each octave and calculate the noise height for the given point.
                 */
                for(int i = 0; i < octaves; i++) {

                    float sampleX = (x - mapWidth / 2f) / scale * frequency + octaveOffsets[i].x; // Calculate the sample X coordinate.
                    float sampleY = (y - mapHeight / 2f) / scale * frequency + octaveOffsets[i].y; // Calculate the sample Y coordinate.
                    float perlinValue = Mathf.PerlinNoise(sampleX, sampleY) * 2 - 1; // Calculate the Perlin noise value.

                    noiseHeight += perlinValue * amplitude; // Add the noise value to the noise height.
                    amplitude *= persistances; // Update the amplitude.
                    frequency *= lacuranity; // Update the frequency.
                }

                return noiseHeight; // Return the calculated noise height.
        }

        /**
         * <summary>
         *  Calculates the noise height for a given point in 3D.
         * </summary>
         * <param name="x">X coordinate of the point.</param>
         * <param name="y">Y coordinate of the point.</param>
         * <param name="z">Z coordinate of the point.</param>
         * <param name="mapWidth">Width of the generated noise map.</param>
         * <param name="mapHeight">Height of the generated noise map.</param>
         * <param name="mapDepth">Depth of the generated noise map.</param>
         * <param name="scale">Scale factor to control the zoom level of the noise.</param>
         * <param name="octaves">Number of layers of Perlin noise to add.</param>
         * <param name="persistances">The persistence factor controls how much each successive octave's influence diminishes.</param>
         * <param name="lacuranity">The lacunarity controls how the frequency of each successive octave changes.</param>
         * <param name="octaveOffsets">Array of offsets for each octave.</param>
         * <returns>The calculated noise height for the given point.</returns>
         */
        private static float CalculateNoiseHeight3D(int x, int y, int z, int mapWidth, int mapHeight, int mapDepth, float scale, int octaves,
            float persistances, float lacuranity, Vector3[] octaveOffsets) {

                float amplitude = 1; // Initialize the amplitude.
                float frequency = 1; // Initialize the frequency.
                float noiseHeight = 0; // Initialize the noise height.

                /*
                 * Loop through each octave and calculate the noise height for the given point.
                 */
                for(int i = 0; i < octaves; i++) {

                    float sampleX = (x - mapWidth / 2f) / scale * frequency + octaveOffsets[i].x; // Calculate the sample X coordinate.
                    float sampleY = (y - mapHeight / 2f) / scale * frequency + octaveOffsets[i].y; // Calculate the sample Y coordinate.
                    float sampleZ = (z - mapDepth / 2f) / scale * frequency + octaveOffsets[i].z; // Calculate the sample Z coordinate.
                    float perlinValue = Mathf.PerlinNoise(sampleX, sampleY) * Mathf.PerlinNoise(sampleZ, sampleY) * 2 - 1; // Calculate the Perlin noise value.

                    noiseHeight += perlinValue * amplitude; // Add the noise value to the noise height.
                    amplitude *= persistances; // Update the amplitude.
                    frequency *= lacuranity; // Update the frequency.
                }

                return noiseHeight; // Return the calculated noise height.
        }

        /**
         * <summary>
         *  Normalizes the noise map values to a range of 0-1 for consistent scaling in 2D.
         * </summary>
         * <param name="noiseMap">The generated noise map.</param>
         * <param name="mapWidth">Width of the generated noise map.</param>
         * <param name="mapHeight">Height of the generated noise map.</param>
         * <param name="minNoiseHeight">Minimum noise height value.</param>
         * <param name="maxNoiseHeight">Maximum noise height value.</param>
         */
        private static void NormalizeNoiseMap2D(float[,] noiseMap, int mapWidth, int mapHeight, float minNoiseHeight, float maxNoiseHeight) {

            /*
             * Normalize the noise map values to a range of 0-1 for consistent scaling.
             */
            for(int y = 0; y < mapHeight; y++) {
                for(int x = 0; x < mapWidth; x++) {

                    // Normalize the noise height value to a range of 0-1.
                    noiseMap[x, y] = Mathf.InverseLerp(minNoiseHeight, maxNoiseHeight, noiseMap[x, y]);
                }
            }
        }

        /**
         * <summary>
         *  Normalizes the noise map values to a range of 0-1 for consistent scaling in 3D.
         * </summary>
         * <param name="noiseMap">The generated noise map.</param>
         * <param name="mapWidth">Width of the generated noise map.</param>
         * <param name="mapHeight">Height of the generated noise map.</param>
         * <param name="mapDepth">Depth of the generated noise map.</param>
         * <param name="minNoiseHeight">Minimum noise height value.</param>
         * <param name="maxNoiseHeight">Maximum noise height value.</param>
         */
        private static void NormalizeNoiseMap3D(float[,,] noiseMap, int mapWidth, int mapHeight, int mapDepth, float minNoiseHeight, float maxNoiseHeight) {

            /*
             * Normalize the noise map values to a range of 0-1 for consistent scaling.
             */
            for(int z = 0; z < mapDepth; z++) {
                for(int y = 0; y < mapHeight; y++) {
                    for(int x = 0; x < mapWidth; x++) {

                        // Normalize the noise height value to a range of 0-1.
                        noiseMap[x, y, z] = Mathf.InverseLerp(minNoiseHeight, maxNoiseHeight, noiseMap[x, y, z]);
                    }
                }
            }
        }
    }
}