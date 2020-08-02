using UnityEngine;
using UnityEditor;

public static class GameObjectUtils
{
    [MenuItem("GameObject/DuplicateEmptyTransform", true, 10)]
    public static bool ValidateSelectedGameObjectInScene()
    {
        GameObject source = UnityEditor.Selection.activeGameObject;
        if (source == null)
        {
            return false;
        }
        
        if(source.scene == null)
        {
            return false;
        }

        return true;
    }

    [MenuItem("GameObject/DuplicateEmptyTransform", false, 10)]
    public static GameObject DuplicateObjectTransformInPlace()
    {
        GameObject source = UnityEditor.Selection.activeGameObject;
        if (source == null)
        {
            return null;
        }

        GameObject result = new GameObject();
        if (source.transform.parent != null)
        {
            result.transform.parent = source.transform.parent;
        }

        result.transform.position = source.transform.position;
        result.name = source.name;

        UnityEditor.Selection.SetActiveObjectWithContext(result, null);
        return result;
    }
}
