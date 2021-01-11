using System;
using System.Collections.Generic;
using UnityEngine;

public class GraphPlotter : MonoBehaviour
{
    [SerializeField] private GameObject _pointPrefab;
    [Range(1,200)]
    [SerializeField] private int _widthResolution;
    [Range(1, 200)]
    [SerializeField] private float _range;
    [Range(1,20)]
    [SerializeField] private float _heightMultiplier;
    [Range(1,100)]
    [SerializeField] private int _circleResolution;
    [SerializeField] private float _rippleMultiplier;
    [SerializeField] private float _downsizeMultiplier = 0.1f;
    [SerializeField] private float _speed;
    [SerializeField] private int _cycles = 10;
    [SerializeField] private float _circleOffset = .5f;
    [SerializeField] private float _radius = 6;
    [SerializeField] private PlotType _plotType;
    
    private List<GameObject> _gos = new List<GameObject>();
    private Func<float, float> _function;
    private float _functionStep;
    private float _increment;
    private GameObject _center;

    public void PlotFunction(Func<float, float> function, float functionStep)
    {
        _increment = 0;
        ResetPlot();
        
        _function = function;
        _functionStep = functionStep; 
        

        var counter = 0;

        switch (_plotType)
        {
            case PlotType.Ripple:
            {
                var widthOffset = _range / _widthResolution;
                _center = Instantiate(_pointPrefab, 
                    new Vector3(0, function(functionStep * _rippleMultiplier) * _heightMultiplier, 0), Quaternion.identity, transform);
                _center.name = $"Sphere_{counter++}";
        
                for (int i = 1; i < _widthResolution; i++)
                {
                    var r = i * widthOffset;
                    var totalSteps = r * _circleResolution;
                    var stepAngle = Mathf.PI * 2 / totalSteps;

                    var y = function(r * functionStep * _rippleMultiplier) * _heightMultiplier;
            
                    for (int j = 0; j < totalSteps; j++)
                    {
                        var x = Mathf.Cos(stepAngle * j) * r;
                        var z = Mathf.Sin(stepAngle * j) * r;
                
                        var go = Instantiate(_pointPrefab, new Vector3(x, y, z), Quaternion.identity, transform);
                        go.name = $"Sphere_{counter++}";
                        _gos.Add(go);
                    }
                }
                break;
            }
            case PlotType.SinSphere:
            {
                var totalSteps = (float)Math.PI * 2 * _radius / _circleOffset;

                var withHeightOffset = (_heightMultiplier * 4) / totalSteps + _circleOffset;
                totalSteps = (float)Math.PI * 2 * _radius / (_circleOffset * _circleOffset / withHeightOffset);

                var stepsInt = Mathf.RoundToInt(totalSteps);
                var singleStepAngle = (float)Math.PI * 2 / stepsInt;
                
                for (int i = 0; i < stepsInt / 2; i++)
                {
                    var angleSpacing = singleStepAngle * i;
                    var y = function(singleStepAngle * i * _cycles) * _heightMultiplier;
                    var x = Mathf.Cos(angleSpacing) * _radius;
                    var z = Mathf.Sin(angleSpacing) * _radius;
                    
                    var go = Instantiate(_pointPrefab, new Vector3(x, y, z), Quaternion.identity, transform);
                    go.name = $"Sphere_{counter++}";
                    _gos.Add(go);
                }
                break;
            }
            case PlotType.SimpleSin:
            {
                var widthOffset = _range / _widthResolution;
                for (int i = 0; i < _widthResolution; i++)
                {
                    var y = function(functionStep * i) * _heightMultiplier;
                    var x = widthOffset * i;
                    var z = 0;
                
                    var go = Instantiate(_pointPrefab, new Vector3(x, y, z), Quaternion.identity, transform);
                    go.name = $"Sphere_{counter++}";
                    _gos.Add(go);
                }
                break;
            }
        }
    }

    private void Update()
    {
        if (_gos.Count == 0) return;
        _increment += Time.deltaTime * _speed;
        
        switch (_plotType)
        {
            case PlotType.Ripple:
            {
                var widthOffset = _range / _widthResolution;
                var centerPos = _center.transform.position;

                centerPos.y = _function(_functionStep * _rippleMultiplier + _increment) * _heightMultiplier;
                _center.transform.position = centerPos;
                var passedStepsCount = 0;
                var heightDownsize = 1f;
                for (int i = 1; i < _widthResolution; i++)
                {
                    heightDownsize += _downsizeMultiplier;
                    var r = i * widthOffset;
                    var totalSteps = r * _circleResolution;

                    var y = _function(r * _functionStep * _rippleMultiplier + _increment) *
                            (_heightMultiplier / heightDownsize);
                    var currentCounter = 0;
                    for (int j = 0; j < totalSteps; j++)
                    {
                        currentCounter++;
                        var currentIndex = j + passedStepsCount;
                        var sphere = _gos[currentIndex];
                        var pos = sphere.transform.position;
                        pos.y = y;
                        sphere.transform.position = pos;
                    }

                    passedStepsCount += currentCounter;
                }

                break;
            }
            case PlotType.SinSphere:
            {
                var singleStepAngle = (float)Math.PI * 2 / _gos.Count;
                for (int i = 0; i < _gos.Count; i++)
                {
                    var y = _function(singleStepAngle * i * _cycles + _increment) * _heightMultiplier;
                    var pos = _gos[i].transform.position;
                    pos.y = y;
                    _gos[i].transform.position = pos;
                }
                break;
            }
            
            case PlotType.SimpleSin:
            {
                for (int i = 0; i < _widthResolution; i++)
                {
                    var y = _function(_functionStep * i + _increment) * _heightMultiplier;
                    var go = _gos[i];
                    var pos = go.transform.position;
                    pos.y = y;
                    go.transform.position = pos;
                }
                break;
            }
        }
    }

    public void ResetPlot()
    {
        if (_center != null) DestroyImmediate(_center);
        _gos.ForEach(DestroyImmediate);
        _gos.Clear();
    }
}

public enum PlotType
{
    SimpleSin,
    Ripple,
    SinSphere
}
