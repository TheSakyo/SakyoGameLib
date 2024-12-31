using System.Reflection;

namespace SakyoGame.Lib.Utils.Debugging.Reflection {

    /**
     * <summary>
     *  Utility class for static reflection.
     * </summary>
     */
    public class StaticUtils {

        /**
         * <summary>
         *  Determines whether a member is static or not.
         * </summary>
         * <param name="member">The member to check.</param>
         * <returns>Whether the member is static or not (Return a string).</returns>
         */
        public static string IsMemberStatic(MemberInfo member) {

            /*
             * Check if the type of member is static
             */
            return member switch {

                // Return a string for the constructor, making sure it's static or not
                ConstructorInfo constructor => IsConstructorStatic(constructor),

                // Return a string for the field, making sure it's static or not
                FieldInfo field => IsFieldStatic(field),

                // Return a string for the property, making sure it's static or not
                PropertyInfo property => IsAccessorsStatic(property),

                // Return a string for the method, making sure it's static or not
                MethodInfo method => IsMethodStatic(method),

                // Return a string for the event, making sure it's static or not
                EventInfo eventInfo => IsEventStatic(eventInfo),

                _ => "Unknown\n" // Otherwise, return "Unknown"
            };
        }

        /**
         * <summary>
         *  Determines whether a constructor is static or not (<see cref="ConstructorInfo"/>).
         * </summary>
         * <param name="constructor">The constructor to check.</param>
         * <returns>Whether the constructor is static or not (Return a string).</returns>
         */
        public static string IsConstructorStatic(ConstructorInfo constructor) =>
            "Is Static: " + constructor.IsStatic + "\n";

        /**
         * <summary>
         *  Determines whether a method is static or not (<see cref="MethodInfo"/>).
         * </summary>
         * <param name="method">The method to check.</param>
         * <returns>Whether the method is static or not (Return a string).</returns>
         */
        public static string IsMethodStatic(MethodInfo method) =>
            "Is Static: " + method.IsStatic + "\n";

        /**
         * <summary>
         *  Determines whether a property is static or not (<see cref="PropertyInfo"/>).
         * </summary>
         * <param name="property">The property to check.</param>
         * <returns>Whether the property is static or not (Return a string).</returns>
         */
        public static string IsAccessorsStatic(PropertyInfo property) {

            // Initialize the result string with "Neither Getter nor Setter are available" by default
            string result = "Neither Getter nor Setter are available";

            MethodInfo getMethod = property.GetMethod; // Get the 'GetMethod' of the property
            MethodInfo setMethod = property.SetMethod; // Get the 'SetMethod' of the property

            /*
             * Check if the 'GetMethod' and 'SetMethod' are available
             */
            if(getMethod != null && setMethod != null) {

                /*
                 * Set the result string with the 'GetMethod' and 'SetMethod' methods,
                 * checking that they are static
                 */
                result = "Getter Is Static: " + getMethod.IsStatic + ", " +
                         "Setter Is Static: " + setMethod.IsStatic;
            }

            // Check if the 'GetMethod' only is available, then set the result string
            if(getMethod != null) result = "Getter Is Static: " + getMethod.IsStatic;

            // Check if the 'SetMethod' only is available, then set the result string
            if(setMethod != null) result = "Setter Is Static: " + setMethod.IsStatic;

            return result + "\n"; // Return the result
        }

        /**
         * <summary>
         *  Determines whether a field is static or not (<see cref="FieldInfo"/>).
         * </summary>
         * <param name="field">The field to check.</param>
         * <returns>Whether the field is static or not (Return a string).</returns>
         */
        public static string IsFieldStatic(FieldInfo field) =>
            "Is Static: " + field.IsStatic + "\n";


        /**
         * <summary>
         *  Determines whether an event is static or not (<see cref="EventInfo"/>).
         * </summary>
         * <param name="eventInfo">The event to check.</param>
         * <returns>Whether the event is static or not (Return a string).</returns>
         */
        public static string IsEventStatic(EventInfo eventInfo) {

            // Initialize the result string with "No event is available" by default
            string result = "No event is available";

            // Check if the 'AddMethod' are available, then set the result string
            if(eventInfo.AddMethod != null) result = "Add Event Method Is Static: " + eventInfo.AddMethod.IsStatic;

            /*
             * Check if the 'RemoveMethod' are available, then set the result string
             */
            if(eventInfo.RemoveMethod != null) {

                CheckResultEvent(result); // Check the result of the event method for making result string better

                // Set the result string with the 'RemoveMethod' method, making sure it's static or not
                result += "Remove Event Method Is Static: " + eventInfo.RemoveMethod.IsStatic;
            }

            /*
             * Check if the 'RaiseMethod' are available, then set the result string,
             */
            if(eventInfo.RaiseMethod != null) {

                CheckResultEvent(result); // Check the result of the event method for making result string better

                // Set the result string with the 'RaiseMethod' method, making sure it's static or not
                result += "Raise Event Method Is Static: " + eventInfo.RaiseMethod.IsStatic;
            }

            /*
             * Loop through the other event methods and add them to the result
             */
            foreach(MethodInfo method in eventInfo.GetOtherMethods(true)) {

                CheckResultEvent(result); // Check the result of the event method for making result string better

                // Set the result string with the current other event method, making sure it's static or not
                result += "Event Method '" + method.Name + "' Is Static: " + method.IsStatic;
            }

            return result + "\n"; // Return the result
        }

        /**
         * <summary>
         *  Checks the result of the event method.
         * </summary>
         * <param name="result">The result of the event method.</param>
         * <returns>The result of the event method.</returns>
         */
        private static string CheckResultEvent(string result) {

            // If result is not "No event is available", add a comma
            if(result != "No event is available") result += "\n";

            else result = ""; // If result is "No event is available", set it to an empty string

            return result; // Return the result
        }
    }
}