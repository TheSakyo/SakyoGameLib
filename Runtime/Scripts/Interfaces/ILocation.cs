using SakyoGame.Lib.Structs;

namespace SakyoGame.Lib.Interfaces {

    /**
     * <summary>
     *  Interface for locations in the game world.
     * </summary>
     * <typeparam name="T">The Vector type of the location.</typeparam>
     */
    public interface ILocation<T> where T : struct {

        /**
         * <summary>
         *  The X coordinate of the location.
         * </summary>
         */
        public float X { get; set; }

        /**
         * <summary>
         *  The Y coordinate of the location.
         * </summary>
         */
        public float Y { get; set; }

        /**
         * <summary>
         *  The Z coordinate of the location (optional).
         * </summary>
         */
        public float Z {

            get => throw new System.NotSupportedException("Getting the value of Z is not supported.");
            set => throw new System.NotSupportedException("Setting the value of Z is not supported.");
        }

        /**
          * <summary>
          *  The current velocity of the location.
          * </summary>
          */
        public T Velocity { get; set; }

        /**
         * <summary>
         *  The direction of the location.
         * </summary>
         */
        public T Direction { get; set; }

        /*** ___________________________________________________ ***/
        /*** ___________________________________________________ ***/

        /**
         * <summary>
         *  Method to check if the location is within a specific region.
         *  This can be used to determine if an object should spawn based on the terrain's height range.
         * </summary>
         * <param name="region">The region to check the location against.</param>
         * <returns>True if the location is within the region's height range, false otherwise.</returns>
         */
        public bool IsInRegion(TerrainType region);

        /**
         * <summary>
         *  Method to calculate the distance between two locations in 3D space.
         *  This can be useful for spawn logic or any other game mechanics that depend on distances.
         * </summary>
         * <param name="otherLocation">The other location to calculate the distance to.</param>
         * <returns>The distance between this location and the other location.</returns>
         */
        public float DistanceTo(ILocation<T> otherLocation);
    }
}
