using System.Collections.Generic;
using SakyoGame.Lib.Interfaces.Actors;
using SakyoGame.Lib.Managers;
using SakyoGame.Lib.Maps;
using SakyoGame.Lib.Maps.Meshes;
using SakyoGame.Lib.Structs;
using UnityEngine;

namespace SakyoGame.Lib.Actors {

    public abstract class BasePrefab : MonoBehaviour, IActor, IPrefab {

        /************************/
        /****** VALIDATING ******/
        /************************/

        /**
         * <summary>
         *  Called when the script instance is being loaded or a value changes in the inspector.
         *  Initializes the prefab by adding it as a child to the static read-only collection of prefab children.
         * </summary>
         */
        private void OnValidate() {
    
            // Add this prefab to the list of existing prefabs
            PrefabManager.ExistingPrefabsTypes.Add(GetType());
        }

        /*********************/
        /****** GETTERS ******/
        /*********************/

        /**
          * <summary>
          *  Returns an collection of game objects that serves as the template for the prefabs to be instantiated.
          *  This prefab is referenced to create prefab clone in the game.
          * </summary>
          */
        public List<GameObject> PrefabsObject { get; } = new();

        /**
          * <summary>
          *  Returns the minimum distance between prefabs.
          * </summary>
          */
        public abstract float MinimumDistance { get; }

        /**
         * <summary>
         * Check if prefab is spawned (if game object is instanced)
         * </summary>
         */
        public bool IsSpawned { get; set; }

        /**
         * <summary>
         *  The location of the prefab, which includes its position (X, Y, Z) and rotation (Yaw, Pitch).
         * </summary>
         */
        public Location3D Location3D { get; set; }

        /****************************/
        /****** PUBLIC METHODS ******/
        /****************************/

        /**
          * <summary>
          *  Tries spawns prefabs at random positions on the mesh based on the provided height map and mesh data.
          *  Prefabs are instantiated only if the number of prefabs is below the specified limit.
          * </summary>
          * <param name="meshData">The mesh data containing information about the terrain.</param>
         * <param name="regions">An array of <see cref="TerrainType"/> structs containing information about terrain types.</param>
          */
        public abstract void TrySpawn(MeshData meshData, TerrainType[] regions);

        /**
          * <summary>
          *  Tries destroy current prefab.
          * </summary>
          */
        public void TryUnSpawn() {

            IsSpawned = false; // Set the spawned flag to false

            if(PrefabsObject.Count <= 0) return; // Return if there are no prefabs to destroy

            /*
             * Loop through the collection of prefabs and destroy each one
             */
            foreach(GameObject prefab in PrefabsObject) {

                /*
                 * Destroy in the Unity Editor (immediate destruction of the object, without waiting for the next frame)
                 */
                #if UNITY_EDITOR

                    DestroyImmediate(prefab);

                /*
                 * Destroy in a standalone build (scheduled for destruction in the next frame)
                 */
                #elif UNITY_STANDALONE

                    Destroy(prefab);

                #endif
            }
        }
    }
}