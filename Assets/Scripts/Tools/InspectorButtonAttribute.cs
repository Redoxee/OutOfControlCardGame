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
public class InspectorButtonAttribute : PropertyAttribute
{
    public static float kDefaultButtonWidth = 80;

    public readonly string MethodName;
    public readonly string ButtonLabel;
    
    public InspectorButtonAttribute(string MethodName, string buttonLabel = "")
    {
        this.MethodName = MethodName;
        this.ButtonLabel = buttonLabel;
    }
}

#if UNITY_EDITOR
[CustomPropertyDrawer(typeof(InspectorButtonAttribute))]
public class InspectorButtonPropertyDrawer : PropertyDrawer
{
    private MethodInfo eventMethodInfo = null;

    public override void OnGUI(Rect position, SerializedProperty prop, GUIContent label)
    {
        InspectorButtonAttribute inspectorButtonAttribute = (InspectorButtonAttribute)this.attribute;
        Rect buttonRect = new Rect(position.x, position.y, position.width, UnityEditor.EditorGUIUtility.singleLineHeight);
        if (GUI.Button(buttonRect, !string.IsNullOrEmpty(inspectorButtonAttribute.ButtonLabel) ? inspectorButtonAttribute.ButtonLabel : label.text))
        {
            System.Type eventOwnerType = prop.serializedObject.targetObject.GetType();
            string eventName = inspectorButtonAttribute.MethodName;

            if (eventMethodInfo == null)
                eventMethodInfo = eventOwnerType.GetMethod(eventName, BindingFlags.Instance | BindingFlags.Static | BindingFlags.Public | BindingFlags.NonPublic);

            if (eventMethodInfo != null)
                eventMethodInfo.Invoke(prop.serializedObject.targetObject, null);
            else
                Debug.LogWarning(string.Format("InspectorButton: Unable to find method {0} in {1}", eventName, eventOwnerType));
        }

        position = new Rect(position.x, position.y + UnityEditor.EditorGUIUtility.singleLineHeight + UnityEditor.EditorGUIUtility.standardVerticalSpacing, position.width, UnityEditor.EditorGUIUtility.singleLineHeight);
        EditorGUI.PropertyField(position, prop, label);
    }

    public override float GetPropertyHeight(SerializedProperty property, GUIContent label)
    {
        return base.GetPropertyHeight(property, label) + UnityEditor.EditorGUIUtility.singleLineHeight + UnityEditor.EditorGUIUtility.standardVerticalSpacing;
    }
}
#endif
