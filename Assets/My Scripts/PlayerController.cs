using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour {

    float x;
	float z;
	ObjectMatrix om;

	public int direction_x;
	public int direction_z;
	public float speed;
	public int ar_camera_side;

	void Start(){
		ar_camera_side = 0;
		//0-->normal
		//1->left
		//2->top
		//3->right

		direction_x = 0;
		direction_z = 0;
		om = GameObject.Find("GameController").GetComponent<ObjectMatrix>();
	}


	// Update is called once per frame
	void Update () {
		//for cross-platform 
		//x = CrossPlatformInputManager.GetAxis("Horizontal") * Time.deltaTime * speed;
		//z = CrossPlatformInputManager.GetAxis("Vertical") * Time.deltaTime * speed;


		//for keyboard keys
		x = Input.GetAxis("Horizontal") * Time.deltaTime * speed;
		z = Input.GetAxis("Vertical") * Time.deltaTime * speed;


		//for on-screen buttons
		if (ar_camera_side == 0) {
			x = direction_x * Time.deltaTime * speed;
			z = direction_z * Time.deltaTime * speed;
		} else if (ar_camera_side == 1) {
			z = -direction_x * Time.deltaTime * speed;
			x = direction_z * Time.deltaTime * speed;
		} else if (ar_camera_side == 2) {
			x = -direction_x * Time.deltaTime * speed;
			z = -direction_z * Time.deltaTime * speed;
		} else {
			z = direction_x * Time.deltaTime * speed;
			x = -direction_z * Time.deltaTime * speed;
		}



		if(Mathf.Abs(x) > Mathf.Abs(z)){
			z=0;	
		}else x=0;

		Vector3 approxPosition = new Vector3(Mathf.Round(transform.position.x), Mathf.Round(transform.position.y), Mathf.Round(transform.position.z));
		
		movement(x,z,approxPosition);
	}

	void movement(float x, float z, Vector3 approxPosition){
		if(x>0){
			transform.eulerAngles = new Vector3(0,0,0);	
			transform.Translate(x,0,0);
			if(approxPosition.x != om.rows-2){
				if((om.level[(int)approxPosition.x + 1, (int)approxPosition.z] != null && transform.position.x > approxPosition.x) 
					&& om.level[(int)approxPosition.x + 1, (int)approxPosition.z].gameObject.layer != 10){
					transform.position = approxPosition;
				}	
			}else{
				if(transform.position.x > approxPosition.x 
					/*&& om.level[(int)approxPosition.x + 1, (int)approxPosition.z].gameObject.layer != 10*/){
					transform.position = approxPosition;
				}
			}
			
		}else if(x<0){
			transform.eulerAngles = new Vector3(0,180,0);
			transform.Translate(-x,0,0);
			if(approxPosition.x != 0){
				if((om.level[(int)approxPosition.x - 1, (int)approxPosition.z] != null && transform.position.x < approxPosition.x)  
					&& om.level[(int)approxPosition.x - 1, (int)approxPosition.z].gameObject.layer != 10){
					transform.position = approxPosition;
				}
			}else{
				if(transform.position.x < approxPosition.x 
					/*&& om.level[(int)approxPosition.x + 1, (int)approxPosition.z].gameObject.layer != 10*/){
					transform.position = approxPosition;
				}
			}
		}

		if(z>0){
			if(transform.rotation.y!=180 ){

				transform.eulerAngles = new Vector3(0,-90,0);
				transform.Translate(z,0,0);

				if(approxPosition.z != om.columns-2){
					if((om.level[(int)approxPosition.x, (int)approxPosition.z + 1] != null && transform.position.z > approxPosition.z)  
						&& om.level[(int)approxPosition.x, (int)approxPosition.z + 1].gameObject.layer != 10){
						transform.position = approxPosition;
					}	
				}else{
					if(transform.position.z > approxPosition.z 
						/*&& om.level[(int)approxPosition.x + 1, (int)approxPosition.z].gameObject.layer != 10*/){
						transform.position = approxPosition;
					}
				}
			}
		}else if(z<0){
			if(transform.rotation.y!=180 ){

				transform.eulerAngles = new Vector3(0,90,0);
				transform.Translate(-z,0,0);

				if(approxPosition.z != 0){
					if((om.level[(int)approxPosition.x, (int)approxPosition.z - 1] != null && transform.position.z < approxPosition.z)  
						&& om.level[(int)approxPosition.x, (int)approxPosition.z - 1].gameObject.layer != 10){
						transform.position = approxPosition;
					}	
				}else{
					if(transform.position.z < approxPosition.z 
						/*&& om.level[(int)approxPosition.x + 1, (int)approxPosition.z].gameObject.layer != 10*/){
						transform.position = approxPosition;
					}
				}
			}
		}
		
	}
}
