using System.Collections;
using System.Collections.Generic;
using UnityEditor.Experimental.GraphView;
using UnityEngine.UIElements;
using UnityEngine;

namespace DS.Elements
{
    using Enumerations;

    public class DSMultipleChoiceNode : DSNode
    {
        public override void Initialize(Vector2 position)
        {
            base.Initialize(position);

            DialogueType = DSDialogueType.MultipleChoice;

            Choices.Add("New Choice");
        }

        public override void Draw()
        {
            base.Draw();

            /* 메인 컨테이너 */
            Button addChoiceButton = new Button()
            {
                text = "Add Choice"
            };
            // 상단 컨테이어 위에 두고싶다면 add 쓰지말고 insert로
            mainContainer.Insert(1, addChoiceButton);

            /* 아웃풋 컨테이너 */
            foreach (string choice in Choices)
            {
                Port choicePort = InstantiatePort(Orientation.Horizontal, Direction.Output, Port.Capacity.Single, typeof(bool));

                choicePort.portName = "";

                Button deleteChoiceButton = new Button()
                {
                    text = "Delete"
                };

                // https://github.com/Wafflus/unity-dialogue-system/issues/1
                // 2022.1 버전 이상 그래프 에지 및 선택 문제 발생 해결방법
                TextField choiceTextField = new TextField()
                {
                    value = choice
                };
                // 해결 방안
                choiceTextField.style.flexDirection = FlexDirection.Column;

                // 항상 우측에서부터 생성되는 경향을 띄고 있음. 그래서 버튼 추가하려면 맨 마지막에 추가해야함 (그래야 좌측에 생성 가능)
                choicePort.Add(choiceTextField);
                choicePort.Add(deleteChoiceButton);

                outputContainer.Add(choicePort);
            }

            RefreshExpandedState();
        }

    }
}
