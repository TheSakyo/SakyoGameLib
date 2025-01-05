using SakyoGame.Lib.Shared.Attributes;
using UnityEngine;

namespace SakyoGame.Lib.Maps.Meshes {

    /**
     * <summary>
     *  Class for storing mesh data, including vertices, triangles, and UV coordinates.
     * </summary>
     */
    public class MeshData {

        /************************/
        /****** PROPERTIES ******/
        /************************/

        /**
         * <summary>
         *  A 2D array representing the height values of the terrain at each point.
         * </summary>
         */
        private readonly float[,] _heightMap;

        /**
         * <summary>
         *  A 3D array representing the volume values of the terrain at each point.
         * </summary>
         */
        [Beta("This property is still in development.")]
        private readonly float[,,] _volumeMap;

        /**
         * <summary>
         *  Array to hold the vertices of the mesh. Each vertex represents a point in 3D space.
         * </summary>
         */
        private readonly Vector3[] _vertices;

        /**
         * <summary>
         *  Array to hold the triangles of the mesh. Each set of three integers represents
         *  an index of vertices that form a triangle.
         * </summary>
         */
        private readonly int[] _triangles;

        /**
         * <summary>
         *  Array to hold the UV mapping for the mesh. Each UV is a 2D texture coordinate.
         * </summary>
         */
        private readonly Vector2[] _uvs;

        /**
         * <summary>
         *  Keeps track of the current index in the triangles array.
         *  This is used to add new triangles in a sequential manner.
         * </summary>
         */
        private int _triangleIndex;

        /*************************/
        /****** CONSTRUCTOR ******/
        /*************************/

        /**
         * <summary>
         *  Constructor for initializing a new MeshData object with specified width and height.
         *  Allocates memory for vertices, UVs, and triangles based on the grid dimensions.
         * </summary>
         *  <param name="heightMap"></param>
         * <returns>A new MeshData object with the specified dimensions.</returns>
         */
        public MeshData(float[,] heightMap) {

            // Allocate a 2D array for the height map in the mesh data
            _heightMap = heightMap;

            Width = _heightMap.GetLength(0); // Set the width of the height map in the mesh data
            Height = _heightMap.GetLength(1); // Set the height of the height map in the mesh data

            _vertices = new Vector3[Width * Width]; // Create an array of vertices
            _uvs = new Vector2[Width * Width]; // Create an array of UV coordinates
            _triangles = new int[(Width - 1) * (Width - 1) * 6]; // Create an array of triangles
        }

        /**
         * <summary>
         *  Constructor for initializing a new MeshData object with specified width, height, and depth.
         *  Allocates memory for vertices, UVs, and triangles based on the grid dimensions.
         * </summary>
         * <param name="volumeMap"></param>
         * <returns>A new MeshData object with the specified dimensions.</returns>
         */
        [Beta("This constructor is still in development and may not be fully functional.")]
        public MeshData(float[,,] volumeMap) {

            // Allocate a 3D array for the volume map in the mesh data
            _volumeMap = volumeMap;

            Width = _volumeMap.GetLength(0); // Set the width of the volume map in the mesh data
            Height = _volumeMap.GetLength(1); // Set the height of the volume map in the mesh data
            Depth = _volumeMap.GetLength(2); // Set the depth of the volume map in the mesh data

            _vertices = new Vector3[Width * Height * Depth]; // Create an array of vertices
            _uvs = new Vector2[Width * Height * Depth]; // Create an array of UV coordinates
            _triangles = new int[(Width - 1) * (Height - 1) * (Depth - 1) * 6]; // Create an array of triangles
        }

        /****************************/
        /****** PUBLIC METHODS ******/
        /****************************/

        /**
         * <summary>
         *  Adds a triangle to the mesh using indices for the vertices.
         *  Each triangle consists of three vertices (a, b, c) which form a triangular face.
         * </summary>
         * <param name="a">Index of the first vertex of the triangle.</param>
         * <param name="b">Index of the second vertex of the triangle.</param>
         * <param name="c">Index of the third vertex of the triangle.</param>
         */
        public void AddTriangle(int a, int b, int c) {

            /*
             * Store the triangle indices in the triangles array
             */
            _triangles[_triangleIndex] = a;
            _triangles[_triangleIndex + 1] = b;
            _triangles[_triangleIndex + 2] = c;

            _triangleIndex += 3; // Move to the next position in the triangles array
        }

        /**
         * <summary>
         *  Creates and returns a Mesh object using the stored vertices, triangles, and UVs.
         *  This method also recalculates the normals for proper lighting and shading.
         * </summary>
         * <returns>A newly created Mesh object.</returns>
         */
        public Mesh CreateMesh() {

            Mesh mesh = new Mesh(); // Create a new Mesh object

            mesh.vertices = _vertices; // Set the vertices for the mesh
            mesh.triangles = _triangles; // Set the triangles for the mesh
            mesh.uv = _uvs; // Set the UV coordinates for the mesh
            mesh.RecalculateNormals(); // Recalculate normals for proper lighting/shading

            return mesh; // Return the fully constructed mesh
        }

        /*********************/
        /****** GETTERS ******/
        /*********************/

        /**
         * <summary>
         *  Gets or Sets the width of the mesh grid.
         * </summary>
         */
        public int Width { get; internal set; }

        /**
         * <summary>
         *  Gets Or Sets the height of the mesh grid.
         * </summary>
         * <remarks>
         *  The setter is not
         * </remarks>
         */
        public int Height { get; internal set; }

        /**
         * <summary>
         *  Gets Or Sets the depth of the mesh grid.
         * </summary>
         * <remarks>
         *  The setter is not
         * </remarks>
         */
        [Beta("This property is still in development.")]
        public int Depth { get; internal set; }

        /**
         * <summary>
         *  Gets the heightMap array.
         * </summary>
         */
        public float[,] HeightMap => _heightMap;

        /**
         * <summary>
         *  Gets the volumeMap array.
         * </summary>
         */
        [Beta("This property is still in development.")]
        public float[,,] VolumeMap => _volumeMap;

        /**
         * <summary>
         *  Gets the vertices array.
         * </summary>
         */
        public Vector3[] Vertices => _vertices;

        /**
         * <summary>
         *  Gets the UV array for texture mapping.
         * </summary>
         */
        public Vector2[] Uvs => _uvs;

        /**
         * <summary>
         *  Gets the triangles array.
         * </summary>
         */
        public int[] Triangles => _triangles;
    }
}