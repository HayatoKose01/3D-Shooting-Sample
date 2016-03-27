using UnityEngine;
using System.Collections;

public class Player : MonoBehaviour {

	public Vector3 cameraPos_;

	Vector3 position_;
	Quaternion rotation_;

	public Vector3 Forward { get; private set; }
	public Vector3 Up { get; private set; }
	public Vector3 Right { get; private set; }

	public float Speed { get; private set; }
	public float speedMin_;
	public float speedMax_;
	public float accel_;

	public int shieldMax_;
	public int Shield { get; private set; }

	public Vector3 rotSpeed_; // roll, pitch, yaw

	public GameObject bullet_;
	public float bulletSpan_;
	float bulletTime_;

	public GameObject effect_;

	public GameManager manager_;

	// Use this for initialization
	void Start () {
	
		position_ = Vector3.zero;
		rotation_ = Quaternion.identity;

		bulletTime_ = 0.0f;

		Forward = Vector3.forward;
		Up = Vector3.up;
		Right = Vector3.right;

		Speed = speedMin_;

		Shield = shieldMax_;
	}
	
	// Update is called once per frame
	void Update () {

		// ロール
		float ar = Input.GetAxis ("Horizontal") * rotSpeed_.x * Time.deltaTime;
		Quaternion qr = Quaternion.AngleAxis (-ar, Vector3.forward);

		// ピッチ
		float ap = Input.GetAxis ("Vertical") * rotSpeed_.y * Time.deltaTime;
		Quaternion qp = Quaternion.AngleAxis (ap, Vector3.right);

		// ヨー
		float ay = Input.GetAxis ("Yawing") * rotSpeed_.z * Time.deltaTime;
		Quaternion qy = Quaternion.AngleAxis (ay, Vector3.up);

		rotation_ = rotation_ * qy * qr * qp;

		// 加減速
		if (Input.GetButton ("Boost") ) {

			Speed += accel_ * Time.deltaTime;
			if (Speed > speedMax_) Speed = speedMax_;

		} else {
			
			Speed -= accel_ * Time.deltaTime;
			if (Speed < speedMin_) Speed = speedMin_;
		}

		// 前進
		position_ += rotation_ * Vector3.forward * Speed * Time.deltaTime;

		this.transform.position = position_;
		this.transform.rotation = rotation_;

		Forward = rotation_ * Vector3.forward;
		Up = rotation_ * Vector3.up;
		Right = rotation_ * Vector3.right;

		// 弾を発射
		if (Input.GetButton ("Fire1") && bulletTime_ <= 0.0f) {

			GameObject bullet = (GameObject)Instantiate (bullet_, position_, Quaternion.identity);
			Bullet b = bullet.GetComponent<Bullet> ();
			b.direction_ = rotation_ * Vector3.forward;

			bulletTime_ = bulletSpan_;
		}
		if (bulletTime_ > 0.0f) {
			bulletTime_ -= Time.deltaTime;
		}
	}

	void LateUpdate() {

		// カメラ位置
		Camera.main.transform.position = position_ + rotation_ * cameraPos_;
		Camera.main.transform.rotation = rotation_;
	}

	void OnCollisionEnter( Collision collision ) {

		Shield -= 10;
		if (Shield <= 0) {

			this.gameObject.SetActive (false);

			Instantiate (effect_, position_, rotation_);

			manager_.GameOver ();
		}
	}

}
