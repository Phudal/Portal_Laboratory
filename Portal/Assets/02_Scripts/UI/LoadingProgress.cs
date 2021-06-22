using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

// 로딩 상태를 나타내는 컴포넌트
public class LoadingProgress : MonoBehaviour
{
    // 로딩 진행 상태에 따라 채울 프로그래스바 이밎
    private Image _ProgressbarImage = null;

    private float targetFillAmount = 0.0f;

    private void Start()
    {
        _ProgressbarImage = GetComponent<Image>();
        _ProgressbarImage.fillAmount = 0.0f;

        LoadNextScene();
    }

    private void LoadNextScene()
    {
        targetFillAmount = 0.0f;
        
        StartCoroutine(StartLoad());
        StartCoroutine(FillProgressbar());
    }
    // 비동기 로드를 수행하는 코루틴
    IEnumerator StartLoad()
    {
        // 로드할 씬을 설정
        AsyncOperation ao = SceneManager.LoadSceneAsync(
            LoadManager.Instance.nextScene.ToString());

        ao.allowSceneActivation = false;

        while (ao.progress < 0.9f)
        {
            targetFillAmount = ao.progress;
            yield return null;
        }

        targetFillAmount = 1.0f;

        // progressbarImage가 찰 때까지 대기
        yield return new WaitUntil(
            () => Mathf.Approximately(_ProgressbarImage.fillAmount, 1.0f));

        yield return new WaitForSeconds(1.0f);

        LoadManager.Instance.LoadFinishedEvent?.Invoke();
        LoadManager.Instance.LoadFinishedEventCalledOnce?.Invoke();
        LoadManager.Instance.LoadFinishedEventCalledOnce = null;

        ao.allowSceneActivation = true;
    }

    IEnumerator FillProgressbar()
    {
        while (true)
        {
            _ProgressbarImage.fillAmount = Mathf.MoveTowards(
                _ProgressbarImage.fillAmount, targetFillAmount, Time.deltaTime);
            yield return null;
        }
    }
}
