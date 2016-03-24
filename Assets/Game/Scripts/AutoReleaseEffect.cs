using UnityEngine;
using System.Collections;

public class AutoReleaseEffect : MonoBehaviour {

	// Use this for initialization
	void Start () {
	
		ParticleSystem[] pp = this.GetComponents<ParticleSystem> ();
		ParticleSystem[] cp = this.GetComponentsInChildren<ParticleSystem> ();

		float duration = 0.0f;
		foreach (ParticleSystem p in pp) {
			if (p.duration > duration) duration = p.duration;
		}
		foreach (ParticleSystem p in cp) {
			if (p.duration > duration) duration = p.duration;
		}

		Destroy (this.gameObject, duration);
	}
	
}
