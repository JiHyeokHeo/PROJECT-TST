using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace TST
{
    public class IngameScene : SceneBase
    {   
        public override IEnumerator OnStart()
        {
            AsyncOperation asyncToTitle = SceneManager.LoadSceneAsync(SceneType.Ingame.ToString(), LoadSceneMode.Single);
            yield return new WaitUntil(() => asyncToTitle.isDone);

            //UIManager.Show<IngameUI>(UIList.IngameUI);
            //UIManager.Show<MinimapUI>(UIList.MinimapUI);
            //UIManager.Show<IndicatorUI>(UIList.IndicatorUI);
        }

        public override IEnumerator OnEnd()
        {
            //UIManager.Hide<IngameUI>(UIList.IngameUI);
            //UIManager.Hide<MinimapUI>(UIList.MinimapUI);
            //UIManager.Hide<IndicatorUI>(UIList.IndicatorUI);

            yield return null;
        }
    }
}
