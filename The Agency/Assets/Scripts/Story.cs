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
}

public class TextEvent : Event {
	public string text;
	public TextEvent(string n, int t, string te, string r){
		type = EventType.Text;
		name = n;
		time = t;
		text = te;
		room = r;
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





public class Story : MonoBehaviour {

	public TextManager txtMan;
	public RoomManager roomM;
	public Story_CSV_Parser csvP;
	public int tick = 0;
	public Dictionary<string,Event> events = new Dictionary<string, Event>();

	public AudioClip clip;
	public AudioSource srrc;

	public GameManager gm;


	// Use this for initialization
	void Start () {

		//Getting text from parser.
		foreach(Event e in csvP.eventsParsed){
			events.Add(e.name,e);
		}
		
		print(events.Count);


		//StartCoroutine(Tick());
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
			tick++;

			//CREATE SYSTEM FOR STATE CHANGING

			//if(tick == 6){
			//	StartCoroutine(WaitForText());
			//	break;
			//}
		}
	}

	public IEnumerator WaitForText(){

		while(txtMan.isRollingLiving || txtMan.isRollingKitchen || txtMan.isRollingBedroom || txtMan.isRollingBathroom){
			yield return new WaitForSeconds(0.2f);
		}

		gm.ChangeState(GameState.Choice);
		yield return new WaitForEndOfFrame();

	}



	public void DoTextEvent(TextEvent e){
		txtMan.AddToText(e, true);
	}

	public void DoAudioEvent(AudioEvent e){
		roomM.PlaySoundInRoom(e.sound);

	}


}
