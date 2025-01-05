using SakyoGame.Lib.Shared.Attributes;
using UnityEngine;

namespace SakyoGame.Lib.Textures {

    /**
     * <summary>
     *  Class for generating primitive procedural textures.
     * </summary>
     */
    public static class TextureGenerator {

        /**
         * <summary>
         *  Nested class for generating 2D textures.
         * </summary>
         */
        public static class TwoDimensional {

            /**
             * <summary>
             *  Generates a 2D texture from a color map.
             * </summary>
             * <param name="mapColor">The color map used to generate the texture.</param>
             * <param name="width">The width of the texture.</param>
             * <param name="height">The height of the texture.</param>
             * <returns>The generated texture.</returns>
             */
            public static Texture2D TextureFromColourMap(Color[] mapColor, int width, int height) {

                Texture2D texture2D = new Texture2D(width, height); // Initialize a new Texture2D.
                texture2D.filterMode = FilterMode.Point; // Set the filter mode to 'Point' for sharp pixel rendering.
                texture2D.SetPixels(mapColor); // Assign the color map to the texture.
                texture2D.Apply(); // Apply changes to the texture to finalize it.

                return texture2D; // Return the generated texture.
            }

            /**
             * <summary>
             *  Generates a 2D texture from a height map.
             * </summary>
             * <param name="heightMap">The height map used to generate the texture.</param>
             * <returns>The generated texture.</returns>
             */
            public static Texture2D TextureFromHeightMap(float[,] heightMap) {

                if(heightMap == null) return null; // Return null if the input height map is invalid.

                int width = heightMap.GetLength(0); // Get the width of the height map.
                int height = heightMap.GetLength(1); // Get the height of the height map.

                Color[] colorMap = new Color[width * height]; // Initialize an array to store colors.

                /*
                 * Loop through each point in the height map and map the height value to a grayscale color.
                 */
                for(int y = 0; y < height; y++) {

                    for(int x = 0; x < width; x++) {

                        // Map the height value to a grayscale color between black and white.
                        colorMap[y * width + x] = Color.Lerp(Color.black, Color.white, heightMap[x, y]);
                    }
                }

                // Use the first method to generate a texture from the color map.
                return TextureFromColourMap(colorMap, width, height);
            }
        }

        /**
         * <summary>
         *  Nested class for generating 3D textures.
         * </summary>
         */
        [Beta("This class is still in development and may not be fully functional.")]
        public static class ThreeDimensional {

            /**
             * <summary>
             *  Generates a 3D texture from a color map.
             * </summary>
             * <param name="mapColor">The color map used to generate the texture.</param>
             * <param name="width">The width of the texture.</param>
             * <param name="height">The height of the texture.</param>
             * <param name="depth">The depth of the texture.</param>
             * <returns>The generated texture.</returns>
             */
            [Beta("This method is still in development and may not be fully functional.")]
            public static Texture3D TextureFromColourMap(Color[] mapColor, int width, int height, int depth) {

                // Initialize a new Texture3D.
                Texture3D texture3D = new Texture3D(width, height, depth, TextureFormat.RGBA32, false);
                texture3D.filterMode = FilterMode.Point; // Set the filter mode to 'Point' for sharp pixel rendering.
                texture3D.SetPixels(mapColor); // Assign the color map to the texture.
                texture3D.Apply(); // Apply changes to the texture to finalize it.

                return texture3D; // Return the generated texture.
            }

            /**
             * <summary>
             *  Generates a 3D texture from a volume map.
             * </summary>
             * <param name="volumeMap">The volume map used to generate the texture. A 3D array representing volume data.</param>
             * <returns>The generated Texture3D object.</returns>
             */
            [Beta("This method is still in development and may not be fully functional.")]
            public static Texture3D TextureFromVolumeMap(float[,,] volumeMap) {

                // Check if the volume map is null. If it is, return null as no texture can be generated.
                if(volumeMap == null) return null;

                // Get the dimensions of the volume map (width, height, and depth)
                int width = volumeMap.GetLength(0);  // Get the width of the volume map.
                int height = volumeMap.GetLength(1); // Get the height of the volume map.
                int depth = volumeMap.GetLength(2);  // Get the depth of the volume map.

                // Initialize an array to hold the colors that will form the texture.
                Color[] colorMap = new Color[width * height * depth];

                /*
                 * Loop through each depth slice in the volume map and map the value to a grayscale color.
                 * This process converts the volume map (3D array) into a 1D array of colors.
                 */
                for(int z = 0; z < depth; z++) {

                    /*
                     * Loop through each height slice in the volume map (Y-axis) and map the value to a grayscale color.
                     */
                    for(int y = 0; y < height; y++) {

                        /*
                         * Loop through each width slice in the volume map (X-axis) and map the value to a grayscale color.
                         */
                        for(int x = 0; x < width; x++) {

                            /*
                             * Map the volume value to a grayscale color between black and white.
                             * volumeMap[x, y, z] is the value at each (x, y, z) position.
                             */
                            colorMap[z * width * height + y * width + x] = Color.Lerp(Color.black, Color.white, volumeMap[x, y, z]);
                        }
                    }
                }

                /*
                 * Once the color map is populated, generate a 3D texture using the color map.
                 * The first method is used to create the Texture3D from the color map and dimensions
                 */
                return TextureFromColourMap(colorMap, width, height, depth);
            }
        }
    }
}

