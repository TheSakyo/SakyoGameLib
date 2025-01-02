using System;
using System.Collections.Generic;
using System.Reflection;
using SakyoGame.Lib.UI.Items;
using Unity.VisualScripting;
using UnityEditor;
using UnityEditor.AnimatedValues;
using UnityEditor.Animations;
using UnityEditor.UI;
using UnityEngine;
using UnityEngine.UI;

namespace SakyoGame.Lib.Editor {

    /**
     * <summary>
     * Custom editor for the <see cref="ButtonUI"/> class.
     * Provides a user interface for modifying ButtonUI properties in the Unity Editor.
     * </summary>
     */
    [CustomEditor(typeof(ButtonUI), true)]
    [CanEditMultipleObjects]
    public class ButtonUIEditor : ButtonEditor {

        /************************/
        /****** PROPERTIES ******/
        /************************/

        /**
         * <summary>
         *  Reference to the <see cref="ButtonUI"/> instance being edited.
         * </summary>
         */
        private ButtonUI _buttonUI;


       /**
        * <summary>
        *  Reference to the script property of the <see cref="ButtonUI"/> instance being edited.
        * </summary>
        */
        private SerializedProperty _scriptProperty;

        /**
         * <summary>
         *  Reference to the “interactable” property of the parent class of the
         *  <see cref="ButtonUI"/> instance being edited.
         * </summary>
         */
        private SerializedProperty _interactableProperty;

        /**
         * <summary>
         *  Reference to the <see cref="Graphic"/> property of the parent class of the
         *  <see cref="ButtonUI"/> instance being edited.
         * </summary>
         */
        private SerializedProperty _targetGraphicProperty;

        /**
         * <summary>
         *  Reference to the <see cref="Selectable.Transition"/> property of the parent class of the
         *  <see cref="ButtonUI"/> instance being edited.
         * </summary>
         */
        private SerializedProperty _transitionProperty;

        /**
         * <summary>
         *  Reference to the <see cref="ColorBlock"/> property of the parent class of the
         *  <see cref="ButtonUI"/> instance being edited.
         * </summary>
         */
        private SerializedProperty _colorBlockProperty;

        /**
         * <summary>
         *  Reference to the <see cref="SpriteState"/> property of the parent class of the
         *  <see cref="ButtonUI"/> instance being edited.
         * </summary>
         */
        private SerializedProperty _spriteStateProperty;

        /**
         * <summary>
         *  Reference to the <see cref="AnimationTriggers"/> property of the parent class of the
         *  <see cref="ButtonUI"/> instance being edited.
         * </summary>
         */
        private SerializedProperty _animTriggerProperty;

        /**
         * <summary>
         *  Reference to the <see cref="Navigation"/> property of the parent class of the
         *  <see cref="ButtonUI"/> instance being edited.
         * </summary>
         */
        private SerializedProperty _navigationProperty;

        /**
         * <summary>
         *  Reference to the clicked event property of the parent class of the
         *  <see cref="ButtonUI"/> instance being edited (<see cref="Button.ButtonClickedEvent"/>).
         * </summary>
         */
        private SerializedProperty _onClickProperty;

        /**
         * <summary>
         *  Reference to the custom color hovered reference to a <see cref="Color"/>
         *  property of the <see cref="ButtonUI"/> instance currently being edited.
         * </summary>
         */
        private SerializedProperty _hoveredColorProperty;

        /**
         * <summary>
         *  Reference to the custom sprite hovered reference to a <see cref="Sprite"/>
         *  property of the <see cref="ButtonUI"/> instance currently being edited.
         * </summary>
         */
        private SerializedProperty _hoveredSpriteProperty;

        /**
         * <summary>
         *  Reference to the custom animation trigger reference to a <see cref="string"/>
         *  property of the <see cref="ButtonUI"/> instance currently being edited.
         * </summary>
         */
        private SerializedProperty _hoveredAnimProperty;


        /**
         * <summary>
         *  Control whether to show the color tint options in the inspector (<see cref="ColorBlock"/>).
         * </summary>
         */
        private readonly AnimBool _showColorTint = new();

        /**
         * <summary>
         *  Control whether to show the sprite swap options in the inspector (<see cref="SpriteState"/>).
         * </summary>
         */
        private readonly AnimBool _showSpriteTransition = new();

        /**
         * <summary>
         *  Control whether to show the animation options in the inspector (<see cref="AnimationTriggers"/>).
         * </summary>
         */
        private readonly AnimBool _showAnimTransition = new();

        /**
         * <summary>
         *  The current transition mode of the <see cref="ButtonUI"/> instance being edited (<see cref="Selectable.Transition"/>.
         * </summary>
         */
        private Selectable.Transition _transitionMode;

        /**
         * <summary>
         *  The <see cref="Graphic"/> component of the <see cref="ButtonUI"/> instance being edited.
         * </summary>
         */
        private Graphic _targetGraphic;

        /**
         * <summary>
         *  The <see cref="Image"/> component of the <see cref="ButtonUI"/> instance being edited.
         * </summary>
         */
        private Image _targetImage;

        /**
         * <summary>
         *  The <see cref="Animator"/> component of the <see cref="ButtonUI"/> instance being edited.
         * </summary>
         */
        private Animator _animator;

        /**
         * <summary>
         *  Whether to show the navigation options in the inspector.
         * </summary>
         */
        private bool _showNavigation;

        /**
         * <summary>
         *  Type of the <see cref="SelectableEditor"/> class.
         * </summary>
         */
        private readonly Type _selectableEditorType = typeof(SelectableEditor);

        /**
         * <summary>
         *  Content for the 'Visualize' label in the inspector.
         * </summary>
         */
        private readonly GUIContent _visualizeNavigation = EditorGUIUtility.TrTextContent("Visualize",
            "Show navigation flows between selectable UI elements.");

        /**
         * <summary>
         *  Property paths to exclude for child classes.
         *  This is used to exclude properties from the inspector for child classes.
         * </summary>
         */
        private string[] _propertyPathToExcludeForChildClasses = {};

        /**
         * <summary>
         *  Key for the 'ShowNavigation' boolean in the editor preferences.
         * </summary>
         */
        private const string ShowNavigationKey = "SelectableEditor.ShowNavigation";

        /**
         * <summary>
         *  Flags to access non-public static methods via reflection.
         * </summary>
         */
        private const BindingFlags ReflectionFlags = BindingFlags.Static | BindingFlags.NonPublic;

        /**********************/
        /****** ENABLING ******/
        /**********************/

        /**
         * <summary>
         *  Called when the editor is enabled. Assigns the <see cref="ButtonUI"/>
         *  instance to the '_buttonUI' field and the animator to the '_animator' field.
         *  Gets the '_showNavigation' value from the editor preferences.
         * </summary>
         */
        protected override void OnEnable() {

            // Initialize serialized properties for the ButtonUI class
            InitializeSerializedProperties();

            // Initialize the property path to exclude for child classes
            InitializePropertyPathToExcludeForChildClasses();

            // Set up the selectable transition
            SetUpSelectableTransition();

            /*************************************/

            // Get the 'RegisterStaticOnSceneGUI' method from the 'SelectableEditor' class using reflection
            MethodInfo registerStaticOnSceneGUI = _selectableEditorType.GetMethod("RegisterStaticOnSceneGUI", ReflectionFlags);

            /*
             * Get the 's_Editors' field from the 'SelectableEditor' class using reflection.
             */
            FieldInfo fieldInfo = _selectableEditorType.GetField("s_Editors", ReflectionFlags);

            /*
             * If the 's_Editors' field exists, add the current editor to the list.
             */
            if(fieldInfo != null) {

                // Cast the field value to a List of 'SelectableEditor'.
                List<SelectableEditor> selectableEditors = (List<SelectableEditor>)fieldInfo.GetValue(null);
                selectableEditors.Add(this); // Add the current editor to the list
            }

            // If the 'RegisterStaticOnSceneGUI' method exists, invoke it
            if(registerStaticOnSceneGUI != null) registerStaticOnSceneGUI.Invoke(null, new object[] {});

            /*************************************/

            // Assign the '_showNavigation' value from the editor preferences
            _showNavigation = EditorPrefs.GetBool(ShowNavigationKey);

            // Assign target button UI and animator components
            AssignButtonUIAndAnimator();

            // Set up the transition mode and target graphic
            SetUpGraphics();
        }

        /***********************/
        /****** INSPECTOR ******/
        /***********************/

        /**
         * <summary>
         *  Called to render the custom inspector GUI for the <see cref="ButtonUI"/>.
         * </summary>
         */
        public override void OnInspectorGUI() {

            serializedObject.Update(); // Update the serialized object

            // Render the target graphic field if it's not already set
            if(_interactableProperty != null) EditorGUILayout.PropertyField(_interactableProperty);

            EditorGUILayout.Space(); // Add a space

            /*
             * If the target graphic field is not set, assign the target graphic
             * associated with ButtonUI if the property doesn't exist.
             * Then, assign the target image of this graphic if it's not already set.
             */
            if(!_targetGraphic) {

                // Assign the target graphic associated with ButtonUI
                _targetGraphic = _buttonUI.targetGraphic;
                
                // Assign the target image if it's not already set
                if(!_targetImage)  _targetImage = _targetGraphic as Image;
            }

            /*************************************/

            RenderTransitionFields(); // Render the transition fields

            bool isColorTintTransition = _transitionMode is Selectable.Transition.ColorTint; // Check if the transition mode is 'ColorTint'
            bool isSpriteSwapTransition = _transitionMode is Selectable.Transition.SpriteSwap; // Check if the transition mode is 'SpriteSwap'

            /*
             * If the transition mode is 'ColorTint' or 'SpriteSwap', render the target graphic field
             */
            if(isColorTintTransition || isSpriteSwapTransition) {

                /*
                 * If the transition mode is 'ColorTint' and the target graphic field is not set,
                 * display a warning message.
                 */
                if(isColorTintTransition && !_targetGraphic)
                    EditorGUILayout.HelpBox("You must have a Graphic target in order to use a color transition.", MessageType.Warning);

                /*
                 * If the transition mode is 'SpriteSwap' and the target image field is not set,
                 * display a warning message.
                 */
                if(isSpriteSwapTransition && !_targetImage)
                    EditorGUILayout.HelpBox("You must have a Graphic target in order to use a color transition.", MessageType.Warning);

                EditorGUILayout.PropertyField(_targetGraphicProperty); // Render the target graphic field
            }

            /*************************************/

            ++EditorGUI.indentLevel; // Increase the indent level
            {
                RenderColorTintTransition(); // Render the color tint transition
                RenderSpriteSwapTransition(); // Render the sprite swap transition
                RenderAnimationTransition(); // Render the animation transition
            }
            --EditorGUI.indentLevel; // Decrease the indent level

            /*************************************/

            EditorGUILayout.Space(); // Add a space
            RenderNavigationFields(); // Render the navigation fields

            /*************************************/

            /*
             * Get the 'DrawPropertiesExcluding' method from the Editor class using reflection
             */
            MethodInfo drawPropertiesExcluding = typeof(UnityEditor.Editor).GetMethod("DrawPropertiesExcluding", ReflectionFlags);

            /*
             * If the 'DrawPropertiesExcluding' method is found, call it with the serialized object
             * and the property path to exclude for child classes
             */
            if(drawPropertiesExcluding != null) drawPropertiesExcluding.Invoke(this, new object[] {
                serializedObject, _propertyPathToExcludeForChildClasses
            });

            /*************************************/

            // Render the onClick field if it's not already set
            if(_onClickProperty != null) EditorGUILayout.PropertyField(_onClickProperty);

            serializedObject.ApplyModifiedProperties(); // Apply the modified properties
        }

        /*****************************/
        /****** PRIVATE METHODS ******/
        /*****************************/

        /**
         * <summary>
         *  Initializes the serialized properties for the fields in the inspector.
         * </summary>
         */
        private void InitializeSerializedProperties() {

            // Retrieve the script property in parent class of the ButtonUI being edited
            _scriptProperty = serializedObject.FindProperty("m_Script");

            // Retrieve the interactable property in parent class of the ButtonUI being edited
            _interactableProperty = serializedObject.FindProperty("m_Interactable");

            // Retrieve the target graphic property in parent class of the ButtonUI being edited
            _targetGraphicProperty = serializedObject.FindProperty("m_TargetGraphic");

            // Retrieve the transition property in parent class of the ButtonUI being edited
            _transitionProperty = serializedObject.FindProperty("m_Transition");

            // Retrieve the color block property in parent class of the ButtonUI being edited
            _colorBlockProperty = serializedObject.FindProperty("m_Colors");

            // Retrieve the sprite state property in parent class of the ButtonUI being edited
            _spriteStateProperty = serializedObject.FindProperty("m_SpriteState");

            // Retrieve the animation triggers property in parent class of the ButtonUI being edited
            _animTriggerProperty = serializedObject.FindProperty("m_AnimationTriggers");

            // Retrieve the navigation property in parent class of the ButtonUI being edited
            _navigationProperty  = serializedObject.FindProperty("m_Navigation");

            // Retrieve the on click property in parent class of the ButtonUI being edited
            _onClickProperty = serializedObject.FindProperty("m_OnClick");

            // Retrieve the hovered color property in parent class of the ButtonUI being edited
            _hoveredColorProperty = serializedObject.FindProperty("hoveredColor");

            // Retrieve the hovered sprite property in parent class of the ButtonUI being edited
            _hoveredSpriteProperty = serializedObject.FindProperty("hoveredSprite");

            // Retrieve the hovered animation property in parent class of the ButtonUI being edited
            _hoveredAnimProperty = serializedObject.FindProperty("hoveredTrigger");
        }

        /**
         * <summary>
         *  Initializes the property path to exclude for child classes
         *  of the <see cref="ButtonUI"/> in an array of strings.
         * </summary>
         */
        private void InitializePropertyPathToExcludeForChildClasses() {

            // Create a list of strings to store the property paths
            List<string> propertyPathList = new();

            // Add the script property path if it exists
            if(_scriptProperty != null) propertyPathList.Add(_scriptProperty.propertyPath);

            // Add the navigation property path if it exists
            if(_navigationProperty != null) propertyPathList.Add(_navigationProperty.propertyPath);

            // Add the transition property path if it exists
            if(_transitionProperty != null) propertyPathList.Add(_transitionProperty.propertyPath);

            // Add the color block property path if it exists
            if(_colorBlockProperty != null) propertyPathList.Add(_colorBlockProperty.propertyPath);

            // Add the sprite state property path if it exists
            if(_spriteStateProperty != null) propertyPathList.Add(_spriteStateProperty.propertyPath);

            // Add the animation triggers property path if it exists
            if(_animTriggerProperty != null) propertyPathList.Add(_animTriggerProperty.propertyPath);

            // Add the interactable property path if it exists
            if(_interactableProperty != null) propertyPathList.Add(_interactableProperty.propertyPath);

            // Add the target graphic property path if it exists
            if(_targetGraphicProperty != null) propertyPathList.Add(_targetGraphicProperty.propertyPath);

            // Add the on click property path if it exists
            if(_onClickProperty != null) propertyPathList.Add(_onClickProperty.propertyPath);

            // Add the hovered color property path if it exists
            if(_hoveredColorProperty != null) propertyPathList.Add(_hoveredColorProperty.propertyPath);

            // Add the hovered sprite property path if it exists
            if(_hoveredSpriteProperty != null) propertyPathList.Add(_hoveredSpriteProperty.propertyPath);

            // Add the hovered animation property path if it exists
            if(_hoveredAnimProperty != null) propertyPathList.Add(_hoveredAnimProperty.propertyPath);

            // Convert the list to an array and assign it to the class field
            _propertyPathToExcludeForChildClasses = propertyPathList.ToArray();
        }

        /**
         * <summary>
         *  Assigns the ButtonUI and animator components to their respective fields.
         * </summary>
         */
        private void AssignButtonUIAndAnimator() {

            _buttonUI = (ButtonUI)target; // Assign the button UI
            _animator = _buttonUI.animator; // Assign the animator
        }

        /**
         * <summary>
         *  Sets up the transition mode based on the transition property
         * </summary>
         * <param name="update">Indicates if the selectable transition should be updated</param>
         */
        private void SetUpSelectableTransition(bool update = false) {

            // Try to assign the '_transitionMode' value from the transition property, if it not exists, set it to 'None'
            _transitionMode = _transitionProperty != null ? (Selectable.Transition)_transitionProperty.enumValueIndex : Selectable.Transition.None;

            /*
             * Check if the transition mode should not be updated,
             * then assign the value from the transition property
             */
            if(!update) {

                /*
                 * Assign the '_showColorTint' value from the transition property (ColorTint), if it exists.
                 * Then add a listener to repaint the inspector if this value is changed
                 */
                if(_showColorTint != null) {

                    // Assign the '_showColorTint' value from the transition property (ColorTint)
                    _showColorTint.value = _transitionMode == Selectable.Transition.ColorTint;

                    // Add a listener to repaint the inspector if this value is changed
                    _showColorTint.valueChanged.AddListener(Repaint);
                }

                /*
                 * Assign the '_showSpriteTransition' value from the transition property (SpriteSwap), if it exists.
                 * Then add a listener to repaint the inspector if this value is changed
                 */
                if(_showSpriteTransition != null) {

                    // Assign the '_showSpriteTransition' value from the transition property (SpriteSwap)
                    _showSpriteTransition.value = _transitionMode == Selectable.Transition.SpriteSwap;

                    // Add a listener to repaint the inspector if this value is changed
                    _showSpriteTransition.valueChanged.AddListener(Repaint);
                }

                // Assign the '_showAnimTransition' value from the transition property (Animation) , if it exists
                if(_showAnimTransition != null) _showAnimTransition.value = _transitionMode == Selectable.Transition.Animation;
            }
        }

        /**
         * <summary>
         *  Sets up the target graphic based on the target graphic property or <see cref="ButtonUI"/> being edited.
         * </summary>
         */
        private void SetUpGraphics() {

            // Assign the target graphic if this property exists
            if(_targetGraphicProperty != null) _targetGraphic = (Graphic)_targetGraphicProperty.objectReferenceValue;

            //Assign the target image if this property exists
            if(_targetGraphic != null) _targetImage = _targetGraphic as Image;

            // Else, assign the target image associated with ButtonUI if this property doesn't exist
            else _targetImage = _buttonUI.image;
        }

        /**
         * <summary>
         *  Renders the transition-related fields, including the mode and properties.
         * </summary>
         */
        private void RenderTransitionFields() {

            // Return if the transition property doesn't exist
            if(_transitionProperty == null) return;

            HandleTransitionType(_transitionProperty.hasMultipleDifferentValues); // Handle the transition type
            EditorGUILayout.PropertyField(_transitionProperty); // Render the transition property
        }

        /**
         * <summary>
         *  Handles the specific rendering logic based on the selected transition type.
         * </summary>
         * <param name="transitionHasMultipleDifferentValues">Indicates if multiple values are selected for the transition.</param>
         */
        private void HandleTransitionType(bool transitionHasMultipleDifferentValues) {

            // Update the selectable transition mode
            SetUpSelectableTransition(true);

            /*
             * Assign a 'boolean value' that checks whether the transition mode is 'ColorTint' and
             * if multiple values are not selected
             */
            if(_showColorTint != null)
                _showColorTint.target = !transitionHasMultipleDifferentValues &&
                                        _transitionMode == Selectable.Transition.ColorTint;

            /*
             * Assign a 'boolean value' that checks whether the transition mode is 'SpriteSwap' and
             * if multiple values are not selected
             */
            if(_showSpriteTransition != null)
                _showSpriteTransition.target = !transitionHasMultipleDifferentValues &&
                                               _transitionMode == Selectable.Transition.SpriteSwap;

            /*
             * Assign a 'boolean value' that checks whether the transition mode is 'Animation' and
             * if multiple values are not selected
             */
            if(_showAnimTransition != null)
                _showAnimTransition.target = !transitionHasMultipleDifferentValues &&
                                             _transitionMode == Selectable.Transition.Animation;
        }

        /**
         * <summary>
         *  Renders the color tint transition settings if applicable.
         * </summary>
         */
        private void RenderColorTintTransition() {
            
            // Return only if the color block property doesn't exist or the color tint transition is not selected
            if(_showColorTint == null || _colorBlockProperty == null) return;

            /*
             * Render the fade group for rendering the color tint transition settings
             */
            if(EditorGUILayout.BeginFadeGroup(_showColorTint.faded)) {

                EditorGUILayout.PropertyField(_colorBlockProperty); // Render the color block property

                /*
                 * Render the custom hovered color property if applicable and if property type is 'Color'
                 */
                if(_hoveredColorProperty?.propertyType == SerializedPropertyType.Color)
                    _hoveredColorProperty.colorValue = EditorGUILayout.ColorField("Hovered Color", _hoveredColorProperty.colorValue);
            }

            EditorGUILayout.EndFadeGroup(); // Close the fade group
        }

        /**
         * <summary>
         *  Renders the sprite swap transition settings if applicable.
         * </summary>
         */
        private void RenderSpriteSwapTransition() {

            // Return only if the sprite state property doesn't exist or the sprite swap transition is not selected
            if(_showSpriteTransition == null || _spriteStateProperty == null) return;

            /*
             * Render the fade group for rendering the sprite swap transition settings
             */
            if(EditorGUILayout.BeginFadeGroup(_showSpriteTransition.faded)) {

                EditorGUILayout.PropertyField(_spriteStateProperty); // Render the sprite state property

                /*
                 * Render the custom hovered sprite property if applicable and if property type is 'Sprite'
                 */
                if(_hoveredSpriteProperty?.objectReferenceValue is Sprite sprite)
                    _hoveredSpriteProperty.objectReferenceValue = EditorGUILayout.ObjectField("Hovered Sprite", sprite, typeof(Sprite), true);
            }

            EditorGUILayout.EndFadeGroup(); // Close the fade group
        }

        /**
         * <summary>
         *  Renders the animation transition settings and auto-generation options.
         * </summary>
         */
        private void RenderAnimationTransition() {

            // Return only if animation trigger property doesn't exist or the animation trigger transition is not selected
            if(_showAnimTransition == null || _animTriggerProperty == null) return;

            /*
             * Render the fade group for rendering the animation trigger transition settings
             */
            if(EditorGUILayout.BeginFadeGroup(_showAnimTransition.faded)) {

                EditorGUILayout.PropertyField(_animTriggerProperty); // Render the animation trigger property

                /*
                 * Render the custom hovered animation trigger property if applicable and if property type is 'String'
                 */
                if(_hoveredAnimProperty?.propertyType == SerializedPropertyType.String)
                    _hoveredAnimProperty.stringValue = EditorGUILayout.TextField("Hovered Trigger", _hoveredAnimProperty.stringValue);

                RenderAutoGenerateAnimatorButton(); // Render the auto-generation button
            }

            EditorGUILayout.EndFadeGroup(); // Close the fade group
        }

        /**
         * <summary>
         *  Renders a button that auto-generates an animator controller if necessary.
         * </summary>
         */
        private void RenderAutoGenerateAnimatorButton() {

            // Return if the animator and controller is not null
            if(_animator && _animator.runtimeAnimatorController) return;

            Rect buttonRect = EditorGUILayout.GetControlRect(); // Get a control rect for the button
            buttonRect.xMin += EditorGUIUtility.labelWidth; // Add the label width in the x direction

            // If the render button is clicked to auto-generate the animation controller, invoke the auto-generation method.
            if(GUI.Button(buttonRect, "Auto Generate Animation", EditorStyles.miniButton)) AutoGenerateAnimatorController();
        }

        /**
         * <summary>
         *  Uses reflection to invoke methods that auto-generate the animator controller.
         * </summary>
         */
        private void AutoGenerateAnimatorController() {

            /*
             * Get the method info for the 'GenerateSelectableAnimatorController' method
             * in the 'SelectableEditor' class using reflection
             */
            MethodInfo generateSelectableAnimationController = _selectableEditorType
                .GetMethod("GenerateSelectableAnimatorContoller", ReflectionFlags);

            /*
             * Get the method info for the 'GenerateTriggerableTransition' method
             * in the 'SelectableEditor' class using reflection
             */
            MethodInfo generateTriggerableTransition = _selectableEditorType
                .GetMethod("GenerateTriggerableTransition", ReflectionFlags);

            /*
             * Get the method info for the 'GetSaveControllerPath' method
             * in the 'SelectableEditor' class using reflection
             */
            MethodInfo getSaveControllerPath = _selectableEditorType
                .GetMethod("GetSaveControllerPath", ReflectionFlags);


            // If the 'GenerateSelectableAnimatorController' method was not found, return
            if(generateSelectableAnimationController == null) return;

            /*
             * If the 'GenerateSelectableAnimatorController' method was found,
             * invoke it with the animation triggers
             */
            object controller = generateSelectableAnimationController.Invoke(null, new object[] {
                _buttonUI.animationTriggers, _buttonUI
            });

            // If the returned controller is not an animator controller, return
            if(controller is not AnimatorController animatorController) return;

            // If the animator component of the ButtonUI instance is null, add the animator component to this instance
            if(!_animator) _animator = _buttonUI.AddComponent<Animator>();

            /*
             * Get the hovered animation name for generating this custom transition
             */
            string hoveredName = string.IsNullOrEmpty(_buttonUI.AnimationTriggersUI.Hovered) ? "Hovered" :
                _buttonUI.AnimationTriggersUI.Hovered;

            /*
             * If the 'GenerateTriggerableTransition' method was found, invoke it with the hovered animation name
             */
            if(generateTriggerableTransition != null) generateTriggerableTransition.Invoke(null, new object[] {
                hoveredName, animatorController
            });

            /*
             * If the 'GetSaveControllerPath' method was found, invoke it
             */
            if(getSaveControllerPath != null) {

                // Get the path of the created animator controller using reflection
                object path = getSaveControllerPath.Invoke(null, new object[] { _buttonUI });

                // If the path is a string, import the asset
                if(path is string pathString) AssetDatabase.ImportAsset(pathString);
            }

            // Set the animator controller of the ButtonUI instance
            AnimatorController.SetAnimatorController(_buttonUI.animator, animatorController);
        }

        /**
         * <summary>
         *  Renders the navigation options if applicable.
         * </summary>
         */
        private void RenderNavigationFields() {

            // If the navigation property is not null, render it
            if(_navigationProperty != null) EditorGUILayout.PropertyField(_navigationProperty);
            if(!_showNavigation) return; // Return if the navigation is not shown

            Rect toggleRect = EditorGUILayout.GetControlRect(); // Get a control rect for the toggle
            toggleRect.xMin += EditorGUIUtility.labelWidth; // Add the label width in the x direction

            // Render the navigation toggle and update the show navigation value
            _showNavigation = GUI.Toggle(toggleRect, _showNavigation, _visualizeNavigation ?? GUIContent.none, EditorStyles.miniButton);

            if(!EditorGUI.EndChangeCheck()) return; // Return if the change is not detected
            EditorPrefs.SetBool(ShowNavigationKey, _showNavigation); // Update the show navigation value
            SceneView.RepaintAll(); // Repaint all scene views
        }
    }
}
