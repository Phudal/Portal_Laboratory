using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityProjectStartUp;

public class GameManager : GameManagerBase
{
    private static GameManager GameManagerinstance = null;

    public static GameManager gameManager =>
        GameManagerinstance = GameManagerinstance ??
        GameObject.Find("GameManager").GetComponent<GameManager>();

    private void Awake()
    {
        Singleton();

        LoadManager.Instance.CallingManagementUpdate();        
    }

    private void Update()
    {
        // if (LoadManager.Instance.isSceneLoaded)
            LoadManager.Instance.CallingManagementUpdate();
    }

    private void Singleton()
    {
        if (GameManagerinstance != null && GameManagerinstance != this)
        {
            Destroy(gameObject);
            return;
        }

        GameManagerinstance = GameManagerinstance ??
            GameObject.Find("GameManager").GetComponent<GameManager>();

        DontDestroyOnLoad(gameObject);
    }

    protected override void InitializeClasses()
    {
        RegisterManagerClass<PortalManager>();
        RegisterManagerClass<BulletManager>();
        RegisterManagerClass<InputManager>();
        RegisterManagerClass<LoadManager>();
        RegisterManagerClass<KeyManager>();
    }    

}
