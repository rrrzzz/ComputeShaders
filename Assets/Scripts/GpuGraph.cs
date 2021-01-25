using System.Diagnostics;
using UnityEngine;

public class GpuGraph : MonoBehaviour
{
    private const int MaxResolution = 1000;
    [SerializeField] private ComputeShader _functionShader;
    [SerializeField] private float _scale = 1;
    [Range(8, 1000)]
    [SerializeField] private int _pointsCount = 100;
    [SerializeField] private FunctionType _firstFunctionType;
    [SerializeField] private FunctionType _secondFunctionType;
    [SerializeField] private float _transitionTime = 1;
    [SerializeField] private float _functionShowingTime = 1;
    [SerializeField] private float _speed = 1;
    [SerializeField] private Mesh _mesh;
    [SerializeField] private Material _material;
    [SerializeField] private bool _isTransitionOn = true;
        
    private readonly int _timeId = Shader.PropertyToID("_Time");
    private readonly int _stepId = Shader.PropertyToID("_Step");
    private readonly int _scaleId = Shader.PropertyToID("_Scale");
    private readonly int _transitionProgressId = Shader.PropertyToID("_TransitionProgress");
    private readonly int _resolutionId = Shader.PropertyToID("_Resolution");
    private readonly int _positionsId = Shader.PropertyToID("_Positions");

    private float _divisor;
    private ComputeBuffer _positionsBuffer;
    private Stopwatch _stopwatch;
    private bool _isTransitioning;
    private int _firstFunctionId;
    private int _secondFunctionId;

    public void OnEnable()
    {
        _firstFunctionId = (int) _firstFunctionType;
        _secondFunctionId = (int) _secondFunctionType;
        _stopwatch = new Stopwatch();
        _stopwatch.Start();
        
        _divisor = 2f / _pointsCount;
        _positionsBuffer = new ComputeBuffer(MaxResolution * MaxResolution, 3 * 4);
    }

    private void Update()
    {
        UpdateFunctionOnGPU();
    }

    private void UpdateFunctionOnGPU()
    { 
        if (!_isTransitionOn)
        {
            _isTransitioning = false;
        }
        else if (!_isTransitioning && _stopwatch.ElapsedMilliseconds > _functionShowingTime * 1000)
        {
            _stopwatch.Restart();
            _isTransitioning = true;
        }
        else if (_isTransitioning && _stopwatch.ElapsedMilliseconds > _transitionTime * 1000)
        {
            _stopwatch.Restart();
            _isTransitioning = false;
            var firstFunc = _firstFunctionId;
            _firstFunctionId = _secondFunctionId;
            _secondFunctionId = firstFunc;
        }
        
        var kernelIndex = _firstFunctionId;

        
        //TODO add proper transitioning, random, cycling functions, play with higher frequency / graph resolution
        if (_isTransitioning)
        {
            
            kernelIndex = _firstFunctionId * 6 + _secondFunctionId;
        }
        
        var time = Time.time * _speed;

        var transitionProgress = _stopwatch.ElapsedMilliseconds / (_transitionTime * 1000);
        
        _functionShader.SetFloat(_timeId, time);
        _functionShader.SetFloat(_stepId, _divisor);
        _functionShader.SetFloat(_transitionProgressId, transitionProgress);
        _functionShader.SetInt(_resolutionId, _pointsCount);
        _functionShader.SetBuffer(kernelIndex, _positionsId, _positionsBuffer);
        
        _material.SetFloat(_scaleId, _scale);
        _material.SetBuffer(_positionsId, _positionsBuffer);

        var groups = Mathf.CeilToInt(_pointsCount / 8f);
            
        _functionShader.Dispatch(kernelIndex, groups, groups, 1);
            
        var bounds = new Bounds(Vector3.zero, Vector3.one * (2f + _divisor));
            
        Graphics.DrawMeshInstancedProcedural(_mesh, 0, _material, bounds, _pointsCount * _pointsCount);
    }

    private void OnDisable()
    {
        _positionsBuffer.Release();
        _positionsBuffer = null;
    }
}