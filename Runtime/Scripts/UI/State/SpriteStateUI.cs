using SakyoGame.Lib.Enums;
using SakyoGame.Lib.Enums.Extensions;
using SakyoGame.Lib.Interfaces.UI;
using UnityEngine;
using UnityEngine.UI;

namespace SakyoGame.Lib.UI.State {

    /**
     * <summary>
     *  Class for updating the sprite state of an <see cref="Selectable"/> UI component (<see cref="SpriteState"/>).
     * </summary>
     */
    public class SpriteStateUI : IButtonTransition {

        /************************/
        /****** PROPERTIES ******/
        /************************/

        /**
         * <summary>
         *  The <see cref="SpriteState"/> instance to update.
         * </summary>
         */
        private SpriteState _spriteState;

        /**************************/
        /****** CONSTRUCTORS ******/
        /**************************/

        /**
         * <summary>
         *  Constructor for the <see cref="SpriteStateUI"/> class.
         *  Used to set the sprite state of a <see cref="Selectable"/> UI component using separate sprite for each state.
         * </summary>
         * <param name="highlighted">The highlighted sprite.</param>
         * <param name="pressed">The pressed sprite.</param>
         * <param name="selected">The selected sprite.</param>
         * <param name="disabled">The disabled sprite.</param>
         * <param name="hovered">The hovered sprite.</param>
         * <param name="selectable">The <see cref="Selectable"/> UI component to update.</param>
         */
        public SpriteStateUI(Sprite highlighted, Sprite pressed, Sprite selected, Sprite disabled,
            Sprite hovered, Selectable selectable) {

                /*
                 * Create a new sprite state instance with the given sprites
                 */
                _spriteState = new SpriteState {
                    highlightedSprite = highlighted, // Set the highlighted sprite
                    pressedSprite = pressed, // Set the pressed sprite
                    selectedSprite = selected, // Set the selected sprite
                    disabledSprite = disabled, // Set the disabled sprite
                };

                Selectable = selectable; // Set the selectable UI component
                Hovered = hovered; // Set the hovered custom sprite
            }

        /**
         * <summary>
         *  Constructor for the <see cref="SpriteStateUI"/> class.
         *  Used to set the sprite state of a <see cref="Selectable"/> UI component using an instance of a <see cref="SpriteState"/>.
         *  The hovered sprite will be a custom sprite state.
         * </summary>
         * <param name="spriteState">The <see cref="SpriteState"/> instance.</param>
         * <param name="hovered">The hovered sprite.</param>
         * <param name="selectable">The <see cref="Selectable"/> UI component to update.</param>
         */
        public SpriteStateUI(SpriteState spriteState, Sprite hovered, Selectable selectable) {

            _spriteState = spriteState; // Set the sprite state instance
            Selectable = selectable; // Set the selectable UI component
            Hovered = hovered; // Set the hovered custom sprite
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
         *  Getter and setter for the highlighted sprite (<see cref="Sprite"/>).
         * </summary>
         */
        public Sprite Highlighted {

            get => _spriteState.highlightedSprite;
            set => VisualStateExtension.UpdateSprite(Selectable, EVisualSate.Highlighted, value);
        }

        /**
         * <summary>
         *  Getter and setter for the pressed sprite (<see cref="Sprite"/>).
         * </summary>
         */
        public Sprite Pressed {

            get => _spriteState.pressedSprite;
            set => VisualStateExtension.UpdateSprite(Selectable, EVisualSate.Pressed, value);
        }

        /**
         * <summary>
         *  Getter and setter for the selected sprite (<see cref="Sprite"/>).
         * </summary>
         */
        public Sprite Selected {

            get => _spriteState.selectedSprite;
            set => VisualStateExtension.UpdateSprite(Selectable, EVisualSate.Selected, value);
        }

        /**
         * <summary>
         *  Getter and setter for the disabled sprite (<see cref="Sprite"/>).
         * </summary>
         */
        public Sprite Disabled {

            get => _spriteState.disabledSprite;
            set => VisualStateExtension.UpdateSprite(Selectable, EVisualSate.Disabled, value);
        }

        /**
         * <summary>
         *  Getter and setter for the hovered custom sprite state (<see cref="Sprite"/>).
         * </summary>
         */
        public Sprite Hovered { get; set; }


        /*******************************/
        /********* PUBLIC METHODS ******/
        /*******************************/

        /**
         * <summary>
         *  Update the sprite state of the <see cref="Selectable"/> UI component.
         *  If no sprite state is specified, the <see cref="Selectable"/> UI component will be used to update the existing sprite state.
         *  Otherwise, the given sprite state will be used for updating the sprite state of <see cref="Selectable"/> UI component.
         * </summary>
         */
        public void UpdateSprites(SpriteState spriteState = default) {
            
            /*
             * If all the sprite states are null, use the existing sprite state of the Selectable UI component
             */
            if(spriteState.highlightedSprite == null && spriteState.pressedSprite == null &&
                spriteState.selectedSprite == null && spriteState.disabledSprite == null) _spriteState = Selectable.spriteState;
           
            // Else, if any of the sprite states is not null, use the given sprite state
            else Selectable.spriteState = spriteState;
        }

        /************************************/
        /********* IMPLEMENTED METHODS ******/
        /************************************/

        /**
         * <summary>
         *  Update the sprite of the <see cref="Selectable"/> UI component.
         * </summary>
         * <param name="selectableUI">An instance of <see cref="ISelectableUI"/> UI component.</param>
         * <param name="isHovered">If the <see cref="Selectable"/> UI component is hovered.</param>
         */
        public void Apply(ISelectableUI selectableUI, bool isHovered) {

            /*
             * Return, if the image component is null or the selectable is not interactable
             */
            if((selectableUI.Image == null || !selectableUI.Interactable) &&
               selectableUI.SpriteStateUI == null) return;

            /*
             * Change the sprite of the selectableUI depending on whether it is hovered or not
             */
            selectableUI.Image.sprite = isHovered ? selectableUI.SpriteStateUI.Hovered :
                selectableUI.CurrentSprite;

            // Update the interactable sprite of the selectable  if is not hovered
            selectableUI.CurrentSprite = isHovered ? selectableUI.CurrentSprite : selectableUI.Image.sprite;

            selectableUI.Image.SetAllDirty(); // Make the image component as dirty
        }
    }
}
