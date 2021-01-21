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
            for (int i = 0; i < parent.childCount; i++)
            {
                DestroyImmediate(parent.GetChild(i).gameObject);
            }
        }
    }
}
