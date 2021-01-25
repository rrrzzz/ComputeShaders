using System;
using System.Diagnostics;
using UnityEngine;

public class Graph : MonoBehaviour
{
    [SerializeField] private float _scale = 1;
    [SerializeField] private int _pointsCount = 100;
    [SerializeField] Transform _pointPrefab;
    [SerializeField] private FunctionType _firstFunctionType;
    [SerializeField] private FunctionType _secondFunctionType;
    [SerializeField] private float _speed = 1;
    [SerializeField] private float _transitionTime = 1;
    [SerializeField] private float _functionShowingTime = 1;
    [SerializeField] private bool _isTransitionOn = true;

    private Transform[] _points;
    private float _divisor;
    private Func<float, float, float, Vector3> _firstFunction;
    private Func<float, float, float, Vector3> _secondFunction;
    private bool _isTransitioning;
    private float _startingTime;
    private Stopwatch _stopwatch;

    public void Plot()
    {
        _stopwatch = new Stopwatch();
        _stopwatch.Start();
        
        _firstFunction = FunctionLib.GetFunction(_firstFunctionType);
        _secondFunction = FunctionLib.GetFunction(_secondFunctionType);
        ResetGraph();
            
        _points = new Transform[_pointsCount * _pointsCount];
        _divisor = 2f / _pointsCount;
        var scale = Vector3.one * (_scale);
            
        for (int i = 0; i < _pointsCount * _pointsCount; i++)
        {
            var point = Instantiate(_pointPrefab, transform, false);
            point.localScale = scale;
            point.name = $"Sphere {i}";
            _points[i] = point;
        }
    }

    private void Update()
    {
        if (_points == null) return;

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
            var firstFunc = _firstFunction;
            _firstFunction = _secondFunction;
            _secondFunction = firstFunc;
        }

        var time = Time.time * _speed;
            
        var v = 0.5f * _divisor - 1;
        for (int i = 0, x = 0, z = 0; i < _pointsCount * _pointsCount; i++, x++)
        {
            if (x == _pointsCount)
            {
                x = 0;
                z += 1;
                v = (z + 0.5f) * _divisor - 1;
            }
                
            var u = (x + 0.5f) * _divisor - 1;
            var point = _points[i];
            Vector3 outputPosition;
            
            var firstOutput = _firstFunction(u, v, time);

            if (_isTransitioning)
            {
                var secondOutput = _secondFunction(u, v, time);
                outputPosition = 
                    Vector3.Lerp(firstOutput, secondOutput, _stopwatch.ElapsedMilliseconds / (_transitionTime * 1000));
            }
            else outputPosition = _firstFunction(u, v, time);
            
            point.localPosition = outputPosition;
        }
    }

    public void ResetGraph()
    {
        if (_points == null) return;

        foreach (var point in _points)
        {
            DestroyImmediate(point.gameObject);
        }
        _points = null;
    }
}