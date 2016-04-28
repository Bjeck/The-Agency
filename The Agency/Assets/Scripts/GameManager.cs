using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;

public enum GameState {Agency, Game, Evaluation, End}

public class GameManager : MonoBehaviour {

	public GameState startState;

	public Canvas gameCanvas;
	public Canvas choiceCanvas;
	public GameObject ControlPanel;
	public GameObject SoundThing;
	public GameObject Buttons;
	public GameObject why;
	public GameObject firstChoice;
	public InputField ipf;
	public Story story;

	public GameState state;


	public List<bool> answers = new List<bool>();
	public List<string> textAnswers = new List<string>();


	void Awake(){
		Application.targetFrameRate = 60;
	}

	// Use this for initialization
	void Start () {
		ChangeState(startState);

	}
	



	public void ChangeState(GameState s){
		state = s;
		if(s==GameState.Game){
			gameCanvas.gameObject.SetActive(true);
			choiceCanvas.gameObject.SetActive(false);
			Buttons.SetActive(true);
			SoundThing.SetActive(true);
			ControlPanel.SetActive(true);
			story.StartTick();
		}
		else if(s==GameState.Evaluation){
			gameCanvas.gameObject.SetActive(false);
			choiceCanvas.gameObject.SetActive(true);
			firstChoice.SetActive(true);
			why.SetActive(false);
		}
		else if(s==GameState.Agency){
			Buttons.SetActive(false);
			SoundThing.SetActive(false);
			ControlPanel.SetActive(false);
			story.IntroText();
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
