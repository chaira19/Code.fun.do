using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class SceneManagement : MonoBehaviour {
	GreatArcStudios.PauseManager pm;
    public int scene = 1;
	//public GameObject End_menu;
	public GameObject pause_menu;
	void Start () {
	}
	
	// Update is called once per frame
	void Update () {
		

		
	}
	public void LoadGame()
	{   pause_menu.SetActive (false);
	//	End_menu.SetActive (true);
			UnityEngine.SceneManagement.SceneManager.LoadScene (scene);

	}

	public void LoadMenu()
	{
		pause_menu.SetActive (false);
		//End_menu.SetActive (true); 
		UnityEngine.SceneManagement.SceneManager.LoadScene (0);

	}
}
