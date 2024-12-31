using SakyoGame.Lib.Enums;
using SakyoGame.Lib.Utils.Component;
using UnityEngine;

namespace SakyoGame.Lib.Actors {

    /**
     * <summary>
     *  Class representing the camera view of the player (<see cref="Camera"/>).
     * </summary>
     */
    [RequireComponent(typeof(Camera))]
    public class CameraView: MonoBehaviour {

        /************************/
        /****** PROPERTIES ******/
        /************************/

        /**
         * <summary>
         *  The 'Camera' component attached to camera view.
         * </summary>
         */
        private Camera _camera;

        /*********************/
        /****** AWAKING ******/
        /*********************/

        /**
         * <summary>
         *  Unity's Awake method that runs once at the start of the game.
         *  Initializes the <see cref="Camera"/> component.
         * </summary>
         * <exception cref="MissingComponentException">Thrown when the component is missing.</exception>
         */
        private void Awake() {

            // Get the 'Camera' component attached to camera view
            _camera = GetComponent<Camera>();

            // Check if the 'Camera' component is missing, then throw an exception
            CheckerComponent.AssertInGameObject<Camera>(gameObject);
        }

        /*******************************/
        /****** GETTERS & SETTERS ******/
        /*******************************/

        /**
         * <summary>
         *  The camera associated with the player.
         * </summary>
         */
        internal Camera Target => _camera;

        /**
         * <summary>
         *  The camera mode of the player.
         * </summary>
         */
        internal ECameraMode Mode { get; set; } = ECameraMode.FirstPersonView;
    }
}