using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Rendering;

namespace TST
{
    public class CrossHairBase : MonoBehaviour
    {
        public bool IsRecoilChange
        {
            get => isRecoilChange;
            set
            {
                isRecoilChange = value;
            }
        }

        public float maxPosX;
        public float maxPosY;

        [field: SerializeField] private List<float> initPosX;
        [field: SerializeField] private List<float> initPosY;

        public float maxPosDivideRatio;
        // 10프로씩 벌어지도록
        public float ratio;

        List<RectTransform> gameObjects;

        private bool isRecoilChange;

        void Start()
        {
            maxPosDivideRatio = 10.0f;
            ratio = 1.05f;
            // 1920 x 1080
            // 정사각형 비율로 변형
            float scaleWidthRatio = (float)Screen.width / Screen.height;

            maxPosX = Screen.width / maxPosDivideRatio;
            maxPosY = Screen.height * scaleWidthRatio / maxPosDivideRatio;
            gameObjects = GetComponentsInChildren<RectTransform>().Where(t => t != transform).ToList();
            
            for (int i = 0; i < gameObjects.Count; i++)
            {
                Vector3 pos = gameObjects[i].GetComponent<RectTransform>().anchoredPosition3D;
                initPosX.Add(pos.x);
                initPosY.Add(pos.y);
            }
        }

        void Update()
        {
            if (!isRecoilChange)
                return;

            for (int i = 0; i < gameObjects.Count; i++) 
            {
                // 비율만큼 곱해준다
                Vector3 pos = gameObjects[i].anchoredPosition3D;

                pos.x *= Mathf.Lerp(pos.x, ratio, Time.deltaTime * 10.0f);
                pos.y *= Mathf.Lerp(pos.y, ratio, Time.deltaTime * 10.0f);

                if (pos.x > maxPosX)
                    pos.x = maxPosX;

                if (pos.y > maxPosY)
                    pos.y = maxPosY;

                //if (pos.x < initPosX[i])
                //    pos.x = initPosX[i];

                //if (pos.y < initPosY[i])
                //    pos.y = initPosY[i];

                gameObjects[i].anchoredPosition3D = new Vector3(pos.x, pos.y, 0f);
            }
        }

        //// 크로스헤어 추가 했을 때 특이한 변화가 있을 것을 대비
        //protected virtual void CrossHairRecoilStart(Vector3 pos, int index)
        //{
        //    pos.x *= ratio;
        //    pos.y *= ratio;

        //    if (pos.x > maxPosX)
        //        pos.x = maxPosX;

        //    if (pos.y > maxPosY)
        //        pos.y = maxPosY;

        //    if (pos.x < initPosX[index])
        //        pos.x = initPosX[index];

        //    if (pos.y < initPosY[index])
        //        pos.y = initPosY[index];
        //}
    }
}
