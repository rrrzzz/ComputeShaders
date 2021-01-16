using Catlike;
using UnityEditor;
using UnityEngine;

namespace Editor.Catlike
{
    [CustomEditor(typeof(Graph))]
    public class GraphEditor : UnityEditor.Editor
    {
        public override void OnInspectorGUI()
        {
            DrawDefaultInspector();

            var graph = target as Graph;
            
            if (GUILayout.Button("Plot Graph"))
            {
                graph.Plot();    
            }
        }
    }
}
