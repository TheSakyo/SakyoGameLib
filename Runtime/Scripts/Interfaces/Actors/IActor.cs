using SakyoGame.Lib.Maps;
using SakyoGame.Lib.Maps.Locations;

namespace SakyoGame.Lib.Interfaces.Actors {

    /**
     * <summary>
     *  Interface for actors in the game world.
     * </summary>
     */
    public interface IActor {

        /**
         * <summary>
         *  Checks if an actor is spawned (e.g., if the game object has been instantiated).
         * </summary>
         */
        bool IsSpawned { get; set; }


        /**
         * <summary>
         *  The <see cref="Location2D"/> of the actor (optional).
         * </summary>
         */
        public Location2D Location2D {

            get => throw new System.NotSupportedException("Getting the value of 2D Location is not supported.");
            set => throw new System.NotSupportedException("Setting the value of 2D Location is not supported.");
        }


        /**
         * <summary>
         *  The <see cref="Location3D"/> of the actor (optional).
         * </summary>
         */
        public Location3D Location3D {

            get => throw new System.NotSupportedException("Getting the value of 3D Location is not supported.");
            set => throw new System.NotSupportedException("Setting the value of 3D Location is not supported.");
        }
    }
}