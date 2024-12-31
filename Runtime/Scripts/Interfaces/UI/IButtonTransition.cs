namespace SakyoGame.Lib.Interfaces.UI {

    /**
     * <summary>
     *  Interface for button transitions.
     * </summary>
     */
    public interface IButtonTransition {

        /**
         * <summary>
         *  Apply the visual transition of the button depending on whether it is hovered or not.
         * </summary>
         * <param name="button"> The button to apply the transition to. </param>
         * <param name="isHovered"> Whether the button is hovered or not. </param>
         */
        public void Apply(ISelectableUI button, bool isHovered);
    }
}