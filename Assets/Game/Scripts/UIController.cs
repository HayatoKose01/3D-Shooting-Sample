using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class UIController : MonoBehaviour {

	public GameObject arrow_;
	public GameObject marker_;

	public Player player_;
	public GameManager manager_;

	public RectTransform radarGrid_;

	public UnityEngine.UI.Image hpMeter_;
	public UnityEngine.UI.Text speed_;
	public UnityEngine.UI.Text score_;

	public UnityEngine.UI.Text gameOver_;

	protected class EnemyInfo {
		public Enemy enemy_;
		public RectTransform arrow_;
		public RectTransform marker_;
	}
	List< EnemyInfo > enemyList_ = new List<EnemyInfo>();

	// Use this for initialization
	void Start () {
		gameOver_.gameObject.SetActive (false);	
	}
	
	// Update is called once per frame
	void Update () {
	
		foreach (EnemyInfo info in enemyList_) {
			UpdateArrow (info);
			UpdateMarker (info);
		}

		speed_.text = string.Format ("{0:F2} km/h", player_.Speed * 100);
		score_.text = string.Format ("Score: {0}", manager_.Score);

		hpMeter_.fillAmount = (float)player_.Shield / (float)player_.shieldMax_;
	}

	public void AddEnemy( Enemy enemy) {

		EnemyInfo info = new EnemyInfo ();
		info.enemy_ = enemy;

		GameObject arrow = (GameObject)Instantiate (arrow_);
		info.arrow_ = arrow.GetComponent<RectTransform> ();
		info.arrow_.SetParent (this.GetComponent<RectTransform> ());
		info.arrow_.localPosition = new Vector3 (0, -20, 0);

		GameObject marker = (GameObject)Instantiate (marker_);
		info.marker_ = marker.GetComponent<RectTransform> ();
		info.marker_.SetParent (radarGrid_);
		info.marker_.localPosition = new Vector3 (0, 0, 0);

		enemyList_.Add (info);

		UpdateArrow (info);
		UpdateMarker (info);
	}

	public void DeleteEnemy( Enemy enemy ) {
		EnemyInfo target = null;
		foreach (EnemyInfo info in enemyList_) {
			if (info.enemy_ == enemy) {
				target = info;
				break;
			}
		}
		if (target != null) {
			enemyList_.Remove (target);

			Destroy (target.arrow_.gameObject);
			Destroy (target.marker_.gameObject);
		}
	}

	protected void UpdateArrow( EnemyInfo info ) {

		Vector3 s = info.enemy_.transform.position - player_.transform.position;
		float d = Vector3.Dot (player_.Forward, s.normalized);
		if (d < 0.9f) {
			float ds = Vector3.Dot (player_.Forward, s);
			Vector3 xy = s - (player_.Forward * ds);
			xy.Normalize ();

			float x = Vector3.Dot (player_.Right, xy);
			float y = Vector3.Dot (player_.Up, xy);

			float r = Mathf.Atan2 (-x, y);

			info.arrow_.gameObject.SetActive (true);
			info.arrow_.rotation = Quaternion.Euler (0, 0, r * Mathf.Rad2Deg);
		} else {
			info.arrow_.gameObject.SetActive (false);
		}
	}

	protected void UpdateMarker( EnemyInfo info ) {

		Vector3 s = info.enemy_.transform.position - player_.transform.position;
		float x = Vector3.Dot (player_.Right, s);
		float y = Vector3.Dot (player_.Forward, s);

		info.marker_.localPosition = new Vector3 (x, y, 0);

		float dx = Vector3.Dot (player_.Right, info.enemy_.Direction);
		float dy = Vector3.Dot (player_.Forward, info.enemy_.Direction);

		float r = Mathf.Atan2 (-dx, dy);
		info.marker_.rotation = Quaternion.Euler (0, 0, r * Mathf.Rad2Deg);

	}

	public void ShowGameOver() {
		gameOver_.gameObject.SetActive (true);
	}
}
