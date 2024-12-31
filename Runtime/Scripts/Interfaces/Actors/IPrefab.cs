using SakyoGame.Lib.Maps.Meshes;
using SakyoGame.Lib.Structs;
using SakyoGame.Lib.Actors;

namespace SakyoGame.Lib.Interfaces.Actors {

    /**
     * <summary>
     *  Interface for prefabs (<see cref="BasePrefab"/>).
     * </summary>
     */
    public interface IPrefab {

        /**
         * <summary>
         * The minimum distance between two same prefab can be spawned.
         * </summary>
         */
        float MinimumDistance { get; }

        /**
        * <summary>
        *  Tries spawns prefabs at random positions on the mesh based on the provided height map and mesh data.
        *  Prefabs are instantiated only if the number of prefabs is below the specified limit.
        * </summary>
        * <param name="meshData">The mesh data containing information about the terrain.</param>
         * <param name="regions">An array of <see cref="TerrainType"/> structs containing information about terrain types.</param>
        */
        public void TrySpawn(MeshData meshData, TerrainType[] regions);

        /**
          * <summary>
          *  Tries destroy prefabs.
          * </summary>
          */
        public void TryUnSpawn();
    }
}