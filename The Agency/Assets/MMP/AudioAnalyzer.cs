using UnityEngine;
using System.Collections;
using UnityEngine.Audio;

public class AudioAnalyzer : MonoBehaviour {


	/// <summary>
	/// This script analyzes the General Audio Output and calcuates the volume present at each frame. It then maps this to dB and sends it to the appropriate scripts.
	/// </summary>


	// Private Variables
	float[] numberleft;
	float[] numberright;
	float[] volumeSamples;
	float[] volumeSamples1;
	float volumenumber;

	[Range(0,100)]
	public float volumeScale;
	public int numSamples = 64;
	public float volumeRef = 0.1f;

	public GlitchEffectArray gle;
	public lineVolume lineVol;

	void Start() {
		numberleft = new float[numSamples];		//Setup of the float arrays needed for the audio data.
		numberright = new float[numSamples];
		volumeSamples = new float[numSamples];
		volumeSamples1 = new float[numSamples];
		volumenumber = 0;
	}

	// Update is called once per frame
	void Update () {

		AudioListener.GetOutputData(volumeSamples, 0); 		//Gathering audio data. Both left and right channel are gathered.
		AudioListener.GetOutputData(volumeSamples1, 1);

		for (int i = 0; i < volumeSamples[0]; i++) {
			volumeSamples[i] = (volumeSamples[i]+volumeSamples1[i])/2;		//General output data is taken as the average between the two channels.
		}

		volumenumber = 0f;
		for(int j=0; j < numSamples; j++){
			volumenumber += volumeSamples[j]*volumeSamples[j]; //sum squared samples are taken of the audio data. This will result in a volume number that is higher the higher the volume is of the data. 
		}
															   //However, as we hear audio logarithmically, a direct numerical representation of audio volume is not accurate to how we hear it. 
															   //Instead, it should be mapped to decibel, which is done with this calculation:

		volumenumber = Mathf.Sqrt(volumenumber/numSamples); //square root of average
		volumenumber = 20*Mathf.Log10(volumenumber/volumeRef); //convert to dB
		if (volumenumber < -160) volumenumber = -160;		   //Minimized, so volume can't be infinitely small.

		gle.audioVolume = volumenumber+volumeScale;			  //Audiovolume is sent to the GlitchEffectArray and lineVolume scripts. Scale is added because the dB value is quite low. 
		lineVol.AddNewVolume(volumenumber+volumeScale);


	}

}
