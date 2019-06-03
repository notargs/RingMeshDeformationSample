using System.Collections.Generic;
using UnityEngine;

namespace Rings.Scripts
{
    public class MeshGenerator
    {
        public Mesh CreateMesh()
        {
            var mesh = new Mesh();

            var vertices = new List<Vector3>();
            var triangles = new List<int>();
            var lines = new List<int>();

            var split = 10;

            // Construct vertices
            for (var i = 0; i < split; ++i)
            {
                vertices.AddRange(new[]
                {
                    new Vector3((float) i / (split - 1), 0),
                    new Vector3((float) i / (split - 1), 1),
                });
            }

            // Construct triangles
            for (var i = 0; i < split - 1; ++i)
            {
                triangles.AddRange(new[]
                {
                    i * 2 + 0,
                    i * 2 + 1,
                    i * 2 + 2,
                    i * 2 + 2,
                    i * 2 + 1,
                    i * 2 + 3,
                });
            }

            // Construct lines
            for (var i = 0; i < split; ++i)
            {
                lines.Add(i * 2);
            }

            for (var i = 0; i < split; ++i)
            {
                lines.Add(split * 2 - i * 2 - 1);
            }

            lines.Add(0);

            mesh.subMeshCount = 2;
            mesh.SetVertices(vertices);
            mesh.SetIndices(triangles.ToArray(), MeshTopology.Triangles, 0);
            mesh.SetIndices(lines.ToArray(), MeshTopology.LineStrip, 1);

            return mesh;
        }
    }
}