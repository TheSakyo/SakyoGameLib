using System;

namespace SakyoGame.Lib.Shared.Attributes {

    /**
     * <summary>
     *  Attribute to mark features that are in beta and may not be fully functional or stable.
     * </summary>
     */
    [AttributeUsage(AttributeTargets.All)]
    public class BetaAttribute : Attribute {

        /**
         * <summary>
         *  The reason why this feature is in beta.
         * </summary>
         */
        public string Reason { get; }

        /**
         * <summary>
         *  Constructor for the BetaAttribute.
         * </summary>
         * <param name="reason">The reason why this feature is in beta.</param>
         */
        public BetaAttribute(string reason = "This feature is in beta.") { Reason = reason; }
    }
}