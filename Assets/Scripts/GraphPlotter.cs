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
    [SerializeField] private float _speed;
    
    
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
        var widthOffset = _range / _widthResolution;

        var counter = 0;
        _center = Instantiate(_pointPrefab, Vector3.zero, Quaternion.identity);
        
        for (int i = 1; i < _widthResolution; i++)
        {
            var r = i * widthOffset;
            var totalSteps = r * _circleResolution;
            var stepAngle = Mathf.PI * 2 / totalSteps;

            var y = function(r * functionStep) * _heightMultiplier;
            
            for (int j = 0; j < totalSteps; j++)
            {
                var x = Mathf.Cos(stepAngle * j) * r;
                var z = Mathf.Sin(stepAngle * j) * r;
                
                var go = Instantiate(_pointPrefab, new Vector3(x, y, z), Quaternion.identity, transform);
                go.name = $"Sphere_{counter++}";
                _gos.Add(go);
            }
        }
    }

    private void Update()
    {
        if (_gos.Count == 0) return;
        _increment += Time.deltaTime * _speed;
        var widthOffset = _range / _widthResolution;
        var centerPos = _center.transform.position;

        centerPos.y = _function(_functionStep + _increment);
        _center.transform.position = centerPos;
        for (int i = 1; i < _widthResolution; i++)
        {
            var r = i * widthOffset;
            var totalSteps = r * _circleResolution;

            var toInt = Mathf.FloorToInt(totalSteps);
            var intSteps = totalSteps / toInt == 0 ? toInt : toInt + 1;
            
            var y = _function(r * _functionStep + _increment) * _heightMultiplier;
            
            for (int j = 0; j < totalSteps; j++)
            {
                var currentIndex = j + (i - 1) * intSteps;
                var sphere = _gos[currentIndex];
                var pos = sphere.transform.position;
                pos.y = y;
                sphere.transform.position = pos;
            }
        }
        
        // for (int i = 0; i < _resolution; i++)
        // {
        //     var x = _gos[i * _zResolution].transform.position.x;
        //     var y = _function(i * _functionStep + _increment) * (Mathf.Abs(x) / _range) * 5f;
        //     for (int z = 0; z < _zResolution; z++)
        //     {
        //         var currentTr = _gos[z + i * _zResolution].transform;
        //         var pos = currentTr.position;
        //         pos.y = y;
        //         currentTr.position = pos;
        //     }
        // }
        //
        // for (int i = 0, q = 1; i < Mathf.FloorToInt(_widthResolution / 2); i++, q++)
        // {
        //     var x = _gos[i * _circleResolution].transform.position.x;
        //     var y = _function(q * _functionStep + _increment) * (Mathf.Abs(x) / _widthResolution) * _heightMultiplier;
        //     for (int z = 0; z < _circleResolution; z++)
        //     {
        //         var currentTr = _gos[z + i * _circleResolution].transform;
        //         var pos = currentTr.position;
        //         pos.y = y;
        //         currentTr.position = pos;
        //     }
        // }
        //
        // for (int i = Mathf.FloorToInt(_widthResolution / 2), q = 1; i < _widthResolution-1; i++, q++)
        // {
        //     var x = _gos[i * _circleResolution].transform.position.x;
        //     var y = _function(q * _functionStep + _increment) * (Mathf.Abs(x) / _widthResolution) * _heightMultiplier;
        //     for (int z = 0; z < _circleResolution; z++)
        //     {
        //         var currentTr = _gos[z + i * _circleResolution].transform;
        //         var pos = currentTr.position;
        //         pos.y = y;
        //         currentTr.position = pos;
        //     }
        // }
    }

    public void ResetPlot()
    {
        _gos.ForEach(DestroyImmediate);
        _gos.Clear();
        DestroyImmediate(_center);
    }
}
