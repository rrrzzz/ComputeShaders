using UnityEditor;
using UnityEngine;

namespace Editor
{
 
    [CustomEditor(typeof(GraphPlotter))]
    public class GraphPlotterEditor : UnityEditor.Editor
    {
        private float _functionMultiplier = 0;
        private string _defaultString = "10";
        
        public override void OnInspectorGUI()
        {
            DrawDefaultInspector();
            var plotter = target as GraphPlotter;

            GUILayout.Label("Function step in degrees");
            _defaultString = GUILayout.TextField(_defaultString);

            if (float.TryParse(_defaultString, out var val))
            {
                _functionMultiplier = val;
            }

            if (GUILayout.Button("Plot function"))
            {
                plotter.PlotFunction(Mathf.Sin, _functionMultiplier * Mathf.PI / 180);
            }

            if (GUILayout.Button("Clear"))
            {
                plotter.ResetPlot();
            }
        }
    }
}