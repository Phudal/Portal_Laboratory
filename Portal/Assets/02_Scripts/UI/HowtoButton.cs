using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HowtoButton : MonoBehaviour
{
    // 버튼 클릭시 전환시킬 씬
    [SerializeField] private SceneName _NextScene = SceneName.MainScene1;

    public SceneName nextScene => _NextScene;

    public void OnStartBtnClicked()
    {
        LoadManager.Instance.LoadScene(_NextScene);
    }

    public void OnBackBtnClicked()
    {
        LoadManager.Instance.LoadScene(SceneName.StartingScene);
    }
}
