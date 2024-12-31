using System;
using SakyoGame.Lib.Utils.Math;
using SakyoGame.Lib.Utils.Component;
using UnityEngine;
using UnityEngine.UI;

namespace SakyoGame.Lib.UI {

    /**
     * <summary>
     *  Class for adjusting the size of the canvas according to the screen size.
     * </summary>
     */
    [RequireComponent(typeof(RectTransform))]
    [RequireComponent(typeof(CanvasScaler))]
    public class ResponsiveCanvas : MonoBehaviour {

        /************************/
        /****** PROPERTIES ******/
        /************************/

        /**
         * <summary>
         *  The <see cref="CanvasScaler"/> component.
         * </summary>
         */
        private CanvasScaler _canvasScaler;

        /**
         * <summary>
         *  The <see cref="RectTransform"/> component.
         * </summary>
         */
        private RectTransform _rectTransform;

        /**********************************/
        /****** AWAKING AND UPDATING ******/
        /**********************************/

        /**
         * <summary>
         *  Called when the script instance is being loaded.
         *  Initializes the <see cref="RectTransform"/> and <see cref="CanvasScaler"/> components.
         * </summary>
         * <exception cref="MissingComponentException">Thrown when a component is missing.</exception>
         */
        private void Awake() {

            _canvasScaler = GetComponent<CanvasScaler>(); // Get the 'CanvasScaler' component
            _rectTransform = GetComponent<RectTransform>(); // Get the 'RectTransform' component

            /*
             * Check if the 'CanvasScaler' component is missing, if the 'CanvasScaler'
             * component is missing : throw an exception
             */
            CheckerComponent.AssertInGameObject<CanvasScaler>(gameObject);

            /*
             * Check if the 'RectTransform' component is missing, if the 'RectTransform'
             * component is missing : throw an exception
             */
            CheckerComponent.AssertInGameObject<RectTransform>(gameObject);
        }

        /**
         * <summary>
         *  Called every frame.
         *  Adjusts the size of the canvas according to the screen size.
         * </summary>
         */
        private void Update() {

            AdjustCanvasTransform(); // Adjust the canvas transform
            AdjustCanvasScaler(); // Adjust the canvas scaler
        }

        /*****************************/
        /****** PRIVATE METHODS ******/
        /*****************************/

        private void AdjustCanvasTransform() {

            if(!_rectTransform) return; // Return, if the 'RectTransform' component is null

            Vector2 zero = Vector2.zero; // Get the Vector : Vector2.zero
            Vector2 one = Vector2.one; // Get the Vector : Vector2.one

            Vector2 screenSize = ScreenCalculatorUtils.ScreenSize; // Get the screen size as Vector2

            // Set the minimum anchor points, if they are not equal to Vector2.zero
            if(_rectTransform.anchorMin != zero)  _rectTransform.anchorMin = Vector2.zero;

            // Set the maximum anchor points, if they are not equal to Vector2.one
            if(_rectTransform.anchorMax != one)  _rectTransform.anchorMax = Vector2.one;

            // Set the minimum offsets, if they are not equal to Vector2.zero
            if(_rectTransform.offsetMin != zero) _rectTransform.offsetMin = Vector2.zero;

            // Set the maximum offsets, if they are not equal to Vector2.zero
            if(_rectTransform.offsetMax != zero) _rectTransform.offsetMax = Vector2.zero;

            // Set the size delta, if it is not equal to the screen size
            if(_rectTransform.sizeDelta != screenSize) _rectTransform.sizeDelta = screenSize;
        }

        /**
         * <summary>
         *  Adjusts the size of the canvas according to the screen size.
         *  Sets the reference resolution, match width or height, scale factor, physical unit, and scale with screen size.
         * </summary>
         */
        private void AdjustCanvasScaler() {

             if(!_rectTransform) return; // Return, if the 'CanvasScaler' component is null

             // Set the reference resolution to match the screen size
             _canvasScaler.referenceResolution = ScreenCalculatorUtils.ScreenSize;

             switch(_canvasScaler.uiScaleMode) {

                 /*
                  * If the scale mode is 'ScaleWithScreenSize', set the match width or height to match the screen size
                  */
                 case CanvasScaler.ScaleMode.ScaleWithScreenSize:

                     // Set the match width or height to match the screen size
                     _canvasScaler.matchWidthOrHeight = ScreenCalculatorUtils.CalculateMatchValue(Screen.width, Screen.height);
                     break; // Break the switch

                 /*
                  * If the scale mode is 'ConstantPixelSize', set the scale factor to match the screen size
                  */
                 case CanvasScaler.ScaleMode.ConstantPixelSize:

                     // Set the scale factor to match the screen size
                     _canvasScaler.scaleFactor = ScreenCalculatorUtils.CalculateScaleFactor(Screen.width);
                     break; // Break the switch

                 /*
                  * If the scale mode is 'ConstantPhysicalSize', set the physical unit to centimeters and
                  * the scale factor to match the screen DPI
                  */
                 case CanvasScaler.ScaleMode.ConstantPhysicalSize:

                     // Set the physical unit to centimeters
                     _canvasScaler.physicalUnit = CanvasScaler.Unit.Centimeters;

                     // Set the scale factor to match the screen DPI
                     _canvasScaler.scaleFactor = ScreenCalculatorUtils.CalculatePhysicalSizeFactor(Screen.dpi);

                     break; // Break the switch

                 // Throw an exception, if the scale mode is unsupported
                 default: throw new InvalidOperationException("Unsupported scale mode: " + _canvasScaler.uiScaleMode);
             }
        }
    }
}