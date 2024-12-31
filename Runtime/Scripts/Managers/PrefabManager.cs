using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Reflection;
using SakyoGame.Lib.Actors;
using SakyoGame.Lib.Interfaces.Actors;
using UnityEngine;

namespace SakyoGame.Lib.Managers {

    /**
     * <summary>
     *  Class for managing prefabs.
     * </summary>
     */
    public static class PrefabManager {

        /*********************/
        /****** GETTERS ******/
        /*********************/

        /**
        * <summary>
        *  Gets the set of existing prefabs.
        * </summary>
        */
        internal static HashSet<Type> ExistingPrefabsTypes { get; } = new();

        /*****************************/
        /****** PRIVATE METHODS ******/
        /*****************************/

        /**
         * <summary>
         *  Retrieves prefabs of the specified type.
         * </summary>
         * <typeparam name="TPrefab">The type of the prefab.</typeparam>
         * <returns>A read-only collection of prefabs.</returns>
         */
        private static ReadOnlyCollection<IPrefab> GetPrefabs<TPrefab>() where TPrefab : BasePrefab, IPrefab {

            // Finds all objects instances of the specified prefab type.
            return UnityEngine.Object.FindObjectsByType<TPrefab>(FindObjectsSortMode.InstanceID).Cast<IPrefab>().ToList().AsReadOnly();
        }

        /**
         * <summary>
         *  Retrieves all prefabs with reflection
         * </summary>
         * <returns>A read-only collection of <see cref="IPrefab"/> prefabs.</returns>
         */
        private static ReadOnlyCollection<IPrefab> TryToGetAllPrefabs() {

            // Create a HashSet to store all prefabs of type IBasePrefab without duplicates.
            HashSet<IPrefab> allPrefabs = new();

            // Get the binding flags for non-public static methods.
            const BindingFlags bindingFlags = BindingFlags.NonPublic | BindingFlags.Static;

            // Get the MethodInfo object for the non-public "GetPrefabs" method from the PrefabsManager class.
            MethodInfo getPrefabsMethod = typeof(PrefabManager).GetMethod(nameof(GetPrefabs), bindingFlags);

            // If the method was not found, return an empty read-only collection of IBasePrefab.
            if(getPrefabsMethod == null) return new List<IPrefab>().AsReadOnly();
            
            /*
             * Iterate through each type in the _existingPrefabs collection (which presumably contains prefab types).
             */
            foreach(Type prefabType in ExistingPrefabsTypes) {

                // Create a generic method using the 'types' array, making it specific to the prefab type.
                MethodInfo genericMethod = getPrefabsMethod.MakeGenericMethod(prefabType);

                // Invoke the generic method dynamically with the current 'filterByEntity' value.
                object existingPrefabs = genericMethod.Invoke(null, null);

                /*
                 * If the existingPrefabs is an instance of ReadOnlyCollection<IPrefab>, add it to the allPrefabs HashSet.
                 */
                if(existingPrefabs is ReadOnlyCollection<IPrefab> prefabs) allPrefabs.UnionWith(prefabs);
            }

            // Return the final list of all prefabs as a read-only list.
            return allPrefabs.ToList().AsReadOnly();
        }

        /****************************/
        /****** PUBLIC METHODS ******/
        /****************************/

        /**
         * <summary>
         *  Retrieves all <see cref="IPrefab"/> prefabs
         * </summary>
         * <returns>A read-only collection of <see cref="IPrefab"/> prefabs.</returns>
         */
        public static ReadOnlyCollection<IPrefab> GetAllPrefabs() {

            return TryToGetAllPrefabs(); // Return a read-only list of all prefabs
        }
    }
}