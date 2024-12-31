using SakyoGame.Lib.Structs;
using UnityEngine;

namespace SakyoGame.Lib.Utils.Component {

    /**
     * <summary>
     *  Class for utility methods related to <see cref="RectTransform"/> component.
     * </summary>
     */
    public static class RectTransformHandler {

        /**
         * <summary>
         *  Adjusts the anchors of the target '<see cref="RectTransform"/>' relative to the referent '<see cref="RectTransform"/>'.
         * </summary>
         * <param name="targetRT">The target '<see cref="RectTransform"/>' to adjust.</param>
         * <param name="referentRT">The referent '<see cref="RectTransform"/>' to adjust relative to.</param>
         * <param name="anchorMin">The minimum anchor points to adjust.</param>
         * <param name="anchorMax">The maximum anchor points to adjust.</param>
         */
        public static void AdjustAnchors(RectTransform targetRT, RectTransform referentRT, AnchorAdjustments anchorMin = new(),
            AnchorAdjustments anchorMax = new()) {

                // Adjust the minimum anchor points of the target 'RectTransform' relative to the referent 'RectTransform'
                targetRT.anchorMin = new Vector2(referentRT.anchorMin.x + anchorMin.xAxis, referentRT.anchorMin.y + anchorMin.yAxis);

                // Adjust the maximum anchor points of the target 'RectTransform' relative to the referent 'RectTransform'
                targetRT.anchorMax = new Vector2(referentRT.anchorMax.x - anchorMax.xAxis, referentRT.anchorMax.y - anchorMax.yAxis);
        }

        /**
         * <summary>
         *  Syncs the offsets, size delta and scaling of the target '<see cref="RectTransform"/>' with those of the referent '<see cref="RectTransform"/>'.
         * </summary>
         * <param name="targetRT">The target '<see cref="RectTransform"/>' to sync.</param>
         * <param name="referentRT">The referent '<see cref="RectTransform"/>' to sync with.</param>
         */
        public static void SyncWithAnother(RectTransform targetRT, RectTransform referentRT) {

            targetRT.offsetMin = referentRT.offsetMin; // Set minimum offsets identical to those of the 'RectTransform' referent
            targetRT.offsetMax = referentRT.offsetMax; // Set maximum offsets identical to those of the 'RectTransform' referent

            targetRT.sizeDelta = referentRT.sizeDelta; // Set size delta identical to those of the 'RectTransform' referent
            targetRT.localScale = Vector3.one; // Uniform scaling (prevent stretching)
        }
    }
}