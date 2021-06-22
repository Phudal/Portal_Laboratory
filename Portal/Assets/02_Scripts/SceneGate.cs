using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneGate : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            if (SceneManager.GetActiveScene().name == "MainScene1")
                LoadManager.Instance.LoadScene(SceneName.MainScene2);
            else if (SceneManager.GetActiveScene().name == "MainScene2")
            {
                if (KeyManager.Instance.Stage2key)
                    LoadManager.Instance.LoadScene(SceneName.MainScene3);
            }

            else if (SceneManager.GetActiveScene().name == "MainScene3")
            {
                if (KeyManager.Instance.Stage3key)
                    LoadManager.Instance.LoadScene(SceneName.EndingScene);
            }
        }
    }
}
