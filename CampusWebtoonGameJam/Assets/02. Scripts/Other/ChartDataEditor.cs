using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(ChartData))]
public class ChartDataEditor : Editor
{
    private SerializedProperty chartIDProperty;
    private SerializedProperty musicProperty;
    private SerializedProperty noteTravelTimeProperty;
    private SerializedProperty notesProperty;

    private void OnEnable()
    {
        chartIDProperty = serializedObject.FindProperty("chartID");
        musicProperty = serializedObject.FindProperty("music");
        noteTravelTimeProperty = serializedObject.FindProperty("noteTravelTime");
        notesProperty = serializedObject.FindProperty("notes");
    }

    public override void OnInspectorGUI()
    {
        serializedObject.Update();

        // 기본 필드들
        EditorGUILayout.PropertyField(chartIDProperty);
        EditorGUILayout.PropertyField(musicProperty);
        EditorGUILayout.PropertyField(noteTravelTimeProperty);

        // 노트 리스트 헤더
        EditorGUILayout.LabelField("Notes", EditorStyles.boldLabel);

        // 노트 리스트 관리
        for (int i = 0; i < notesProperty.arraySize; i++)
        {
            SerializedProperty noteProperty = notesProperty.GetArrayElementAtIndex(i);
            SerializedProperty typeProperty = noteProperty.FindPropertyRelative("type");
            SerializedProperty timeProperty = noteProperty.FindPropertyRelative("time");

            EditorGUILayout.BeginHorizontal();

            // 노트의 타입과 타임 입력 필드
            EditorGUILayout.PropertyField(typeProperty, GUIContent.none);
            EditorGUILayout.PropertyField(timeProperty, GUIContent.none);

            // 노트 삭제 버튼
            if (GUILayout.Button("Remove", GUILayout.Width(70)))
            {
                notesProperty.DeleteArrayElementAtIndex(i);
            }

            EditorGUILayout.EndHorizontal();
        }

        // 노트 추가 버튼
        if (GUILayout.Button("Add Note"))
        {
            notesProperty.InsertArrayElementAtIndex(notesProperty.arraySize);
            SerializedProperty newNote = notesProperty.GetArrayElementAtIndex(notesProperty.arraySize - 1);
            SerializedProperty typeProperty = newNote.FindPropertyRelative("type");
            SerializedProperty timeProperty = newNote.FindPropertyRelative("time");

            // 기본값 설정
            typeProperty.enumValueIndex = (int)NoteType.None;
            timeProperty.floatValue = 0f;
        }

        serializedObject.ApplyModifiedProperties();
    }
}
