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

	public TextEvent(string n, int t, string te, string r){
		type = EventType.Text;
		name = n;
		time = t;
		text = te;
		room = r;
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
			events.Add(e.name,e);
		}
		
		print(events.Count);

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

			//CREATE SYSTEM FOR STATE CHANGING

			//if(tick == 6){
			//	StartCoroutine(WaitForText());
			//	break;
			//}
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
		txtMan.AddToText(e, true);
	}

	public void DoAudioEvent(AudioEvent e){
		roomM.PlaySoundInRoom(e.sound);

	}



	public void IntroText(){
		
		//string s = "Welcome to your workstation, Agent. This is where you will be positioned.";
		introTexts.Add("<color=#"+txtMan.ColorToHex(Color.red)+"> "+"Welcome to your workstation, Agent. This is where you will be positioned.</color> "+"\n\n");
		introTexts.Add("You will be given a random suspect that you will follow for seven days.\n");
		introTexts.Add("During this time you must follow their every move, and judge them accordingly.");


		print (txtMan.ColorToHex(Color.black));
		
		StartCoroutine(IntroTextProgression());
	}

	IEnumerator IntroTextProgression(){
		for (int i = 0; i < introTexts.Count; i++) {
			while(txtMan.isRollingMaster){
				yield return new WaitForSeconds(1);
			}
			yield return new WaitForSeconds(1.5f);
			txtMan.AddToMaster(introTexts[i]);
		}
		while(txtMan.isRollingMaster){
			yield return new WaitForEndOfFrame();
		}
		yield return new WaitForSeconds(2f);
		txtMan.masterString = "";
		gm.ChangeState(GameState.Game);
	}




	void SetupGameEvents(){

	//	gameEvents.Add(new AgencyEvent(GameState.Evaluation,3));
	//THIS IS HOW I DO CUTS. YAY!

	}



}
