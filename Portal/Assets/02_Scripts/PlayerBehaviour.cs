using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerBehaviour : MonoBehaviour
{   
    private void Awake()
    {

    }
    private void Update()
    {
        // if (Input.GetKeyDown(KeyCode.Mouse0))
        if (InputManager.Instance.GetLeftMouse()) 
            BulletManager.Instance.MakeBullet(true);
        // else if (Input.GetKeyDown(KeyCode.Mouse1))
        if (InputManager.Instance.GetRightMouse()) 
            BulletManager.Instance.MakeBullet(false);        
    }
}
