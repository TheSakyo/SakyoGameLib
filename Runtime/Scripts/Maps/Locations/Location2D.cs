using SakyoGame.Lib.Interfaces;
using SakyoGame.Lib.Structs;
using UnityEngine;

namespace SakyoGame.Lib.Maps.Locations {

    /**
     * <summary>
     *  Class representing a 2D location in the game world.
     *  This class manages the spawning of prefabs based on the location.
     * </summary>
     */
    public class Location2D : ILocation<Vector2> {

        /*******************************/
        /****** GETTERS & SETTERS ******/ 
        /*******************************/

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
         *  The current velocity of the location.
         * </summary>
         */
        public Vector2 Velocity { get; set; }

        /**
         * <summary>
         *  The direction of the location.
         * </summary>
         */
        public Vector2 Direction { get; set; }

        /**************************/
        /****** CONSTRUCTORS ******/
        /**************************/

        /**
         * <summary>
         *  Constructor for the Location2D class.
         *  Initializes the location with specified coordinates, velocity, and direction.
         * </summary>
         * <param name="x">The X coordinate of the location.</param>
         * <param name="y">The Y coordinate of the location.</param>
         * <param name="velocity">The current velocity of the location.</param>
         * <param name="direction">The direction of the location.</param>
         */
        public Location2D(float x = 0f, float y = 0f, Vector2 velocity = default, Vector2 direction = default) {

            X = x; Y = y; // Set the coordinates
            Velocity = velocity; // Set the velocity
            Direction = direction; // Set the direction
        }

        /****************************/
        /****** PUBLIC METHODS ******/
        /****************************/

        /**
         * <summary>
         *  Method to check if the location is within a specific region.
         *  This can be used to determine if an object should spawn based on the terrain's height range.
         * </summary>
         * <param name="region">The region to check the location against.</param>
         * <returns>True if the location is within the region's height range, false otherwise.</returns>
         */
        public bool IsInRegion(TerrainType region) {

            // Check if the location's Y coordinate is within the region's height range
            return Y >= region.minHeight && Y <= region.maxHeight;
        }

        /**
         * <summary>
         *  Method to calculate the distance between two locations in 2D space.
         *  This can be useful for spawn logic or any other game mechanics that depend on distances.
         * </summary>
         * <param name="otherLocation">The other location to calculate the distance to.</param>
         * <returns>The distance between this location and the other location.</returns>
         */
        public float DistanceTo(ILocation<Vector2> otherLocation) {

            // Calculate the distance between the two locations using the Euclidean distance formula
            return Vector2.Distance(new Vector2(X, Y), new Vector2(otherLocation.X, otherLocation.Y));
        }
    }
}
