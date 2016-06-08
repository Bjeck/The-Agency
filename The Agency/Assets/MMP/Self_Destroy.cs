using UnityEngine;
using System.Collections.Generic;

public class Self_Destroy : MonoBehaviour {

	/// <summary>
	/// This script is placed on the spawned object that spawns where the sound plays.
	/// </summary>

	public float timeTilDestroy = 0;
	public int numSamples = 64;

	float[] volumeSamples;
	float volumenumber;
	float volumeScale = 40f;
	float volumeRef = 0.1f;

	public lineVolume lineVol;
	public GlitchEffectArray gle;
	public AudioSource source;


	void Start() {
		volumeSamples = new float[numSamples];
		volumenumber = 0;

		gle = GameObject.FindGameObjectWithTag("GLITCH").GetComponent<GlitchEffectArray>();

		gle.AddPosition(transform.position,gameObject);	//Adds this position to the array of positions in GlitchEffectArray
	
	}
	

	void Update () {

		source.GetOutputData(volumeSamples, 0);			//Method for getting audio volume is the same as AudioAnalyzer. See that script.

		volumenumber = 0f;
		for(int j=0; j < numSamples; j++){
			volumenumber += volumeSamples[j]*volumeSamples[j]; 
		}

		volumenumber = Mathf.Sqrt(volumenumber/numSamples); 
		volumenumber = 20*Mathf.Log10(volumenumber/volumeRef); 
		if (volumenumber < -160) volumenumber = -160;

		gle.positions[gameObject].scale = volumenumber+volumeScale;	//Audiovolume is sent to the GlitchEffectArray. Scale is added because the dB value is quite low. 


		timeTilDestroy -= Time.deltaTime;		//Time ticks down until it reaches 0, where this object is destroyed.

		if(timeTilDestroy <= 0){
			gle.RemovePosition(gameObject);		//On destroy, this object's position is removed from the list, as this sound now stops playing.
			Destroy(gameObject);
		}
	
	}
}
