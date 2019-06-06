using System.Collections.Generic;
using UnityEngine;

namespace RingsWithoutInstancing
{
    [RequireComponent(typeof(MeshFilter))]
    public class Sample2 : MonoBehaviour
    {
        private MeshFilter _meshFilter;
        private Mesh _mesh;

        private readonly List<Vector3> _vertices = new List<Vector3>();
        private readonly List<int> _triangles = new List<int>();
        private readonly List<int> _lines = new List<int>();
        private readonly List<Color> _colors = new List<Color>();
        
        private void Start()
        {
            _mesh = new Mesh {subMeshCount = 2};
            _mesh.MarkDynamic();
            _meshFilter = GetComponent<MeshFilter>();
            _meshFilter.sharedMesh = _mesh;
        }

        private void Update()
        {
            _vertices.Clear();
            _triangles.Clear();
            _lines.Clear();
            _colors.Clear();
            
            const int count = 10;
            for (var y = 0; y < count; ++y)
            {
                for (var x = 0; x < count; ++x)
                {
                    var id = x + y;
                    var color = Mathf.Sin(id + Time.time) * 0.5f + 0.5f;
                    var surfaceColor = new Color(color, color, color);
                    var edgeColor = new Color(1 - color, 1 - color, 1 - color);
                    AddBox(surfaceColor, edgeColor, Matrix4x4.TRS(new Vector3(x, y, 0) / count, Quaternion.identity, Vector3.one * 0.1f));
                }
            }
            
            _mesh.SetVertices(_vertices);
            _mesh.SetColors(_colors);
            _mesh.SetIndices(_triangles.ToArray(), MeshTopology.Triangles, 0);
            _mesh.SetIndices(_lines.ToArray(), MeshTopology.Lines, 1);
        }

        private static Vector3 ToPolarCoordinate(Vector3 position)
        {
            var angle = position.x * Mathf.PI * 2;
            var length = position.y;
            return new Vector3(Mathf.Sin(angle) * length, Mathf.Cos(angle) * length, position.z);
        }

        private void AddBox(Color surfaceColor, Color edgeColor, Matrix4x4 transform)
        {
            var verticesOffset = _vertices.Count;
            
            const int split = 10;
            for (var i = 0; i < split; ++i)
            {
                _vertices.Add(ToPolarCoordinate(transform.MultiplyPoint(new Vector3((float)i / (split - 1), 0, 0))));
                _vertices.Add(ToPolarCoordinate(transform.MultiplyPoint(new Vector3((float)i / (split - 1), 1, 0))));
                _colors.Add(surfaceColor);
                _colors.Add(surfaceColor);
            }
            
            for (var i = 0; i < split - 1; ++i)
            {
                _triangles.Add(verticesOffset + i * 2 + 0);
                _triangles.Add(verticesOffset + i * 2 + 1);
                _triangles.Add(verticesOffset + i * 2 + 2);
                _triangles.Add(verticesOffset + i * 2 + 2);
                _triangles.Add(verticesOffset + i * 2 + 1);
                _triangles.Add(verticesOffset + i * 2 + 3);
            }
            
            verticesOffset = _vertices.Count;
            for (var i = 0; i < split; ++i)
            {
                _vertices.Add(ToPolarCoordinate(transform.MultiplyPoint(new Vector3((float)i / (split - 1), 0, 0))));
                _colors.Add(edgeColor);
            }
            for (var i = 0; i < split; ++i)
            {
                _vertices.Add(ToPolarCoordinate(transform.MultiplyPoint(new Vector3((float)(split - i - 1) / (split - 1), 1, 0))));
                _colors.Add(edgeColor);
            }
            
            for (var i = 0; i < split * 2; ++i)
            {
                _lines.Add(verticesOffset + i);
                _lines.Add(verticesOffset + (i + 1) % (split * 2));
            }
        }
    }
}
