using System;
using SakyoGame.Lib.Managers;
using SakyoGame.Lib.Shared.Attributes;
using SakyoGame.Lib.Structs;
using UnityEngine;

namespace SakyoGame.Lib.Maps.Meshes {

    /**
     * <summary>
     *  Class for generating mesh data and prefabs for the game world.
     * </summary>
     */
    public static class MeshGenerator {

        /*******************************/
        /****** GETTERS & SETTERS ******/
        /********************************/

        /**
         * <summary>
         *  Getter for the generated mesh data.
         * </summary>
         */
        public static MeshData MeshData { get; private set; }
        
        /****************************/
        /****** PUBLIC METHODS ******/
        /****************************/

        /**
         * <summary>
         *  Generates a terrain mesh based on the provided heightMap and various parameters.
         *  The mesh is created with a level of detail and includes proper terrain height adjustments using a height curve.
         * </summary>
         * <param name="heightMap">A 2D array representing the height values of the terrain at each point.</param>
         * <param name="meshHeightValue">A multiplier to scale the mesh height values.</param>
         * <param name="heightCurve">An AnimationCurve used to evaluate and transform heightMap values.</param>
         * <param name="levelOfDetail">The level of detail (LOD) determines the density of the mesh vertices.</param>
         */
        public static void Generate2DTerrainMesh(float[,] heightMap, float meshHeightValue, AnimationCurve heightCurve, int levelOfDetail) {

            // Create a new MeshData object
            MeshData = new (heightMap);

            int width = MeshData.Width; // Get the width of the height map in the mesh data
            int height = MeshData.Height; // Get the height of the height map in the mesh data

            float halfWidth = (width - 1) / -2f; // Calculate the half width of the mesh
            float halfHeight = (height - 1) / -2f; // Calculate the half height of the mesh

            // Determine the increment based on the level of detail
            int meshIncrement = levelOfDetail == 0 ? 1 : levelOfDetail * 2;

            // Number of vertices per row in relation to LOD
            int newVertices = (width - 1) / meshIncrement + 1;

            MeshData.Width = newVertices;
            MeshData.Height = newVertices;

            // Index for vertices
            int vertexIndex = 0;

            /*
             * Iterate through the heightMap and calculate vertex positions and texture UVs
             */
            for(int y = 0; y < height; y += meshIncrement) {
                for(int x = 0; x < width; x += meshIncrement) {

                    // Set the vertex position based on heightMap value and apply heightCurve scaling
                    MeshData.Vertices[vertexIndex] = new Vector3(halfWidth + x, heightCurve.Evaluate(heightMap[x, y]) * meshHeightValue, halfHeight - y);

                    // Set the UV coordinates for texturing
                    MeshData.Uvs[vertexIndex] = new Vector2(x / (float)width, y / (float)height);

                    /*
                     * Add triangles to the mesh (using indices of vertices)
                     */
                    if(x < width - 1 && y < height - 1) {

                        // Add the first triangle: indices are vertexIndex, vertexIndex + newVertices + 1, vertexIndex + newVertices
                        MeshData.AddTriangle(vertexIndex, vertexIndex + newVertices + 1, vertexIndex + newVertices);

                        // Add the second triangle: indices are vertexIndex + newVertices + 1, vertexIndex, vertexIndex + 1
                        MeshData.AddTriangle(vertexIndex + newVertices + 1, vertexIndex, vertexIndex + 1);
                    }

                    vertexIndex++; // Move to the next vertex
                }
            }
        }

        /**
         * <summary>
         *  Generates a 3D terrain mesh based on the provided volumeMap and various parameters.
         *  The mesh is created with a level of detail and includes proper terrain height adjustments using a height curve.
         * </summary>
         * <param name="volumeMap">A 3D array representing the volume values of the terrain at each point.</param>
         * <param name="meshHeightValue">A multiplier to scale the mesh height values.</param>
         * <param name="heightCurve">An AnimationCurve used to evaluate and transform volumeMap values.</param>
         * <param name="levelOfDetail">The level of detail (LOD) determines the density of the mesh vertices.</param>
         */
        [Beta("This method is still in development and may not be fully functional.")]
        public static void Generate3DTerrainMesh(float[,,] volumeMap, float meshHeightValue, AnimationCurve heightCurve, int levelOfDetail) {

            // Create a new MeshData object
            MeshData = new (volumeMap);

            int width = MeshData.Width; // Get the width of the height map in the mesh data
            int height = MeshData.Height; // Get the height of the height map in the mesh data
            int depth = MeshData.Depth; // Get the depth of the volume map in the mesh data

            float halfWidth = (width - 1) / -2f;
            float halfDepth = (depth - 1) / -2f;

            // Determine the increment based on the level of detail
            int meshIncrement = levelOfDetail == 0 ? 1 : levelOfDetail * 2;

            // Number of vertices per row in relation to LOD
            int newVertices = (width - 1) / meshIncrement + 1;

            MeshData.Width = newVertices;
            MeshData.Height = newVertices;
            MeshData.Depth = newVertices;

            // Index for vertices
            int vertexIndex = 0;

            /*
             * Iterate through the volumeMap and calculate vertex positions and texture UVs
             */
            for(int z = 0; z < depth; z += meshIncrement) {
                for(int y = 0; y < height; y += meshIncrement) {
                    for(int x = 0; x < width; x += meshIncrement) {

                        // Set the vertex position based on volumeMap value and apply heightCurve scaling
                        MeshData.Vertices[vertexIndex] = new Vector3(halfWidth + x, heightCurve.Evaluate(volumeMap[x, y, z]) * meshHeightValue, halfDepth - z);

                        // Set the UV coordinates for texturing
                        MeshData.Uvs[vertexIndex] = new Vector2(x / (float)width, y / (float)height);

                        /*
                         * Add triangles to the mesh (using indices of vertices)
                         */
                        if(x < width - 1 && y < height - 1 && z < depth - 1) {

                            // Add the first triangle: indices are vertexIndex, vertexIndex + newVertices + 1, vertexIndex + newVertices
                            MeshData.AddTriangle(vertexIndex, vertexIndex + newVertices + 1, vertexIndex + newVertices);

                            // Add the second triangle: indices are vertexIndex + newVertices + 1, vertexIndex, vertexIndex + 1
                            MeshData.AddTriangle(vertexIndex + newVertices + 1, vertexIndex, vertexIndex + 1);
                        }

                        vertexIndex++; // Move to the next vertex
                    }
                }
            }
        }
        /**
         * <summary>
         *  Spawns prefabs in the game world based on the provided regions and resetPrefabsOnRespawn flag.
         *  This method uses the SpawnManager class to spawn prefabs in the world.
         * </summary>
         * <param name="regions">An array of regions to spawn prefabs in.</param>
         * <param name="resetPrefabsOnRespawn">A flag indicating whether to reset prefabs on respawn.</param>
         * <exception cref="NullReferenceException">Thrown when the mesh data is null.</exception>
         */
        public static void SpawnPrefabs(TerrainType[] regions, bool resetPrefabsOnRespawn) {

            // Throw an exception if the mesh data is null
            if(MeshData == null) throw new NullReferenceException("Mesh data is null, please generate a mesh first.");

            // Save the resetPrefabsOnRespawn flag of the 'SpawnManager'
            SpawnManager.ResetPrefabsOnRespawn = resetPrefabsOnRespawn;

            // Spawn prefabs in the game world using the 'SpawnManager'
            SpawnManager.SpawnAllPrefabsInWorld(MeshData, regions);
        }
    }
}