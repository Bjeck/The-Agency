using UnityEngine;
using System.Collections;
using UnityEngine.Audio;

public class audiotesting : MonoBehaviour {

	public AudioClip ac;
	public AudioSource aso;
	public AudioMixer master;
	public AudioListener al;

	public AudioListener Audio;
	public int numSamples = 64;
	public GameObject abar;

	// Private Varaibles
	float[] numberleft;
	float[] numberright;
	float[] volumeSamples;
	float volumenumber;
	public GameObject[] thebarsleft;
	public GameObject[] thebarsright;
	float spacing;
	float width;

	public float pitch;
	float threshold = 0.02f;

	[Range(0,100)]
	public float volumeScale;
	public float volumeRef = 0.1f;
	public float specScale = 20f;

	public GlitchEffectArray gle;

	void Start() {
		numberleft = new float[numSamples];
		numberright = new float[numSamples];
		volumeSamples = new float[numSamples];

		thebarsleft = new GameObject[numSamples];
		thebarsright = new GameObject[numSamples];
		volumenumber = 0;
		spacing = 0.4f - (numSamples * 0.001f);
		width = 0.3f - (numSamples * 0.001f);
		for(int i=0; i < numSamples; i++){
			float xpos = i*spacing -8.0f;
			Vector3 positionleft = new Vector3(xpos,3, 0);
			thebarsleft[i] = (GameObject)Instantiate(abar, positionleft, Quaternion.identity) as GameObject;
			thebarsleft[i].transform.localScale = new Vector3(width,1,0.2f);

			Vector3 positionright = new Vector3(xpos,-3, 0);
			thebarsright[i] = (GameObject)Instantiate(abar, positionright, Quaternion.identity) as GameObject; 
			thebarsright[i].transform.localScale = new Vector3(width,1,0.2f);
		}

	}

	// Update is called once per frame
	void Update () {
		AudioListener.GetSpectrumData (numberleft, 0,FFTWindow.BlackmanHarris);
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

				//if(maxN > 0 && maxN < numSamples - 1){ //interpolate index using neighbors
			//		float dl = numberleft[maxN - 1] / numberleft[maxN];
				//	float dr = numberleft[maxN + 1] / numberleft[maxN];
				//	freq = 0.5f * (dr*dr-dl*dl);




			thebarsleft[i].transform.localScale = new Vector3(width, (1/Mathf.Abs(Mathf.Log10 (numberleft[i])))*specScale,0.2f);
			thebarsright[i].transform.localScale = new Vector3(width, (1/Mathf.Abs(Mathf.Log10 (numberright[i])))*specScale,0.2f); 


		/*		specLeft = numberleft[i];
				if(specLeft != 0){
					specLeft *= 10f;
					specLeft = Mathf.Log10(specLeft);
				}
				specRight = numberleft[i];
				if(specRight != 0){
					specRight *= 10f;
				//	specRight = Mathf.Log10(specRight);
				}

				specLeft = Mathf.Abs(specLeft);
				specRight = Mathf.Abs(specRight);

				//print (numberleft[i]+" "+numberright[i]);

				thebarsleft[i].transform.localScale = new Vector3(width, specLeft,0.2f);
				thebarsright[i].transform.localScale = new Vector3(width, specRight,0.2f); */
			//}

			//	if(i== 16){
				//	print(i+"!!! LOG: "+specLeft+" NONLOG:"+specRight);
				//}


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



		AudioListener.GetOutputData(volumeSamples, 0);

		volumenumber = 0f;
		for(int j=0; j < numSamples; j++){
		//	if(numberleft[j] != 0){
			//	volumenumber += numberleft[j];
		//	}
			volumenumber += volumeSamples[j]*volumeSamples[j]; //sum squared samples.
		}

		volumenumber = Mathf.Sqrt(volumenumber/numSamples); //rms = square root of average
		volumenumber = (1/Mathf.Abs(20*Mathf.Log10(volumenumber/volumeRef))); //convert to dB
		//if(volumenumber < -160)
		//	volumenumber = -160f; //clamp to -160 dB

		//volumenumber /= numSamples;
		//volumenumber = (1/Mathf.Log(Mathf.Abs(volumenumber)))*volumeScale;
		transform.localScale = new Vector3(transform.localScale.x,(volumenumber)*volumeScale,1); 
	//	print(volumenumber*volumeScale);
		gle.audioVolume = volumenumber*volumeScale;
		//transform.localScale = Vector3.Lerp(transform.localScale,new Vector3(transform.localScale.x,volumenumber*volumeScale,1),Time.deltaTime);
	}

/*
	void Update() {

		
		AudioListener.GetSpectrumData(spectrum, 0, FFTWindow.BlackmanHarris);
		//print(spectrum[0]);
	int i = 1;
	while (i < spectrum.Length-1) {
		Debug.DrawLine(new Vector3(i - 1, spectrum[i] + 10, 0), new Vector3(i, spectrum[i + 1] + 10, 0), Color.red);
		Debug.DrawLine(new Vector3(i - 1, Mathf.Log(spectrum[i - 1]) + 10, 2), new Vector3(i, Mathf.Log(spectrum[i]) + 10, 2), Color.cyan);
		Debug.DrawLine(new Vector3(Mathf.Log(i - 1), spectrum[i - 1] - 10, 1), new Vector3(Mathf.Log(i), spectrum[i] - 10, 1), Color.green);
		Debug.DrawLine(new Vector3(Mathf.Log(i - 1), Mathf.Log(spectrum[i - 1]), 3), new Vector3(Mathf.Log(i), Mathf.Log(spectrum[i]), 3), Color.yellow);
		i++;
	}

		

		//float[] spec = new float[128];
		//aso.GetOutputData(spec,1);
		//int i = 0;
		//while (i < spectrum.Length-1) {
		//	Debug.DrawLine(Vector3.zero,Vector3.zero*spec[i]);
		//	i++;
		//}

	}*/
}
