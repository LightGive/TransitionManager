using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using System.Reflection;

namespace LightGive
{
	[CustomEditor(typeof(TransitionManager))]
	public class TransitionManagerEditor : Editor
	{
		private Vector3 m_centerPosition;
		private Material m_previewMat;
		private float m_lerp = 0.5f;

		private SerializedProperty m_transitionTypeProp;
		private SerializedProperty m_durationProp;
		private SerializedProperty m_ruleTexProp;
		private SerializedProperty m_texColorProp;
		private SerializedProperty m_transShaderProp;
		private SerializedProperty m_animCurveProp;
		private SerializedProperty m_isInvertProp;

		private bool m_isCustom
		{
			get
			{
				if (m_transitionTypeProp == null)
					return false;
				return m_transitionTypeProp.enumValueIndex == (int)TransitionManager.TransitionType.Custom;
			}
		}

		private void OnEnable()
		{
			//SerializedProperty取得
			m_transitionTypeProp = serializedObject.FindProperty("m_transitionType");
			m_durationProp = serializedObject.FindProperty("m_duration");
			m_ruleTexProp = serializedObject.FindProperty("m_ruleTex");
			m_texColorProp = serializedObject.FindProperty("m_texColor");
			m_transShaderProp = serializedObject.FindProperty("m_transShader");
			m_animCurveProp = serializedObject.FindProperty("m_animCurve");
			m_isInvertProp = serializedObject.FindProperty("m_isInvert");

			var transShader = Shader.Find(TransitionManager.TransitionShaderName);
			serializedObject.Update();
			m_transShaderProp.objectReferenceValue = transShader;

			Debug.Log("Shader設定");
			m_previewMat = new Material((Shader)transShader);
			if ((Texture)m_ruleTexProp.objectReferenceValue != null)
			{
				SetMaterialParamAll();
			}
			serializedObject.ApplyModifiedProperties();
		}


		public override void OnInspectorGUI()
		{
			serializedObject.Update();

			EditorGUILayout.Space();
			EditorGUILayout.LabelField("TransitionManager", EditorStyles.boldLabel);
			EditorGUILayout.Space();

			EditorGUI.BeginChangeCheck();
			EditorGUILayout.PropertyField(m_transitionTypeProp);
			if (EditorGUI.EndChangeCheck())
			{
				SetMaterialParamAll();
			}

			EditorGUILayout.PropertyField(m_durationProp);
			EditorGUILayout.PropertyField(m_animCurveProp);

			//色を変更した時
			EditorGUI.BeginChangeCheck();
			EditorGUILayout.PropertyField(m_texColorProp);
			if (EditorGUI.EndChangeCheck())
			{
				m_previewMat.SetColor(TransitionManager.ShaderParamColor, m_texColorProp.colorValue);
			}

			//TransitionTypeがCustomになっている時
			if (m_isCustom)
			{
				EditorGUILayout.Space();
				EditorGUILayout.LabelField("【CustomSetting】");

				if (m_transShaderProp.objectReferenceValue == null)
				{
					EditorGUILayout.HelpBox("Please add a 'LightGive/Unlit/TransitionShader'shader to the project", MessageType.Error);
				}
				else
				{
					EditorGUI.BeginDisabledGroup(true);
					EditorGUILayout.PropertyField(m_transShaderProp);
					EditorGUI.EndDisabledGroup();

					//ルール画像の変更をチェック***
					EditorGUI.BeginChangeCheck();
					EditorGUILayout.PropertyField(m_ruleTexProp);
					if (EditorGUI.EndChangeCheck())
					{
						SetMaterialParamAll();
					}

					//反転のトグルをチェック***
					EditorGUI.BeginChangeCheck();
					EditorGUILayout.PropertyField(m_isInvertProp);
					if (EditorGUI.EndChangeCheck())
					{
						m_previewMat.SetFloat(TransitionManager.ShaderParamFloatInvert, m_isInvertProp.boolValue ? 1.0f : 0.0f);
					}

					EditorGUILayout.Space();
					EditorGUILayout.LabelField("TransitionPreview");

					//ルール画像が設定されているかで処理を分ける
					if ((Texture)m_ruleTexProp.objectReferenceValue != null)
					{
						//ルール画像が設定されている時
						//スライダーの変更をチェック
						EditorGUI.BeginChangeCheck();
						m_lerp = EditorGUILayout.Slider(m_lerp, 0.0f, 1.0f);
						if (EditorGUI.EndChangeCheck())
						{
							m_previewMat.SetFloat(TransitionManager.ShaderParamFloatCutoff, m_lerp);
						}

						float contextWidth = (float)typeof(EditorGUIUtility).GetProperty("contextWidth", BindingFlags.NonPublic | BindingFlags.Static).GetValue(null, null);
						var w = contextWidth - 30f;
						var h = w * 0.563f;
						GUILayout.Box(GUIContent.none, GUILayout.Width(w), GUILayout.Height(h));
						var lastRect = GUILayoutUtility.GetLastRect();
						lastRect.width -= 4;
						lastRect.height -= 4;
						lastRect.x += 2;
						lastRect.y += 2;
						EditorGUI.DrawPreviewTexture(lastRect, Texture2D.whiteTexture, m_previewMat);
					}
					else
					{
						EditorGUILayout.HelpBox("Please set rule texture.", MessageType.Info);
					}
				}
			}
			serializedObject.ApplyModifiedProperties();
		}

		/// <summary>
		/// マテリアルにパラメータを設定
		/// </summary>
		void SetMaterialParamAll()
		{
			m_previewMat.SetTexture(TransitionManager.ShaderParamTextureGradation, (Texture)m_ruleTexProp.objectReferenceValue);
			m_previewMat.SetColor(TransitionManager.ShaderParamColor, m_texColorProp.colorValue);
			m_previewMat.SetFloat(TransitionManager.ShaderParamFloatInvert, m_isInvertProp.boolValue ? 1.0f : 0.0f);
			m_previewMat.SetFloat(TransitionManager.ShaderParamFloatCutoff, m_lerp);

		}
	}
}