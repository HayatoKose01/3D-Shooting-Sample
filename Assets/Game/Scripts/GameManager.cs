using UnityEngine;
using System.Collections;

public class GameManager : MonoBehaviour {

	public GameObject asteroid_;
	public int asteroidNum_;
	public float asteroidScale_;

	public Vector3 asteroidRange_;
	public Vector3 enemyRange_;

	public GameObject player_;
	public GameObject enemy_;

	public int enemyNumStart_;

	public int Score { get; private set; }

	public UIController uiController_;

	bool gameOver_;

	// Use this for initialization
	void Start () {

		gameOver_ = false;
	
		for (int i = 0; i < asteroidNum_; ++i) {

			GameObject a = (GameObject)GameManager.Instantiate (asteroid_, GetRandomPos(asteroidRange_), Random.rotation);
			a.transform.localScale = Vector3.one * Random.Range (1.0f, asteroidScale_);
			a.transform.parent = this.transform;
		}

		for (int i = 0; i < enemyNumStart_; ++i) {
			AddEnemy ();
		}
	}

	protected void AddEnemy() {
		GameObject e = (GameObject)GameManager.Instantiate (enemy_, GetRandomPos(enemyRange_), Random.rotation);
		Enemy enemy = e.GetComponent<Enemy> ();
		enemy.player_ = player_;
		enemy.manager_ = this;

		uiController_.AddEnemy (enemy);

		enemy.player_ = player_;
	}

	protected Vector3 GetRandomPos(Vector3 range) {
		float x = Random.Range (-range.x, range.x);
		float y = Random.Range (-range.y, range.y);
		float z = Random.Range (-range.z, range.z);

		return new Vector3 (x, y, z);
	}
	
	// Update is called once per frame
	void Update () {
	
		if (gameOver_) {
			if( Input.GetButton( "Fire1" ) == true ) {
				Application.LoadLevel( "Game" );
			}
		}
	}

	public void DeleteEnemy( Enemy enemy ) {
		Score += 10;
		uiController_.DeleteEnemy (enemy);

		for (int i = 0; i < 2; ++i) {
			AddEnemy ();
		}
	}

	public void GameOver() {

		gameOver_ = true;
		uiController_.ShowGameOver ();
	}
}
