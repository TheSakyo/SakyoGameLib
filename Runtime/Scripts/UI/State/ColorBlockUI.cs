using SakyoGame.Lib.Enums;
using SakyoGame.Lib.Enums.Extensions;
using SakyoGame.Lib.Interfaces.UI;
using UnityEngine;
using UnityEngine.UI;

namespace SakyoGame.Lib.UI.State {

    /**
     * <summary>
     *  Class for updating the color block of a <see cref="Selectable"/> UI component (<see cref="ColorBlock"/>).
     * </summary>
     */
    public class ColorBlockUI : IButtonTransition {

        /************************/
        /****** PROPERTIES ******/
        /************************/

        /**
         * <summary>
         *  The <see cref="ColorBlock"/> instance to update.
         * </summary>
         */
        private ColorBlock _colorBlock;

        /**************************/
        /****** CONSTRUCTORS ******/
        /**************************/

        /**
         * <summary>
         *  Constructor for the <see cref="ColorBlockUI"/> class.
         *  Used to set the color block of a <see cref="Selectable"/> UI component using separate colors for each state.
         * </summary>
         * <param name="normal">The normal color.</param>
         * <param name="highlighted">The highlighted color.</param>
         * <param name="pressed">The pressed color.</param>
         * <param name="selected">The selected color.</param>
         * <param name="disabled">The disabled color.</param>
         * <param name="hovered">The hovered color.</param>
         * <param name="selectable">The <see cref="Selectable"/> UI component to update.</param>
         */
        public ColorBlockUI(Color normal, Color highlighted, Color pressed, Color selected, Color disabled,
            Color hovered, Selectable selectable) {

                /*
                 * Create a new color block instance with the given colors
                 */
                _colorBlock = new ColorBlock {
                    normalColor = normal, // Set the normal color
                    highlightedColor = highlighted, // Set the highlighted color
                    pressedColor = pressed, // Set the pressed color
                    selectedColor = selected, // Set the selected color
                    disabledColor = disabled, // Set the disabled color
                    colorMultiplier = 1f, // Set the color multiplier
                    fadeDuration = 0.1f // Set the fade duration
                };

                Selectable = selectable; // Set the selectable UI component
                Hovered = hovered; // Set the hovered custom color
            }

        /**
         * <summary>
         *  Constructor for the <see cref="ColorBlockUI"/> class.
         *  Used to set the color block of a <see cref="Selectable"/> UI component using an instance of a <see cref="ColorBlock"/>.
         *  The hovered color will be a custom color.
         * </summary>
         * <param name="colorBlock">The <see cref="ColorBlock"/> instance.</param>
         * <param name="hovered">The hovered color.</param>
         * <param name="selectable">The <see cref="Selectable"/> UI component to update.</param>
         */
        public ColorBlockUI(ColorBlock colorBlock, Color hovered, Selectable selectable) {

            _colorBlock = colorBlock; // Set the color block instance
            Selectable = selectable; // Set the selectable UI component
            Hovered = hovered; // Set the hovered custom color
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
         *  Getter and setter for the normal color (<see cref="Color"/>).
         * </summary>
         */
        public Color Normal {

            get => _colorBlock.normalColor;
            set => VisualStateExtension.UpdateColor(Selectable, EVisualSate.Normal, value);
        }

        /**
         * <summary>
         *  Getter and setter for the highlighted color (<see cref="Color"/>).
         * </summary>
         */
        public Color Highlighted {

            get => _colorBlock.highlightedColor;
            set => VisualStateExtension.UpdateColor(Selectable, EVisualSate.Highlighted, value);
        }

        /**
         * <summary>
         *  Getter and setter for the pressed color (<see cref="Color"/>).
         * </summary>
         */
        public Color Pressed {

            get => _colorBlock.pressedColor;
            set => VisualStateExtension.UpdateColor(Selectable, EVisualSate.Pressed, value);
        }

        /**
         * <summary>
         *  Getter and setter for the selected color (<see cref="Color"/>).
         * </summary>
         */
        public Color Selected {

            get => _colorBlock.selectedColor;
            set => VisualStateExtension.UpdateColor(Selectable, EVisualSate.Selected, value);
        }

        /**
         * <summary>
         *  Getter and setter for the disabled color (<see cref="Color"/>).
         * </summary>
         */
        public Color Disabled {

            get => _colorBlock.disabledColor;
            set => VisualStateExtension.UpdateColor(Selectable, EVisualSate.Disabled, value);
        }

        /**
         * <summary>
         *  Getter and setter for the hovered custom color (<see cref="Color"/>).
         * </summary>
         */
        public Color Hovered { get; set; }

        /**
         * <summary>
         *  Getter for the color multiplier (the value can be modified directly using theUI component).
         * </summary>
         */
        public float ColorMultiplier => _colorBlock.colorMultiplier;

        /**
         * <summary>
         *  Getter for the fade duration (the value can be modified directly using theUI component).
         * </summary>
         */
        public float FadeDuration => _colorBlock.fadeDuration;

        /*******************************/
        /********* PUBLIC METHODS ******/
        /*******************************/

        /**
         * <summary>
         *  Update the color block of the <see cref="Selectable"/> UI component.
         *  If no color block is specified, the <see cref="Selectable"/> UI component will be used to update the existing color block.
         *  Otherwise, the given color block will be used for updating the color block of <see cref="Selectable"/> UI component.
         * </summary>
         */
        public void UpdateColors(ColorBlock colorBlock = default) {

            // If no color block is specified, use the existing color block of the Selectable UI component
            if(colorBlock == default) _colorBlock = Selectable.colors;

            // Else, if any of the color blocks is not null, use the given color block
            else Selectable.colors = _colorBlock;
        }

        /************************************/
        /********* IMPLEMENTED METHODS ******/
        /************************************/

        /**
         * <summary>
         *  Update the color of the <see cref="Selectable"/> UI component.
         * </summary>
         * <param name="selectableUI">An instance of <see cref="ISelectableUI"/> UI component.</param>
         * <param name="isHovered">If the <see cref="Selectable"/> UI component is hovered.</param>
         */
        public void Apply(ISelectableUI selectableUI, bool isHovered) {

            /*
             * Return, if the graphic component is null or the selectable is not interactable
             * and the instance of the custom color block is not null
             */
            if((selectableUI.TargetGraphic == null || !selectableUI.Interactable)
               && selectableUI.ColorBlockUI == null) return;

            /*
             * Change the color of the selectable depending on whether it is hovered or not
             */
            selectableUI.TargetGraphic.color = isHovered ? selectableUI.ColorBlockUI.Hovered :
                selectableUI.CurrentColor;

            // Update the interactable color of the selectable if is not hovered
            selectableUI.CurrentColor = isHovered ? selectableUI.CurrentColor :
                selectableUI.TargetGraphic.color;

            selectableUI.TargetGraphic.SetAllDirty(); // Make the graphic component as dirty
        }
    }
}
