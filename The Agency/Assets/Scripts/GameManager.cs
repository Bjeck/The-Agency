using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;

public enum GameState {Game, Choice}

public class GameManager : MonoBehaviour {



	public Canvas gameCanvas;
	public Canvas choiceCanvas;
	public GameObject why;
	public GameObject firstChoice;
	public InputField ipf;
	public Story story;

	public GameState state;


	public List<bool> answers = new List<bool>();
	public List<string> textAnswers = new List<string>();


	void Awake(){
	}

	// Use this for initialization
	void Start () {
		ChangeState(GameState.Game);

	}
	



	public void ChangeState(GameState s){
		state = s;
		if(s==GameState.Game){
			gameCanvas.gameObject.SetActive(true);
			choiceCanvas.gameObject.SetActive(false);
			story.StartTick();
		}
		else if(s==GameState.Choice){
			gameCanvas.gameObject.SetActive(false);
			choiceCanvas.gameObject.SetActive(true);
			firstChoice.SetActive(true);
			why.SetActive(false);
		}

	}



	public void Choose(bool answer){

		firstChoice.SetActive(false);

		if(answer){
			answers.Add(true);
			why.gameObject.SetActive(true);
		}
		else{
			answers.Add(false);

			ActivateWhy();
		}


	}

	public void ActivateWhy(){
		why.SetActive(true);
	}



	public void SubmitWhyAnswer(){
		textAnswers.Add(ipf.text);
		ipf.textComponent.text = "";

		ChangeState(GameState.Game);

	}




}
