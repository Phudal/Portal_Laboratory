using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MainButton : MonoBehaviour
{
    // 버튼 클릭시 전환시킬 씬
    [SerializeField] private SceneName _NextScene = SceneName.MainScene1;

    [SerializeField] private GameObject HowtoCanvas;

    private void Awake()
    {
        HowtoCanvas = GameObject.Find("HowtoCanvas");
    }

    public SceneName nextScene => _NextScene;   
    
    // Start 버튼을 눌렀을 경우 호출할 메서드
    public void OnStartBtnClicked()
    {
        LoadManager.Instance.LoadScene(_NextScene);
    }

    public void OnHowToBtnClicked()
    {
        LoadManager.Instance.LoadScene(SceneName.HowtoScene);
    }

    public void OnExitBtnClicked()
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
