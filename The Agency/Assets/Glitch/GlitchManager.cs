using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine.Audio;

public class GlitchManager : MonoBehaviour {
	public static GlitchManager instance { get; private set; }

	public List<GameObject> textObjects = new List<GameObject>();

	public float timeToGlitchMin;
	public float timeToGlitchMax;
	public float sustainGlitchTimeMin = 0.03f;
	public float sustainGlitchTimeMax = 0.4f;
	public bool shouldUpdateGlitchTiming;
	[SerializeField]
	int GlitchProgression = 0;
	bool firsttime = true;

	public GlitchEffect glEf;
	public float glitchIntensity;
	public float glitchTimeMin;
	public float glitchTimeMax;
	float timeToGlitch;
	public float glitchSustainMin;
	public float glitchSustainMax;

	public AudioSource[] noiseSounds;
	public AudioMixer mixer;
	public float timeScreenGlitch;


	//0.01, 0.02, 0.005, 0.015

	void Awake()
	{
		if (instance != null && instance != this) {
			Destroy (gameObject);
		}
		instance = this;
		DontDestroyOnLoad (gameObject);
	}


	public void Update(){
		if (timeScreenGlitch > 0) {
			timeScreenGlitch -= Time.deltaTime;
		} else {
			StartCoroutine(GlitchScreen());
		}

		if(Input.GetKey(KeyCode.Escape)){
			Application.Quit();	
		}
		
	}

	public IEnumerator GlitchScreen(){
		if (firsttime) {
			glEf.enabled = true;
			timeScreenGlitch = UnityEngine.Random.Range (glitchTimeMin, glitchTimeMax);
			timeToGlitch = 1f;
			glEf.intensity = glitchIntensity;
			firsttime = false;
		} else {
			glEf.enabled = true;
			timeScreenGlitch = UnityEngine.Random.Range (glitchTimeMin, glitchTimeMax);
			timeToGlitch = UnityEngine.Random.Range (glitchSustainMin, glitchSustainMax);
			glEf.intensity = glitchIntensity * Random.Range (0.5f, 1.5f);
		}

		//PlayGlitchSound ();
		
		while (timeToGlitch > 0) {
			timeToGlitch -= Time.deltaTime;
			yield return 0;
		}
		glEf.intensity = 0;
		glEf.enabled = false;
		yield return 0;
	}

	public void GlitchScreenOnCommand(float t){
		StartCoroutine (GlitchScreenOC (t));
	}

	public void GlitchScreenOnCommand(float t, float inten){
		StartCoroutine (GlitchScreenOC (t, inten));
	}

	IEnumerator GlitchScreenOC(float time){

		glEf.enabled = true;
		glEf.intensity = glitchIntensity * Random.Range (0.5f, 1.5f);
		
		//PlayGlitchSound ();
		
		while (time > 0) {
			time -= Time.deltaTime;
			yield return 0;
		}
		glEf.intensity = 0;
		glEf.enabled = false;
		yield return 0;
	}

	IEnumerator GlitchScreenOC(float time, float inten){
		
		glEf.enabled = true;
		glEf.intensity = inten;
		
		//PlayGlitchSound ();
		
		while (time > 0) {
			time -= Time.deltaTime;
			yield return 0;
		}
		glEf.intensity = 0;
		glEf.enabled = false;
		yield return 0;
	}



	public void ChangeGlitchTimings(){ //timeToGlitchMin = 1f; timeToGlitchMax = 4f; sustainGlitchTimeMin = 0.03f; sustainGlitchTimeMax = 0.08f;
		if (GlitchProgression == 0) {  //START GLITCHING, sage comes
			timeToGlitchMin = 2f; timeToGlitchMax = 20f; sustainGlitchTimeMin = 0.03f; sustainGlitchTimeMax = 0.08f; 
			glitchTimeMin = 20f; glitchTimeMax = 70f; glitchSustainMin = 0.1f; glitchSustainMax = 0.4f;
		}
		else if (GlitchProgression == 1) {//WENT TO POND
			timeToGlitchMin = 2f; timeToGlitchMax = 15f; sustainGlitchTimeMin = 0.02f; sustainGlitchTimeMax = 0.1f;
			glitchTimeMin = 20f; glitchTimeMax = 60f; glitchSustainMin = 0.1f; glitchSustainMax = 0.4f;
		}
		else if(GlitchProgression == 2){ //C cursed
			timeToGlitchMin = 1f; timeToGlitchMax = 15f; sustainGlitchTimeMin = 0.02f; sustainGlitchTimeMax = 0.15f;
			glitchTimeMin = 15f; glitchTimeMax = 50f; glitchSustainMin = 0.1f; glitchSustainMax = 1.0f;
		}
		else if(GlitchProgression == 3){ //ritual
			timeToGlitchMin = 0.5f; timeToGlitchMax = 10f; sustainGlitchTimeMin = 0.01f; sustainGlitchTimeMax = 0.15f;
			glitchTimeMin = 10f; glitchTimeMax = 30f; glitchSustainMin = 0.1f; glitchSustainMax = 0.6f;
		}
		else if(GlitchProgression == 4){ //ritual go nuts
			timeToGlitchMin = 0.01f; timeToGlitchMax = 4f; sustainGlitchTimeMin = 0.01f; sustainGlitchTimeMax = 0.15f;
			glitchTimeMin = 5f; glitchTimeMax = 20f; glitchSustainMin = 0.1f; glitchSustainMax = 0.8f;
		}
		else if(GlitchProgression == 5){ //reboot start
			timeToGlitchMin = 5f; timeToGlitchMax = 20f; sustainGlitchTimeMin = 0.04f; sustainGlitchTimeMax = 0.07f;
			glitchTimeMin = 10f; glitchTimeMax = 40f; glitchSustainMin = 0.1f; glitchSustainMax = 0.8f;
		}
		else if(GlitchProgression == 6){ //search start
			timeToGlitchMin = 5f; timeToGlitchMax = 20f; sustainGlitchTimeMin = 0.04f; sustainGlitchTimeMax = 0.07f;
			glitchTimeMin = 10f; glitchTimeMax = 40f; glitchSustainMin = 0.1f; glitchSustainMax = 0.8f;
		}
		else if(GlitchProgression == 7){ // search go nuts
			timeToGlitchMin = 0.01f; timeToGlitchMax = 1.0f; sustainGlitchTimeMin = 0.01f; sustainGlitchTimeMax = 0.5f;
			glitchTimeMin = 1f; glitchTimeMax = 5f; glitchSustainMin = 0.1f; glitchSustainMax = 1f;
		}
		else if(GlitchProgression == 8){ //search end
			timeToGlitchMin = 100000f; timeToGlitchMax = 100000000000f; sustainGlitchTimeMin = 0.00f; sustainGlitchTimeMax = 0.00f;
			glitchTimeMin = 1000000f; glitchTimeMax = 1000000000000f; glitchSustainMin = 0.0f; glitchSustainMax = 0.0f;
		}

		timeScreenGlitch = 0;
		GlitchProgression++;
	}

	 //START GLITCHING, sage comes
	 //WENT TO POND
	//C cursed
	//ritual
	//ritual go nuts
	//search start
	// search go nuts
	//search end
			


	/*public void PlayGlitchSound(int c){
		int soundChooser = 0;
		switch (c) {
		case 0 :
			soundChooser = Random.Range (0, noiseSounds.Length+1);
			if(soundChooser == 2){
				soundChooser += Random.Range(-2,1);
			}
			mixer.SetFloat("drymix",Random.Range(0,1f));
			mixer.SetFloat ("wetmix", Random.Range (0, 1f));
			mixer.SetFloat("rate",Random.Range(0,20));
			mixer.SetFloat("lowpassCut",22000f);
			if(soundChooser < noiseSounds.Length){
				noiseSounds [soundChooser].Play ();
			}
			break;
		case 1 :
			soundChooser = Random.Range (0, noiseSounds.Length+1);
			if(soundChooser == 2){
				soundChooser += Random.Range(-1,1);
			}
			mixer.SetFloat("lowpassCut",Random.Range(100f,20000f));
			if(soundChooser < noiseSounds.Length){
				noiseSounds [soundChooser].Play ();
			}
			break;
		case 2 :
			mixer.SetFloat("drymix",Random.Range(0,1f));
			mixer.SetFloat ("wetmix", Random.Range (0, 1f));
			mixer.SetFloat("rate",Random.Range(0,20));
			mixer.SetFloat("lowpassCut",22000f);
			noiseSounds [2].Play ();
			break;
		}
		if (soundChooser == noiseSounds.Length) {
			foreach (AudioSource a in noiseSounds) {
				a.Stop();		
			}
		}
		SoundManager.instance.master.SetFloat ("volume", -80);

	}*/





}
