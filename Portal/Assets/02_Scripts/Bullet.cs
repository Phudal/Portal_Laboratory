using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour, IObjectPoolable
{
    public bool canRecyclable { get; set; } = false;

    public Action OnRecycleStartSignature { get; set; }

    public Action OnRecycleFinishSignature { get; set; }

    // public PortalType portalType;

    public bool IsBlue = false;

    private Rigidbody _rigidbody;

    private Transform bullet_transform;

    private MeshRenderer bullet_material;        

    public float bullet_speed = 100.0f;

    private void Awake()
    {
        _rigidbody = GetComponent<Rigidbody>();
        bullet_material = GetComponent<MeshRenderer>();
    }

    private void OnEnable()
    {
        canRecyclable = false;
        bullet_transform = BulletManager.Instance.BulletPos.transform;
        gameObject.transform.position = bullet_transform.position;
        StartCoroutine(BulletTimerCoroutine());
        // SetMaterialColor();
        _rigidbody.AddForce(bullet_transform.forward * bullet_speed, ForceMode.Impulse);
    }

    private void OnDisable()
    {
        canRecyclable = true;
    }

    private void FixedUpdate()
    {
        SetMaterialColor();
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player") || other.CompareTag("ItemPos") || other.CompareTag("Water"))
            return;

        // 포탈이나 이동 가능 물체에 충돌하면 return
        if (other.CompareTag("Blue_Portal") || other.CompareTag("Orange_Portal") || other.CompareTag("PortalableObject"))
            return;

        if (PortalManager.Instance == null)
        {
            Debug.Log("portalmanager null");
            return;
        }

        if (PortalManager.Instance.GetComponent<PortalManager>() == null)
            Debug.Log("Management Null");

        // 파랑색 포탈 총알일 때
        if (IsBlue)
        {            
            PortalManager.Instance.BlueCollision = gameObject.transform;            
        }

        // 오렌지색 포탈 총알일 때
        else 
        {
            PortalManager.Instance.OrangeCollision = gameObject.transform;            
        }

        // PortalManager에 포탈 타입 전달
        PortalManager.Instance.CallingPortal(IsBlue);

        // 충돌 시 오브젝트 풀링 대기상태로 전환
        DestroyBullet();
    }    

    private void SetMaterialColor()
    {
        if (IsBlue)
        {
            bullet_material.material.color = Color.blue;
        }

        else
        {
            bullet_material.material.color = Color.red;
        }
    }

    // Addforce값을 초기화시키고
    // 오브젝트 풀링 대기상태로 전환하는 함수
    private void DestroyBullet()
    {
        _rigidbody.velocity = Vector3.zero;        

        IEnumerator BulletDestroyTimeCoroutine()
        {
            while (true)
            {
                yield return new WaitForSeconds(2.0f);
            }
        }
        StartCoroutine(BulletDestroyTimeCoroutine());
        gameObject.SetActive(false);        
    }

    // 2초 후 오브젝트 풀링 대기상태로 전환하는 코루틴
    IEnumerator BulletTimerCoroutine()
    {
        while(true)
        {
            yield return new WaitForSeconds(2.0f);
            DestroyBullet();
        }
    }
}
