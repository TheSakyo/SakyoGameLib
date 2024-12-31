using System;
using UnityEngine;

namespace SakyoGame.Lib.Utils.Math {

    public class ScreenCalculatorUtils {

        /*********************/
        /****** GETTERS ******/
        /*********************/

        /**
         * <summary>
         *   Returns the screen size as a <see cref="Vector2"/>.
         * </summary>
         * <returns>The screen size as a <see cref="Vector2"/>.</returns>
         */
        public static Vector2 ScreenSize => new(Screen.width, Screen.height);

        /****************************/
        /****** PUBLIC METHODS ******/
        /****************************/

        /**
         * <summary>
         *   Calculates the scale factor based on the screen width.
         * </summary>
         * <param name="screenWidth">The width of the target screen.</param>
         * <returns>The calculated scale factor.</returns>
         */
        public static float CalculateScaleFactor(float screenWidth) {

            /*
             * Return the calculated scale factor based on the screen width.
             */
            return screenWidth switch {

               < 800 => 0.5f, // If the screen width is less than 800, return 0.5
               >= 800 and < 1200 => 1f, // If the screen width is between 800 and 1200, return 1
               >= 1200 => 1.5f, // If the screen width is greater than or equal to 1200, return 1.5

               // Throw an exception, if the screen width is unexpected
                _ => throw new ArgumentException("Unexpected screenWidth: " + screenWidth)
            };
        }

        /**
         * <summary>
         *   Calculates the match value based on the screen width and height.
         * </summary>
         * <param name="screenWidth">The width of the target screen.</param>
         * <param name="screenHeight">The height of the target screen.</param>
         * <returns>The calculated match value.</returns>
         */
        public static float CalculateMatchValue(float screenWidth, float screenHeight) {

            float aspectRatio = screenWidth / screenHeight; // Calculate the aspect ratio

            /*
             * Return the calculated match value based on the aspect ratio
             */
            return aspectRatio switch {

                < 1f => 1f, // If the aspect ratio is less than 1, return 1
                >= 1f and <= 1.5f => 0.5f, // If the aspect ratio is between 1 and 1.5, return 0.5
                > 1.5f => 0f, // If the aspect ratio is greater than 1.5, return 0

                // Throw an exception, if the aspect ratio is unexpected
                _ => throw new ArgumentException("Unexpected aspect ratio: " + aspectRatio)
            };
        }

        /**
         * <summary>
         *   Calculates the physical size factor based on the DPI.
         * </summary>
         * <param name="dpi">The DPI of the target screen.</param>
         * <returns>The calculated physical size factor.</returns>
         */
        public static float CalculatePhysicalSizeFactor(float dpi) {

            /*
             * Return the calculated physical size factor based on the DPI
             */
            return dpi switch {

                < 160 => 0.8f, // If the DPI is less than 160, return 0.8
                >= 160 and < 320 => 1f, // If the DPI is between 160 and 320, return 1
                >= 320 => 1.2f, // If the DPI is greater than or equal to 320, return 1.2

                // Throw an exception, if the DPI is unexpected
                _ => throw new ArgumentException("Unexpected dpi: " + dpi)
            };
        }
    }
}