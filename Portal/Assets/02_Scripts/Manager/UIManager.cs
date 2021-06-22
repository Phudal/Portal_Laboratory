using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class UIManager : ManagerClassBase<UIManager>
{
    public GameObject UICanvas;
    private void Awake()
    {
        UICanvas = GameObject.Find("UICanvas");                
    }

    private void Start()
    {
        UICanvas.SetActive(false); 
    }

    private void Update()
    {
        if (SceneManager.GetActiveScene().name != "StartingScene" &&
            SceneManager.GetActiveScene().name != "LoadingScene")
        {
            OnOffCanvas();
        }
    }

    private void OnOffCanvas()
    {
        if (InputManager.Instance.GetUI())
        {
            // 캔버스가 활성화 되어있다면
            if (UICanvas.activeSelf)
                UICanvas.SetActive(false);
            
            // 캔버스가 비활성화 되어있다면
            else
                UICanvas.SetActive(true);
        }
    }

    public override void InitializeManagerClass() { }
    
}
