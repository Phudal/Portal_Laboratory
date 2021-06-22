using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LoadManager : ManagerClassBase<LoadManager>
{
    // 로드할 다음 씬에 대한 프로퍼티
    public SceneName nextScene { get; set; } = SceneName.MainScene1;

    // 로드한 리소스를 가지고 있을 Dictionary
    private Dictionary<string, Object> _LoadResources = null;

    // 로딩씬으로 전환된 후, 로딩 시작시에 호출되는 대리자
    public System.Action LoadStartedEvent { get; set; } = null;

    // 로딩씬으로 전환된 후, 로딩을 끝나고 씬이 전환되기 전에 호출되는 대리자
    public System.Action LoadFinishedEvent { get; set; } = null;

    // 로딩씬으로 전환된 후, 로딩 시작시에 한 번만 호출되는 대리자
    // 한 번 호출이 끝난다면 대리 목록이 비워집니다.
    public System.Action LoadStartedEventCalledOne { get; set; } = null;

    // 로딩씬으로 전환된 후, 로딩이 끝나고 씬이 전환되기 전에 한 번만 호출되는 대리자
    // 한 번 호출이 끝난다면 대리 목록이 비워집니다.
    public System.Action LoadFinishedEventCalledOnce { get; set; } = null;

    // 씬이 로드되었는지 확인
    public bool isSceneLoaded = false;


    // Manager 활성화용 변수
    [SerializeField] private GameObject PM;
    [SerializeField] private GameObject BM;

    public override void InitializeManagerClass()
    {
        _LoadResources = new Dictionary<string, Object>();
        LoadStartedEvent = () => _LoadResources.Clear();
        LoadFinishedEvent = () => LoadResources<Sprite>(
            "Images/ItemImage/ItemImages");
    }

    public void LoadScene(SceneName sceneName)
    {
        // 로드할 다음 씬을 저장합니다.
        nextScene = sceneName;

        LoadStartedEvent?.Invoke();
        LoadStartedEventCalledOne?.Invoke();
        LoadStartedEventCalledOne = null;

        // 로딩 씬으로 전환합니다.
        SceneManager.LoadScene(SceneName.LoadingScene.ToString());

        isSceneLoaded = true;
        // CallingManagementUpdate();
    }

    public T LoadResource<T>(string resourceName, string filePath = null) where T :Object
    {
        if (filePath == null)
            return _LoadResources[resourceName] as T;

        else if (_LoadResources.ContainsKey(resourceName))
            return _LoadResources[resourceName] as T;

        else
            return (_LoadResources[resourceName] = Resources.Load<T>(filePath)) as T;

        // - Resourcs.Load<T>(filePath) : filePath 위치에 있는 T 형식의 리소스를
        // 동기 방식으로 로드하여 리턴합니다.
        // - 무조건 Resources 폴더 내에 로드할 리소스들을 저장해야 합니다.
    }

    public T[] LoadResources<T>(string filePath) where T : Object
    {
        var resources = Resources.LoadAll<T>(filePath);
        // - Resources.LoadAll<T>(filePath) : filePath 위치의 폴더 내에 있는 모든 리소스들을 
        // 로드하여 T[] 형식으로 리턴합니다.
        // - 무조건 Resources 폴더 내에 로드할 리소스들을 저장해야 합니다.

        foreach (var resource in resources)
            _LoadResources[resource.name] = resource;

        return resources;
    }

    public T LoadJson<T>(string filePath)
    {
        var readJsonData = File.ReadAllText(Application.dataPath +
            "/Resources/" + filePath + ".json");

        return JsonUtility.FromJson<T>(readJsonData);
        // -JsonUtility : Json 데이터들을 다룰 수 있는
        // 유틸성 정적 메서드를 제공하는 클래스
        // -FromJson<T>(string jsonFile) : 읽어들인 jsonFile 파일을 T 형식으로 변환하여
        // 반환하여 메서드
    }

    // resourcesName과 일치하는 리소스가 이미 로드되어 있는지 검사합니다.
    public bool ContainsCheck(string resourcesName)
    {
        return _LoadResources.ContainsKey(resourcesName);
    }
    public void CallingManagementUpdate()
    {
        Debug.Log("CallingManagementUpdate - " + SceneManager.GetActiveScene().name);
        if (SceneManager.GetActiveScene().name == "LoadingScene")
            return;

        else if (SceneManager.GetActiveScene().name == "StartingScene")
        {
            // PortalManager.Instance.gameObject.SetActive(false);
            // BulletManager.Instance.gameObject.SetActive(false);
            
            if (PM.activeSelf)
                PM.SetActive(false);
            if (BM.activeSelf)
                BM.SetActive(false);
        }

        else if (SceneManager.GetActiveScene().name == "MainScene1")
        {
            // PortalManager.Instance.gameObject.SetActive(true);
            // BulletManager.Instance.gameObject.SetActive(true);

            if (!PM.activeSelf)
                PM.SetActive(true);
            if (!BM.activeSelf)
                BM.SetActive(true);
        }

        else if (SceneManager.GetActiveScene().name == "MainScene2")
        {
            // PortalManager.Instance.gameObject.SetActive(true);
            // BulletManager.Instance.gameObject.SetActive(true);
            
            if (!PM.activeSelf)
                PM.SetActive(true);
            if (!BM.activeSelf)
                BM.SetActive(true);
        }

        isSceneLoaded = false;
    }
}
