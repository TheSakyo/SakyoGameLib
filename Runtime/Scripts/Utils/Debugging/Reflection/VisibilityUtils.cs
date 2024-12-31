using System.Reflection;

namespace SakyoGame.Lib.Utils.Debugging.Reflection {

    /**
     * <summary>
     *  Utility class for reflection visibility.
     * </summary>
     */
   public class VisibilityUtils {

       /**
        * <summary>
        *  Determines whether a member is public, private, etc.
        * </summary>
        * <param name="member">The member to get the visibility of.</param>
        * <returns>The visibility of the member.</returns>
        */
       public static string GetMemberVisibility(MemberInfo member) {

           /*
            * Return the visibility of the member
            */
           return member switch {

               // Return the visibility of the constructor
               ConstructorInfo constructor => GetConstructorVisibility(constructor),

               // Return the visibility of the field
               FieldInfo field => GetFieldVisibility(field),

               // Return the visibility of the property
               PropertyInfo property => GetAccessorsVisibility(property),

               // Return the visibility of the method
               MethodInfo method => GetMethodVisibility(method),

               // Return the visibility of the event
               EventInfo eventInfo => GetEventVisibility(eventInfo),

               _ => "Unknown\n" // Otherwise, return "Unknown"
           };
       }

       /**
        * <summary>
        *  Determines whether a constructor is public, private, etc (<see cref="ConstructorInfo"/>).
        * </summary>
        * <param name="constructor">The constructor to get the visibility of.</param>
        * <returns>The visibility of the constructor.</returns>
        */
       public static string GetConstructorVisibility(ConstructorInfo constructor) {

           // Start the result string with "Constructor Visibility:"
           string result = "Constructor Visibility: ";

           // Check if the constructor is public
           if(constructor.IsPublic) result += "Public, ";

           // Check if the constructor is private
           if(constructor.IsPrivate) result += "Private, ";

           // Check if the constructor is protected
           if(constructor.IsFamily) result += "Protected, ";

           // Check if the constructor is internal
           if(constructor.IsAssembly) result += "Internal, ";

           // Check if the constructor is protected internal
           if(constructor.IsFamilyOrAssembly) result += "Protected-Internal";

           // Add "Unknown" if the result is empty
           if(!result.EndsWith("Constructor Visibility: ")) result += "Unknown";

           return result + "\n"; // Return the result with a new line
       }

       /**
        * <summary>
        *  Determines whether a field is public, private, etc (<see cref="FieldInfo"/>).
        * </summary>
        * <param name="field">The field to get the visibility of.</param>
        * <returns>The visibility of the field.</returns>
        */
       public static string GetFieldVisibility(FieldInfo field) {

            // Start the result string with "Field Visibility:"
            string result = "Field Visibility: ";

            // Check if the field is public
            if(field.IsPublic) result += "Public, ";

            // Check if the field is private
            if(field.IsPrivate) result += "Private, ";

            // Check if the field is protected
            if(field.IsFamily) result += "Protected, ";

            // Check if the field is internal
            if(field.IsAssembly) result += "Internal, ";

            // Check if the field is protected internal
            if(field.IsFamilyOrAssembly) result += "Protected-Internal";

            // Add "Unknown" if the result is empty
            if(!result.EndsWith("Field Visibility: ")) result += "Unknown";

            return result + "\n"; // Return the result
        }

        /**
         * <summary>
         *  Determines whether a property is public, private, etc (<see cref="PropertyInfo"/>).
         * </summary>
         * <param name="property">The property to get the accessors information of.</param>
         * <returns>The accessors information of the property.</returns>
         */
       public static string GetAccessorsVisibility(PropertyInfo property) {

            // Initialize the result string with "Neither Getter nor Setter are available" by default
            string result = "Neither Getter nor Setter are available";

            MethodInfo getMethod = property.GetMethod; // Get the 'GetMethod' of the property
            MethodInfo setMethod = property.SetMethod; // Get the 'SetMethod' of the property

            /*
             * Check if the 'GetMethod' or 'SetMethod' are available
             */
            if(getMethod != null && setMethod != null) {

                // Set the result string with the 'GetMethod' and 'SetMethod' visibility together
                result = GetMethodVisibility(getMethod) + GetMethodVisibility(setMethod);
            }

            // Check if the 'GetMethod' only is available, then set the result string
            if(getMethod != null) result = GetMethodVisibility(getMethod);

            // Check if the 'SetMethod' only is available, then set the result string
            if(setMethod != null) result = GetMethodVisibility(setMethod);

            return result + "\n"; // Return the result
        }

        /**
         * <summary>
         *  Determines whether a method is public, private, etc (<see cref="MethodInfo"/>).
         * </summary>
         * <param name="method">The method to get the visibility of.</param>
         * <returns>The visibility of the method.</returns>
         */
        public static string GetMethodVisibility(MethodInfo method) {

            // Start the result string with "Method Visibility:"
            string result = "Method Visibility of " + method.Name + ": ";

            // Check if the method is public
            if(method.IsPublic) result += "Public, ";

            // Check if the method is private
            if(method.IsPrivate) result += "Private, ";

            // Check if the method is protected
            if(method.IsFamily) result += "Protected, ";

            // Check if the method is internal
            if(method.IsAssembly) result += "Internal, ";

            // Check if the method is protected internal
            if(method.IsFamilyOrAssembly) result += "Protected-Internal";

            // Add "Unknown" if the result is empty
            if(!result.EndsWith("Method Visibility: ")) result += "Unknown";

            return result + "\n"; // Return the result with the new line
        }


        /**
         * <summary>
         *  Determines whether an event method is public, private, etc (<see cref="EventInfo"/>).
         * </summary>
         * <param name="eventInfo">The event to get the visibility of.</param>
         * <returns>The visibility of the event.</returns>
         */
        public static string GetEventVisibility(EventInfo eventInfo) {

            // Initialize the result string with "No event is available" by default
            string result = "No event is available";

            // Check if the 'AddMethod' are available, then set the result string
            if(eventInfo.AddMethod != null) result = GetMethodVisibility(eventInfo.AddMethod);

            /*
             * Check if the 'RemoveMethod' are available, then set the result string
             */
            if(eventInfo.RemoveMethod != null) {

                CheckResultEvent(result); // Check the result of the event method for making result string better

                // Set the result string with the 'RemoveMethod' method, getting the visibility
                result += GetMethodVisibility(eventInfo.RemoveMethod);
            }

            /*
             * Check if the 'RaiseMethod' are available, then set the result string,
             */
            if(eventInfo.RaiseMethod != null) {

                CheckResultEvent(result); // Check the result of the event method for making result string better

                // Set the result string with the 'RaiseMethod' method, getting the visibility
                result += GetMethodVisibility(eventInfo.RaiseMethod);
            }

            /*
             * Loop through the other event methods and add them to the result
             */
            foreach(MethodInfo method in eventInfo.GetOtherMethods(true)) {

                CheckResultEvent(result); // Check the result of the event method for making result string better

                // Set the result string with the current other event method, getting the visibility
                result += GetMethodVisibility(method);
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

            // If result is "No event is available", set it to an empty string
            if(result == "No event is available") result = "";
            return result + "\n"; // Return the result with the new line
        }
    }
}