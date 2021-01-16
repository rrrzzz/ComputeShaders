using System;
using UnityEngine;

namespace Catlike
{
    public class Graph : MonoBehaviour
    {
        [SerializeField] private float _range;
        [SerializeField] private float _resolution;
        [SerializeField] private float _height;
        [SerializeField] private int _pointsCount;
        [SerializeField] Transform _pointPrefab;
        
        private Transform[] _points;

        public void Plot()
        {
            var offset = _range / _resolution;
            ResetGraph();
            var count = Mathf.RoundToInt(_resolution);
            _points = new Transform[count];
            for (int i = 0; i < count; i++)
            {
                var point = Instantiate(_pointPrefab, transform);
                point.localPosition = new Vector3(i * offset, 0, 0);
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
                pos.y = FunctionLib.Wave(pos.x, time) * _height;
                point.localPosition = pos;
            }
        }

        private void ResetGraph() 
        {
            if (_points != null) Array.ForEach(_points, x=> Destroy(x.gameObject));
        } 
    }
}