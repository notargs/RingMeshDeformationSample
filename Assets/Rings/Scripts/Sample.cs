using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace Rings.Scripts
{
    public class Sample : MonoBehaviour
    {
        [SerializeField] private Material surfaceMaterial;
        [SerializeField] private Material lineMaterial;
        [SerializeField] private Slider slider;

        const int Count = 10;

        private readonly List<Matrix4x4> _matrices = new List<Matrix4x4>();
        private readonly List<Vector4> _colors = new List<Vector4>();

        private Mesh _mesh;
        private MaterialPropertyBlock _materialPropertyBlock;
        private static readonly int ColorProperty = Shader.PropertyToID("_Color");

        private void Start()
        {
            _materialPropertyBlock = new MaterialPropertyBlock();

            var meshGenerator = new MeshGenerator();

            _mesh = meshGenerator.CreateMesh();

            for (var y = 0; y < Count; ++y)
            {
                for (var x = 0; x < Count; ++x)
                {
                    _matrices.Add(Matrix4x4.TRS(
                        new Vector2(x, y) / Count,
                        Quaternion.identity,
                        Vector3.one / Count
                    ));
                }
            }

            for (var i = 0; i < Count * Count; ++i)
            {
                _colors.Add(Vector4.zero);
            }
        }

        private void Update()
        {
            _materialPropertyBlock.SetFloat("_PolarBlend", slider.value);
            
            for (var i = 0; i < Count * Count; ++i)
            {
                var t = Time.time * 3.0f;
                
                _colors[i] = new Vector4(
                    Mathf.Sin((i + t) * 0.7f) * 0.5f,
                    Mathf.Sin((i + t) * 1.3f) * 0.5f ,
                    Mathf.Sin((i + t) * 2.3f) * 0.5f,
                    0
                );
            }
            _materialPropertyBlock.SetVectorArray(ColorProperty, _colors);

            Graphics.DrawMeshInstanced(_mesh, 0, surfaceMaterial, _matrices, _materialPropertyBlock);
            
            for (var i = 0; i < Count * Count; ++i)
            {
                var t = Time.time * 3.0f;
                
                _colors[i] = new Vector4(
                    Mathf.Sin((i + t) * 0.7f) * 0.5f + 0.5f,
                    Mathf.Sin((i + t) * 1.3f) * 0.5f + 0.5f,
                    Mathf.Sin((i + t) * 2.3f) * 0.5f + 0.5f,
                    0
                );
            }
            _materialPropertyBlock.SetVectorArray(ColorProperty, _colors);

            Graphics.DrawMeshInstanced(_mesh, 1, lineMaterial, _matrices, _materialPropertyBlock);
        }
    }
}