using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Linq;


public class SpectrumAnalyzer : MonoBehaviour {

	public int fftSize = 1024;
	float[] spectrum;
	Texture2D texture;
	int x=0;
	public float scrollSpeed = 0.01f;
	List<List<float>> batches = new List<List<float>>();

	public List<GameObject> boxes = new List<GameObject>();
	public GlitchEffectArray gle;

	void Start() {
		batches.Add(new List<float>()); 
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

		texture = new Texture2D(fftSize, fftSize);
		GetComponent<Renderer>().material.shader = Shader.Find("Unlit/Texture");
		GetComponent<Renderer>().material.mainTexture = texture;

		// Sets the Texture to black
		for(x=0; x<texture.width; x++){
			for(int y=0; y<texture.height; y++){
				Color color = new Color(0, 0, 0);
				texture.SetPixel(x, y, color);
			}
		}
		texture.Apply();
		x=0;

	}

	void Update() {
		AudioListener.GetSpectrumData(spectrum, 0,FFTWindow.BlackmanHarris);

		for (int i = 0; i < batches.Count; i++) {
			batches[i].Clear();	
		}

		batches[0].AddRange((SubArray(spectrum,0,2)).ToArray());
		batches[1].AddRange((SubArray(spectrum,2,2)).ToArray());
		batches[2].AddRange((SubArray(spectrum,4,4)).ToArray());
		batches[3].AddRange((SubArray(spectrum,8,8)).ToArray());
		batches[4].AddRange((SubArray(spectrum,16,16)).ToArray());
		batches[5].AddRange((SubArray(spectrum,32,32)).ToArray());
		batches[6].AddRange((SubArray(spectrum,64,64)).ToArray());
		batches[7].AddRange((SubArray(spectrum,128,128)).ToArray());
		batches[8].AddRange((SubArray(spectrum,256,256)).ToArray());
		batches[9].AddRange((SubArray(spectrum,512,512)).ToArray());

		x ++;
		for(int y=0; y<texture.height;y++){
			float db = spectrum[y]*100;
			Color color = new Color(db, db, db);
			texture.SetPixel(x, y, color);
		}  
		texture.Apply();

		for (int i = 0; i < batches.Count; i++) {
			gle.scaleFreqModifiers[i] = sum(batches[i].ToArray());
		}

	}


	public float sum(float[] nrs){

		float retVal = 0;
		for (int i = 0; i < nrs.Length; i++) {
			retVal += nrs[i];
		}
		return retVal;
	}

	public float[] SubArray(float[] data, int index, int length) {
		float[] result = new float[length];
		System.Array.Copy(data, index, result, 0, length);
		return result;
	}


}
