using UnityEngine;
using System.Collections;

public class Bullet : MonoBehaviour {

	public float speed_;
	public Vector3 direction_;

	float lifeTime_ = 5;

	// Update is called once per frame
	void Update () {
	
		this.transform.position = this.transform.position + direction_ * speed_ * Time.deltaTime;

		if ((lifeTime_ -= Time.deltaTime) < 0.0f) {
			Destroy (this.gameObject);
		}
	}

	void OnCollisionEnter( Collision collision ) {
		lifeTime_ = 0.0f;
	}
}
