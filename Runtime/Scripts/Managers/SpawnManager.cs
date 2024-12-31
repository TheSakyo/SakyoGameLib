using SakyoGame.Lib.Interfaces.Actors;
using SakyoGame.Lib.Maps.Meshes;
using SakyoGame.Lib.Structs;
using SakyoGame.Lib.Actors;

namespace SakyoGame.Lib.Managers {
    
    /**
     * <summary>
     *  Class for managing all spawnable entities in the game world.
     * </summary>
     */
    public static class SpawnManager {

        /*******************************/
        /****** GETTERS & SETTERS ******/
        /*******************************/

        /**
         * <summary>
         *  Gets or sets a value indicating whether to reset prefabs on respawn.
         * </summary>
         */
        public static bool ResetPrefabsOnRespawn { get; set; }

        /***************************/
        /****** PUBLIC METHOD ******/
        /***************************/

        /**
         * <summary>
         *  Spawns all prefabs in the game world (<see cref="BasePrefab"/>).
         * </summary>
         * <param name="meshData">The mesh data for the game world.</param>
         * <param name="regions">An array of <see cref="TerrainType"/> structs containing information about terrain types.</param>
         */
        public static void SpawnAllPrefabsInWorld(MeshData meshData, TerrainType[] regions) {

            foreach(IPrefab prefab in PrefabManager.GetAllPrefabs())
                SpawnPrefabInWorld(prefab, meshData, regions);
        }

        /**
         * <summary>
         *  Destroys all prefabs in the game world (<see cref="BasePrefab"/>).
         * </summary>
         */
        public static void UnSpawnAllPrefabsInWorld() {

            foreach(IPrefab prefab in PrefabManager.GetAllPrefabs())
                UnSpawnPrefabInWorld(prefab);
        }

        /**
         * <summary>
         *  Spawns a prefab in the game world (<see cref="BasePrefab"/>).
         * </summary>
         * <param name="prefab">The prefab to spawn.</param>
         * <param name="meshData">The mesh data for the game world.</param>
         * <param name="regions">An array of <see cref="TerrainType"/> structs containing information about terrain types.</param>
         */
        public static void SpawnPrefabInWorld(IPrefab prefab, MeshData meshData, TerrainType[] regions) {

            // Reset the prefab if 'resetPrefabs' flag is true
            if(ResetPrefabsOnRespawn) UnSpawnPrefabInWorld(prefab);

            // Try to spawn the prefab
            prefab.TrySpawn(meshData, regions);
        }

        /**
         * <summary>
         *  Destroys a prefab in the game world (<see cref="BasePrefab"/>).
         * </summary>
         * <param name="prefab">The prefab to destroy.</param>
         */
        public static void UnSpawnPrefabInWorld(IPrefab prefab) { prefab.TryUnSpawn(); }
    }
}