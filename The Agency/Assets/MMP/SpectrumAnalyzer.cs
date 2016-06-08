using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Linq;


public class SpectrumAnalyzer : MonoBehaviour {

	/// <summary>
	/// This script analyzes the spectrum of the audio of the game. It does two things with this: It writes to a texture the direct spectrogram, and it collects the data into batches and sends these to the GlitchEffectArray script.
	/// </summary>


	public int fftSize = 1024;	//The spectrum of the audio is calculated with an FFT, which needs a sample size.
	public List<GameObject> boxes = new List<GameObject>();	//boxes were used for testing, and cannot be seen in the main game.

	float[] spectrum;	
	Texture2D texture;
	int x=0;
	List<List<float>> batches = new List<List<float>>();

	public GlitchEffectArray gle;

	void Start() {
		batches.Add(new List<float>()); //Batches are set up.
		batches.Add(new List<float>()); 
		batches.Add(new List<float>()); 
		batches.Add(new List<float>());  
		batches.Add(new List<float>());  
		batches.Add(new List<float>());  
		batches.Add(new List<float>());
		batches.Add(new List<float>());
		batches.Add(new List<float>());
		batches.Add(new List<float>());

		spectrum = new float[fftSize];

		texture = new Texture2D(fftSize, fftSize);								//Texture is set up.
		GetComponent<Renderer>().material.shader = Shader.Find("Unlit/Texture");
		GetComponent<Renderer>().material.mainTexture = texture;

		for(x=0; x<texture.width; x++){			// Sets the Texture to black
			for(int y=0; y<texture.height; y++){
				Color color = new Color(0, 0, 0);
				texture.SetPixel(x, y, color);
			}
		}
		texture.Apply();
		x=0;

	}

	void Update() {
		AudioListener.GetSpectrumData(spectrum, 0,FFTWindow.BlackmanHarris);		//Spectrum data is gathered with an FFT and written into the "spectrum" array.

		for (int i = 0; i < batches.Count; i++) {
			batches[i].Clear();	
		}

		batches[0].AddRange((SubArray(spectrum,0,2)).ToArray());			//The spectrum data is written into the batches. This is done "logarithmically" also, so fewer samples are written in the lower frequencies and more are written in the higher frequencies.
		batches[1].AddRange((SubArray(spectrum,2,2)).ToArray());			//This makes all batches matter, since if this wasn't done, the higher frequencies would be almost pointless, and the lower batches would dominate.
		batches[2].AddRange((SubArray(spectrum,4,4)).ToArray());
		batches[3].AddRange((SubArray(spectrum,8,8)).ToArray());
		batches[4].AddRange((SubArray(spectrum,16,16)).ToArray());			//The frequencies of each batch can be calculated with this formula:
		batches[5].AddRange((SubArray(spectrum,32,32)).ToArray());			// sampleNr * (22050/fftsize)
		batches[6].AddRange((SubArray(spectrum,64,64)).ToArray());
		batches[7].AddRange((SubArray(spectrum,128,128)).ToArray());	
		batches[8].AddRange((SubArray(spectrum,256,256)).ToArray());
		batches[9].AddRange((SubArray(spectrum,512,512)).ToArray());




		for (int i = 0; i < batches.Count; i++) {						//The batches are summed up and sent to the GlitchEffectArray.
			gle.scaleFreqModifiers[i] = sum(batches[i].ToArray());
		}

		x ++;
		for(int y=0; y<texture.height;y++){		//Texture is written here.
			float db = spectrum[y]*100;			//the spectrum values are written along the y axis of the texture, with higher values shown as more white than lower values.
			Color color = new Color(db, db, db);
			texture.SetPixel(x, y, color);
		}  
		texture.Apply();



	}


	public float sum(float[] nrs){		//Function used to calculate the sum of an array.
		float retVal = 0;
		for (int i = 0; i < nrs.Length; i++) {
			retVal += nrs[i];
		}
		return retVal;
	}

	public float[] SubArray(float[] data, int index, int length) {		//Function used to return a subarray of an array.
		float[] result = new float[length];
		System.Array.Copy(data, index, result, 0, length);
		return result;
	}


}
