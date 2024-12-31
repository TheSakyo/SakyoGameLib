using SakyoGame.Lib.UI.State;
using UnityEngine;
using UnityEngine.UI;

namespace SakyoGame.Lib.Interfaces.UI {

    /**
     * <summary>
     *  Interface for selectable UI components.
     * </summary>
     */
    public interface ISelectableUI {

        /**
         * <summary>
         *  Get the target graphic component of the selectable.
         * </summary>
         */
        public Graphic TargetGraphic { get; }

        /**
         * <summary>
         *  Get the image component of the selectable.
         * </summary>
         */
        public Image Image { get; }

        /**
         * <summary>
         *  Get the animator component of the selectable.
         * </summary>
         */
        public Animator Animator { get; }

        /**
         * <summary>
         *  Check if the selectable is interactable.
         * </summary>
         */
        public bool Interactable { get; }

        /*** ___________________________________________________ ***/

        /**
         * <summary>
         *  Check if the selectable is pointed.
         * </summary>
         */
        public bool IsPointed { get; }

        /*** ___________________________________________________ ***/

        /**
         * <summary>
         *  Get the custom instance of the <see cref="ColorBlockUI"/> class.
         * </summary>
         */
        public ColorBlockUI ColorBlockUI { get; }

        /**
        * <summary>
        *  Get the custom instance of the <see cref="SpriteStateUI"/> class.
        * </summary>
        */
        public SpriteStateUI SpriteStateUI { get; }

        /**
         * <summary>
         *  Get the custom instance of the <see cref="AnimationTriggersUI"/> class.
         * </summary>
         */
        public AnimationTriggersUI AnimationTriggersUI { get; }

        /*** ___________________________________________________ ***/

        /**
         * <summary>
         *  Get the current color of the selectable.
         * </summary>
         */
        internal Color CurrentColor { get; set; }

        /**
         * <summary>
         *  Get the current sprite of the selectable.
         * </summary>
         */
        internal Sprite CurrentSprite { get; set; }

        /**
         * <summary>
         *  Get the current animation trigger of the selectable.
         * </summary>
         */
        internal string CurrentAnimationTrigger { get; set; }

        /*** ___________________________________________________ ***/
        /*** ___________________________________________________ ***/

        /**
         * <summary>
         *  Apply the visual transition of the selectable depending on whether it is hovered or not.
         * </summary>
         * <param name="hovered"> Whether the selectable is hovered or not. </param>
         * <remarks>
         *  This method is called when the selectable changes hover state (for example, when the mouse enters
         *  or exits the selectable). It adjusts the color, sprite or animation according to the selectable
         *  configuration.
         * </remarks>
         */
        public void ApplyVisualTransition(bool hovered = false);
    }
}