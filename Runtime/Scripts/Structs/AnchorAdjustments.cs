using System;

namespace SakyoGame.Lib.Structs {

    /**
     * <summary>
     *  Struct for storing anchor adjustment values.
     * </summary>
     */
    [Serializable]
    public struct AnchorAdjustments {

        /************************/
        /****** PROPERTIES ******/
        /************************/

        /**
         * <summary>
         *  The X-axis adjustment.
         * </summary>
         */
        public float xAxis;

        /**
         * <summary>
         *  The Y-axis adjustment.
         * </summary>
         */
        public float yAxis; // Y-axis adjustment

        /**************************/
        /****** CONSTRUCTORS ******/
        /**************************/

        /**
         * <summary>
         *  Initializes a new instance of the <see cref="AnchorAdjustments"/> struct.
         * </summary>
         * <param name="x">The X-axis adjustment.</param>
         * <param name="y">The Y-axis adjustment.</param>
         */
        public AnchorAdjustments(float x, float y) { xAxis = x; yAxis = y; }
    }
}