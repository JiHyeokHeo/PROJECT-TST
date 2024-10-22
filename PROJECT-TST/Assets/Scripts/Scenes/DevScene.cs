using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DevScene : BaseScene
{
    public override bool Init()
    {
        if (base.Init() == false)
            return false;

        //// 카메라 생성 후 게임 매니저에 넣기
        //Managers.GameManager.VirtualCamera = Managers.Resources.Instantiate("Virtual Camera");
        //Managers.GameManager.Player = Managers.Resources.Instantiate("Y Bot");


        return true;
    }

    private float _elapsedTime = 0.0f;

    private void Update()
    {
        _elapsedTime += (Time.deltaTime - _elapsedTime) * 0.1f;
    }

    private void OnGUI()
    {
        int w = Screen.width, h = Screen.height;

        GUIStyle style = new GUIStyle();

        Rect rect = new Rect(100, 100, w, h * 2 / 100);
        style.alignment = TextAnchor.UpperLeft;
        style.fontSize = h * 2 / 100;
        style.normal.textColor = new Color(0.0f, 0.0f, 0.5f, 1.0f);
        float msec = Time.deltaTime * 1000.0f;
        float fps = 1.0f / Time.deltaTime;
        string text = string.Format("{0:0.0} ms ({1:0.} fps)", msec, fps);
        GUI.Label(rect, text, style);
    }

    public override void Clear() { }
}
