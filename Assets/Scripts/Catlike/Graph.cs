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
        
        private Transform[] _points;
        private float _divisor;
        private Func<float, float, float, Vector3> _function;

        public void Plot()
        {
            _function = FunctionLib.GetFunction(_functionType);
            ResetGraph();
            
            _points = new Transform[_pointsCount * _pointsCount];
            _divisor = _pointsCount / 2f;
            var scale = Vector3.one * _scale / _divisor;
            
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
            
            var time = Time.time * _speed;
            
            var v = 0.5f / _divisor - 1;
            for (int i = 0, x = 0, z = 0; i < _pointsCount * _pointsCount; i++, x++)
            {
                if (x == _pointsCount)
                {
                    x = 0;
                    z += 1;
                    v = (z + 0.5f) / _divisor - 1;
                }
                
                var u = (x + 0.5f) / _divisor - 1;
                var point = _points[i, j];
                var outputPosition = _function(u, v, time);
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
}