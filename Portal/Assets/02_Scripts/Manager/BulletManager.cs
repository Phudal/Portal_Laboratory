using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class BulletManager : ManagerClassBase<BulletManager>
{
    // 복사 생성시킬 Bullet 프리팹을 참조할 변수
    [SerializeField] private Bullet _BulletInstancePrefab = null;

    // Bullet 풀
    private ObjectPool<Bullet> _BulletPool = null;

    [SerializeField] public Transform BulletPos;


    private void Awake()
    {
        
        _BulletPool = new ObjectPool<Bullet>();
    }

    private void Start()
    {
        BulletPos = GameObject.FindWithTag("BulletShooter").transform;
    }

    private void Update()
    {
        if (SceneManager.GetActiveScene().name != "LoadingScene" &&
            SceneManager.GetActiveScene().name != "EndingScene")
            Initialize();
    }

    private void Initialize()
    {
        _BulletPool = new ObjectPool<Bullet>();
        BulletPos = GameObject.FindWithTag("BulletShooter").transform;
    }

    public Bullet MakeBullet(bool _isblue)
    {
        // 재사용 가능한 Bullet을 찾고, 존재하지 않으면
        // 새로 생성합니다.
        Bullet bulletInstance =
            _BulletPool.GetRecyclableObject(checkCanRecycle: true) ??
            _BulletPool.RegisterRecyclableObject(Instantiate(_BulletInstancePrefab));

        // 활성화
        bulletInstance.gameObject.SetActive(true);

        // 포탈 타입에 맞춰서 불릿 생성
        // bulletInstance.portalType = portalType;        
        bulletInstance.IsBlue = _isblue;

        // 위치 조정
        bulletInstance.transform.position = BulletPos.position;        
        
        return bulletInstance;
    }

    public override void InitializeManagerClass() { }
}
