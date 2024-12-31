using System.Reflection;
using SakyoGame.Lib.Interfaces.UI;
using SakyoGame.Lib.UI.State;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace SakyoGame.Lib.UI.Items {
    
    /**
     * <summary>
     *  Class for buttons in the game.
     * </summary>
     */
    public class ButtonUI : Button, ISelectableUI {

        /************************/
        /****** PROPERTIES ******/
        /************************/

        /**
         * <summary>
         *  The Alpha color of enabled text.
         * </summary>
         */
        private const float ActiveTextAlpha = 255f / 255f;

        /**
         * <summary>
         *  The Alpha color of disabled text.
         * </summary>
         */
        private const float InactiveTextAlpha = 180f / 255f;


        /**
         * <summary>
         *  The color of the button when is interactable.
         * </summary>
         */
        private Color _interactableColor;

        /**
         * <summary>
         *  The sprite of the button when is interactable.
         * </summary>
         */
        private Sprite _interactableSprite;

        /**
         * <summary>
         *  The animation trigger of the button when is interactable.
         * </summary>
         */
        private string _interactableTrigger;

        /**
         * <summary>
         *  The text component of the button.
         * </summary>
         */
        private TextMeshProUGUI _text;

        /**
         * <summary>
         *  The 'isPointerInside' property of the selectable button using reflection.
         * </summary>
         */
        private readonly PropertyInfo _isPointerInside = typeof(Selectable).GetProperty("isPointerInside",
            BindingFlags.NonPublic | BindingFlags.Instance);

        /**
         * <summary>
         *  The cached value of the 'isPointerInside' property.
         * </summary>
         */
        private bool? _isPointerInsideCached;

        /**
         * <summary>
         *  The hovered color of the button.
         * </summary>
         */
        [SerializeField]
        private Color hoveredColor;

        /**
         * <summary>
         *  The hovered sprite of the button.
         * </summary>
         */
        [SerializeField]
        private Sprite hoveredSprite;

        /**
         * <summary>
         *  The hovered animation trigger of the button.
         * </summary>
         */
        [SerializeField]
        private string hoveredTrigger = "Hovered";

        /************************/
        /****** VALIDATING ******/
        /************************/

        /**
         * <summary>
         *  Unity's 'OnValidate' method that runs when properties are changed in the inspector or the script is recompiled.
         *  Check if the transitions is 'ColorTint' or not and update the normal color of the button if it is.
         * </summary>
         */
        protected override void OnValidate() {

            base.OnValidate(); // Call the base method

            // Set the custom color block instance of the button if it is not null and the initial color block is not default
            if(ColorBlockUI == null && colors != default) ColorBlockUI = new(colors, hoveredColor, this);

            // Set the custom sprite state instance of the button if it is not null
            if(SpriteStateUI == null) SpriteStateUI = new(spriteState, hoveredSprite, this);

            // Set the custom animation triggers instance of the button if it is not null and the initial animation triggers is not default
            if(AnimationTriggersUI == null && animationTriggers != null) AnimationTriggersUI = new(animationTriggers, hoveredTrigger, this);

            // Get the text component of the button if it is null
            if(!_text) _text = GetComponentInChildren<TextMeshProUGUI>();

            // Return if the button can't be processed
            if(!CanProcessButton()) return;

            // Get the current normal color
            Color normalColor = targetGraphic.color;

            /*
             * Update the normal color of the button if it is different and not highlighted,
             * and if the instance of the custom color block is not null.
             */
            if(!IsPointed && ColorBlockUI != null && colors.normalColor != normalColor) ColorBlockUI.Normal = normalColor;
        }

        /*********************/
        /****** GETTERS ******/
        /*********************/


        /**
         * <summary>
         *  Whether the button is currently pointed.
         * </summary>
         * <remarks>
         *  Retrieves the value of the 'isPointerInside' property from
         *  the associated '<see cref="Selectable"/>' using reflection,
         *  while also considering the 'interactable' flag to determine
         *  if the button can be pointed to. If the button is not interactable,
         *  the property will return false.
         * </remarks>
         */
        public bool IsPointed {

            get {

                // Return the cached value of the 'isPointerInside' property
                if(_isPointerInsideCached.HasValue) return _isPointerInsideCached.Value;

                // Return false, if the 'isPointerInside' property is null or the 'isPointerInside' property is not a boolean
                if(_isPointerInside == null || _isPointerInside.GetValue(this) is not bool isPointed) return false;

                // Set the cached value of the 'isPointerInside' property (is interactable and is pointed)
                _isPointerInsideCached = interactable && isPointed;
                return _isPointerInsideCached.Value; // Return the cached value
            }

            private set {

                // Return if the 'isPointerInside' property is null
                if(_isPointerInside == null) return;

                // Set the 'isPointerInside' property with reflection
                _isPointerInside.SetValue(this, value);

                // Set the cached value of the 'isPointerInside' property
                _isPointerInsideCached = value;
            }
        }

        /*** ___________________________________________________ ***/

        /**
         * <summary>
         *  Get the target graphic of the <see cref="ButtonUI"/>.
         * </summary>
         */
        public Graphic TargetGraphic => targetGraphic;

        /**
         * <summary>
         *  Get the image component of the <see cref="ButtonUI"/>.
         * </summary>
         */
        public Image Image => image;


        /**
         * <summary>
         *  Get the animator component of the <see cref="ButtonUI"/>.
         * </summary>
         */
        public Animator Animator => animator;

        /**
         * <summary>
         *  Get the interactable state of the <see cref="ButtonUI"/>.
         * </summary>
         */
        public bool Interactable => interactable;

        /*** ___________________________________________________ ***/

        /**
         * <summary>
         *  Get the custom instance of the <see cref="ColorBlockUI"/> class.
         * </summary>
         */
        public ColorBlockUI ColorBlockUI { get; protected set; }

        /**
         * <summary>
         *  Get the custom instance of the <see cref="SpriteStateUI"/> class.
         * </summary>
         */
        public SpriteStateUI SpriteStateUI { get; protected set; }

        /**
         * <summary>
         *  Get the custom instance of the <see cref="AnimationTriggersUI"/> class.
         * </summary>
         */
        public AnimationTriggersUI AnimationTriggersUI { get; protected set; }

        /*** ___________________________________________________ ***/

        /**
         * <summary>
         *  Get the current color of the button.
         * </summary>
         */
        Color ISelectableUI.CurrentColor { get => _interactableColor; set => _interactableColor = value; }

        /**
         * <summary>
         *  Get the current sprite of the button.
         * </summary>
         */
        Sprite ISelectableUI.CurrentSprite { get => _interactableSprite; set => _interactableSprite = value; }

        /**
         * <summary>
         *  Get the current animation trigger of the button.
         * </summary>
         */
        string ISelectableUI.CurrentAnimationTrigger { get => _interactableTrigger; set => _interactableTrigger = value; }

        /***************************/
        /****** EVENT METHODS ******/
        /***************************/

        /**
         * <summary>
         *  Unity's 'OnPointerEnter' method that runs when the mouse enters the button.
         *  The button is currently pointed, change the color of the button when is hovered.
         * </summary>
         */
        public override void OnPointerEnter(PointerEventData eventData) {

            base.OnPointerEnter(eventData); // Call the base method
            UpdatePointerState(true); // Update the hovered state and apply the visual transition
        }

        /**
         * <summary>
         *  Unity's 'OnPointerExit' method that runs when the mouse exits the button.
         *  The button is no longer pointed at, change the color of the button when is not hovered.
         * </summary>
         */
        public override void OnPointerExit(PointerEventData eventData) {

            base.OnPointerExit(eventData); // Call the base method
            UpdatePointerState(false); // Update the hovered state and apply the visual transition
        }

        /****************************/
        /****** PUBLIC METHODS ******/
        /****************************/

        /**
         * <summary>
         *  Apply the visual transition of the button depending on whether it is hovered or not.
         * </summary>
         * <param name="hovered"> Whether the button is hovered or not. </param>
         * <remarks>
         * This method is called when the button changes hover state (for example, when the mouse enters or exits the button).
         * It adjusts the color, sprite or animation according to the button configuration.
         * </remarks>
         */
        public void ApplyVisualTransition(bool hovered = false) {

            IButtonTransition transitionButton = GetTransitionStrategy();
            transitionButton?.Apply(this, hovered);
        }

        /**
         * <summary>
         *  Check if the button can be processed.
         * </summary>
         */
        public bool CanProcessButton() {

            // Return false, if the image component is null
            if(targetGraphic == null) return false;

            /*
             * Check validity of values assigned to interaction colors or sprites depending on the transition
             */
            switch(transition) {

                /*
                 * Update the interactable color state
                 */
                case Transition.ColorTint:
                    UpdateInteractableColorState(); // Update the validity of the interaction color state
                    break; // Break the switch statement

                /*
                 * Update the interactable sprite state
                 */
                case Transition.SpriteSwap:
                    UpdateInteractableSpriteState(); // Update the validity of the interaction sprite state
                    break; // Break the switch statement

                /*
                 * Update the interactable animation state
                 */
                case Transition.Animation:
                    UpdateInteractableAnimationState(); // Update the validity of the interaction animation state
                    break; // Break the switch statement

                /*
                 * Break the switch statement if the transition is default or 'None'
                 */
                case Transition.None:
                default: break;
            }

            return interactable;  // Return true, if the button is interactable
        }

        /******************************/
        /****** PRIVATES METHODS ******/
        /******************************/

        /**
         * <summary>
         *  Get the transition strategy of the button (<see cref="IButtonTransition"/>).
         * </summary>
         */
        private IButtonTransition GetTransitionStrategy() {

            /*
             * Return the transition strategy depending on the transition
             */
            return transition switch {
                Transition.ColorTint => ColorBlockUI, // Return the instance of the custom color block
                Transition.SpriteSwap => SpriteStateUI, // Return the instance of the custom sprite state
                Transition.Animation => AnimationTriggersUI, // Return the instance of the custom animation triggers
                _ => null
            };
        }

        /**
         * <summary>
         *  Update the hovered state and apply the visual transition
         * </summary>
         * <param name="isHovered"> Whether the button is hovered or not. </param>
         */
        private void UpdatePointerState(bool isHovered) {

            // Return, if the button is not interactable or the hovered state is the same
            if(!interactable || IsPointed == isHovered) return;

            IsPointed = isHovered; // Update the hovered state
            ApplyVisualTransition(isHovered); // Apply the visual transition
        }

        /**
         * <summary>
         *  Update the validity of the interaction color state
         * </summary>
         */
        private void UpdateInteractableColorState() {

            // Return, if the graphic component is null or the button is hovered
            if(targetGraphic == null || IsPointed) return;

            // Get the current color of the graphic component
            Color currentColor = targetGraphic.color;

            // Save the interactable color of the button, if the current color it is different of the disabled color or hovered color
            if(currentColor != ColorBlockUI.Disabled && currentColor != ColorBlockUI.Hovered) _interactableColor = currentColor;

            // Change the color of the button when is not interactable and the color is not disabled
            targetGraphic.color = !interactable && _interactableColor != ColorBlockUI.Disabled ? ColorBlockUI.Disabled : _interactableColor;

            targetGraphic.SetAllDirty(); // Make the graphic component as dirty
            UpdateTextTransparency(); // Update the transparency (alpha) of the text component color
        }

        /**
         * <summary>
         *  Update the validity of the interaction sprite state
         * </summary>
         */
        private void UpdateInteractableSpriteState() {

            // Return, if the image component is null or the button is hovered
            if(image == null || IsPointed) return;

            // Get the current color of the image component
            Sprite currentSprite = image.sprite;

            // Save the interactable sprite of the button, if the current sprite it is different of the disabled sprite or hovered sprite
            if(currentSprite != SpriteStateUI.Disabled && currentSprite != SpriteStateUI.Hovered) _interactableSprite = currentSprite;

            // Change the color of the button when is not interactable and the sprite is not disabled
            image.sprite = !interactable && _interactableSprite != SpriteStateUI.Disabled ? SpriteStateUI.Disabled : _interactableSprite;

            image.SetAllDirty(); // Make the graphic component as dirty
            UpdateTextTransparency(); // Update the transparency (alpha) of the text component color
        }

        /**
         * <summary>
         *  Update the validity of the interaction animation
         * </summary>
         */
        private void UpdateInteractableAnimationState() {

            // Return, if the animator component is null or the button is hovered
            if(animator == null || IsPointed) return;

            // Get the array of current animation clips info of the animator
            AnimatorClipInfo[] clips = animator.GetCurrentAnimatorClipInfo(0);

            // If there is no clip or the first clip is null, return
            if(clips.Length == 0 || clips[0].clip == null) return;

            // Get the current clip of the animation clip info
            AnimationClip currentClip = clips[0].clip;

            // If the current clip is null, return
            if(!currentClip) return;

            // Save the interactable trigger of the button, if the current trigger it is different of the disabled trigger or hovered trigger
            if(currentClip.name != AnimationTriggersUI.Disabled && currentClip.name != AnimationTriggersUI.Hovered) _interactableTrigger = currentClip.name;

            // Set the trigger depending on whether it is not interactable and the current trigger is not disabled
            animator.SetTrigger(!interactable && _interactableTrigger != AnimationTriggersUI.Disabled ? AnimationTriggersUI.Disabled : _interactableTrigger);

            UpdateTextTransparency(); // Update the transparency (alpha) of the text component color
        }

        /**
         * <summary>
         *  Update the transparency (alpha) of the text component color.
         * </summary>
         */
        private void UpdateTextTransparency() {

            if(!_text) return; // Return, if the text component is null

            // Set the new color changing the transparency (alpha) of text component color
            Color newTextColor = new Color(_text.color.r, _text.color.g, _text.color.b, !interactable ? InactiveTextAlpha : ActiveTextAlpha);

            // Change the transparency (alpha) of the text component color, if it is different of the new color
            if(_text.color != newTextColor) _text.color = newTextColor;

            _text.SetAllDirty(); // Make the text component as dirty
        }
    }
}