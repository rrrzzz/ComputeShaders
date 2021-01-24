using System;
using UnityEngine;

namespace Catlike
{
    public class GpuGraph : MonoBehaviour
    {
        [SerializeField] private ComputeShader _functionShader;
        [SerializeField] private float _scale = 1;
        [SerializeField] private int _pointsCount = 100;
        [SerializeField] private FunctionType _functionType;
        [SerializeField] private float _speed = 1;
        [SerializeField] private Mesh _mesh;
        [SerializeField] private Material _material;
        

        private readonly int _timeId = Shader.PropertyToID("_Time");
        private readonly int _stepId = Shader.PropertyToID("_Step");
        private readonly int _resolutionId = Shader.PropertyToID("_Resolution");
        private readonly int _positionsId = Shader.PropertyToID("_Positions");

        private float _divisor;
        private ComputeBuffer _positionsBuffer;

        public void OnEnable()
        {
            _divisor = _pointsCount / 2f;
            _positionsBuffer = new ComputeBuffer(_pointsCount * _pointsCount, 3 * 4);
        }

        private void Update()
        {
            UpdateFunctionOnGPU();
        }

        private void UpdateFunctionOnGPU()
        {
            var time = Time.time * _speed;
            
            _functionShader.SetFloat(_timeId, time);
            _functionShader.SetFloat(_stepId, _divisor);
            _functionShader.SetInt(_resolutionId, _pointsCount);
            _functionShader.SetBuffer(0, _positionsId, _positionsBuffer);

            var groups = Mathf.CeilToInt(_pointsCount) / 8;
            
            _functionShader.Dispatch(0, groups, groups, 1);
            
            var bounds = new Bounds(Vector3.zero, Vector3.one * (2f + 2f / _pointsCount));
            
            Graphics.DrawMeshInstancedProcedural(_mesh, 0, _material, bounds, _positionsBuffer.count);
            
            // var v = 0.5f / _divisor - 1;
            // for (int i = 0, x = 0, z = 0; i < _pointsCount * _pointsCount; i++, x++)
            // {
            //     if (x == _pointsCount)
            //     {
            //         x = 0;
            //         z += 1;
            //         v = (z + 0.5f) / _divisor - 1;
            //     }
            //     
            //     var u = (x + 0.5f) / _divisor - 1;
            //     var outputPosition = _function(u, v, time);
            // }
        }

        private void OnDisable()
        {
            _positionsBuffer.Release();
            _positionsBuffer = null;
        }
    }
}