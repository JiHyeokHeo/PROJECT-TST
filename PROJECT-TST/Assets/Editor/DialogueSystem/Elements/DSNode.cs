using NUnit.Framework;
using System.Collections.Generic;
using UnityEditor.Experimental.GraphView;
using UnityEngine.UIElements;

namespace DS.Elements
{
    using Enumerations;
    

    public class DSNode : Node
    {
        public string DialogueName { get; set; }
        
        public List<string> Choices { get; set; }

        public string Text { get; set; }

        public DSDialogueType DialogueType { get; set; }

        public void Initialize()
        {
            DialogueName = "DialogueName";
            Choices = new List<string>();
            Text = "Dialogue text.";
        }

        public void Draw()
        {
            /* 타이틀 컨테이너 Node 내부 자체 구현 */
            TextField dialougeNameTextField = new TextField()
            {
                value = DialogueName
            };

            titleContainer.Insert(0, dialougeNameTextField);

            /* 인풋 컨테이너 */
            // 맨 마지막 파라미터의 typeof는 셰이더 그래프 , 노드 정보를 다른 노드에 전달하려는 기타 프로젝트에 주로 사용된다. 
            // ex 셰이더 등의 대비에 관련하여 한 노드에서 다른 노드로 전달 하는 경우 int형을 사용 할 수 있다. 
            Port inputPort = InstantiatePort(Orientation.Horizontal, Direction.Input, Port.Capacity.Multi, typeof(bool));

            inputPort.portName = "Dialogue Connection";

            inputContainer.Add(inputPort);

            /* 익스텐션 컨테이너 */
            VisualElement customDataContainer = new VisualElement();

            Foldout textFoldout = new Foldout()
            {
                text = "Dialogue Text"
            };

            TextField textTextField = new TextField()
            {
                value = Text
            };

            textFoldout.Add(textTextField);

            customDataContainer.Add(textFoldout);

            extensionContainer.Add(customDataContainer);

            RefreshExpandedState();
        }
    } 
}
