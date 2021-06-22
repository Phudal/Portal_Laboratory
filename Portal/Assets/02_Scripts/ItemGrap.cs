using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemGrap : MonoBehaviour
{
    private bool isGrapped = false;

    private bool CallGrap = false;

    private GameObject Item;
    

    // Update is called once per frame
    void Update()
    {
        if (CallGrap && Item != null)
            Item.transform.position = gameObject.transform.position;
        else
            Item = null;
    }

    private void OnTriggerStay(Collider other)
    {
        if (InputManager.Instance.GetItem())
        {            
            if (!isGrapped)
            {
                Item = other.gameObject;
                CallGrap = true;        
            }
            else
            {
                CallGrap = false;                           
            }
        }
    }   
}
