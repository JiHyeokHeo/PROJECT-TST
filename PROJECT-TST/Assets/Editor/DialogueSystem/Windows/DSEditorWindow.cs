using System;
using UnityEditor;
using UnityEngine.UIElements;


// 호오 네임스페이스도 달라지면 에디터 상에서 실행 xxx 안됩니다 // AddStyle StyleSheet 에서 문제생김
// .uss 파일에 항상 namespace First 네임 컨벤션이 같아야함 클래스는 상관없음
namespace DS.Windows
{
    public class DSEditorWindow : EditorWindow
    {
        [MenuItem("TST/Dialogue Graph %#D")]
        public static void ShowExample()
        {
            GetWindow<DSEditorWindow>("Dialogue Graph");
        }

        private void OnEnable()
        {
            AddGraphView();

            AddStyles();
        }

        // 화면에 검은 화면 그리는 용도
        private void AddGraphView()
        {
            DSGraphicView graphicView = new DSGraphicView();

            // 사이즈를 늘려줘야 보이기 가능 기본값 = 0;
            graphicView.StretchToParentSize();

            rootVisualElement.Add(graphicView);
        }

        // pixcel 그리드 스타일 추가
        private void AddStyles()
        {
            StyleSheet styleSheet = (StyleSheet)EditorGUIUtility.Load("DialogueSystem/DSVariables.uss");

            rootVisualElement.styleSheets.Add(styleSheet);
        }
    }
}