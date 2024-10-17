using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Managers : MonoBehaviour
{
    // Dirty Flag
    private static bool Initialized = false;
    private static Managers s_Instance;
    private static Managers Instance { get { Init(); return s_Instance; } }

    #region Contents
    private PoolingManager _poolingManager = new PoolingManager();

    public static PoolingManager PoolManager { get { return Instance?._poolingManager; } }
    #endregion

    #region Core
    private ResourceManager _resourceManager = new ResourceManager();
    private SceneManagerEX _sceneManager = new SceneManagerEX();
   
    public static ResourceManager Resources { get { return Instance?._resourceManager; } }
    public static SceneManagerEX Scenes { get { return Instance?._sceneManager; } }
    #endregion




    public static void Init()
    {
        if (Initialized)
            return;

        Initialized = true;

        GameObject go = GameObject.Find("@Managers");
        if (go == null)
        {
            go = new GameObject { name = "@Managers" };
            go.AddComponent<Managers>();
        }

        DontDestroyOnLoad(go);
        s_Instance = go.GetComponent<Managers>();
    }


}
