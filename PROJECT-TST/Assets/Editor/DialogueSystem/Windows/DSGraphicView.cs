using UnityEditor;
using UnityEditor.Experimental.GraphView;
using UnityEngine.UIElements;

namespace DS.Windows
{
    using Elements;
    // 눈에 보이는 요소들을 추가 시키는 클래스
    public class DSGraphicView : GraphView
    {
        public DSGraphicView()
        {
            AddManipulators();
            AddGridBackGround();

            CreateNode();

            AddStyles();
        }

        private void CreateNode()
        {
            DSNode node = new DSNode();

            node.Initialize();
            node.Draw();

            AddElement(node);
        }

        private void AddManipulators()
        {
            // 유니티 자체 내장 되어있는 함수 심심하면 내부함수 구경
            SetupZoom(ContentZoomer.DefaultMinScale, ContentZoomer.DefaultMaxScale);
            // GraphView 자체가 VisualElement의 자식임 그래서 this 키워드 필요
            this.AddManipulator(new ContentDragger()); // 마우스 휠로 움직임
            //this.AddManipulator(new ContentZoomer()); // 마우스 휠 줌인 줌아웃
        }

        private void AddGridBackGround()
        {
            GridBackground gridBackground = new GridBackground();

            gridBackground.StretchToParentSize();

            Insert(0, gridBackground);
        }

        private void AddStyles()
        {
            StyleSheet styleSheet = (StyleSheet) EditorGUIUtility.Load("DialogueSystem/DSGraphicViewStyles.uss");
            
            styleSheets.Add(styleSheet);
        }

    }
}
