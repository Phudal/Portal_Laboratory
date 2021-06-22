using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class UIButton : MonoBehaviour
{
    public void OnUIRestartMapBtnClicked()
    {
        if (SceneManager.GetActiveScene().name == "MainScene1")
            LoadManager.Instance.LoadScene(SceneName.MainScene1);
        else if (SceneManager.GetActiveScene().name == "MainScene2")
            LoadManager.Instance.LoadScene(SceneName.MainScene2);
        else if (SceneManager.GetActiveScene().name == "MainScene3")
            LoadManager.Instance.LoadScene(SceneName.MainScene3);        
    }

    public void OnUIExitBtnClicked()
    {
        // 유니터 에디터 상태라면        
#if UNITY_EDITOR
        // 플레이 상태를 끕니다.
        UnityEditor.EditorApplication.isPlaying = false;

#else
        // 어플리케이션을 종료합니다.
        Application.Quit();

#endif
    }
}
