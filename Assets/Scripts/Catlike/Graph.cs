using System;
using UnityEngine;

namespace Catlike
{
    public class Graph : MonoBehaviour
    {
        [SerializeField] private float _range;
        [SerializeField] private float _scale = 1;
        [SerializeField] private float _height = 1;
        [SerializeField] private int _pointsCount = 10;
        [SerializeField] Transform _pointPrefab;
        [SerializeField] private FunctionType _functionType;
        [SerializeField] private float _speed = 1;
        
        private Transform[,] _points;
        private Func<float, float, float, float> _function;

        public void Plot()
        {
            _function = FunctionLib.GetFunction(_functionType);
            ResetGraph();
            
            _points = new Transform[_pointsCount, _pointsCount];
            var position = Vector3.zero;
            var divisor = _pointsCount / 2f;
            var scale = Vector3.one * _scale / divisor;
            
            for (int i = 0; i < _pointsCount; i++)
            {
                position.x = (i + 0.5f) / divisor - 1;
   
                for (int j = 0; j < _pointsCount; j++)
                {
                    var point = Instantiate(_pointPrefab, transform, false);
                    
                    position.y = _function(position.x, 0, 0);
                    position.y *= _height;
                    
                    position.z = (j + 0.5f) / divisor - 1;
                    
                    point.localPosition = position;
                    point.localScale = scale;

                    var pointIndex = (j + i * _pointsCount);
                    point.name = $"Sphere {pointIndex}";
                    _points[i,j] = point;
                }
            }
        }

        private void Update()
        {
            if (_points == null) return;
            
            var time = Time.time * _speed;
            
            for (int i = 0; i < _pointsCount; i++)
            {
                for (int j = 0; j < _pointsCount; j++)
                {
                    var point = _points[i, j];
                    var position = point.localPosition;
                    
                    var y = _function(position.x, 0, time);
                    position.y *= _height;

                    position.y = y;
                    point.localPosition = position;
                }
            }
            foreach (var point in _points)
            {
                var pos = point.localPosition;
                
                pos.y = _function(pos.x, 0, time);
                
                pos.y *= _height;
                point.localPosition = pos;
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
    
    public enum FunctionType
    {
        Wave,
        MultiWave,
        Ripple
    }
}