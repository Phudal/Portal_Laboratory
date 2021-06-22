using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityProjectStartUp;
using UnityEngine.SceneManagement;

public sealed class PortalManager : ManagerClassBase<PortalManager>
{   
    public bool _IsportalUsed = false;

    public bool IsCallingBluePortal = false;
    public bool IsCallingOrangePortal = false;

    private bool isInitialized = false;

    [SerializeField] 
    private float portal_offset = 3.5f;
    private float portal_camera_offset = 2.0f;

    public Transform playerRoot, playerCam;

    public Transform Blue_portalCam;
    public Transform Orange_portalCam;

    public Transform Blue_Cam_Pos;
    public Transform Orange_Cam_Pos;

    public Transform Blue_Portal;
    public Transform Orange_Portal;

    public Transform Blue_Portal_Output;
    public Transform Orange_Portal_Output;

    public Transform BlueCollision;
    public Transform OrangeCollision;

    // public RenderTexture renderTex;

    private void Awake()
    {
      
    }

    // Start is called before the first frame update
    void Start()
    {        
        // renderTex.width = Screen.width;
        // renderTex.height = Screen.height;
    }

    // Update is called once per frame
    void Update()
    {        
        if (SceneManager.GetActiveScene().name == "StartingScene" || SceneManager.GetActiveScene().name == "LoadingScene")
            return;

        // if (!isInitialized)
        InitializeingTag();

        // Vector3 playerOffset = playerCam.position - land1.position;

        // Physics.Raycast(playerRoot.position, playerRoot.transform.up * -1, out Ground, 100);

        //        카메라나 plane 포탈에서 RayCast를 하여서
        //        player와 portal의 거리 == portal과 portalCam의 거리
        //        로 조정을 해야함

        // 블루 포탈캠에서 오렌지 포탈 방향으로 광선을 쏴서 정보를 받습니다.
        // 포탈에 해당하는 9번 레이어만 검출합니다.
        //Physics.Raycast(Blue_portalCam.position, Blue_portalCam.transform.forward * -1, out Hit_Orange_portal, 100, 9);
        //Physics.Raycast(Orange_portalCam.position, Orange_portalCam.transform.forward * -1, out Hit_Blue_portal, 100, 9);
        //Physics.Raycast(playerRoot.position, )

        // if (Hit_Orange_portal.distance !=  )


        // Blue_portalCam.position = land2.position + playerOffset;
        // Blue_portalCam.position = new Vector3(

        //Blue_portalCam.position = new Vector3(
        //    Blue_Portal.position.x - playerRoot.position.x + Orange_Portal.position.x,
        //    playerRoot.position.y,
        //    Blue_Portal.position.z - playerRoot.position.z + Orange_Portal.position.z);

        //Blue_portalCam.localPosition = new Vector3(
        //    -playerRoot.position.x,
        //     Ground.distance,
        //    -playerRoot.position.z);

        PortalCam_Adjusting1();
        // PortalCam_Adjusting2();
        // PortalCam_Adjusting3();
    }

    private void InitializeingTag()
    {
        playerRoot = GameObject.FindWithTag("Player").transform;
        playerCam = GameObject.FindWithTag("MainCamera").transform;
        Blue_Portal = GameObject.FindWithTag("Blue_Portal").transform;
        Orange_Portal = GameObject.FindWithTag("Orange_Portal").transform;
        Blue_portalCam = GameObject.FindWithTag("Blue_portalCam").transform;
        Orange_portalCam = GameObject.FindWithTag("Orange_portalCam").transform;
        Blue_Cam_Pos = GameObject.FindWithTag("Blue_Cam_Pos").transform;
        Orange_Cam_Pos = GameObject.FindWithTag("Orange_Cam_Pos").transform;
        Blue_Portal_Output = GameObject.FindWithTag("Blue_Portal_Output").transform;
        Orange_Portal_Output = GameObject.FindWithTag("Orange_Portal_Output").transform;

        isInitialized = true;
    }

    public void Teleport(bool _isBlue)
    {
        //var playerLand = land1;
        //land1 = land2;
        //land2 = playerLand;

        // playerRoot.position = portalCam.position;

        if (_isBlue)
        {
            // 블루 -> 오렌지 실행
            playerRoot.rotation = Quaternion.Euler(0f, Blue_portalCam.localRotation.y + Orange_Portal.localRotation.y + Blue_Cam_Pos.localRotation.y, 0f);
                        
            Debug.Log("오렌지 포탈 localR.y : "+ Orange_Portal.rotation.y);
            Debug.Log("블루 포탈 캠 오브젝트 localR.y : " + Blue_Cam_Pos.rotation.y);
            Debug.Log("블루 포탈 캠 localR.y : " + Blue_portalCam.rotation.y);
            playerCam.rotation = Quaternion.Euler(Blue_portalCam.localRotation.x + Orange_Portal.localRotation.x + Blue_Cam_Pos.localRotation.x, 0f, 0f);
            
            // playerCam.rotation = Quaternion.Euler(0, 200, 0);
            Debug.Log("위쪽 실행");
        }

        else
        {
            // 오렌지 -> 블루 실행
            playerRoot.rotation = Quaternion.Euler(
                0f, Orange_portalCam.rotation.y, 0f);
            playerCam.rotation = Quaternion.Euler(
                Orange_portalCam.rotation.x, 0f, 0f);

            // playerCam.rotation = Quaternion.Euler(0, 0, 0);
            Debug.Log("아래쪽 실행");
        }

        _IsportalUsed = true;
    }

    // 포탈 캠의 카메라 Rotation만 조정하는 함수입니다.
    private void PortalCam_Adjusting1()
    {
        //Blue_portalCam.position = new Vector3
        //    (Orange_Portal.position.x,
        //    playerRoot.position.y,
        //    Orange_Portal.position.z);

        Blue_portalCam.position = new Vector3
            (Orange_Portal.position.x,
            Orange_Portal.position.y - portal_camera_offset,
            Orange_Portal.position.z);

        Blue_portalCam.localRotation = playerCam.rotation;

        //Orange_portalCam.position = new Vector3
        //    (Blue_Portal.position.x,
        //    playerRoot.position.y,
        //    Blue_Portal.position.z);

        Orange_portalCam.position = new Vector3
            (Blue_Portal.position.x,
            Blue_Portal.position.y - portal_camera_offset,
            Blue_Portal.position.z);

        Orange_portalCam.localRotation = playerCam.rotation;
    }

    // 포탈 캠의 Position 과 Roation 모두 조정하는 함수입니다.
    // 포탈과 포탈캠과의 거리와 플레이어와 포탈간의 거리를 벡터로 동기화시킵니다.
    // 즉, position x, y, z 값 모두 동기화 시킵니다.
    private void PortalCam_Adjusting2()
    {
        Blue_portalCam.localPosition = new Vector3(
            Blue_Portal.position.x - playerRoot.position.x + Orange_Portal.position.x,
            playerRoot.localPosition.y,
            Blue_Portal.position.z - playerRoot.position.z + Orange_Portal.position.z);

        // Blue_portalCam.rotation = playerCam.rotation;
        Blue_portalCam.localRotation = playerCam.localRotation;

        Orange_portalCam.localPosition = new Vector3(
            Orange_Portal.localPosition.x - playerRoot.localPosition.x + Blue_Portal.localPosition.x,
            playerRoot.localPosition.y,
            Orange_Portal.localPosition.z - playerRoot.localPosition.z + Blue_Portal.localPosition.z);

        // Orange_portalCam.rotation = playerCam.rotation;
        Orange_portalCam.localRotation = playerCam.localRotation;
    }

    // 포탈캠의 Position과 Rotation 모두 조정하는 함수입니다.
    // 하지만 Position은 로컬 x 값만 동기화시키고, y, z 값은 고정합니다.
    private void PortalCam_Adjusting3()
    {
        // 블루 포탈캠 로컬 위치 =
        // 블루 포탈의 로컬 위치 - 플레이어 로컬 위치 + 오렌지 로컬 위치,
        // 플레이어 카메라 높이,
        // 오렌지 포탈 위치
        Blue_portalCam.localPosition = new Vector3(
            Blue_Portal.localPosition.x - playerRoot.localPosition.x + Orange_Portal.localPosition.x,
            playerRoot.position.y,
            Orange_Portal.position.z);

        // Blue_portalCam.rotation = playerCam.rotation;
        Blue_portalCam.localRotation = playerCam.localRotation;

        Orange_portalCam.localPosition = new Vector3(
            Orange_Portal.localPosition.x - playerRoot.localPosition.x + Blue_portalCam.localPosition.x,
            playerRoot.position.y,
            Blue_Portal.position.z);

        // Orange_portalCam.rotation = playerCam.rotation;
        Orange_portalCam.localRotation = playerCam.localRotation;
    }

    // 포탈 생성 메세지를 받았을 때, 포탈을 해당 위치로 옮기는 메소드
    public void CallingPortal(bool _isblue)
    {
        // Position Calling
        if (_isblue)
        {
            Blue_Portal.position = new Vector3(BlueCollision.position.x,
                BlueCollision.position.y + portal_offset,
                BlueCollision.position.z);

            // Blue_Portal.rotation = Quaternion.Euler(90, playerRoot.rotation.y, 0);
            Blue_Portal.rotation = Quaternion.Euler(DirectVector.CalculateVectorToDirection(playerRoot.eulerAngles, _isblue));
        }

        else
        {
            Orange_Portal.position = new Vector3(OrangeCollision.position.x,
                OrangeCollision.position.y + portal_offset,
                OrangeCollision.position.z);

            // Orange_Portal.rotation = Quaternion.Euler(90, -playerRoot.rotation.y, 0);
            Orange_Portal.rotation = Quaternion.Euler(DirectVector.CalculateVectorToDirection(playerRoot.eulerAngles, _isblue));
        }
        
        // 각도 수정 파트 구현해야함              
    }

    // 구현해야 하는 함수 - 포탈을 1회 사용했을 시, 대기 장소로 포탈을 옮기는 함수

    public void CallingPortalMove()
    {
        Blue_Portal.position = new Vector3(0, 0, 500);
        Orange_Portal.position = new Vector3(0, 0, 600);
    }

    public override void InitializeManagerClass()
    {
    }
}
