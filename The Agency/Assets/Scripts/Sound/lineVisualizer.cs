using UnityEngine;
using System.Collections;

[RequireComponent(typeof(LineRenderer))]
public class lineVisualizer : MonoBehaviour {

	public int sampleNr = 128;
	float[] samples;

	public float size = 10f;
	public float amplitude = 1f;
	public int cutOffSample;
	float stepSize;


	LineRenderer line;

	void Start(){
		samples = new float[sampleNr];
		cutOffSample = sampleNr/2;

		line = GetComponent<LineRenderer>();
		line.SetVertexCount(samples.Length);
		stepSize = size/cutOffSample;
	}

	// Update is called once per frame
	void Update () {
		//channel: 0 = LEFT, 1 = RIGHT
		AudioListener.GetSpectrumData(samples,0,FFTWindow.BlackmanHarris);

		for(int i = 0; i < cutOffSample;i++){
			Vector3 pos = new Vector3(i*stepSize - size/2f, (1/Mathf.Log10 (samples[i]))*amplitude,0.0f); //i offset by half the size of line
			line.SetPosition(i,pos);
		}



	
	}
}
