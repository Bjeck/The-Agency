using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using System.Linq;
using UnityEngine.Audio;

public enum EventType {Text, Audio, StateChange}

public class Event {
	public string name;
	public string room; //L, K, B, T
	public EventType type;
	public int time;
	//public Color color;
}

public class TextEvent : Event {
	public string text;
	public string person;

	public TextEvent(string n, int t, string te, string r, string p){
		type = EventType.Text;
		name = n;
		time = t;
		text = te;
		room = r;
		person = p;
	//	color = c;
	}
}

public class AudioEvent : Event {	
	public string sound;
	public AudioEvent(string n, int t, string au, string r){
		type = EventType.Audio;
		name = n;
		time = t;
		sound = au;
		room = r;
	}	
}


public struct AgencyEvent{
	public GameState stateToChangeTo;
	public int time;

	public AgencyEvent(GameState gs, int t){
		stateToChangeTo = gs;
		time = t;
	}

};



public class Story : MonoBehaviour {



	public TextManager txtMan;
	public RoomManager roomM;
	public Story_CSV_Parser csvP;
	public int tick = 0;
	public Dictionary<string,Event> events = new Dictionary<string, Event>();
	
	List<AgencyEvent> gameEvents = new List<AgencyEvent>();

	public GameManager gm;

	List<string> introTexts = new List<string>();


	// Use this for initialization
	void Start () {

		//Getting text from parser.

		foreach(Event e in csvP.eventsParsed){
			try{
				events.Add(e.name,e);
			}
			catch{
				Debug.LogError ("DID NOT ADD "+e.name);
			}
		}
		
		print("Beginning Story with "+events.Count+" events.");

		SetupGameEvents();

	}


	public void StartTick(){
		StartCoroutine(Tick());
		//roomM.ChangeRoom("Living Room");
	}

	//THE BIG TICK
	public IEnumerator Tick(){
		while(true){

			List<Event> eventsToTrigger = events.Values.ToList().FindAll(x=>x.time==tick);
			foreach(Event e in eventsToTrigger){
				print ("PLAYING EVENT: "+e.name);

				switch(e.type){
				case EventType.Text:    
					DoTextEvent(e as TextEvent);
	           		break;
	            case EventType.Audio:
					DoAudioEvent(e as AudioEvent);
	            	break;
	            }
			}

			yield return new WaitForSeconds(1);

			foreach(AgencyEvent ge in gameEvents){
				if(ge.time == tick){
					StartCoroutine(ChangeStateWaitForText());
					tick++;
					return false;
				}
			}


			tick++;
		}
	}

	public IEnumerator ChangeStateWaitForText(){

		while(txtMan.isRollingLiving || txtMan.isRollingKitchen || txtMan.isRollingBedroom || txtMan.isRollingBathroom){
			yield return new WaitForSeconds(0.2f);
		}

		yield return new WaitForSeconds(1.5f);

		gm.ChangeState(GameState.Evaluation);
		yield return new WaitForEndOfFrame();

	}



	public void DoTextEvent(TextEvent e){
		try{
			txtMan.AddToText(e, true);
		}
		catch{
			Debug.LogError("COULD NOT PLAY "+e.name);
		}
	}

	public void DoAudioEvent(AudioEvent e){
		try{
			roomM.PlaySoundInRoom(e);
		}
		catch{
			Debug.LogError("COULD NOT PLAY "+e.name);
		}

	}



	public void IntroText(){
		
		//string s = "Welcome to your workstation, Agent. This is where you will be positioned.";
		introTexts.Add("Welcome to The Interface. This is where you will be working."+"\n\n");
		introTexts.Add("As this is your first day, we will brief you on the basics. \n");
		introTexts.Add("However, as you have been accepted into this position, Agent, we assume you know both the gravity and severity of the task at hand. \n\n\n");
		introTexts.Add("Soon, you will be given a suspect, picked at random. Your job is to report any suspicious activity. You will monitor them all day in their home, for seven full days.\n\n");
		introTexts.Add("Anything that happens, you will be able to hear. Any outgoing and incoming messaging traffic you will be able to read.         \n\n");
		introTexts.Add("We record and process all data. It is cleaned so all linguistic artifacts, accent, vocal intonation, and gender has been removed.            \n");
		introTexts.Add("They are completely anonymous.\n");
		introTexts.Add("Thus, you will judge them based on their words alone.                           \n\n\nWe trust you will judge well.\n");




		print (txtMan.ColorToHex(Color.black));
		
		StartCoroutine(IntroTextProgression());
	}

	IEnumerator IntroTextProgression(){
		for (int i = 0; i < introTexts.Count; i++) {
			while(txtMan.isRollingMaster){
				yield return new WaitForSeconds(1);
			}
			if(gm.state != GameState.Agency){
				break;
			}
			yield return new WaitForSeconds(1.5f);
			txtMan.AddToMaster(introTexts[i]);
		}
		while(txtMan.isRollingMaster){
			yield return new WaitForEndOfFrame();
		}
		yield return new WaitForSeconds(2f);
		if(gm.state == GameState.Agency){
			txtMan.masterString = "";
			gm.ChangeState(GameState.Game);		
		}
		
	}




	void SetupGameEvents(){

	//	gameEvents.Add(new AgencyEvent(GameState.Evaluation,185));
	//THIS IS HOW I DO CUTS. YAY!

	}



}
