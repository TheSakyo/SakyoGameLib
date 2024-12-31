using System;
using SakyoGame.Lib.Enums;
using UnityEngine;

namespace SakyoGame.Lib.Structs {

    /**
     * <summary>
     *  Struct for storing terrain type information, including name, height, and color.
     * </summary>
     */
    [Serializable]
    public struct TerrainType : IEquatable<TerrainType> {

        /************************/
        /****** PROPERTIES ******/
        /************************/

        /**
         * <summary>
         *   The name of the terrain type.
         * </summary>
         */
        public string name;

        /**
         * <summary>
         *   The minimum height value that determines where this terrain type starts to apply.
         * </summary>
         */
        public float minHeight;

        /**
         * <summary>
         *   The maximum height value that determines where this terrain type stops applying.
         * </summary>
         */
        public float maxHeight;

        /**
         * <summary>
         *   The color associated with this terrain type, used for visual representation.
         * </summary>
         */
        public Color color;

        /****************************/
        /****** PUBLIC METHODS ******/
        /****************************/

        /**
         * <summary>
         *   Checks if the terrain width matches the given comparison type and value.
         * </summary>
         * <param name="comparisonType">The type of comparison to perform.</param>
         * <param name="width">The width to compare against.</param>
         * <returns>True if the comparison is true, false otherwise.</returns>
         */
        public bool CompareWidth(EComparisonType comparisonType, float width) {

            float terrainWidth = maxHeight - minHeight; // Calculate the terrain width

            return comparisonType switch {

                // Check if the terrain width is equal to the given width
                EComparisonType.Equal => Mathf.Approximately(terrainWidth, width),

                // Check if the terrain width is not equal to the given width
                EComparisonType.NotEqual => !Mathf.Approximately(terrainWidth, width),

                // Check if the terrain width is greater than the given width
                EComparisonType.Greater => terrainWidth > width,

                // Check if the terrain width is greater than or equal to the given width
                EComparisonType.GreaterOrEqual => terrainWidth >= width,

                // Check if the terrain width is less than the given width
                EComparisonType.Less => terrainWidth < width,

                // Check if the terrain width is less than or equal to the given width
                EComparisonType.LessOrEqual => terrainWidth <= width,

                _ => false // Return false if the comparison type is invalid
            };
        }

        /*********************************/
        /****** IMPLEMENTED METHODS ******/
        /*********************************/

        /**
         * <summary>
         *   Compares two <see cref="TerrainType"/> objects for equality.
         * </summary>
         */
        public bool Equals(TerrainType other) {

            /*
             * Return true if the name, minHeight, maxHeight, and color are equal,
             * false otherwise.
             */
            return name == other.name &&
                   Mathf.Approximately(minHeight, other.minHeight) &&
                   Mathf.Approximately(maxHeight, other.maxHeight) &&
                   color.Equals(other.color);
        }

        /**
         * <summary>
         *   Compares the current <see cref="TerrainType"/> object with another object for equality.
         * </summary>
         */
        public override bool Equals(object obj) { return obj is TerrainType other && Equals(other); }

        /**
         * <summary>
         *   Returns the hash code for the current <see cref="TerrainType"/> object.
         * </summary>
         */
        public override int GetHashCode() {

            int hash = 17; // Initialize the hash code
            hash = hash * 23 + (name != null ? name.GetHashCode() : 0); // Calculate the hash code for the name
            hash = hash * 23 + minHeight.GetHashCode(); // Calculate the hash code for the minHeight
            hash = hash * 23 + maxHeight.GetHashCode(); // Calculate the hash code for the maxHeight
            hash = hash * 23 + color.GetHashCode();  // Calculate the hash code for the color
            return hash; // Return the calculated hash code
        }
    }
}