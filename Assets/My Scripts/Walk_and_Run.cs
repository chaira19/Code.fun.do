using System.Collections;
using System.Collections.Generic;
using UnityEngine;
//using UnityStandardAssets.CrossPlatformInput;

public class Walk_and_Run : MonoBehaviour {
	public Animator animator;


void Start(){
		
		animator = GetComponent<Animator> ();
	}
	void Update () {
}
	public void Walk()
	{
	animator.SetBool ("Walk", true);
	animator.SetBool ("SprintJump", false);
	animator.SetBool ("SprintSlide", false);
}
	public void Idle ()
	{	
		animator.SetBool ("Walk", false);
		animator.SetBool ("SprintJump", false);
		animator.SetBool ("SprintSlide", false);
	}
}