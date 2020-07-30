using UnityEngine;
#if UNITY_EDITOR
using UnityEditor;
#endif
using System.Reflection;

/// <summary>
/// This attribute can only be applied to fields because its
/// associated PropertyDrawer only operates on fields (either
/// public or tagged with the [SerializeField] attribute) in
/// the target MonoBehaviour.
/// </summary>
[System.AttributeUsage(System.AttributeTargets.Field)]
public class InspectorRefreshAttribute : PropertyAttribute
{
    public readonly string MethodeName;
    public InspectorRefreshAttribute(string methodeName)
    {
        this.MethodeName = methodeName;
    }
}

#if UNITY_EDITOR
[CustomPropertyDrawer(typeof(InspectorRefreshAttribute))]
public class InspectorRefreshPropertyDrawer : PropertyDrawer
{
    private MethodInfo eventMethodInfo = null;

    public override void OnGUI(Rect position, SerializedProperty prop, GUIContent label)
    {
        EditorGUI.BeginChangeCheck();
        EditorGUI.PropertyField(position, prop, label);
        if (EditorGUI.EndChangeCheck())
        {
            InspectorRefreshAttribute inspectorRefreshAttribute = (InspectorRefreshAttribute)this.attribute;
            System.Type eventOwnerType = prop.serializedObject.targetObject.GetType();

            if (eventMethodInfo == null)
                eventMethodInfo = eventOwnerType.GetMethod(inspectorRefreshAttribute.MethodeName, BindingFlags.Instance | BindingFlags.Static | BindingFlags.Public | BindingFlags.NonPublic);

            if (eventMethodInfo != null)
                eventMethodInfo.Invoke(prop.serializedObject.targetObject, null);
            else
                Debug.LogWarning(string.Format("InspectorRefresh: Unable to find method {0} in {1}", inspectorRefreshAttribute.MethodeName, eventOwnerType));
        }
    }
}
#endif