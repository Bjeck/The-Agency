using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;


public class lineVolume : MonoBehaviour {

	LineRenderer line;
	List<float> volumeList = new List<float>();

	public int lineLength = 100;
	public float dist = 0.1f;
	public float visualScale = 1.3f;
	public float maxVal = 4f;

	float testnr = 0;

	public Image img;

	// Use this for initialization
	void Start () {

		line = GetComponent<LineRenderer>();
		line.SetVertexCount(lineLength);
		for (int i = 0; i < lineLength; i++) {
			volumeList.Add(0f);
		}
	}
	
	// Update is called once per frame
	void Update () {

		for(int i = 0; i < lineLength;i++){
			Vector3 pos = transform.position + new Vector3(+i*dist,volumeList[i]*visualScale,0.0f); //i offset by half the size of line
			line.SetPosition(i,pos);
		}

	}


	public void AddNewVolume(float f){

		if(f > maxVal){
			f = maxVal;
		}

		volumeList.Add(f);
		volumeList.RemoveAt(0);
		print("ADDED "+f+". NOW AT: "+volumeList.Count);

	}



}
