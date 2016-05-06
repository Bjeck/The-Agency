using UnityEngine;
using System.Collections;

[RequireComponent(typeof(LineRenderer))]
public class volumeLine : MonoBehaviour {

	LineRenderer line;

	public int numSamples = 64;
	float[] volumeSamples;
	float volumenumber;

	[Range(0,100)]
	public float volumeScale;
	public float volumeRef = 0.1f;
	public float specScale = 20f;


	// Use this for initialization
	void Start () {
	
		volumeSamples = new float[numSamples];

		volumenumber = 0;


		line = GetComponent<LineRenderer>();
		line.SetVertexCount(100);

	}
	
	// Update is called once per frame
	void Update () {

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

		volumenumber = volumenumber*volumeScale;
		print(volumenumber);
		line.SetPosition(1,new Vector3(0f,volumenumber,0f));
	
	}
}
