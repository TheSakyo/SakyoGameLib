using System.Linq;
using System.Reflection;
using UnityEngine;

namespace SakyoGame.Lib.Utils.Component {

        /**
         * <summary>
         *  Class for utility methods related to <see cref="Component"/> control.
         * </summary>
         */
        public static class CheckerComponent {

            /**
             * <summary>
             *  Checks if a <see cref="Component"/> of type T is missing on the specified <see cref="GameObject"/>.
             * </summary>
             * <typeparam name="T">The type of the <see cref="Component"/> to check.</typeparam>
             * <param name="gameObject">The <see cref="GameObject"/> to check.</param>
             * <exception cref="MissingComponentException">Thrown when a component is missing.</exception>
             */
            public static void AssertInGameObject<T>(GameObject gameObject) where T : UnityEngine.Component {

                T component = gameObject.GetComponent<T>(); // Get the component of type T
                if(component != null) return; // Return if the component is not null

                // Create the exception with the error message to display
                MissingComponentException missingComponentException = new($"{typeof(T).Name} is missing on {gameObject.name}");

                Debug.unityLogger.LogException(missingComponentException); // Log the exception
                throw missingComponentException; // Throw the exception
            }

            /**
             * <summary>
             *  Checks if a member of type T is missing on the specified <see cref="Component"/>.
             * </summary>
             * <typeparam name="T">The type of the member to check.</typeparam>
             * <param name="componentInstance">The <see cref="Component"/> to check.</param>
             * <param name="memberName">The name of the member to check.</param>
             * <param name="bindingFlags">The binding flags for the member search.</param>
             * <exception cref="MissingComponentException">Thrown when the member is missing.</exception>
             */
            public static void AssertMemberPresence<T>(UnityEngine.Component componentInstance, string memberName,
                BindingFlags bindingFlags) where T : UnityEngine.Component {

                    // Use reflection to trying to get the specified field or property from the 'MonoBehaviour'
                    MemberInfo memberInfo = componentInstance.GetType().GetMember(memberName, bindingFlags).FirstOrDefault();

                    /*
                     * Get the value of the field or property depending on its MonoBehaviour type.
                     * If the value is not found, return null
                     */
                    object valueInfo = memberInfo switch {

                        FieldInfo field => field.GetValue(componentInstance), // Get the value of the field
                        PropertyInfo property => property.GetValue(componentInstance), // Get the value of the property
                        _ => null // Otherwise, return null
                    };

                    /*
                     * If the value is not found, throw an exception
                     */
                    if(valueInfo == null) {

                        // Create the exception for the not found member
                        MissingComponentException notFoundException = new($"Member {memberName} not found in {componentInstance.GetType().Name}.");

                        Debug.unityLogger.LogException(notFoundException); // Log the exception
                        throw notFoundException; // Throw the exception
                    }

                    // Check
                    ValidatePresenceOfMember<T>(componentInstance, memberName, valueInfo);
            }


            /**
             * <summary>
             *  Continuation of the 'AssertMemberPresence' method.Continuation of the 'AssertMemberPresence' method.
             *  It ensures that a member of type T exists on the specified <see cref="Component"/>,
             *  if so, an exception is raised.
             * </summary>
             * <typeparam name="T">The type of the member to check.</typeparam>
             * <param name="componentInstance">The <see cref="Component"/> to check.</param>
             * <param name="memberName">The name of the member to check.</param>
             * <param name="objInfo"></param>
             * <exception cref="MissingComponentException">Thrown when the member is missing.</exception>
             */
            private static void ValidatePresenceOfMember<T>(UnityEngine.Component componentInstance, string memberName, object objInfo) where T : UnityEngine.Component {

                    /*
                     * Switch case for both individual component and array
                     */
                    switch(objInfo) {

                        /*
                         * If the value is a single component of type T and is null,
                         * throw an exception
                         */
                        case T component:

                            // Break the switch, if the component is not null
                            if(component != null) break;

                            /*
                             * Create the exception for the missing component
                             */
                            MissingComponentException missingException = new($"{memberName} component is missing in " +
                                $"{componentInstance.GetType().Name}.");

                            Debug.unityLogger.LogException(missingException); // Log the exception
                            throw missingException; // Throw the exception

                        /*
                         * If the value is an array of components of type T and is empty or null,
                         * throw an exception
                         */
                        case T[] componentArray:

                            /*
                             * If the array is empty, throw an exception
                             */
                            if(componentArray.Length <= 0) {

                                /*
                                 * Create the exception for the empty array
                                 */
                                MissingComponentException emptyArrayException = new($"{memberName} is an empty array in" +
                                    $" {componentInstance.GetType().Name}.");

                                Debug.unityLogger.LogException(emptyArrayException); // Log the exception
                                throw emptyArrayException; // Throw the exception
                            }

                            /*
                             * If an element is null in the array, throw an exception
                             */
                            if(componentArray.Any(element => element == null)) {

                                /*
                                 * Create the exception for the missing array element
                                 */
                                MissingComponentException missingArrayElementException = new($"{memberName} contains a missing element in " +
                                    $"{componentInstance.GetType().Name}.");

                                Debug.unityLogger.LogException(missingArrayElementException); // Log the exception
                                throw missingArrayElementException; // Throw the exception
                            }

                            break; // Break the switch

                        /*
                         * If the value is neither a component nor a valid array, log a warning
                         */
                        default:

                            /*
                             * Create the warning message for the invalid field type
                             */
                            string warningMessage = $"{memberName} is neither a {typeof(T).Name} nor a {typeof(T).Name}[] in " +
                                $"{componentInstance.GetType().Name}.";

                            Debug.unityLogger.Log(LogType.Warning, warningMessage); // Log the warning message
                            break; // Break the switch
                    }
            }
        }
}