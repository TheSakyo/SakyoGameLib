using System;
using UnityEngine;

namespace SakyoGame.Lib.Utils.Debugging {

    /**
     * <summary>
     *  Utility class for debugging logging.
     * </summary>
     */
    public static class DebugUtils {

        /**
         * <summary>
         *  Logs object values (can be a string to send a message).
         *  The text displayed in the console will be in white.
         * </summary>
         * <param name="values">Values to log.</param>
         */
        public static void Log(params object[] values) {

            // Loop through the values and log them as warnings
            foreach(object obj in values) Log(obj, LogType.Log);
        }

        /**
         * <summary>
         *  Logs warning object values (can be a string to send a message).
         *  The text displayed in the console will be in yellow.
         * </summary>
         * <param name="values">Values to log.</param>
         */
        public static void LogWarning(params object[] values) {

            // Loop through the values and log them as warnings
            foreach(object obj in values) Log(obj, LogType.Warning);
        }

        /**
         * <summary>
         *  Logs error objects values (can be a string to send a message).
         *  The text displayed in the console will be in red.
         * </summary>
         * <param name="values">Values to log.</param>
         */
        public static void LogError(params object[] values) {

            // Loop through the values and log them as warnings
            foreach(object obj in values) Log(obj, LogType.Error);
        }

        /**
         * <summary>
         *  Logs an exception of type T with the given error message.
         *  The text displayed in the console will be in red.
         * </summary>
         * <param name="errorMessage">The error message to log.</param>
         * <typeparam name="T">The type of the exception to log.</typeparam>
         * <exception cref="NotSupportedException">Thrown when the type T does not have a constructor that takes a string.</exception>
         */
        public static void LogException<T>(string errorMessage) where T : Exception {

            /*
             * If T has a constructor that takes a string, create a new instance of T
             */
            if(typeof(T).GetConstructor(new[] { typeof(string) }) != null) {

                // Create a new instance of T, passing the error message
                Exception exception = (T)Activator.CreateInstance(typeof(T), errorMessage);
                Debug.unityLogger.LogException(exception); // Log the exception

            /*
             * If T does not have a constructor that takes a string, throw an exception
             */
            } else {

                // Create the exception for the not supported type and log it
                NotSupportedException notSupported = new($"The type {typeof(T).Name} does not have a constructor that takes a string");

                Debug.unityLogger.LogException(notSupported); // Log the exception
                throw notSupported; // Throw the exception
            }
        }

        private static void Log(object obj, LogType logType) {

            string message = obj.ToString(); // Convert the object to a string

            // If the object is not a string, get its reflection as JSON
            if(obj is not string) message = ObjectUtils.GetObjectAsJsonWithReflection(obj);

            Debug.unityLogger.Log(logType, message); // Log the warning message with the message
        }
    }
}