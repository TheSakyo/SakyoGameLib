using UnityEngine;
using UnityEngine.UI;
using SakyoGame.Lib.UI.State;

namespace SakyoGame.Lib.Enums.Extensions {

    /**
     * <summary>
     *  Extension class for the color block enumeration (<see cref="EVisualSate"/>).
     * </summary>
     */
    public static class VisualStateExtension {

        /****************************/
        /****** PUBLIC METHODS ******/
        /****************************/

        /**
         * <summary>
         *  Change dynamically the color of the <see cref="Selectable"/> UI component according
         *  to this color block property (<see cref="ColorBlockUI"/>).
         * </summary>
         */
        public static void UpdateColor(Selectable selectableUI, EVisualSate targetedState, Color newColor) {

            // Return if the selectable UI component is null or this transition is not 'ColorTint'
            if(selectableUI == null || selectableUI.transition != Selectable.Transition.ColorTint) return;
            ColorBlock selectableUIColors = selectableUI.colors; // Get the current color block

            /*
             * Switch statement to change the color of the selectable UI according to this color block property
             */
            switch(targetedState) {

                /*
                 * Change the normal color of the selectable UI
                 */
                case EVisualSate.Normal:
                    selectableUIColors.normalColor = newColor;
                    break;

                /*
                 * Change the highlighted color of the selectable UI
                 */
                case EVisualSate.Highlighted:
                    selectableUIColors.highlightedColor = newColor;
                    break;

                /*
                 * Change the pressed color of the selectable UI
                 */
                case EVisualSate.Pressed:
                    selectableUIColors.pressedColor = newColor;
                    break;

                /*
                 * Change the selected color of the selectable UI
                 */
                case EVisualSate.Selected:
                    selectableUIColors.selectedColor = newColor;
                    break;

                /*
                 * Change the disabled color of the selectable UI
                 */
                case EVisualSate.Disabled:
                    selectableUIColors.disabledColor = newColor;
                    break;

                // Throw an exception if the color block type is invalid
                default: throw new System.ArgumentException("Invalid color block type", nameof(targetedState));
            }

            selectableUI.colors = selectableUIColors; // Set the new color block
        }

        /**
         * <summary>
         *  Change dynamically the sprite state of the <see cref="Selectable"/> UI component according
         *  to this sprite state property (<see cref="SpriteStateUI"/>).
         * </summary>
         */
        public static void UpdateSprite(Selectable selectableUI, EVisualSate targetedState, Sprite newSprite) {

            // Return if the selectable UI component is null or this transition is not 'SpriteSwap'
            if(selectableUI == null || selectableUI.transition != Selectable.Transition.SpriteSwap) return;
            SpriteState selectableUISpriteState = selectableUI.spriteState; // Get the current sprite state

            /*
             * Switch statement to change the sprite state of the selectable UI according to this sprite property
             */
            switch(targetedState) {

                /*
                 * Change the highlighted sprite state of the selectable UI
                 */
                case EVisualSate.Highlighted:
                    selectableUISpriteState.highlightedSprite = newSprite;
                    break;

                /*
                 * Change the pressed sprite state of the selectable UI
                 */
                case EVisualSate.Pressed:
                    selectableUISpriteState.pressedSprite = newSprite;
                    break;

                /*
                 * Change the selected sprite state of the selectable UI
                 */
                case EVisualSate.Selected:
                    selectableUISpriteState.selectedSprite = newSprite;
                    break;

                /*
                 * Change the disabled sprite state of the selectable UI
                 */
                case EVisualSate.Disabled:
                    selectableUISpriteState.disabledSprite = newSprite;
                    break;

                /*
                 * Throw an exception if the sprite state type is invalid or 'Normal'
                 */
                case EVisualSate.Normal:
                default: throw new System.ArgumentException("Invalid sprite state type", nameof(targetedState));
            }

            selectableUI.spriteState = selectableUISpriteState; // Set the new sprite state
        }

        /**
         * <summary>
         *  Change dynamically the animation triggers of the <see cref="Selectable"/> UI component according
         *  to this animation trigger property (<see cref="AnimationTriggersUI"/>).
         * </summary>
         */
        public static void UpdateAnimationTriggers(Selectable selectableUI, EVisualSate targetedState, string newAnimationTrigger) {

            // Return if the selectable UI component is null or this transition is not 'Animation'
            if(selectableUI == null || selectableUI.transition != Selectable.Transition.Animation) return;
            AnimationTriggers selectableUIAnimationTriggers = selectableUI.animationTriggers; // Get the current animation triggers

            /*
             * Switch statement to change the animation triggers of the selectable UI according to this sprite property
             */
            switch(targetedState) {

                /*
                 * Change the normal animation of the selectable UI
                 */
                case EVisualSate.Normal:
                    selectableUIAnimationTriggers.normalTrigger = newAnimationTrigger;
                    break;

                /*
                 * Change the highlighted animation of the selectable UI
                 */
                case EVisualSate.Highlighted:
                    selectableUIAnimationTriggers.highlightedTrigger = newAnimationTrigger;
                    break;

                /*
                 * Change the pressed animation f the selectable UI
                 */
                case EVisualSate.Pressed:
                    selectableUIAnimationTriggers.pressedTrigger = newAnimationTrigger;
                    break;

                /*
                 * Change the selected animation of the selectable UI
                 */
                case EVisualSate.Selected:
                    selectableUIAnimationTriggers.selectedTrigger = newAnimationTrigger;
                    break;

                /*
                 * Change the disabled animation of the selectable UI
                 */
                case EVisualSate.Disabled:
                    selectableUIAnimationTriggers.disabledTrigger = newAnimationTrigger;
                    break;

                // Throw an exception if the animation type is invalid
                default: throw new System.ArgumentException("Invalid animation triggers type", nameof(targetedState));
            }

            selectableUI.animationTriggers = selectableUIAnimationTriggers; // Set the new animation triggers
        }
    }
}