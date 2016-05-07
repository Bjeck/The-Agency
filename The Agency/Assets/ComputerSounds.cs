using UnityEngine;
using System.Collections;

public class ComputerSounds : MonoBehaviour {

	public AudioSource hoverSound;
	public AudioSource clickSound;
	public AudioSource inputSound;

	// Use this for initialization
	void Start () {
	
	}
	

	public void ButtonHover(){
		hoverSound.Play();
	}


	public void ButtonClick(){
		clickSound.Play();
	}

	public void ButtonClick(float p){
		clickSound.pitch = p;
		clickSound.Play();
	}

	public void InputDone(){
		inputSound.Play();
	}

}
