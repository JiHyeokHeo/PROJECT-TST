using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace TST
{
    public enum CrossHairType
    {
        CrossHair_A,
        CrossHair_B,
        CrossHair_C,
        CrossHair_D,
    }

    [System.Serializable]
    public class CrossHairData
    {
        public CrossHairType type;
        public GameObject prefab;
        public int UIButtonOrder;
    }

    public class OptionManager : MonoBehaviour
    {
        public GameObject usingCrossHair;
        public static OptionManager Instance { get; private set; }
        public List<CrossHairData> crossHairContainer = new List<CrossHairData>();
        public GameObject UICanvas;

        public CrossHairBase usingCrossHairComponent;

        public bool IsGameStopped
        {
            get => isGameStopped;
            set
            {
                isGameStopped = value;
                if (isGameStopped)
                    Time.timeScale = 0.0f;
                else
                    Time.timeScale = 1.0f;
                // 옵션 띄우기
                ShowOption();
            }
        }

        private bool isGameStopped = false;

        private void Start()
        {
            Instance = this;
            // 초기값 크로스헤어 A 
            usingCrossHair = crossHairContainer[0].prefab;
            usingCrossHairComponent = usingCrossHair.GetComponent<CrossHairBase>();
            usingCrossHair.SetActive(true);
        }

        public GameObject ChangeCrossHair(CrossHairType crossHairType)
        {
            CrossHairData crossHairData = crossHairContainer.Find(x => x.type == crossHairType);
            if (crossHairData == null) 
            {
                Debug.LogError("CrossHair not found");
                return null;
            }

            GameObject crossHair = Instantiate(crossHairData.prefab);
            crossHair.SetActive(true);
            usingCrossHair = crossHair;

            return crossHair;
        }

        public bool ChangeCrossHair(UITest crossHair)
        {
            usingCrossHair.SetActive(false);
            usingCrossHair = crossHair.gameObject;
            usingCrossHairComponent = usingCrossHair.GetComponent<CrossHairBase>();

            crossHair.gameObject.SetActive(true);

            return true;
        }

        private void ShowOption()
        {
            UICanvas.SetActive(!UICanvas.activeSelf);
        }
    }
}
