using SakyoGame.Lib.Interfaces;
using SakyoGame.Lib.Structs;
using UnityEngine;

namespace SakyoGame.Lib.Maps {

    /**
     * <summary>
     *  Class representing a 3D location in the game world.
     *  This class manages the spawning of prefabs based on the location.
     * </summary>
     */
    public class Location3D : ILocation<Vector3> {

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
         *  The Z coordinate of the location.
         * </summary>
         */
        public float Z { get; set; }

        /**
         * <summary>
         *  The Yaw (rotation around the Y-axis) of the location.
         * </summary>
         */
        public float Yaw { get; set; }

        /**
         * <summary>
         *  The Pitch (rotation around the X-axis) of the location.
         * </summary>
         */
        public float Pitch { get; set; }

        /**
         * <summary>
         *  The current velocity of the location '<see cref="Vector3"/>.
         * </summary>
         */
        public Vector3 Velocity { get; set; }

        /**
         * <summary>
         *  The direction of the location '<see cref="Vector3"/>.
         * </summary>
         */
        public Vector3 Direction { get; set; }

        /**
         * <summary>
         *  Method to get the rotation of the location as a Quaternion.
         *  This can be used for object rotation based on Yaw and Pitch.
         * </summary>
         * <returns>A Quaternion representing the rotation of the location based on Yaw and Pitch.</returns>
         */
        public Quaternion Rotation => Quaternion.Euler(Pitch, Yaw, 0f);

        /**************************/
        /****** CONSTRUCTORS ******/
        /**************************/

        /**
         * <summary>
         *  Constructor for the Location3D class.
         *  Initializes the location with specified coordinates and rotations.
         * </summary>
         * <param name="x">The X coordinate of the location.</param>
         * <param name="y">The Y coordinate of the location.</param>
         * <param name="z">The Z coordinate of the location.</param>
         * <param name="yaw">The Yaw (rotation around the Y-axis) of the location.</param>
         * <param name="pitch">The Pitch (rotation around the X-axis) of the location.</param>
         * <param name="velocity">The current velocity of the location.</param>
         * <param name="direction">The direction of the location.</param>
         */
        public Location3D(float x = 0f, float y = 0f, float z = 0f, float yaw = 0f, float pitch = 0f,
            Vector3 velocity = default, Vector3 direction = default) {

                X = x; Y = y; Z = z; // Set the coordinates
                Yaw = yaw; Pitch = pitch; // Set the rotations
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
         *  Method to calculate the distance between two locations in 3D space.
         *  This can be useful for spawn logic or any other game mechanics that depend on distances.
         * </summary>
         * <param name="otherLocation3D">The other location to calculate the distance to.</param>
         * <returns>The distance between this location and the other location.</returns>
         */
        public float DistanceTo(ILocation<Vector3> otherLocation3D) {

            /*
             * Calculate the distance between the two locations using the Euclidean distance formula.
             */
            return Vector3.Distance(new Vector3(X, Y, Z),
                new Vector3(otherLocation3D.X, otherLocation3D.Y, otherLocation3D.Z));
        }
    }
}