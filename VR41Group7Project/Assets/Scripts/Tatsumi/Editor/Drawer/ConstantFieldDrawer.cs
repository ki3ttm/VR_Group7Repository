using UnityEngine;
using UnityEditor;

[CustomPropertyDrawer(typeof(ConstantFieldAttribute))]
public class ConstantFieldDrawer : PropertyDrawer {
	public override void OnGUI(Rect position, SerializedProperty property, GUIContent label) {
		label.text += " (Constant)";
		EditorGUI.BeginDisabledGroup(EditorApplication.isPlaying);
		EditorGUI.PropertyField(position, property, label);
		EditorGUI.EndDisabledGroup();
	}
}