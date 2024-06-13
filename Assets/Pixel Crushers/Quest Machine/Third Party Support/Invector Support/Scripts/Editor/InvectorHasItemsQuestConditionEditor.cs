// Copyright © Pixel Crushers. All rights reserved.

using UnityEngine;
using UnityEditor;

namespace PixelCrushers.QuestMachine
{

    [CustomEditor(typeof(InvectorHasItemsQuestCondition))]
    public class InvectorHasItemsQuestConditionEditor : Editor
    {
        private string[] m_nameList = null;

        protected void OnEnable()
        {
            if (target == null || serializedObject == null) return;
            m_nameList = QuestEditorUtility.GetCounterNames();
        }

        public override void OnInspectorGUI()
        {
            serializedObject.Update();
            EditorGUILayout.PropertyField(serializedObject.FindProperty("m_itemID"));
            EditorGUILayout.PropertyField(serializedObject.FindProperty("m_valueMode"));
            EditorGUILayout.PropertyField(serializedObject.FindProperty("m_requiredAmount"));
            EditorGUILayout.BeginHorizontal();
            var counterIndexProperty = serializedObject.FindProperty("m_counterIndex");
            QuestEditorUtility.EditorGUILayoutCounterNamePopup(counterIndexProperty, m_nameList);
            if (GUILayout.Button("x", EditorStyles.miniButton, GUILayout.Width(24))) counterIndexProperty.intValue = -1;
            EditorGUILayout.EndHorizontal();
            serializedObject.ApplyModifiedProperties();
        }
    }
}