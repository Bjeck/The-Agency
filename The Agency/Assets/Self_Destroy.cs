using UnityEngine;
using System.Collections.Generic;

public class Self_Destroy : MonoBehaviour {

	public float timeTilDestroy = 0;

	public GlitchEffectArray gle;
	public AudioSource source;
	public int numSamples = 64;

	float[] volumeSamples;
	float volumenumber;

	float volumeScale = 40f;
	float volumeRef = 0.1f;

	public lineVolume lineVol;
	float myRef;

	void Start() {
		volumeSamples = new float[numSamples];
		volumenumber = 0;

		gle = GameObject.FindGameObjectWithTag("GLITCH").GetComponent<GlitchEffectArray>();
		gle.AddPosition(transform.position,gameObject);
	
	}
	

	void Update () {

		source.GetOutputData(volumeSamples, 0);

		volumenumber = 0f;
		for(int j=0; j < numSamples; j++){
			volumenumber += volumeSamples[j]*volumeSamples[j]; //sum squared samples.
		}

		volumenumber = Mathf.Sqrt(volumenumber/numSamples); //rms = square root of average
		volumenumber = 20*Mathf.Log10(volumenumber/volumeRef); //convert to dB
		if (volumenumber < -160) volumenumber = -160;

		gle.positions[gameObject].scale = volumenumber+volumeScale;


		timeTilDestroy -= Time.deltaTime;

		if(timeTilDestroy <= 0){
			gle.RemovePosition(gameObject);
			Destroy(gameObject);
		}
	
	}
}
