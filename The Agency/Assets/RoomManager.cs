using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using System.Linq;
using UnityEngine.Audio;

public class RoomManager : MonoBehaviour {

	public List<Transform> micPoss = new List<Transform>();
	public Dictionary<string,Transform> rooms = new Dictionary<string, Transform>();
	public List<Button> buttons = new List<Button>();
	//public List<AudioMixerGroup> mixerlist = new List<AudioMixerGroup>();
	//public Dictionary<string,AudioMixerGroup> roommixers = new Dictionary<string, AudioMixer>();
	
	public AudioMixer roomMixer;

	public List<GameObject> objectLists = new List<GameObject>();
	public Dictionary<string, AudioObject> roomAudio = new Dictionary<string,AudioObject>();

	// Use this for initialization
	void Start () {
		rooms.Add("Living Room",micPoss[0]);
		rooms.Add("Kitchen",micPoss[1]);
		rooms.Add("Bedroom",micPoss[2]);
		rooms.Add("Bathroom",micPoss[3]);

		//SETUP AUDIO OBJECTS (SHOULD DO THIS AUTOMATICALLY / RIGHT NOW THEY HAVE TO BE SET IN EDITOR)

		foreach(GameObject g in objectLists){
			foreach(Transform c in g.transform){
				print (c.gameObject.name);
				roomAudio.Add(c.gameObject.name,c.gameObject.GetComponent<AudioObject>());
				foreach(Transform cc in c){
					print (cc.gameObject.name);
					roomAudio.Add(cc.gameObject.name,cc.gameObject.GetComponent<AudioObject>());
				}
			}
		}

		//foreach(AudioObject g in audioObjects){
			//roomAudio.Add(g.gameObject.name,g.GetComponent<AudioObject>());
		//}
	}
	
	// Update is called once per frame
	void Update () {
	
	}

	//Takes care of changing sound mixing when changing rooms. SHOULD ALSO SEND TO room switching for text. Eventually.
	public void ChangeRoom(Button b){
		buttons.Find (x=>x==b).interactable = false;
		foreach(Button bu in buttons.FindAll(x=>x!=b)){
			bu.interactable = true;

			roomMixer.SetFloat(bu.GetComponentInChildren<Text>().text,600f);
		}

		Camera.main.transform.position = rooms[buttons.Find (x=>x==b).GetComponentInChildren<Text>().text].position;
		Camera.main.transform.rotation = rooms[buttons.Find (x=>x==b).GetComponentInChildren<Text>().text].rotation;

		//AudioMixerGroup currMix = roommixers[buttons.Find (x=>x==b).GetComponentInChildren<Text>().text];
		roomMixer.SetFloat(buttons.Find (x=>x==b).GetComponentInChildren<Text>().text,22000);
	}



	public void PlaySoundInRoom(string s){
		roomAudio[s].PlayAudio();
	}





}
