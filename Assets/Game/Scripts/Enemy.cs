using UnityEngine;
using System.Collections;

public class Enemy : MonoBehaviour {

	public float speed_;
	public float rotate_;

	public GameObject effect_;

	public GameObject player_;

	public GameManager manager_;

	public GameObject bullet_;
	public float bulletSpan_;
	float bulletTime_;

	public Vector3 Direction { get; private set; }

	protected enum Pattern {
		Follow,
		Straight,
		Back,

		Count,
	}
	protected Pattern pattern_;
	protected float patternTime_;
	public float patternSpan_;

	// Use this for initialization
	void Start () {
	
		Direction = Vector3.forward;
		SwitchPattern ();

		bulletTime_ = bulletSpan_;
	}

	protected void SwitchPattern() {
		pattern_ = (Pattern)Random.Range (0, (int)Pattern.Count);
		patternTime_ = patternSpan_;
	}
	
	// Update is called once per frame
	void Update () {

		// パターン切り替え
		if ((patternTime_ -= Time.deltaTime) < 0.0f) {
			SwitchPattern ();
		}

		switch( pattern_ ) {

		case Pattern.Follow:
			RotateTo (player_.transform.position);
			break;

		case Pattern.Straight:
			break;

		case Pattern.Back:
			RotateTo (Vector3.zero);
			break;
		}

		// 弾
		if (bulletTime_ <= 0.0f) {
			Vector3 s = player_.transform.position - this.transform.position;
			if ( s.magnitude < 50.0f && Vector3.Dot (Direction, s.normalized) > 0.85f) {

				GameObject bullet = (GameObject)Instantiate (bullet_, this.transform.position, Quaternion.identity);
				Bullet b = bullet.GetComponent<Bullet> ();
				b.direction_ = s.normalized;
			}
			bulletTime_ = bulletSpan_;

		} else {
			bulletTime_ -= Time.deltaTime;
		}



		this.transform.position = this.transform.position + Direction * speed_ * Time.deltaTime;
		this.transform.rotation = Quaternion.FromToRotation (Vector3.forward, Direction);
	}

	protected void RotateTo( Vector3 target ) {
		
		Vector3 s = target - this.transform.position;
		s.Normalize ();

		if (Vector3.Dot (s, Direction) < 0.98f) {
			Vector3 up = Vector3.Cross (Direction, s);

			Quaternion q = Quaternion.AngleAxis (rotate_ * Time.deltaTime, up);
			Direction = q * Direction;
		}
	}

	void OnCollisionEnter( Collision collision ) {

		manager_.DeleteEnemy (this);
		Instantiate (effect_, this.transform.position, Quaternion.identity);

		Destroy (this.gameObject);
	}

}
