using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;


public class TextManager : MonoBehaviour {

	public Text txt;
	public Scrollbar scrb;

	public string masterString;
	public string toAdd;
	public bool isRolling = false;
	public float timeTilDone = 0;

	public float del = 0.1f;

	public bool addText = false;


	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {

		if(addText){
			AddToText(toAdd, true);
			addText = false;
		}
	
	}




	public void AddToText(string s, bool addspace){
		if(isRolling){
			if(addspace){
				toAdd += "\n";
			}
			
			toAdd += s;
		}
		else{
			if(addspace){
				toAdd = "\n";
			}
			
			toAdd += s;
			StartCoroutine(RollText());
		}
	}





	public IEnumerator RollText(){
		int i = 0;
		isRolling = true;
		while (i< toAdd.Length) {

			masterString += toAdd[i];
			txt.text = masterString;
			i++;
			scrb.value = 0;
			timeTilDone = ((toAdd.Length-i)*del);
			yield return new WaitForSeconds(del);

		}
		isRolling = false;
		toAdd = "";
	}



}
