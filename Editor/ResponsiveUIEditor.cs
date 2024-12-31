using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using SakyoGame.Lib.Enums;
using SakyoGame.Lib.Structs;
using SakyoGame.Lib.UI;
using UnityEditor;
using UnityEngine;

namespace SakyoGame.Lib.Editor {

    /**
     * <summary>
     *  Custom editor for the <see cref="ResponsiveUI"/> class.
     * </summary>
     */
    [CustomEditor(typeof(ResponsiveUI))]
    internal class ResponsiveUIDrawer : UnityEditor.Editor {

        /************************/
        /****** PROPERTIES ******/
        /************************/

        /**
         * <summary>
         *  Reference to the <see cref="ResponsiveUI"/> instance being edited.
         * </summary>
         */
        private ResponsiveUI _responsiveUI;

        /**
         * <summary>
         *  Whether to show the anchor folder.
         * </summary>
         */
        private readonly Dictionary<int, bool> _showAnchorFolders = new();

        /**
         * <summary>
         *  Array of variants of the word "adjustment".
         * </summary>
         */
        private readonly string[] _adjustmentWords = {
            "adjustment", "adjustments",
            "adjust", "adjusts",
            "adjusting", "adjusted",
        };

        /**********************/
        /****** ENABLING ******/
        /**********************/

        /**
         * <summary>
         *  Called when the editor is enabled. Assigns the <see cref="ResponsiveUI"/> instance to the '_responsiveUI' field.
         * </summary>
         */
        private void OnEnable() { _responsiveUI = (ResponsiveUI)target; }

        /***********************/
        /****** INSPECTOR ******/
        /***********************/

        /**
         * <summary>
         *  Unity's 'OnInspectorGUI' method that runs when the inspector is drawn.
         *  Draws the responsive mode dropdown and the corresponding fields.
         * </summary>
         */
        public override void OnInspectorGUI() {

            serializedObject.Update(); // Update the serialized object

            // Draw the responsive mode dropdown and the set corresponding fields accordingly
            _responsiveUI.ResponsiveMode =
                (EResponsiveMode)EditorGUILayout.EnumPopup("Responsive Mode", _responsiveUI.ResponsiveMode);
            EResponsiveMode responsiveMode = _responsiveUI.ResponsiveMode; // Get the responsive mode value

            GUILayout.Space(10); // Add some spacing

            Type responsiveUIType = _responsiveUI.GetType(); // Get the type of the 'ResponsiveUI' class
            BindingFlags bindingFlags = BindingFlags.Instance | BindingFlags.NonPublic; // Get the binding flags for non-public instance

            /*
             * Switch statement to draw the corresponding fields based on the responsive mode.
             * Each case corresponds to a different responsive mode.
             */
            switch(responsiveMode) {

                /*
                 * If the responsive mode is 'ParentSize', draw the parent size responsive mode fields :
                 * It contains one foldout per anchor point adjustment for minimum or maximum anchor points
                 */
                case EResponsiveMode.ParentSize:

                    /*
                     * Get all the fields of type 'AnchorAdjustments' in the 'ResponsiveUI' class.
                     */
                    FieldInfo[] fields = responsiveUIType.GetFields(bindingFlags).Where(f =>
                        f.FieldType == typeof(AnchorAdjustments)).ToArray();

                    /*
                     * Iterate through each field and draw it using 'DrawAnchorProperty'
                     * Then add some spacing between each field for better legibility.
                     */
                    foreach(FieldInfo field in fields) {

                        int fieldToken = field.MetadataToken; // Get the metadata token of the field
                        _showAnchorFolders.TryAdd(fieldToken,
                            false); // Add the field token to the dictionary if it doesn't exist

                        // Draw the anchor property using the 'DrawAnchorProperty' method
                        DrawAnchorProperty(serializedObject.FindProperty(field.Name), fieldToken);
                        if (field != fields.Last()) GUILayout.Space(5); // Add spacing between each field
                    }

                    break; // Break the switch

                /*
                 * If the responsive mode is 'ScreenSize', draw the screen size responsive mode fields :
                 * It contains two fields for percentage width and height of screen
                 */
                case EResponsiveMode.ScreenSize:

                    // Draw the percentage width field for the screen size responsive mode
                    _responsiveUI.PercentageWidth =
                        EditorGUILayout.FloatField("Percentage Width", _responsiveUI.PercentageWidth);

                    // Draw the percentage height field for the screen size responsive mode
                    _responsiveUI.PercentageHeight =
                        EditorGUILayout.FloatField("Percentage Height", _responsiveUI.PercentageHeight);

                    break; // Break the switch

                default: throw new InvalidDataException("Invalid ResponsiveMode: " + responsiveMode);
            }

            serializedObject.ApplyModifiedProperties(); // Apply changes to the serialized properties

            // If any changes were made, mark the target as dirty
            if(GUI.changed) EditorUtility.SetDirty(target);

            /*
             * If the current platform is the editor, get the 'AdjustToParentSize' method from the 'ResponsiveUI' class using reflection
             */
            #if UNITY_EDITOR
            
                // Get the 'AdjustToParentSize' method from the 'ResponsiveUI' class using reflection
                MethodInfo method = responsiveUIType.GetMethod("AdjustToParentSize", bindingFlags);

                if(method != null) method.Invoke(_responsiveUI, null); // If the method exists, invoke it

            #endif
        }

        /*****************************/
        /****** PRIVATE METHODS ******/
        /*****************************/

        /**
         * <summary>
         *  Draws the anchor property in the inspector.
         * </summary>
         * <param name="property">The serialized property to draw.</param>
         * <param name="token">The metadata token of the field.</param>
         */
        private void DrawAnchorProperty(SerializedProperty property, int token) {
            
            // Check if the property name ends with any of the 'adjustment' words
            bool endsWithAdjustmentWord = _adjustmentWords.Any(word => property.name.ToLower().EndsWith(word));

            /*
             * Set label of property : If the property name ends with any of the 'adjustment'
             * words, add "Adjust(s)", else add the default property name with "("
             */
            string label = endsWithAdjustmentWord ? "Adjust(s)" : property.name + "(";

            // If the property name starts with "anchorMin", add "Anchor Minimum Points"
            if(property.name.StartsWith("anchorMin")) label += " Anchor Minimum Point(s)";

            // Else, if the property name starts with "anchorMax", add "Anchor Maximum Points"
            else if(property.name.StartsWith("anchorMax")) label += " Anchor Maximum Point(s)";

            // If the property name does not end with any of the 'adjustment' words, add ")"
            if(!endsWithAdjustmentWord) label += ")";

            // Draw the folding for the anchors and enable or disable the associated '_showAnchorFolders' flag accordingly
            _showAnchorFolders[token] = EditorGUILayout.Foldout(_showAnchorFolders[token], label);

            if(!_showAnchorFolders[token]) return; // Return, if the 'showFoldout' flag is disabled

            EditorGUI.indentLevel++; // Increase the indentation level

            // Draw the xAxis field with adjusted position for spacing
            EditorGUILayout.PropertyField(property.FindPropertyRelative("xAxis"), new GUIContent("X-Axis"));

            // Draw the yAxis field with adjusted position for spacing
            EditorGUILayout.PropertyField(property.FindPropertyRelative("yAxis"), new GUIContent("Y-Axis"));

            EditorGUI.indentLevel--; // Decrease the indentation level
        }
    }
}

