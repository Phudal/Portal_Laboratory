//
// Mecanimのアニメーションデータが、原点で移動しない場合の Rigidbody付きコントローラ
// サンプル
// 2014/03/13 N.Kobyasahi
//
using UnityEngine;
using System.Collections;
using System;
using System.Diagnostics;
using UnityEngine.PlayerLoop;

namespace UnityChan
{
	// 必要なコンポーネントの列記
	[RequireComponent(typeof(Animator))]
	[RequireComponent(typeof(CapsuleCollider))]
	[RequireComponent(typeof(Rigidbody))]

	public class UnityChanControlScriptWithRgidBody : MonoBehaviour
	{
		public float animSpeed = 1.5f;              // アニメーション再生速度設定
		public float lookSmoother = 3.0f;           // a smoothing setting for camera motion
		public bool useCurves = true;               // Mecanimでカーブ調整を使うか設定する
													// このスイッチが入っていないとカーブは使われない
		public float useCurvesHeight = 0.5f;        // カーブ補正の有効高さ（地面をすり抜けやすい時には大きくする）

		private float m_StepCycle;
		private float m_NextStep;

		// 以下キャラクターコントローラ用パラメタ
		// 前進速度
		public float forwardSpeed = 7.0f;
		// 後退速度
		public float backwardSpeed = 2.0f;
		// 旋回速度
		public float rotateSpeed = 2.0f;
		// ジャンプ威力
		public float jumpPower = 3.0f;
		// キャラクターコントローラ（カプセルコライダ）の参照
		private CapsuleCollider col;
		private Rigidbody rb;
		// キャラクターコントローラ（カプセルコライダ）の移動量
		private Vector3 velocity;
		// CapsuleColliderで設定されているコライダのHeiht、Centerの初期値を収める変数
		private float orgColHight;
		private Vector3 orgVectColCenter;
		private Animator anim;                          // キャラにアタッチされるアニメーターへの参照
		private AnimatorStateInfo currentBaseState;         // base layerで使われる、アニメーターの現在の状態の参照

		private RaycastHit hitInfo;

		[SerializeField] private AudioClip[] m_FootstepSounds;

		private AudioSource m_AudioSource;

		[SerializeField] private float cameraRotationLimit = 0;
		[SerializeField] private float LookSensitivity = 0;

		private float currentCameraRotationX = 0;

		[SerializeField] private Transform FootTransform;

		private GameObject cameraObject;    // メインカメラへの参照
		private GameObject BulletShooterObject;

		// アニメーター各ステートへの参照
		static int idleState = Animator.StringToHash("Base Layer.Idle");
		static int locoState = Animator.StringToHash("Base Layer.Locomotion");
		static int jumpState = Animator.StringToHash("Base Layer.Jump");
		static int restState = Animator.StringToHash("Base Layer.Rest");

		// 初期化
		private void Start()
		{			
			// Animatorコンポーネントを取得する
			anim = GetComponent<Animator>();
			// CapsuleColliderコンポーネントを取得する（カプセル型コリジョン）
			col = GetComponent<CapsuleCollider>();
			rb = GetComponent<Rigidbody>();

			m_AudioSource = GetComponent<AudioSource>();

			m_StepCycle = 0f;
			m_NextStep = m_StepCycle / 2f;

			//メインカメラを取得する
			cameraObject = GameObject.FindWithTag("MainCamera");
			BulletShooterObject = GameObject.FindWithTag("BulletShooter");
			// CapsuleColliderコンポーネントのHeight、Centerの初期値を保存する
			orgColHight = col.height;
			orgVectColCenter = col.center;
		}

		private void Update()
		{
			CameraRotation();

			// RayCastGround();
			Jump();
		}

		// 以下、メイン処理.リジッドボディと絡めるので、FixedUpdate内で処理を行う.
		private void FixedUpdate()
		{
			// float h = Input.GetAxis("Horizontal");              // 入力デバイスの水平軸をhで定義
			// float v = Input.GetAxis("Vertical");               // 入力デバイスの垂直軸をvで定義
			float h = InputManager.Instance.GetHorizontal();
			float v = InputManager.Instance.GetVertical();

			CharacterRotation();

			anim.SetFloat("Speed", v);                          // Animator側で設定している"Speed"パラメタにvを渡す
			anim.SetFloat("Direction", h);                      // Animator側で設定している"Direction"パラメタにhを渡す
			anim.speed = animSpeed;                             // Animatorのモーション再生速度に animSpeedを設定する
			currentBaseState = anim.GetCurrentAnimatorStateInfo(0); // 参照用のステート変数にBase Layer (0)の現在のステートを設定する
			rb.useGravity = true;//ジャンプ中に重力を切るので、それ以外は重力の影響を受けるようにする



			// 以下、キャラクターの移動処理
			velocity = new Vector3(0, 0, v);        // 上下のキー入力からZ軸方向の移動量を取得
													// キャラクターのローカル空間での方向に変換
			velocity = transform.TransformDirection(velocity);
			//以下のvの閾値は、Mecanim側のトランジションと一緒に調整する
			if (v > 0.1)
			{
				velocity *= forwardSpeed;       // 移動速度を掛ける
			}
			else if (v < -0.1)
			{
				velocity *= backwardSpeed;  // 移動速度を掛ける
			}

			// 上下のキー入力でキャラクターを移動させる
			transform.localPosition += velocity * Time.fixedDeltaTime;

			// 左右のキー入力でキャラクタをY軸で旋回させる
			transform.Rotate(0, h * rotateSpeed, 0);


			// 以下、Animatorの各ステート中での処理
			// Locomotion中
			// 現在のベースレイヤーがlocoStateの時
			if (currentBaseState.nameHash == locoState)
			{
				//カーブでコライダ調整をしている時は、念のためにリセットする
				if (useCurves)
				{
					resetCollider();
				}
			}
			// JUMP中の処理
			// 現在のベースレイヤーがjumpStateの時
			else if (currentBaseState.nameHash == jumpState)
			{
				cameraObject.SendMessage("setCameraPositionJumpView");  // ジャンプ中のカメラに変更
																		// ステートがトランジション中でない場合
				if (!anim.IsInTransition(0))
				{

					// 以下、カーブ調整をする場合の処理
					if (useCurves)
					{
						// 以下JUMP00アニメーションについているカーブJumpHeightとGravityControl
						// JumpHeight:JUMP00でのジャンプの高さ（0〜1）
						// GravityControl:1⇒ジャンプ中（重力無効）、0⇒重力有効
						float jumpHeight = anim.GetFloat("JumpHeight");
						float gravityControl = anim.GetFloat("GravityControl");
						if (gravityControl > 0)
							rb.useGravity = false;  //ジャンプ中の重力の影響を切る

						// レイキャストをキャラクターのセンターから落とす
						Ray ray = new Ray(transform.position + Vector3.up, -Vector3.up);
						RaycastHit hitInfo = new RaycastHit();
						// 高さが useCurvesHeight 以上ある時のみ、コライダーの高さと中心をJUMP00アニメーションについているカーブで調整する
						if (Physics.Raycast(ray, out hitInfo))
						{
							if (hitInfo.distance > useCurvesHeight)
							{
								col.height = orgColHight - jumpHeight;          // 調整されたコライダーの高さ
								float adjCenterY = orgVectColCenter.y + jumpHeight;
								col.center = new Vector3(0, adjCenterY, 0); // 調整されたコライダーのセンター
							}
							else
							{
								// 閾値よりも低い時には初期値に戻す（念のため）					
								resetCollider();
							}
						}
					}
					// Jump bool値をリセットする（ループしないようにする）				
					anim.SetBool("Jump", false);
				}
			}
			// IDLE中の処理
			// 現在のベースレイヤーがidleStateの時
			else if (currentBaseState.nameHash == idleState)
			{
				//カーブでコライダ調整をしている時は、念のためにリセットする
				if (useCurves)
				{
					resetCollider();
				}
				// スペースキーを入力したらRest状態になる
				// if (Input.GetButtonDown("Jump"))
				if (InputManager.Instance.GetJump())
				{
					anim.SetBool("Rest", true);
				}
			}
			// REST中の処理
			// 現在のベースレイヤーがrestStateの時
			else if (currentBaseState.nameHash == restState)
			{
				//cameraObject.SendMessage("setCameraPositionFrontView");		// カメラを正面に切り替える
				// ステートが遷移中でない場合、Rest bool値をリセットする（ループしないようにする）
				if (!anim.IsInTransition(0))
				{
					anim.SetBool("Rest", false);
				}
			}
		}

		//private void OnGUI()
		//{
		//	GUI.Box(new Rect(Screen.width - 260, 10, 250, 150), "Interaction");
		//	GUI.Label(new Rect(Screen.width - 245, 30, 250, 30), "Up/Down Arrow : Go Forwald/Go Back");
		//	GUI.Label(new Rect(Screen.width - 245, 50, 250, 30), "Left/Right Arrow : Turn Left/Turn Right");
		//	GUI.Label(new Rect(Screen.width - 245, 70, 250, 30), "Hit Space key while Running : Jump");
		//	GUI.Label(new Rect(Screen.width - 245, 90, 250, 30), "Hit Spase key while Stopping : Rest");
		//	GUI.Label(new Rect(Screen.width - 245, 110, 250, 30), "Left Control : Front Camera");
		//	GUI.Label(new Rect(Screen.width - 245, 130, 250, 30), "Alt : LookAt Camera");
		//}

		private bool IsGround()
        {
			return Physics.Raycast(transform.position, -Vector3.up, 0.2f);
        }

		private void PlayFootStepAudio()
		{

			UnityEngine.Debug.Log("PlayFootStepAudio");
			int n = UnityEngine.Random.Range(1, m_FootstepSounds.Length);
			m_AudioSource.clip = m_FootstepSounds[n];
			m_AudioSource.PlayOneShot(m_AudioSource.clip);
			m_FootstepSounds[n] = m_FootstepSounds[0];
			m_FootstepSounds[0] = m_AudioSource.clip;
		}
		 
		private void Jump()
		{
			// if (Input.GetButtonDown("Jump"))
			if (InputManager.Instance.GetJump())
			{ // スペースキーを入力したら		
			  // UnityEngine.Debug.Log("Jump key Down");

				if (!IsGround())
					return;

				//アニメーションのステートがLocomotionの最中のみジャンプできる
				if (currentBaseState.nameHash == locoState)
				{
					//ステート遷移中でなかったらジャンプできる
					if (!anim.IsInTransition(0))
					{
						rb.AddForce(Vector3.up * jumpPower, ForceMode.VelocityChange);
						anim.SetBool("Jump", true);     // Animatorにジャンプに切り替えるフラグを送る
					}
				}
			}
		}

		//private void ProgressStepCycle(float speed)
		//      {
		//	if (m_characterController.velocity.sqrMagnitude > 0 && (h != 0 || v != 0))
		//          {
		//		m_StepCycle += (m_characterController.velocity.magnitude + (speed *(h || v) ? 1f):
		//          }
		//      }

		// キャラクターのコライダーサイズのリセット関数
		private void resetCollider()
		{
			// コンポーネントのHeight、Centerの初期値を戻す
			col.height = orgColHight;
			col.center = orgVectColCenter;
		}
		private void CameraRotation()
		{
			// float _xRotation = Input.GetAxisRaw("Mouse Y");
			float _xRotation = InputManager.Instance.GetMouseVertical();
			float _cameraRotationX = _xRotation * LookSensitivity * 0.4f;

			currentCameraRotationX -= _cameraRotationX;
			currentCameraRotationX = Mathf.Clamp(currentCameraRotationX, -cameraRotationLimit, cameraRotationLimit);

			cameraObject.transform.localEulerAngles = new Vector3(currentCameraRotationX, 0f, 0f);
			BulletShooterObject.transform.localEulerAngles = new Vector3(currentCameraRotationX, 0f, 0f);
		}

		private void CharacterRotation()
		{
			// float _yRotation = Input.GetAxisRaw("Mouse X");
			float _yRotation = InputManager.Instance.GetMouseHorizontal();
			Vector3 _characterRotationY = new Vector3(0f, _yRotation, 0f) * LookSensitivity;
			rb.MoveRotation(rb.rotation * Quaternion.Euler(_characterRotationY));
		}
	}
}