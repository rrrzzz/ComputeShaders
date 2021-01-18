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
        

        private Transform[] _points = null;

        public void Plot()
        {
            // var offset = _range / _resolution;
            // ResetGraph();
            // var count = Mathf.RoundToInt(_resolution);
            // _points = new Transform[count];
            // for (int i = 0; i < count; i++)
            // {
            //     var point = Instantiate(_pointPrefab, transform);
            //     point.localPosition = new Vector3(i * offset, 0, 0);
            //     point.name = "Sphere " + i;
            //     _points[i] = point;
            // }
            ResetGraph();
            
            _points = new Transform[_pointsCount];
            var position = Vector3.zero;
            for (int i = 0; i < _pointsCount; i++)
            {
                var point = Instantiate(_pointPrefab, transform, false);
                var divisor = _pointsCount / 2f;

                position.x = (i + 0.5f) / divisor - 1;
                // position.y = position.x * position.x;
                
                point.localScale = Vector3.one * _scale / divisor;
                point.localPosition = position;
                point.name = "Sphere " + i;
                _points[i] = point;
            }
        }

        private void Update()
        {
            if (_points == null) return;
            
            var time = Time.time;
            foreach (var point in _points)
            {
                var pos = point.localPosition;

                switch (_functionType)
                {
                    case FunctionType.Wave:
                        pos.y = FunctionLib.Wave(pos.x, time);
                        break;
                    case FunctionType.MultiWave:
                        pos.y = FunctionLib.MultiWave(pos.x, time);
                        break;
                    case FunctionType.Ripple:
                        pos.y = FunctionLib.Ripple(pos.x, time);
                        break;
                    default:
                        throw new ArgumentOutOfRangeException();
                }

                pos.y *= _height;
                point.localPosition = pos;
            }
        }

        public void ResetGraph() 
        {
            if (_points != null)
            {
                Array.ForEach(_points, x=> DestroyImmediate(x.gameObject));
                _points = null;
            }
        } 
    }

    public enum FunctionType
    {
        Wave,
        MultiWave,
        Ripple
    }
}