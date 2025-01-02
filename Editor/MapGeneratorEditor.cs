using SakyoGame.Lib.Enums;
using SakyoGame.Lib.Maps.World;
using UnityEditor;
using UnityEngine;

namespace SakyoGame.Lib.Editor {

    /**
     * <summary>
     *  Custom editor for the <see cref="MapGenerator"/> class.
     * </summary>
     */
    [CustomEditor(typeof(MapGenerator))]
    public class MapGeneratorEditor : UnityEditor.Editor {

        /************************/
        /****** PROPERTIES ******/
        /************************/

        /**
         * <summary>
         *  Reference to the <see cref="MapGenerator"/> instance being edited.
         * </summary>
         */
        private MapGenerator _mapGenerator;

        /**********************/
        /****** ENABLING ******/
        /**********************/

        /**
         * <summary>
         *  Called when the editor is enabled. Assigns the <see cref="MapGenerator"/> instance to the '_mapGenerator' field.
         * </summary>
         */
        private void OnEnable() { _mapGenerator = (MapGenerator)target; }

        /***********************/
        /****** INSPECTOR ******/
        /***********************/

        /**
         * <summary>
         *  Called when inspector is drawn. Adds custom UI elements to enable/disable “Can Prefabs Spawn” and “Auto Update”.
         *  The “Auto Update” checkbox lets you modify the map in real time, and the “Can Prefabs Spawn” checkbox allows prefabs to spawn.
         * </summary>
         */
        public override void OnInspectorGUI() {

            EditorGUI.BeginChangeCheck(); // Start detecting changes to serialized properties.

            // Add a toggle for determining if the map is a 2D or 3D map.
            _mapGenerator.IsThreeDimensional = EditorGUILayout.Toggle("Generate 3D Map", _mapGenerator.IsThreeDimensional);

            DrawInspector(); // Draw the inspector layout.

            bool previousAutoUpdate = _mapGenerator.AutoUpdate; // Cache the previous state of AutoUpdate.
            bool previousUnSpawnPrefabs  = _mapGenerator.UnSpawnPrefabs; // Cache the previous state of unSpawnPrefabs.
            bool previousIsThreeDimensional = _mapGenerator.IsThreeDimensional; // Cache the previous state of IsThreeDimensional.

            // Add a toggle for enabling or disabling the "Auto Update" property.
            _mapGenerator.AutoUpdate = EditorGUILayout.Toggle("Auto Update", _mapGenerator.AutoUpdate);

            /*
             * Detect whether default property changes are detected and whether automatic generation is enabled :
             *   > We also check that the “Auto Udpate”, “Unspawn Prefabs” and “Generate 3D Map” boxes have not been ticked.
             *  In this case, we can detect a change in the map generation parameters.
             *  (the map will be generated automatically without pressing the “Generate” button)
             */
            bool propertyChanged = EditorGUI.EndChangeCheck() && _mapGenerator.AutoUpdate && _mapGenerator.AutoUpdate == previousAutoUpdate &&
                                   _mapGenerator.UnSpawnPrefabs == previousUnSpawnPrefabs && _mapGenerator.IsThreeDimensional == previousIsThreeDimensional;

            EditorGUILayout.Space(); // Adds spacing for better UI structure.

            GUI.enabled = !_mapGenerator.AutoUpdate; // Enable or disable the "Generate" button based on AutoUpdate's state.

            // Determine the map dimension based on the 'IsThreeDimensional' property.
            EMapDimension mapDimension = _mapGenerator.IsThreeDimensional ? EMapDimension.ThreeDimensional : EMapDimension.TwoDimensional;

            // Add a button for generating the map, with logic to detect property changes.
            if(GUILayout.Button("Generate") || propertyChanged) _mapGenerator.GenerateMap(mapDimension);

            GUI.enabled = true; // Re-enable GUI for any further actions.
        }

        /*****************************/
        /****** PRIVATE METHODS ******/
        /*****************************/

        /**
         * <summary>
         *  Draws the properties of the <see cref="MapGenerator"/> class in the inspector.
         * </summary>
         */
        private void DrawInspector() {

            EditorGUI.BeginChangeCheck(); // Start detecting changes to serialized properties.

            serializedObject.UpdateIfRequiredOrScript(); // Update the serialized object if required.

            SerializedProperty iterator = serializedObject.GetIterator(); // Get the iterator for the serialized object.

            /*
             * Iterate through the properties of the serialized object and draw them in the inspector.
             */
            for(bool enterChildren = true; iterator.NextVisible(enterChildren); enterChildren = false) {

                // Skip "m_Script" property, as it is handled separately.
                if("m_Script" != iterator.propertyPath) EditorGUILayout.PropertyField(iterator, true);
            }

            serializedObject.ApplyModifiedProperties(); // Apply modified properties to the serialized object.
            EditorGUI.EndChangeCheck(); // Close the change check.
        }
    }
}


