using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using System.Linq;
using UnityEngine.Audio;

public enum EventType {Text, Audio, StateChange}

public class Event {
	public string name;
	public EventType type;
	public int time;
}

public class TextEvent : Event {
	public string text;
	public TextEvent(string n, int t, string te){
		type = EventType.Text;
		name = n;
		time = t;
		text = te;
	}
}

public class AudioEvent : Event {	
	public string sound;
	public AudioEvent(string n, int t, string au){
		type = EventType.Audio;
		name = n;
		time = t;
		sound = au;
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


	// Use this for initialization
	void Start () {
		//TEST
		//TextEvent uh = new TextEvent("hello",2,"this is a text to tell you hello");
		//events.Add(uh.name,uh);

		//AudioEvent sink = new AudioEvent("sink",4,"BathroomSink");
		//events.Add(sink.name,sink);
		//print ( uh.time);

	//	
		//TEST OVER

		foreach(Event e in csvP.eventsParsed){
			events.Add(e.name,e);
		}


		StartCoroutine(Tick());
	}


	public IEnumerator Tick(){
		while(true){

			List<Event> eventsToTrigger = events.Values.ToList().FindAll(x=>x.time==tick);
			foreach(Event e in eventsToTrigger){
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
		}
	}


	public void DoTextEvent(TextEvent e){
		txtMan.AddToText(e.text, true);
	}

	public void DoAudioEvent(AudioEvent e){
		roomM.roomAudio[e.sound].Play();
	}


}
