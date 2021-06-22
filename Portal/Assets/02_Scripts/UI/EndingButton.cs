using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EndingButton : MonoBehaviour{
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
