using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class textRoll : MonoBehaviour {

	Text text;

	public float startDel = 0;
	bool hasStartedRolling = false;
	public bool shouldStopRolling = false;

	public float del;
	bool isColored = false;

	public string textToDisplay;
	// Use this for initialization
	public void Start () {
		text = GetComponent<Text> ();
		textToDisplay = textToDisplay.Replace("¤", System.Environment.NewLine);

		if (startDel == 0) {
			hasStartedRolling = true;
			StartCoroutine (RollText ());
		}
	}
	
	// Update is called once per frame
	void Update () {
		if (hasStartedRolling) {
			return;
		}

		if (startDel <= 0) {
			hasStartedRolling = true;
			StartCoroutine (RollText ());
		} else {
			startDel -= Time.deltaTime;
		}
	}

	public IEnumerator RollText(){
		int i = 0;
		while (i< textToDisplay.Length) {
		//	if(shouldStopRolling){
		//		Debug.Log("STOP");
		//		return true;
		//	}
			if(textToDisplay[i] == '<'){
				text.text += "<color=#1f1f1fff>"+"</color>";
				isColored = true;
			}
			else if(isColored){
				text.text = text.text.Substring(0,text.text.Length-8);
				if(textToDisplay[i] == '>'){
					isColored = false;
					text.text += "</color>";
				}
				else{
					text.text += textToDisplay[i]+"</color>";
				}
			}
			else{

				text.text += textToDisplay[i];
			}
			if(textToDisplay[i] != ' '){ //SOUNDS
				//SoundManager.instance.PlayTextSound();
			}

			i++;
			yield return new WaitForSeconds(del);
		}

	}


}
