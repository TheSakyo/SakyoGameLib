using System;
using SakyoGame.Lib.Maps.Meshes;
using UnityEngine;

namespace SakyoGame.Lib.Textures {

    /**
     * <summary>
     *  Class for displaying and updating the game world.
     * </summary>
     */
    public class Display : MonoBehaviour {

        /************************/
        /****** PROPERTIES ******/
        /************************/

        /**
         * <summary>
         *  A reference to the <see cref="MeshFilter"/> component used to render the terrain mesh.
         * </summary>
         */
        [SerializeField]
        private MeshFilter meshFilter;

        /**
         * <summary>
         *  A reference to the <see cref="MeshRenderer"/> component that handles rendering the mesh.
         * </summary>
         */
        [SerializeField]
        private MeshRenderer meshRenderer;


        /**
         * <summary>
         *  A reference to the <see cref="MeshCollider"/> component that handles
         *  collision detection for the mesh.
         * </summary>
         */
        [SerializeField]
        private MeshCollider meshCollider;

        /****************************/
        /****** PUBLIC METHODS ******/
        /****************************/

        /**
         * <summary>
         *  Updates the terrain mesh and applies a texture to it.
         *  Sets the mesh data for the terrain and applies the provided texture.
         * </summary>
         * <param name="meshData">The mesh data to be applied to the mesh filter.</param>
         * <param name="texture">The texture to be applied to the mesh renderer.</param>
         * <exception cref="NullReferenceException">Thrown when the mesh data is null.</exception>
         * <exception cref="MissingComponentException">Thrown when no mesh components are found.</exception>
         */
        public virtual void DrawMeshMap(MeshData meshData, Texture texture) {

            // Throw an exception if the mesh data is null
            if(meshData == null) throw new NullReferenceException("Mesh data is null, please generate a mesh first.");

            // Throw an exception if no mesh components are found
            if(!meshCollider && !meshFilter && !meshRenderer) throw new MissingComponentException("No mesh components found");

            // Generate the mesh using the mesh data
            Mesh drawnMesh = meshData.CreateMesh();

            meshCollider.sharedMesh = drawnMesh; // Assign the generated mesh to the mesh collider
            meshFilter.sharedMesh = drawnMesh; // Assign the generated mesh to the mesh filter

            // Apply the provided texture to the mesh renderer
            meshRenderer.sharedMaterial.mainTexture = texture;
        }
    }
}

