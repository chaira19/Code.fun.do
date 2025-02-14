using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class BombSpawnAndExplode : MonoBehaviour {

	Vector3 bombSpawn;
	float lastSpawnTime;
	float nextSpawnTime;
	ObjectMatrix om;
	randomLevelMaker rlm;
	int rows;
	int columns;
	int bombs_spawned;

	public bool isDead;
	public GameObject power_up_increase_blast;
	public GameObject power_up_increase_speed;
	public GameObject bomb;
	public float bombWaitTime;
	public GameObject player;
	public GameObject fireCenter;
	public GameObject fireSidewaysNoSmoke;
	public int explosionSpread;//if zero then explosion only in one cube, i.e. center
	public float explosionWaitTime;
	public float explosionStay;
	public float blockStayInExplosion;
	public GameObject broken_wooden_crate;
	public float broken_wooden_crate_stay_time;
	public float enemy_stay_in_explosion_time;
	public bool fire_button_pressed;
	public int max_bombs_allowed;
	public AudioSource music,music2;

	void Start(){
		isDead = false;
		lastSpawnTime=nextSpawnTime=0.0f;
		om = GameObject.Find("GameController").GetComponent<ObjectMatrix>();
		om.getObjectMatrix();
		rows = om.rows;
		columns = om.columns;
		rlm = GameObject.Find("GameController").GetComponent<randomLevelMaker>();
		rlm.generateLevel();
		fire_button_pressed = false;
		bombs_spawned = 0;
	}

	void FixedUpdate(){
		if (player == null) {
			isDead = true;
			return;
		} else {
			//position of bomb 
			bombSpawn = new Vector3(Mathf.Round(player.transform.position.x), Mathf.Round(player.transform.position.y), Mathf.Round(player.transform.position.z));

			//comment if don't want to use keyboard keys	
			if(Input.GetKey(KeyCode.F) && bombs_spawned < max_bombs_allowed && Time.time > nextSpawnTime && om.level[(int)bombSpawn.x, (int)bombSpawn.z]==null){
				lastSpawnTime = Time.time;//time of last bomb spawn
				nextSpawnTime = lastSpawnTime + bombWaitTime;//predicted time after which bomb spawn is possible
				GameObject spawnedBomb = (GameObject)Instantiate(bomb, bombSpawn, Quaternion.identity);//spawned bomb object
				Debug.Log(bombs_spawned);
				bombs_spawned++;
				om.level[(int)spawnedBomb.transform.position.x, (int)spawnedBomb.transform.position.z] = spawnedBomb.gameObject;//filling level matrix with the instantiated bomb
				Destroy(om.level[(int)spawnedBomb.transform.position.x, (int)spawnedBomb.transform.position.z],3);//destroying spawned bomb object
				Destroy(spawnedBomb,3);
				StartCoroutine(explosionSpawn(bombSpawn, spawnedBomb));//coroutine to do all the explosion stuff of the spwaned bomb
			}

			//used to control through on screen buttons
			//if(fire_button_pressed && bombs_spawned < max_bombs_allowed &&  Time.time > nextSpawnTime && om.level[(int)bombSpawn.x, (int)bombSpawn.z]==null){
			//	lastSpawnTime = Time.time;//time of last bomb spawn
			//	nextSpawnTime = lastSpawnTime + bombWaitTime;//predicted time after which bomb spawn is possible
			//	GameObject spawnedBomb = (GameObject)Instantiate(bomb, bombSpawn, Quaternion.identity);//spawned bomb object
			//	bombs_spawned++;
			//	om.level[(int)spawnedBomb.transform.position.x, (int)spawnedBomb.transform.position.z] = spawnedBomb.gameObject;//filling level matrix with the instantiated bomb
			//	Destroy(om.level[(int)spawnedBomb.transform.position.x, (int)spawnedBomb.transform.position.z],3);//destroying spawned bomb object
			//	Destroy(spawnedBomb,3);
			//	StartCoroutine(explosionSpawn(bombSpawn, spawnedBomb));//coroutine to do all the explosion stuff of the spwaned bomb
			//}else{
			//	fire_button_pressed = false;
			//}	
		}


	}


//Uncomment if want to use on-screen keys

//	public void spawn(){
//		bombSpawn = new Vector3(Mathf.Round(player.transform.position.x), Mathf.Round(player.transform.position.y), Mathf.Round(player.transform.position.z));
//		if(Time.time > nextSpawnTime && om.level[(int)bombSpawn.x, (int)bombSpawn.z]==null){
//			lastSpawnTime = Time.time;//time of last bomb spawn
//			nextSpawnTime = lastSpawnTime + bombWaitTime;//predicted time after which bomb spawn is possible
//			GameObject spawnedBomb = (GameObject)Instantiate(bomb, bombSpawn, Quaternion.identity);//spawned bomb object
//			om.level[(int)spawnedBomb.transform.position.x, (int)spawnedBomb.transform.position.z] = spawnedBomb.gameObject;//filling level matrix with the instantiated bomb
//			Destroy(om.level[(int)spawnedBomb.transform.position.x, (int)spawnedBomb.transform.position.z],3);//destroying spawned bomb object
//			Destroy(spawnedBomb,3);
//			StartCoroutine(explosionSpawn(bombSpawn, spawnedBomb));//coroutine to do all the explosion stuff of the spwaned bomb
//		}
//}


	//does all the explosion stuff by taking bomb position as explosion center
	IEnumerator explosionSpawn(Vector3 bombSpawnCenter, GameObject spawnedBomb){
		yield return new WaitForSeconds(3.0f);//made to wait 3 sec so that bomb can explode
		music.Play ();
		bombs_spawned--;
		bool forward = true;//true if explosion spawn allowed in +z
		bool back = true;//true if explosion spawn allowed in -z
		bool left = true;//true if explosion spawn allowed in -x
		bool right = true;//true if explosion spawn allowed in +x

		bool forwardLast = false;//true if current explosion is last explosion in +z
		bool backLast = false;//true if current explosion is last explosion in -z
		bool leftLast = false;//true if current explosion is last explosion in -x
		bool rightLast = false;//true if current explosion is last explosion in +x


		GameObject explosionCenter = (GameObject)Instantiate(fireCenter, bombSpawnCenter + Vector3.up * 0.11f, Quaternion.identity);
		//spawned explosion at the origin of the explosion or the position of the bomb

		Destroy(explosionCenter, explosionStay);//destroying spawned explosion game object
		yield return new WaitForSeconds(explosionWaitTime);//time gap between two consecutive explosions to illusion of moving outward

		for(int i=1; i<=explosionSpread; i++){
			//forward->positive Z
			//backward->negative Z
			//left->negative X
			//right->positive X


			if(i == explosionSpread){
				forwardLast = true;
				backLast = true;
				leftLast = true;
				rightLast = true;
			}//last explosion in all directions


			if(!(bombSpawnCenter.z + Vector3.forward.z *(i+1) <= columns-2)){
				forwardLast = true;
			}//last explosion in forward direction if encountered wall

			if(!(bombSpawnCenter.z + Vector3.back.z *(i+1) >= 0)){
				backLast = true;
			}//last explosion in backward direction if encountered wall

			if(!(bombSpawnCenter.x + Vector3.left.x *(i+1) >= 0)){
				leftLast = true;
			}//last explosion in left direction if encountered wall
			
			if(!(bombSpawnCenter.x + Vector3.right.x *(i+1) <= rows-2)){
				rightLast = true;
			}//last explosion in right direction if encountered wall


			//following four conditions all spawning explosions after checking availability of space
			if(forward && bombSpawnCenter.z + Vector3.forward.z *i <= columns-2)
				spawnExplosion(bombSpawnCenter + Vector3.forward *i + Vector3.up * 0.11f, Quaternion.Euler(new Vector3(0.0f, 180.0f, 0.0f)),ref forward, forwardLast);

			if(back && bombSpawnCenter.z + Vector3.back.z *i >= 0 )
				spawnExplosion(bombSpawnCenter + Vector3.back *i + Vector3.up * 0.11f, Quaternion.Euler(new Vector3(0.0f, 0.0f, 0.0f)),ref back, backLast);

			if(left && bombSpawnCenter.x + Vector3.left.x *i >= 0 )
				spawnExplosion(bombSpawnCenter + Vector3.left *i + Vector3.up * 0.11f, Quaternion.Euler(new Vector3(0.0f, 90.0f, 0.0f)),ref left, leftLast);

			if(right && bombSpawnCenter.x + Vector3.right.x *i <= rows-2)	
				spawnExplosion(bombSpawnCenter + Vector3.right *i + Vector3.up * 0.11f, Quaternion.Euler(new Vector3(0.0f, -90.0f, 0.0f)),ref right, rightLast);

			yield return new WaitForSeconds(explosionWaitTime);
		}
	}



	void spawnExplosion(Vector3 position, Quaternion rotation, ref bool spawnCheck, bool last){
		//spawnCheck is the permission for the explosion to be spawned in that direction

		// GameObject fireToBeSpawned;

		// if(!last){
		// 	fireToBeSpawned = fireSidewaysNoSmoke;	
		// }else{
		// 	fireToBeSpawned = fireSidewaysSmoke;	
		// }//game object to be spawned as explosions changes type if last in its direction
		Debug.Log("Reached here");
		Debug.Log ("x: "+position.x+"y: "+position.y+"z: "+position.z);
		if(om.level[(int)position.x, (int)position.z]==null){//if position in level matrix is empty
			GameObject spawnedExplosion = (GameObject)Instantiate(fireSidewaysNoSmoke, position, rotation);//spawn fire/explosion
			Destroy(spawnedExplosion, explosionStay);//destroy fire/explosion
		}else{
			//if position in level matrix not empty but could also be due to wooden box that needs to be destroyed if comes in explosion's range
			if(om.level[(int)position.x, (int)position.z].gameObject.tag=="newcrate"){
				//GameObject spawnedExplosion = (GameObject)Instantiate(fireSidewaysSmoke, position, rotation);//spawn explosion spawned on end points of explosion/fire as explosion/fire chain terminates here
				//Destroy(spawnedExplosion, explosionStay);//destroy the explosion/fire
				GameObject spawnedExplosion = (GameObject)Instantiate(fireSidewaysNoSmoke, position, rotation);//spawn fire/explosion
				Destroy(spawnedExplosion, explosionStay);//destroy fire/explosion
				Destroy(om.level[(int)position.x, (int)position.z].gameObject, blockStayInExplosion);//destroying the wooden block
				om.level[(int)position.x, (int)position.z]=null;//emptying the place in level matrix as wooden box is destroyed
				GameObject spawned_broken_crate = (GameObject)Instantiate(broken_wooden_crate,position,Quaternion.identity);
				Destroy(spawned_broken_crate, broken_wooden_crate_stay_time);
				spawnCheck = false;//no more permission to spawn explosion in this direction
				music2.Play();

			}else if(om.level[(int)position.x, (int)position.z].gameObject.tag=="woodenBox_power_up_increase_blast"){
				GameObject spawnedExplosion = (GameObject)Instantiate(fireSidewaysNoSmoke, position, rotation);//spawn fire/explosion
				Destroy(spawnedExplosion, explosionStay);//destroy fire/explosion
				Destroy(om.level[(int)position.x, (int)position.z].gameObject, blockStayInExplosion);
				GameObject spawned_power_up_increase_blast = (GameObject)Instantiate(power_up_increase_blast, position, Quaternion.identity);
				om.level[(int)position.x, (int)position.z] = spawned_power_up_increase_blast.gameObject;
				GameObject spawned_broken_crate = (GameObject)Instantiate(broken_wooden_crate,position,Quaternion.identity);
				Destroy(spawned_broken_crate, broken_wooden_crate_stay_time);
				spawnCheck = false;//no more permission to spawn explosion in this direction
				music2.Play();

			}else if(om.level[(int)position.x, (int)position.z].gameObject.tag=="woodenBox_power_up_increase_speed"){
				GameObject spawnedExplosion = (GameObject)Instantiate(fireSidewaysNoSmoke, position, rotation);//spawn fire/explosion
				Destroy(spawnedExplosion, explosionStay);//destroy fire/explosion
				Destroy(om.level[(int)position.x, (int)position.z].gameObject, blockStayInExplosion);
				GameObject spawned_power_up_increase_speed = (GameObject)Instantiate(power_up_increase_speed, position, Quaternion.identity);
				om.level[(int)position.x, (int)position.z] = spawned_power_up_increase_speed.gameObject;
				GameObject spawned_broken_crate = (GameObject)Instantiate(broken_wooden_crate,position,Quaternion.identity);
				Destroy(spawned_broken_crate, broken_wooden_crate_stay_time);
				spawnCheck = false;//no more permission to spawn explosion in this direction
				music2.Play();

			}else if(om.level[(int)position.x, (int)position.z].gameObject.tag=="Enemy"){
			

				GameObject spawnedExplosion = (GameObject)Instantiate(fireSidewaysNoSmoke, position, rotation);//spawn fire/explosion
				Destroy(spawnedExplosion, explosionStay);//destroy fire/explosion
				Destroy(om.level[(int)position.x, (int)position.z].gameObject, enemy_stay_in_explosion_time);
				om.level[(int)position.x, (int)position.z]=null;//emptying the place in level matrix as wooden box is destroyed
				//no spawnCheck = false in this case as explosion will pass through enemy
				//DO STUFF

			}else if(om.level[(int)position.x, (int)position.z].gameObject.tag=="bomb"){
			

				GameObject spawnedExplosion = (GameObject)Instantiate(fireSidewaysNoSmoke, position, rotation);//spawn fire/explosion
				Destroy(spawnedExplosion, explosionStay);//destroy fire/explosion
				//no spawnCheck = false in this case as explosion will pass through bomb
			
			}else{
				spawnCheck = false;
			}

			
		}
	}
}
