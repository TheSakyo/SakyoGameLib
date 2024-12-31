using SakyoGame.Lib.Enums;
using SakyoGame.Lib.Structs;
using SakyoGame.Lib.Utils.Component;
using UnityEngine;

namespace SakyoGame.Lib.UI {

    [RequireComponent(typeof(RectTransform))]
    public class ResponsiveUI : MonoBehaviour {

        /************************/
        /****** PROPERTIES ******/
        /************************/

        /**
         * <summary>
         *  The responsive mode to use (<see cref="EResponsiveMode"/>).
         *  Either screen size or parent size adjustment.
         * </summary>
         */
        [SerializeField]
        private EResponsiveMode responsiveMode;

        /**
         * <summary>
         *  The percentage of the <see cref="Screen"/> width or the parent
         * '<see cref="RectTransform"/>' width to set the size delta.
         * </summary>
         */
        [SerializeField]
        private float percentageWidth;

        /**
         * <summary>
         *  The percentage of the <see cref="Screen"/> height or the parent
         * '<see cref="RectTransform"/>' height to set the size delta.
         * </summary>
         */
        [SerializeField]
        private float percentageHeight;

        /**
         * <summary>
         *  The percentage of the <see cref="Screen"/> width or the parent
         * '<see cref="RectTransform"/>' width to set the position delta.
         * </summary>
         */
        [SerializeField]
        private AnchorAdjustments anchorMinAdjusts;

        /**
         * <summary>
         *  The percentage of the <see cref="Screen"/> height or the parent
         * '<see cref="RectTransform"/>' height to set the position delta.
         * </summary>
         */
        [SerializeField]
        private AnchorAdjustments anchorMaxAdjusts;

        /**
         * <summary>
         *  The <see cref="RectTransform"/> component of the current <see cref="GameObject"/>.
         * </summary>
         */
        private RectTransform _rectTransform;

        /**
         * <summary>
         *  The parent <see cref="RectTransform"/> component of the current <see cref="GameObject"/>.
         * </summary>
         */
        private RectTransform _rectTransformParent;

        /*****************************************/
        /****** AWAKING AND FIXING UPDATING ******/
        /*****************************************/

        /**
         * <summary>
         *  Called when the script instance is being loaded.
         *  Initializes the <see cref="RectTransform"/> and
         *  parent <see cref="RectTransform"/> components.
         * </summary>
         * <exception cref="MissingComponentException">Thrown when a component is missing.</exception>
         */
        private void Awake() {

            _rectTransform = GetComponent<RectTransform>(); // Get the 'RectTransform' component

            // Check if the 'RectTransform' component is missing, if the 'RectTransform' component is missing : throw an exception
            CheckerComponent.AssertInGameObject<RectTransform>(gameObject);

            // Get the parent 'RectTransform' component, if the 'RectTransform' component is not null
            if(_rectTransform) _rectTransformParent = _rectTransform.parent.GetComponent<RectTransform>();
        }

        /**
         * <summary>
         *  Called every fixed framerate frame. Adjusts the size of the canvas according
         *  to the screen size or the parent <see cref="RectTransform"/> size.
         * </summary>
         */
        private void FixedUpdate() { AdjustToParentSize(); }

        /*******************************/
        /****** GETTERS & SETTERS ******/
        /*******************************/

        /**
         * <summary>
         *  The responsive mode to use (<see cref="EResponsiveMode"/>).
         *  Either screen size or parent size adjustment.
         * </summary>
         */
        public EResponsiveMode ResponsiveMode { get => responsiveMode; set => responsiveMode = value; }

        /**
         * <summary>
         *  The percentage of the <see cref="Screen"/> width or the parent
         * '<see cref="RectTransform"/>' width to set the size delta.
         * </summary>
         */
        public float PercentageWidth { get => percentageWidth; set => percentageWidth = value; }

        /**
         * <summary>
         *  The percentage of the <see cref="Screen"/> height or the parent
         * '<see cref="RectTransform"/>' height to set the size delta.
         * </summary>
         */
        public float PercentageHeight { get => percentageHeight; set => percentageHeight = value; }

        /*****************************/
        /****** PRIVATE METHODS ******/
        /*****************************/

        /**
         * <summary>
         *  Adjusts the size of the canvas according to the screen size
         *  or the parent '<see cref="RectTransform"/>' size.
         * </summary>
         */
        private void AdjustToParentSize() {

            if(!_rectTransform) return; // Return, if the 'RectTransform' component is null
            _rectTransform.pivot = new Vector2(0f, 0f); // Set the pivot point (0f, 0f)

            /*
             * If the parent 'RectTransform' component is not null, and the responsive mode is 'ParentSize',
             * adjust the anchors, offsets, size delta and scaling with those of the parent 'RectTransform'
             */
            if(_rectTransformParent && responsiveMode == EResponsiveMode.ParentSize) {

                /*
                 * Adjusts the anchors points relative to the parent 'RectTransform'
                 */
                RectTransformHandler.AdjustAnchors(_rectTransform, _rectTransformParent,
                    anchorMinAdjusts, anchorMaxAdjusts);

                // Sync the offsets, size delta and scaling
                RectTransformHandler.SyncWithAnother(_rectTransform, _rectTransformParent);

           /*
            * Else, if the responsive mode is 'ScreenSize', set the size delta to match the screen width
            * or the parent 'RectTransform' width
            */
            } else {

                /*
                 * Set the size delta to match the screen width or the parent 'RectTransform' width
                 */
                _rectTransform.SetSizeWithCurrentAnchors(RectTransform.Axis.Horizontal, Screen.width * percentageWidth);

                /*
                 * Set the size delta to match the screen height or the parent 'RectTransform' height
                 */
                _rectTransform.SetSizeWithCurrentAnchors(RectTransform.Axis.Vertical, Screen.height * percentageHeight);
            }
        }
    }
}