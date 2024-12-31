using SakyoGame.Lib.Enums;
using SakyoGame.Lib.Enums.Extensions;
using SakyoGame.Lib.Interfaces.UI;
using UnityEngine.UI;

namespace SakyoGame.Lib.UI.State {

    /**
     * <summary>
     *  Class for updating the animation trigger of an <see cref="Selectable"/> UI component (<see cref="AnimationTriggers"/>).
     * </summary>
     */
    public class AnimationTriggersUI : IButtonTransition {

        /************************/
        /****** PROPERTIES ******/
        /************************/

        /**
         * <summary>
         *  The <see cref="AnimationTriggers"/> instance to update.
         * </summary>
         */
        private AnimationTriggers _animationTriggers;

        /**************************/
        /****** CONSTRUCTORS ******/
        /**************************/

        /**
         * <summary>
         *  Constructor for the <see cref="AnimationTriggersUI"/> class.
         *  Used to set the animation trigger of a <see cref="Selectable"/> UI component using separate animation for each state.
         * </summary>
         * <param name="normal">The normal animation (<see cref="string"/>).</param>
         * <param name="highlighted">The highlighted animation (<see cref="string"/>).</param>
         * <param name="pressed">The pressed animation (<see cref="string"/>).</param>
         * <param name="selected">The selected animation (<see cref="string"/>).</param>
         * <param name="disabled">The disabled animation (<see cref="string"/>).</param>
         * <param name="hovered">The hovered animation (<see cref="string"/>).</param>
         * <param name="selectable">The <see cref="Selectable"/> UI component to update.</param>
         */
        public AnimationTriggersUI(string normal, string highlighted, string pressed, string selected, string disabled,
            string hovered, Selectable selectable) {

                /*
                 * Create a new animation trigger instance with the given animations
                 */
                _animationTriggers = new AnimationTriggers {
                    normalTrigger = normal, // Set the normal animation
                    highlightedTrigger = highlighted, // Set the highlighted animation
                    pressedTrigger = pressed, // Set the pressed animation
                    selectedTrigger = selected, // Set the selected animation
                    disabledTrigger = disabled, // Set the disabled animation
                };

                Selectable = selectable; // Set the selectable UI component
                Hovered = hovered; // Set the hovered custom animation
            }

        /**
         * <summary>
         *  Constructor for the <see cref="AnimationTriggersUI"/> class.
         *  Used to set the animation trigger of a <see cref="Selectable"/> UI component using an instance of a <see cref="AnimationTriggers"/>.
         *  The hovered animation will be a custom animation trigger.
         * </summary>
         * <param name="animationTriggers">The <see cref="AnimationTriggers"/> instance.</param>
         * <param name="hovered">The hovered animation (<see cref="string"/>).</param>
         * <param name="selectable">The <see cref="Selectable"/> UI component to update.</param>
         */
        public AnimationTriggersUI(AnimationTriggers animationTriggers, string hovered, Selectable selectable) {

            _animationTriggers = animationTriggers; // Set the animation trigger instance
            Selectable = selectable; // Set the selectable UI component
            Hovered = hovered; // Set the hovered custom animation
        }

        /*******************************/
        /****** GETTERS & SETTERS ******/
        /*******************************/

        /**
         * <summary>
         *  Getter and setter for the <see cref="Selectable"/> UI component.
         * </summary>
         */
        public Selectable Selectable { get; private set; }

        /**
         * <summary>
         *  Getter and setter for the normal animation (<see cref="string"/>).
         * </summary>
         */
        public string Normal {

            get => _animationTriggers.normalTrigger;
            set => VisualStateExtension.UpdateAnimationTriggers(Selectable, EVisualSate.Normal, value);
        }

        /**
         * <summary>
         *  Getter and setter for the highlighted animation (<see cref="string"/>).
         * </summary>
         */
        public string Highlighted {

            get => _animationTriggers.highlightedTrigger;
            set => VisualStateExtension.UpdateAnimationTriggers(Selectable, EVisualSate.Highlighted, value);
        }

        /**
         * <summary>
         *  Getter and setter for the pressed animation (<see cref="string"/>).
         * </summary>
         */
        public string Pressed {

            get => _animationTriggers.pressedTrigger;
            set => VisualStateExtension.UpdateAnimationTriggers(Selectable, EVisualSate.Pressed, value);
        }

        /**
         * <summary>
         *  Getter and setter for the selected animation (<see cref="string"/>).
         * </summary>
         */
        public string Selected {

            get => _animationTriggers.selectedTrigger;
            set => VisualStateExtension.UpdateAnimationTriggers(Selectable, EVisualSate.Selected, value);
        }

        /**
         * <summary>
         *  Getter and setter for the disabled animation (<see cref="string"/>).
         * </summary>
         */
        public string Disabled {

            get => _animationTriggers.disabledTrigger;
            set => VisualStateExtension.UpdateAnimationTriggers(Selectable, EVisualSate.Disabled, value);
        }

        /**
         * <summary>
         *  Getter and setter for the hovered custom animation trigger (<see cref="string"/>).
         * </summary>
         */
        public string Hovered { get; set; }

        /*******************************/
        /********* PUBLIC METHODS ******/
        /*******************************/

        /**
         * <summary>
         *  Update the animation trigger of the <see cref="Selectable"/> UI component.
         *  If no animation trigger is specified, the <see cref="Selectable"/> UI component will be used to update the existing animation trigger.
         *  Otherwise, the given animation trigger will be used for updating the animation trigger of <see cref="Selectable"/> UI component.
         * </summary>
         */
        public void UpdateAnimationTriggers(AnimationTriggers animationTriggers = null) {

            /*
             * If the animation trigger is null, use the existing animation trigger of the Selectable UI component
             */
            if(animationTriggers == null) _animationTriggers = Selectable.animationTriggers;

            // Else, if any of the animation triggers is not null, use the given animation trigger
            else Selectable.animationTriggers = animationTriggers;
        }

        /************************************/
        /********* IMPLEMENTED METHODS ******/
        /************************************/

        /**
         * <summary>
         *  Apply the animation trigger to the <see cref="Selectable"/> UI component.
         * </summary>
         * <param name="selectableUI">An instance of <see cref="ISelectableUI"/> UI component.</param>
         * <param name="isHovered">Whether the button is hovered or not.</param>
         */
        public void Apply(ISelectableUI selectableUI, bool isHovered) {

            /*
             * Return, if the animator component is null or the selectable is not interactable
             */
            if((selectableUI.Animator == null || !selectableUI.Interactable)
               && selectableUI.AnimationTriggersUI == null) return;

            /*
             * Set the trigger depending on whether it is hovered or not
             */
            string trigger = isHovered ? selectableUI.AnimationTriggersUI.Hovered :
                selectableUI.CurrentAnimationTrigger;

            // Update the animation of the selectable if is not hovered
            selectableUI.Animator.SetTrigger(isHovered ? selectableUI.CurrentAnimationTrigger : trigger);
        }
    }
}