using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;


public class lineVolume : MonoBehaviour {

	/// <summary>
	/// This script renders the volume line that shows the current volume, calcuated from the AudioAnalyzer script.
	/// </summary>

	LineRenderer line;
	List<float> volumeList = new List<float>();		//The line is done with a Line Renderer, which is changed with this list of floats.

	public int lineLength = 100;
	public float dist = 0.1f;
	public float visualScale = 1.3f;
	public float maxVal = 4f;

	public SpectrumAnalyzer specAn;
	public Image img;

	void Start () {

		line = GetComponent<LineRenderer>();
		line.SetVertexCount(lineLength);
		for (int i = 0; i < lineLength; i++) {		//Line Renderer is set up, and given a length of vertices, which is the amount of "bends" the line can have.
			volumeList.Add(0f);
		}
	}

	void Update () {

		for(int i = 0; i < lineLength;i++){
			Vector3 pos = transform.position + new Vector3(i*dist,volumeList[i]*visualScale,0.0f); //The positions of the volume floats (which are all part of the line) are controlled by this forloop, where each float is continually set as the volume present at that point in the array.
			line.SetPosition(i,pos);
		}

	}


	public void AddNewVolume(float f){	//This function adds a new volume (called from AudioAnalyzer), which makes sure the line constantly updates.

		if(f > maxVal){
			f = maxVal;
		}

		volumeList.Add(f);		//the new volume is added to the end of the volume list, but the first element is deleted, which creates the "rolling" effect.
		volumeList.RemoveAt(0);
	}



}
