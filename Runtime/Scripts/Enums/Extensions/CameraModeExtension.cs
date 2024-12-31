using System;
using System.Linq;

namespace SakyoGame.Lib.Enums.Extensions {

    /**
     * <summary>
     *  Extension class for the camera mode enumeration (<see cref="ECameraMode"/>).
     * </summary>
     */
    public static class CameraModeExtension {

        /****************************/
        /****** PUBLIC METHODS ******/
        /****************************/

        /**
         * <summary>
         *  Converts a string to a <see cref="ECameraMode"/>.
         * </summary>
         * <param name="cameraMode">The string to convert.</param>
         * <returns>The converted <see cref="ECameraMode"/>.</returns>
         */
        public static ECameraMode GetModeFromString(string cameraMode) =>
            (ECameraMode)Enum.Parse(typeof(ECameraMode), cameraMode, true);

        /**
         * <summary>
         *  Converts a <see cref="ECameraMode"/> to an index.
         * </summary>
         * <param name="cameraMode">The <see cref="ECameraMode"/> to convert.</param>
         * <returns>The index of the <see cref="ECameraMode"/>.</returns>
         */
        public static int GetIndexFromMode(ECameraMode cameraMode) {

            ECameraMode[] values = GetModes(); // Get all camera modes except 'None' and 'Default'

            // Return index of specified camera mode in filtered array
            return Array.IndexOf(values, cameraMode);
        }

        /**
         * <summary>
         *  Converts an index to a <see cref="ECameraMode"/>.
         * </summary>
         * <param name="index">The index to convert.</param>
         * <returns>The converted <see cref="ECameraMode"/>.</returns>
         * <remarks>This method wraps the index around using modulo.</remarks>
         */
        public static ECameraMode GetModeFromIndex(int index) {

            ECameraMode[] values = GetModes(); // Get all camera modes except 'None' and 'Default'

            // Wrap using the modulo index in the filtered array
            int wrappedIndex = index % values.Length;

            // Convert wrapped index to ECameraMode in the filtered array
            return (ECameraMode)values.GetValue(wrappedIndex);
        }

        /**
         * <summary>
         *  Checks if two camera modes are equal.
         * </summary>
         * <param name="mode1">The first camera mode to compare.</param>
         * <param name="mode2">The second camera mode to compare.</param>
         * <returns>True if the camera modes are equal, false otherwise.</returns>
         */
        public static bool AreModesEquals(ECameraMode mode1, ECameraMode mode2) => mode1 == mode2;

        /*****************************/
        /****** PRIVATE METHODS ******/
        /*****************************/

        /**
         * <summary>
         *  Gets all camera modes except None and Default.
         * </summary>
         * <returns>An array of all camera modes except None and Default.</returns>
         */
        private static ECameraMode[] GetModes() {

            /*
             * Get all camera modes with filter removed 'None' and 'Default' modes
             */
            return Enum.GetValues(typeof(ECameraMode))

                // Cast to ECameraMode
                .Cast<ECameraMode>()

                // Filter removed 'None' and 'Default' modes
                .Where(mode => mode != ECameraMode.None && mode != ECameraMode.Default)

                // Convert to array
                .ToArray();
        }
    }
}