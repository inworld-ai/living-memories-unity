#if UNITY_EDITOR
using System;
using System.Linq;
using Inworld.Framework.Attributes;
using Inworld.Framework.Primitive;
using UnityEditor;
using UnityEngine;
using Object = UnityEngine.Object;

namespace Inworld.Framework.Editor
{
	[CustomEditor(typeof(InworldFrameworkModule), true)]
	public class InworldFrameworkModuleEditor : UnityEditor.Editor
	{
		public override void OnInspectorGUI()
		{
			serializedObject.Update();

			Object obj = target;
			Type type = obj.GetType();
			ModelTypeAttribute policy = Attribute.GetCustomAttribute(type, typeof(ModelTypeAttribute)) as ModelTypeAttribute;

			string activeTarget = EditorUserBuildSettings.activeBuildTarget.ToString();
			string activeGroup  = EditorUserBuildSettings.selectedBuildTargetGroup.ToString();

			SerializedProperty modelTypeProp = serializedObject.FindProperty("m_ModelType");

			bool shouldLock = false;
			string forceName = null;

			if (policy != null)
			{
				forceName = policy.ForceEnumName;

				if (policy.LockAlways)
					shouldLock = true;
				else if (policy.OnlyTargets != null && policy.OnlyTargets.Length > 0)
					shouldLock = MatchesAny(activeTarget, activeGroup, policy.OnlyTargets);
				else if (policy.ExcludeTargets != null && policy.ExcludeTargets.Length > 0)
					shouldLock = !MatchesAny(activeTarget, activeGroup, policy.ExcludeTargets);
			}

			using (new EditorGUI.DisabledScope(true))
				EditorGUILayout.ObjectField("Script", MonoScript.FromMonoBehaviour((MonoBehaviour)obj), typeof(MonoScript), false);

			// m_ModelType
			if (modelTypeProp != null)
			{
				if (shouldLock && !string.IsNullOrEmpty(forceName))
				{
					// Keep the serialized value in sync with the forced enum shown in the inspector
					ApplyForcedEnumToTargets(modelTypeProp, forceName);
					using (new EditorGUI.DisabledScope(true))
						EditorGUILayout.PropertyField(modelTypeProp, true);
				}
				else
				{
					EditorGUILayout.PropertyField(modelTypeProp, true);
				}
			}

			DrawPropertiesExcluding(serializedObject, "m_Script", "m_ModelType");

			serializedObject.ApplyModifiedProperties();
		}
		static void ApplyForcedEnumToTargets(SerializedProperty prop, string enumName)
		{
			if (prop == null)
				return;
			
			// First update the current SerializedObject
			ForceEnumByName(prop, enumName);
			prop.serializedObject.ApplyModifiedProperties();
			
			// Then sync to all target objects so the value is actually persisted
			foreach (Object targetObj in prop.serializedObject.targetObjects)
			{
				if (!targetObj)
					continue;
				
				SerializedObject so = new SerializedObject(targetObj);
				SerializedProperty p = so.FindProperty(prop.propertyPath);
				if (p == null)
					continue;
				
				ForceEnumByName(p, enumName);
				so.ApplyModifiedPropertiesWithoutUndo();
			}
		}
		static bool MatchesAny(string activeTarget, string activeGroup, string[] tokens)
		{
			return tokens.Any(t => 
				string.Equals(t, activeTarget, StringComparison.OrdinalIgnoreCase) || 
				string.Equals(t, activeGroup, StringComparison.OrdinalIgnoreCase));
		}

		static void ForceEnumByName(SerializedProperty prop, string enumName)
		{
			for (int i = 0; i < prop.enumNames.Length; i++)
			{
				if (!string.Equals(prop.enumNames[i], enumName, StringComparison.OrdinalIgnoreCase) &&
				    !string.Equals(prop.enumDisplayNames[i], enumName, StringComparison.OrdinalIgnoreCase)) 
					continue;
				prop.enumValueIndex = i;
				return;
			}
		}
	}
}
#endif