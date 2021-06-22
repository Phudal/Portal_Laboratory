using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InputManager : ManagerClassBase<InputManager>
{
    public float GetHorizontal()
    {
        return Input.GetAxisRaw("Horizontal");
    }

    public float GetVertical()
    {
        return Input.GetAxisRaw("Vertical");
    }

    public float GetMouseHorizontal()
    {
        return Input.GetAxisRaw("Mouse X");
    }

    public float GetMouseVertical()
    {
        return Input.GetAxisRaw("Mouse Y");
    }

    public bool GetJump()
    {
        return Input.GetKey(KeyCode.Space);
    }

    public bool GetLeftMouse()
    {
        return Input.GetKeyDown(KeyCode.Mouse0);
    }

    public bool GetRightMouse()
    {
        return Input.GetKeyDown(KeyCode.Mouse1);
    }

    public bool GetItem()
    {
        return Input.GetKeyDown(KeyCode.F);        
    }

    public bool GetUI()
    {
        return Input.GetKeyDown(KeyCode.Escape);
    }

    public override void InitializeManagerClass() { }
}
