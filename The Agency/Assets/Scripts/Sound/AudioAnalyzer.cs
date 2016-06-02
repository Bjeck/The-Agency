using UnityEngine;
using System.Collections;
using UnityEngine.Audio;

public class AudioAnalyzer : MonoBehaviour {

	public int numSamples = 64;

	// Private Varaibles
	float[] numberleft;
	float[] numberright;
	float[] volumeSamples;
	float[] volumeSamples1;
	float volumenumber;

	public float pitch;
	float threshold = 0.02f;

	[Range(0,100)]
	public float volumeScale;
	public float volumeRef = 0.1f;
	public float specScale = 20f;

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
	/*	AudioListener.GetSpectrumData (numberleft, 0,FFTWindow.BlackmanHarris);
		AudioListener.GetSpectrumData (numberright, 1,FFTWindow.BlackmanHarris);

		//	22050/samplenumber = 21
		//	TO FIND ELEMENT IN ARRAY: 
		//	FrequencyYouWant / 21(result from last)
		// 441 Hz -> [21]. that's then numberleft[21]
		//	print (numberleft[21]);


		float freq;

		int maxV = 0;
		int maxN = 0;

		float specLeft;
		float specRight;

		for(int i=0; i < numSamples; i++){ 
			if (float.IsInfinity(numberleft[i]) || float.IsNaN(numberleft[i])){
			}else{


			}




			//PITCH STUFF
			if(!(numberleft[i] > maxV) || !(numberleft[i] > threshold))
				continue;

			maxV = (int)numberleft[i];
			maxN = i;
		}


		//print (numberleft[0]+" "+numberleft[numSamples-2]+"       "+numberright[0]+" "+numberleft[numSamples-2]);

		//PITCH CALCULATION
		float freqN = maxN;
		if(maxN > 0 && maxN < numSamples - 1){ //interpolate index using neighbors
			float dl = numberleft[maxN - 1] / numberleft[maxN];
			float dr = numberleft[maxN + 1] / numberleft[maxN];
			freqN += 0.5f * (dr*dr-dl*dl);
		}

		pitch = freqN * (AudioSettings.outputSampleRate / 2) / numSamples;
		//print(pitch);

*/

		AudioListener.GetOutputData(volumeSamples, 0);
		AudioListener.GetOutputData(volumeSamples1, 1);

		for (int i = 0; i < volumeSamples[0]; i++) {
			volumeSamples[i] = (volumeSamples[i]+volumeSamples1[i])/2;
		}

		volumenumber = 0f;
		for(int j=0; j < numSamples; j++){
			//	if(numberleft[j] != 0){
			//	volumenumber += numberleft[j];
			//	}
			volumenumber += volumeSamples[j]*volumeSamples[j]; //sum squared samples.
		}

		volumenumber = Mathf.Sqrt(volumenumber/numSamples); //rms = square root of average
		volumenumber = (1/Mathf.Abs(20*Mathf.Log10(volumenumber/volumeRef))); //convert to dB

		gle.audioVolume = volumenumber*volumeScale;

		lineVol.AddNewVolume(volumenumber*volumeScale);


	}

}
