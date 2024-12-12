using UnityEditor;
using UnityEditor.Experimental.GraphView;
using UnityEngine.UIElements;
using UnityEngine;
using System;

namespace DS.Windows
{
    using Elements;
    using Enumerations;

    // 눈에 보이는 요소들을 추가 시키는 클래스
    public class DSGraphicView : GraphView
    {
        public DSGraphicView()
        {
            AddManipulators();
            AddGridBackGround();

            AddStyles();
        }

        private void AddManipulators()
        {
            // 유니티 자체 내장 되어있는 함수 심심하면 내부함수 구경
            SetupZoom(ContentZoomer.DefaultMinScale, ContentZoomer.DefaultMaxScale);
            //this.AddManipulator(new ContentZoomer()); // 마우스 휠 줌인 줌아웃

            // GraphView 자체가 VisualElement의 자식임 그래서 this 키워드 필요
            this.AddManipulator(new ContentDragger()); // 마우스 휠로 움직임
            this.AddManipulator(new SelectionDragger()); // ****셀렉션 드래그랑 사각 셀렉션 순서 중요 ! 뒤바뀌면 셀렉트 안됨****
            this.AddManipulator(new RectangleSelector()); // ****셀렉션 드래그랑 사각 셀렉션 순서 중요 ! 뒤바뀌면 셀렉트 안됨****

            this.AddManipulator(CreateNodeContextualMenu("Add Node (Single Choice)", DSDialogueType.SingleChoice));
            this.AddManipulator(CreateNodeContextualMenu("Add Node (Multiple Choice)",DSDialogueType.MultipleChoice));
        }

        private IManipulator CreateNodeContextualMenu(string actionTitle, DSDialogueType dialogueType)
        {
            ContextualMenuManipulator contextualMenuManipulator = new ContextualMenuManipulator(
                menuEvent => menuEvent.menu.AppendAction(actionTitle, actionEvent => AddElement(CreateNode(dialogueType, actionEvent.eventInfo.localMousePosition)))
            );

            return contextualMenuManipulator;
        }

        private DSNode CreateNode(DSDialogueType dialogueType, Vector2 position)
        {
            // 신기한 리플렉션 관련 코드라 적어놓음 Activator 활용 구간
            Type nodeType = Type.GetType($"DS.Elements.DS{dialogueType}Node");

            DSNode node = (DSNode) Activator.CreateInstance(nodeType);

            node.Initialize(position);
            node.Draw();

            return node;
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
