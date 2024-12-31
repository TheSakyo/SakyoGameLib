using System;
using System.Reflection;
using System.Collections.Generic;
using SakyoGame.Lib.Utils.Debugging.Reflection;

namespace SakyoGame.Lib.Utils.Debugging {

    /**
     * <summary>
     *  Utility class for objects debugging.
     * </summary>
     */
    public static class ObjectUtils {


        /**
         * <summary>
         *  Gets the object as JSON with reflection.
         * </summary>
         * <param name="obj">The object to get as JSON.</param>
         * <returns>The object as JSON with reflection.</returns>
         */
        public static string GetObjectAsJsonWithReflection(object obj) {

            string json = ""; // Initialize the JSON string

            // Get a tabulation string, can be used for indentation in the JSON string
            const string tabulation = "     ";

            // Use BindingFlags to include all types of members (public, non-public, static, instance)
            const BindingFlags bindingFlags = BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance | BindingFlags.Static;

            // Get the type of the object
            Type type = obj.GetType();

            // Get all members of the type (fields, methods, properties, etc.)
            MemberInfo[] members = type.GetMembers(bindingFlags);

            // Add the prefix to the JSON string if there are members
            if(members.Length > 0) json += "Members of " + type.Name + ":\n";

            /*
             * Loop through all members and add them to the JSON string
             */
            for(int i = 0; i < members.Length; i++) {

                json += "   " + i + " {\n";

                // Add a comma to the JSON string if it is not the last member
                string endLineOrComma = i == members.Length - 1 ? "\n" : ",\n";

                string name = "Name : " + members[i].Name + "\n"; // Get the name of the member
                string typeString = "Type : " + members[i].MemberType + "\n"; // Get the type of the member

                // Get the visibility of the member
                string visibility = VisibilityUtils.GetMemberVisibility(members[i]);

                // Get the static status of the member
                string isStatic = StaticUtils.IsMemberStatic(members[i]);

                /*
                 * Add the member to the JSON string
                 */
                json += tabulation + name + // Add the name of the member
                        tabulation + typeString + // Add the type of the member
                        tabulation + visibility + // Add the visibility of the member
                        tabulation + isStatic + // Add the static status of the member
                        "   " + "}" + endLineOrComma; // Add a closing bracket
            }

            return json; // Return the JSON string
        }

        /**
         * <summary>
         *  Returns the reflection type name of the object.
         * </summary>
         * <param name="obj">The object to get the reflection type of.</param>
         * <returns>The reflection type name of the object.</returns>
         */
        public static string GetReflectionTypeName(object obj) {

            /*
             * Return the type name of the object
             */
            return obj switch {

                FieldInfo => "Field", // Return the type name of the field
                PropertyInfo => "Property", // Return the type name of the property
                EventInfo => "Event", // Return the type name of the event
                ParameterInfo => "Parameter", // Return the type name of the parameter
                MethodInfo => "Method", // Return the type name of the method
                ConstructorInfo => "Constructor", // Return the type name of the constructor
                Delegate => "Delegate", // Return the type name of the delegate
                _ => "Unknown" // Return "Unknown"
            };
        }


        public static string GetReflectionMoreInformations(object obj) {

            // If the object is null, throw an exception
            if(obj == null) throw new ArgumentNullException(nameof(obj));
            Type type = obj.GetType(); // Get the type of the object

            /*
             * Create a dictionary of type checks and their names
             */
            Dictionary<Func<Type, bool>, string> typeChecks = new Dictionary<Func<Type, bool>, string> {

                // Check if the type is abstract
                { t => t.IsAbstract, "Abstract" },

                // Check if the type is ansi class
                { t => t.IsAnsiClass, "AnsiClass" },

                // Check if the type is an array
                { t => t.IsArray, "Array" },

                // Check if the type is auto class
                { t => t.IsAutoClass, "AutoClass" },

                // Check if the type is auto layout
                { t => t.IsAutoLayout, "AutoLayout" },

                // Check if the type is by ref
                { t => t.IsByRef, "ByRef" },

                // Check if the type is by ref like
                { t => t.IsByRefLike, "ByRefLike" },

                // Check if the type is a class
                { t => t.IsClass, "Class" },

                // Check if the type is a COM object
                { t => t.IsCOMObject, "COMObject" },

                // Check if the type is constructed generic type
                { t => t.IsConstructedGenericType, "ConstructedGenericType" },

                // Check if the type is a contextful
                { t => t.IsContextful, "Contextful" },

                // Check if the type is an enum
                { t => t.IsEnum, "Enum" },

                // Check if the type is explicit layout
                { t => t.IsExplicitLayout, "ExplicitLayout" },

                // Check if the type is a generic method parameter
                { t => t.IsGenericMethodParameter, "GenericMethodParameter" },

                // Check if the type is a generic parameter
                { t => t.IsGenericParameter, "GenericParameter" },

                // Check if the type is generic type definition
                { t => t.IsGenericType, "GenericType" },

                // Check if the type is generic type definition
                { t => t.IsGenericTypeDefinition, "GenericTypeDefinition" },

                // Check if the type is import
                { t => t.IsImport, "Import" },

                // Check if the type is an interface
                { t => t.IsInterface, "Interface" },

                // Check if the type is layout sequential
                { t => t.IsLayoutSequential, "LayoutSequential" },

                // Check if the type is marshal by ref
                { t => t.IsMarshalByRef, "MarshalByRef" },

                // Check if the type is nested
                { t => t.IsNested, "Nested" },

                // Check if the type is nested assembly
                { t => t.IsNestedAssembly, "NestedAssembly" },

                // Check if the type is nested fam and assem
                { t => t.IsNestedFamANDAssem, "NestedFamANDAssem" },

                // Check if the type is nested fam or assem
                { t => t.IsNestedFamily, "NestedFamily" },

                // Check if the type is nested fam or assem
                { t => t.IsNestedFamORAssem, "NestedFamORAssem" },

                // Check if the type is nested private
                { t => t.IsNestedPrivate, "NestedPrivate" },

                // Check if the type is nested public
                { t => t.IsNestedPublic, "NestedPublic" },

                // Check if the type is not public
                { t => t.IsNotPublic, "NotPublic" },

                // Check if the type is object
                { t => t.IsPointer, "Pointer" },

                // Check if the type is primitive
                { t => t.IsPrimitive, "Primitive" },

                // Check if the type is public
                { t => t.IsPublic, "Public" },

                // Check if the type is readonly
                { t => t.IsSealed, "Sealed" },

                // Check if the type is security critical
                { t => t.IsSecurityCritical, "SecurityCritical" },

                // Check if the type is security safe critical
                { t => t.IsSecuritySafeCritical, "SecuritySafeCritical" },

                // Check if the type is security transparent
                { t => t.IsSecurityTransparent, "SecurityTransparent" },

                // Check if the type is serializable
                { t => t.IsSerializable, "Serializable" },

                // Check if the type is signed
                { t => t.IsSignatureType, "SignatureType" },

                // Check if the type is special name
                { t => t.IsSpecialName, "SpecialName" },

                // Check if the type is string
                { t => t.IsSZArray, "SZArray" },

                // Check if the type is a type definition
                { t => t.IsTypeDefinition, "TypeDefinition" },

                // Check if the type is unicode
                { t => t.IsUnicodeClass, "UnicodeClass" },

                // Check if the type is value type
                { t => t.IsValueType, "ValueType" },

                // Check if the type is variable bound array
                { t => t.IsVariableBoundArray, "VariableBoundArray" },

                // Check if the type is visible
                { t => t.IsVisible, "Visible" }
            };

            /*
             * Get all the type checks and their names in a list of strings.
             */
            List<string> results = new List<string>();

            /*
             * Loop through the type checks and add the name of the check if the type matches
             */
            foreach(KeyValuePair<Func<Type, bool>, string> check in typeChecks) {

                // Check if the type matches, if so, add the name
                if(check.Key(type)) results.Add(check.Value);
            }

            // Return the results as a string joined by a comma with 'Informations:' at the start
            return "Informations: " + string.Join(", ", results);
        }
    }
}