using System;
using UnityEngine;

public class GpuGraph : MonoBehaviour
{
    private const int MaxResolution = 1000;
    [SerializeField] private ComputeShader _functionShader;
    [SerializeField] private float _scale = 1;
    [Range(8, 1000)]
    [SerializeField] private int _pointsCount = 100;
    [SerializeField] private FunctionType _functionType;
    [SerializeField] private float _speed = 1;
    [SerializeField] private Mesh _mesh;
    [SerializeField] private Material _material;
        
    private readonly int _timeId = Shader.PropertyToID("_Time");
    private readonly int _stepId = Shader.PropertyToID("_Step");
    private readonly int _scaleId = Shader.PropertyToID("_Scale");
    private readonly int _resolutionId = Shader.PropertyToID("_Resolution");
    private readonly int _positionsId = Shader.PropertyToID("_Positions");

    private float _divisor;
    private ComputeBuffer _positionsBuffer;

    public void OnEnable()
    {
        _divisor = 2f / _pointsCount;
        _positionsBuffer = new ComputeBuffer(MaxResolution * MaxResolution, 3 * 4);
    }

    private void Update()
    {
        UpdateFunctionOnGPU();
    }

    private void UpdateFunctionOnGPU()
    { 
        var kernelIndex = (int)_functionType;
        var time = Time.time * _speed;
        
        _functionShader.SetFloat(_timeId, time);
        _functionShader.SetFloat(_stepId, _divisor);
        _functionShader.SetInt(_resolutionId, _pointsCount);
        _functionShader.SetBuffer(kernelIndex, _positionsId, _positionsBuffer);
        
        _material.SetFloat(_scaleId, _scale);
        _material.SetBuffer(_positionsId, _positionsBuffer);

        var groups = Mathf.CeilToInt(_pointsCount / 8f);
            
        _functionShader.Dispatch(kernelIndex, groups, groups, 1);
            
        var bounds = new Bounds(Vector3.zero, Vector3.one * (2f));
            
        Graphics.DrawMeshInstancedProcedural(_mesh, 0, _material, bounds, _pointsCount * _pointsCount);
    }

    private void OnDisable()
    {
        _positionsBuffer.Release();
        _positionsBuffer = null;
    }
}