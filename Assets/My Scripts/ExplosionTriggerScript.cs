using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ExplosionTriggerScript : MonoBehaviour {
	public AudioSource music;
	public GameObject dead;

	immunity im;
	randomLevelMaker_cfd rlm;
	score_counter sc;

	bool player_dead;

	void Start ()
	{
		rlm = GameObject.Find ("GameController").GetComponent<randomLevelMaker_cfd> ();
		sc = GameObject.Find ("GameController").GetComponent<score_counter> ();
		player_dead  = false;
	}

	void OnTriggerEnter(Collider collider){
		if(collider.tag=="Player" && !player_dead){
			Destroy (collider.gameObject);
			music.Play ();
			sc.net_score += sc.points_death;
			player_dead = true;
		}

		if(collider.tag=="Enemy_moving"){
			im = collider.gameObject.GetComponent<immunity> ();

			if (!im.immune) {
				Vector3 rot = new Vector3 (collider.transform.eulerAngles.x, collider.transform.eulerAngles.y, collider.transform.eulerAngles.z);
				dead.transform.eulerAngles=rot;
				Vector3 tra = new Vector3 (collider.transform.position.x,collider.transform.position.y,collider.transform.position.z);
				dead.transform.position=tra;
				Destroy(collider.gameObject);
				Instantiate (dead);
				rlm.enemyCount--;
				//rlm.set_count_canvas ();
				sc.net_score += sc.points_enemyKill;
			}


		}
	}

	//void OnTriggerStay(Collider collider){
	//}

	
}
