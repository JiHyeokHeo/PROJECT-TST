using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace TST
{
    public class TitleUI : UIBase
    {
        public void OnClickGameStartButton()
        {
            Main.Singleton.ChangeScene(SceneType.Ingame);
        }

        public void OnClickQuitButton()
        {
            Main.Singleton.SystemQuit();
        }
    }
}
