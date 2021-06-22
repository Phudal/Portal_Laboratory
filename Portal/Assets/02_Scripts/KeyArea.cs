using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class KeyArea : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Key"))
        {
            if (SceneManager.GetActiveScene().name == "MainScene2")
                KeyManager.Instance.Stage2key = true;
            else if (SceneManager.GetActiveScene().name == "MainScene3")
                KeyManager.Instance.Stage3key = true;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Key"))
        {
            if (SceneManager.GetActiveScene().name == "MainScene2")
                KeyManager.Instance.Stage2key = false;
            else if (SceneManager.GetActiveScene().name == "MainScene3")
                KeyManager.Instance.Stage3key = false;
        }
    }
}
