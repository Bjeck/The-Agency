using UnityEngine;
using System.Collections;
using UnityEngine.Audio;

public class AudioAnalyzer : MonoBehaviour {

	public int numSamples = 64;

	// Private Variables
	float[] numberleft;
	float[] numberright;
	float[] volumeSamples;
	float[] volumeSamples1;
	float volumenumber;

	[Range(0,100)]
	public float volumeScale;
	public float volumeRef = 0.1f;

	public GlitchEffectArray gle;
	public lineVolume lineVol;

	void Start() {
		numberleft = new float[numSamples];
		numberright = new float[numSamples];
		volumeSamples = new float[numSamples];
		volumeSamples1 = new float[numSamples];
		volumenumber = 0;
	}

	// Update is called once per frame
	void Update () {

		AudioListener.GetOutputData(volumeSamples, 0);
		AudioListener.GetOutputData(volumeSamples1, 1);

		for (int i = 0; i < volumeSamples[0]; i++) {
			volumeSamples[i] = (volumeSamples[i]+volumeSamples1[i])/2;
		}

		volumenumber = 0f;
		for(int j=0; j < numSamples; j++){

			volumenumber += volumeSamples[j]*volumeSamples[j]; //sum squared samples.

		}

		volumenumber = Mathf.Sqrt(volumenumber/numSamples); //rms = square root of average
		volumenumber = 20*Mathf.Log10(volumenumber/volumeRef); //convert to dB
		if (volumenumber < -160) volumenumber = -160;

		gle.audioVolume = volumenumber+volumeScale;
		lineVol.AddNewVolume(volumenumber+volumeScale);


	}

}
