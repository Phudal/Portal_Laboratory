using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class portalTeleport : MonoBehaviour
{
    public Transform opposite_portal;

    private bool _isPortalUsing = false;  
    

    private void Update()
    {
        if (LoadManager.Instance.isSceneLoaded)
            Initialize();
    }

    private void Initialize()
    {
        if (CompareTag("Orange_Portal"))
            opposite_portal = PortalManager.Instance.Blue_Portal;
        else
            opposite_portal = PortalManager.Instance.Orange_Portal;
    }

    //private void OnTriggerEnter(Collider other)
    //{
    //    if (other.CompareTag("NonePortalableObject"))
    //    {
    //        Debug.Log("NonePortalableObject");
    //        return;
    //    }

    //    _isPortalUsing = true;

    //    if (other.gameObject.GetComponent<PortalableObject>().IsBluePortalUsing ||
    //        other.gameObject.GetComponent<PortalableObject>().IsOrangePortalUsing)
    //        return;

    //    if (CompareTag("Blue_Portal"))
    //    {
    //        other.gameObject.GetComponent<PortalableObject>().IsBluePortalUsing = true;

    //        portalController.Teleport(true);

    //        other.gameObject.transform.position = portalController.Orange_Portal_Output.position;
    //    }
    //    else if (CompareTag("Orange_Portal"))
    //    {
    //        other.gameObject.GetComponent<PortalableObject>().IsOrangePortalUsing = true;
            
    //        portalController.Teleport(false);

    //        other.gameObject.transform.position = portalController.Blue_Portal_Output.position;
    //    }
    //}

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("NonePortalableObject"))
        {
            Debug.Log("NonePortalableObject");
            return;
        }

        if (other.CompareTag("ItemPos"))
            return;


        if (CompareTag("Blue_Portal"))
        {
            // portalController.Teleport(true);
            PortalManager.Instance.Teleport(true);

            Debug.Log("씬 객체 위치" + other.gameObject.transform.position);
            // Debug.Log("유니티짱 객체 위치" + unityChan.m_characterController.transform);
            // other.gameObject.transform.position = portalController.Orange_Portal_Output.position;
            other.gameObject.transform.position = PortalManager.Instance.Orange_Portal_Output.position;
            Debug.Log(other.gameObject.transform.position);
            Debug.Log("블루 포탈 사용");
        }
        else if (CompareTag("Orange_Portal"))
        {
            // portalController.Teleport(false);
            PortalManager.Instance.Teleport(false);

            Debug.Log(other.gameObject.transform.position);
            // Debug.Log(unityChan.m_characterController.transform);
            // other.gameObject.transform.position = portalController.Blue_Portal_Output.position;
            other.gameObject.transform.position = PortalManager.Instance.Blue_Portal_Output.position;
            Debug.Log(other.gameObject.transform.position);
            Debug.Log("오렌지 포탈 사용");
        }

        PortalManager.Instance.CallingPortalMove();
    }

    //private void OnTriggerExit(Collider other)
    //{
    //    if (other.CompareTag("NonePortalableObject"))
    //    {
    //        Debug.Log("NonePortalableObject");
    //        return;
    //    }

    //    if (CompareTag("Blue_Portal") && other.gameObject.GetComponent<PortalableObject>().IsOrangePortalUsing)
    //    {
    //        other.gameObject.GetComponent<PortalableObject>().IsOrangePortalUsing = false;
    //    }

    //    else if (CompareTag("Orange_Portal") && other.gameObject.GetComponent<PortalableObject>().IsBluePortalUsing)
    //    {
    //        other.gameObject.GetComponent<PortalableObject>().IsBluePortalUsing = false;
    //    }

    //    PortalManager.Instance.CallingPortalMove();

    //    //if (_isPortalUsing)
    //    //{
    //    //    _isPortalUsing = false;
    //    //    // other.gameObject.GetComponent<PortalableObject>().IsBluePortalUsing = true;


    //    //    // Debug.Log("포탈 이용");
    //    //    // other.gameObject.transform.position = new Vector3(50, 50, 50);                

    //    //    if (CompareTag("Blue_Portal") &&
    //    //        !other.gameObject.GetComponent<PortalableObject>().IsOrangePortalUsing)
    //    //    {
    //    //        //portalController.Teleport(true);

    //    //        //other.gameObject.transform.position = portalController.Orange_Portal_Output.position;

    //    //        //other.gameObject.GetComponent<PortalableObject>().IsBluePortalUsing = false;
    //    //    }

    //    //    else if (CompareTag("Orange_Portal") &&
    //    //        !other.gameObject.GetComponent<PortalableObject>().IsBluePortalUsing)
    //    //    {
    //    //        //portalController.Teleport(false);

    //    //        //other.gameObject.transform.position = portalController.Blue_Portal_Output.position;

    //    //        //other.gameObject.GetComponent<PortalableObject>().IsOrangePortalUsing = false;
    //    //    }
    //    //}
    //}  


    //private IEnumerator UsingPortalCoroutine()
    //{
    //    WaitUntil waitUsingPortal = new WaitUntil(() => _isPortalUsing);

    //    while (true)
    //    {
    //        yield return waitUsingPortal;

    //    }
    //}
}
