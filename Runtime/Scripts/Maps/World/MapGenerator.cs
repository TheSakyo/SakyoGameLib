using System;
using System.Collections;
using SakyoGame.Lib.Enums;
using SakyoGame.Lib.Maps.Meshes;
using SakyoGame.Lib.Shared.Attributes;
using SakyoGame.Lib.Structs;
using SakyoGame.Lib.Textures;
using UnityEngine;
using Display = SakyoGame.Lib.Textures.Display;

namespace SakyoGame.Lib.Maps.World {

    /**
     * <summary>
     *  Class for generating and displaying the game world.
     * </summary>
     */
    public class MapGenerator : MonoBehaviour {

        /************************/
        /****** PROPERTIES ******/
        /************************/

        /**
         * <summary>
         *  Constant for the chunk size, used in generating map size
         * </summary>
         */
        const int ChunkSize = 241;

        /**
         * <summary>
         *  Defines the level of detail for the terrain mesh generation.
         *  A value between 0 (low detail) and 6 (high detail).
         * </summary>
         */
        [Range(0,6)]
        [SerializeField]
        private int levelOfDetail;

        /**
         * <summary>
         *  Controls the scale of the terrain, affecting the "zoom" of the noise map.
         * </summary>
         */
        [SerializeField]
        private float scale;

        /**
         * <summary>
         *  The number of octaves (layers of noise) for Perlin noise generation.
         * </summary>
         */
        [SerializeField]
        private int octaves;

        /**
         * <summary>
         *  Determines the persistence, which controls how each octave influences the final result.
         * </summary>
         */
        [SerializeField]
        private float persistances;

        /**
         * <summary>
         *  Affects the lacunarity, which impacts the frequency of each octave.
         * </summary>
         */
        [SerializeField]
        private float lacuranity;

        /**
         * <summary>
         *  Defines the maximum height of the terrain, used in scaling the mesh height.
         * </summary>
         */
        [SerializeField]
        private float meshHeightValue;

        /**
         * <summary>
         *  A curve that maps height values to actual terrain heights, useful for customizing terrain shape.
         * </summary>
         */
        [SerializeField]
        private AnimationCurve meshHeightCurve;

        /**
         * <summary>
         *  The seed value for randomization of the terrain generation.
         * </summary>
         */
        [SerializeField]
        private int seed;

        /**
         * <summary>
         *  Array of regions that define different terrain types and their color/height properties.
         * </summary>
         */
        [SerializeField]
        private TerrainType[] regions;

        /**
         * <summary>
         *  Controls the spawning of prefabs on the terrain, useful for adding details and decorations.
         * </summary>
         */
        [SerializeField]
        private bool unSpawnPrefabs;

        /**
         * <summary>
         *  Array to store the map's colors based on height values for each point.
         * </summary>
         */
        private Color[] _mapColor;

        /*********************/
        /****** AWAKING ******/
        /*********************/

        /**
         * <summary>
         *  Awake is called when the script instance is being loaded.
         *  Sets the 'IsGenerated' flag to false.
         * </summary>
         */
        private void Awake() { IsGenerated = false; }

        /*******************************/
        /****** GETTERS & SETTERS ******/
        /*******************************/

        /**
         * <summary>
         *  Getter and setter for determining if the map should be generated automatically.
         * </summary>
         */
        public bool AutoUpdate { get; set; }

        /**
         * <summary>
         *  Getter and setter for determining if the map is 2D or 3D dimensional.
         * </summary>
         */
        [Beta("This property is still in development.")]
        public bool IsThreeDimensional { get; set; }

        /**
         * <summary>
         *  Getter for the 'unSpawnPrefabs' flag.
         * </summary>
         */
        public bool UnSpawnPrefabs => unSpawnPrefabs;
        
        /**
         * <summary>
         *  Indicates if the map has been generated completely.
         * </summary>
         */
        public bool IsGenerated { get; private set; }

        /***************************/
        /****** PUBLIC METHOD ******/
        /***************************/

        /**
         * <summary>
         *  Method for generating the map in the game world based on the specified map dimension.
         * </summary>
         * <param name="isCoroutine">If true, uses coroutine for progressive map generation.</param>
         * <param name="mapDimension">The map dimension (2D or 3D) to generate.</param>
         * <exception cref="NotSupportedException">Thrown if the map dimension is not supported.</exception>
         */
        public virtual void GenerateMap(EMapDimension mapDimension, bool isCoroutine = false) {

            IEnumerator mapGeneration = null; // Variable to store the map generation process

            // if map dimension is 2D, start the 2D map generation process as a coroutine.
            if(mapDimension == EMapDimension.TwoDimensional) mapGeneration = Process2DMapGeneration(isCoroutine);

            // If map dimension is 3D, start the 3D map generation process as a coroutine
            if(mapDimension == EMapDimension.ThreeDimensional) mapGeneration = Process3DMapGeneration(isCoroutine);

            /*
             * If the map dimension is not supported, throw an exception.
             */
            if(mapGeneration == null) throw new NotSupportedException("Map dimension not supported.");

            // Start the map generation process as a coroutine
            StartCoroutine(mapGeneration);
            
            // Spawn prefabs in the game world, if the map has been generated or auto-update is enabled
            if(IsGenerated || AutoUpdate) MeshGenerator.SpawnPrefabs(regions, unSpawnPrefabs);
        }

        /*****************************/
        /****** PRIVATE METHODS ******/
        /*****************************/

        /**
         * <summary>
         *  Generates the 2D map in a coroutine, allowing for step-by-step rendering.
         *  Uses Perlin noise to generate terrain height values and assigns colors based on regions.
         * </summary>
         * <param name="isCoroutine">Flag to determine if coroutine is used for progressive rendering.</param>
         * <returns>An IEnumerator for coroutine execution.</returns>
         */
        protected IEnumerator Process2DMapGeneration(bool isCoroutine = false) {

            // Generate the 2D noise map based on the defined parameters
            float[,] noiseMap = PerlinNoise.GenerateNoiseMap2D(ChunkSize, ChunkSize, scale, octaves, persistances, lacuranity, seed);

            // Initialize color array to store the color values for the terrain
            _mapColor = new Color[ChunkSize * ChunkSize];

            /*
             * Loop through each point in the chunk and determine its height/color
             */
            for(int y = 0; y < ChunkSize; y++) {
                for(int x = 0; x < ChunkSize; x++) {

                    // Get the height value for the current point
                    float currentHeight = noiseMap[x, y];

                    // Assign the corresponding color based on the terrain regions
                    for(int i = 0; i < regions.Length; i++) {

                        // If the current height is not within the region's range, continue to the next region
                        if(!(currentHeight >= regions[i].minHeight && currentHeight <= regions[i].maxHeight)) continue;

                        _mapColor[y * ChunkSize + x] = regions[i].color; // Assign the region color to the current point
                        break; // Exit the loop once the color is assigned
                    }
                }

                if(!isCoroutine) continue; // If not using coroutine, skip to the next iteration

                Render2DMap(_mapColor, noiseMap); // Render the current map
                yield return null; // Wait for one frame before continuing
            }

            // If not using coroutine, render the final map after generation
            if(!isCoroutine) Render2DMap(_mapColor, noiseMap);

            IsGenerated = true; // Set the generated flag to true
        }

        /**
         * <summary>
         *  Generates the 3D map in a coroutine, allowing for step-by-step rendering.
         *  Uses Perlin noise to generate terrain height values and assigns colors based on regions.
         * </summary>
         * <param name="isCoroutine">Flag to determine if coroutine is used for progressive rendering.</param>
         * <returns>An IEnumerator for coroutine execution.</returns>
         */
        [Beta("This method is still in development and may not be fully functional.")]
        protected IEnumerator Process3DMapGeneration(bool isCoroutine = false) {

            /*
             * Generate the 3D noise map based on the defined parameters
             */
            float[,,] volumeMap = PerlinNoise.GenerateNoiseMap3D(ChunkSize, ChunkSize, ChunkSize, scale, octaves, persistances,
                lacuranity, seed);

            // Initialize color array to store the color values for the terrain
            Color[] mapColor = new Color[ChunkSize * ChunkSize * ChunkSize];

            // Loop through each point in the chunk and determine its height/color
            for(int z = 0; z < ChunkSize; z++) {
                for(int y = 0; y < ChunkSize; y++) {
                    for(int x = 0; x < ChunkSize; x++) {

                        // Get the height value for the current point
                        float currentHeight = volumeMap[x, y, z];

                        // Assign the corresponding color based on the terrain regions
                        for(int i = 0; i < regions.Length; i++) {

                            // If the current height is not within the region's range, continue to the next region
                            if(!(currentHeight >= regions[i].minHeight && currentHeight <= regions[i].maxHeight)) continue;

                            mapColor[z * ChunkSize * ChunkSize + y * ChunkSize + x] = regions[i].color; // Assign the region color to the current point
                            break; // Exit the loop once the color is assigned
                        }
                    }
                }

                if(!isCoroutine) continue; // If not using coroutine, skip to the next iteration

                Render3DMap(mapColor, volumeMap); // Render the current map
                yield return null; // Wait for one frame before continuing
            }

            // If not using coroutine, render the final map after generation
            if(!isCoroutine) Render3DMap(mapColor, volumeMap);

            IsGenerated = true; // Set the generated flag to true
        }

        /**
         * <summary>
         *  Renders the map as a 2D noise map using the provided color map and height map.
         * </summary>
         * <param name="mapColor">The color map to apply to the terrain (used for color-based maps).</param>
         * <param name="noiseMap">The noise map used for terrain elevation data.</param>
         */
        private void Render2DMap(Color[] mapColor, float[,] noiseMap) {

            // Find the Display object in the scene to show the map
            Display display = FindFirstObjectByType<Display>();

            // Throw an exception if the Display component is missing
            if(display == null) throw new MissingComponentException("Display component not found.");

            // Generate the terrain mesh based on the noise map and render it
            MeshGenerator.Generate2DTerrainMesh(noiseMap, meshHeightValue, meshHeightCurve, levelOfDetail);

            // Generate the texture for the terrain based on the color map
            Texture2D texture = TextureGenerator.TwoDimensional.TextureFromColourMap(mapColor, ChunkSize, ChunkSize);

            // Draw the mesh map using the display object (2D)
            display.DrawMeshMap(MeshGenerator.MeshData, texture);
        }

        /**s
         * <summary>
         *  Renders the map as a 3D noise map using the provided color map and height map.
         * </summary>
         * <param name="mapColor">The color map to apply to the terrain (used for color-based maps).</param>
         * <param name="noiseMap">The noise map used for terrain elevation data.</param>
         */
        [Beta("This method is still in development.")]
        private void Render3DMap(Color[] mapColor, float[,,] noiseMap) {

            // Find the Display object in the scene to show the map
            Display display = FindFirstObjectByType<Display>();

            // Throw an exception if the Display component is missing
            if(display == null) throw new MissingComponentException("Display component not found.");

            // Generate the terrain mesh based on the noise map and render it
            MeshGenerator.Generate3DTerrainMesh(noiseMap, meshHeightValue, meshHeightCurve, levelOfDetail);

            // Generate the texture for the terrain based on the color map
            Texture3D texture = TextureGenerator.ThreeDimensional.TextureFromColourMap(mapColor, ChunkSize, ChunkSize, ChunkSize);

            // Draw the mesh map using the display object (3D)
            display.DrawMeshMap(MeshGenerator.MeshData, texture);
        }
    }
}
