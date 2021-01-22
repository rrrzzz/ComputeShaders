using UnityEditor;
using UnityEngine;

namespace Editor
{
    public class DeleteChildren : MonoBehaviour
    {
        [MenuItem("CONTEXT/Transform/DeleteChildren", false, 151)]
        private static void DeleteAllChildren()
        {
            var parent = Selection.activeGameObject.transform;
            var components = parent.GetComponentsInChildren<MeshFilter>();

            foreach (var component in components)
            {
                DestroyImmediate(component.gameObject);
            }
        }
    }
}
