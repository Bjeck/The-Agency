using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using System.Linq;
using UnityEngine.Audio;

public class RoomManager : MonoBehaviour {

	public Dictionary<string,Vector3> rooms = new Dictionary<string, Vector3>();
	public List<Button> buttons = new List<Button>();
	//public List<AudioMixerGroup> mixerlist = new List<AudioMixerGroup>();
	//public Dictionary<string,AudioMixerGroup> roommixers = new Dictionary<string, AudioMixer>();
	
	public AudioMixer roomMixer;

	public Dictionary<string, AudioSource> roomAudio = new Dictionary<string,AudioSource>();
	public List<GameObject> audioObjects = new List<GameObject>();

	// Use this for initialization
	void Start () {
		rooms.Add("Living Room",new Vector3(0,0,2));
		rooms.Add("Kitchen",new Vector3(8,0,2));
		rooms.Add("Bedroom",new Vector3(0,0,11));
		rooms.Add("Bathroom",new Vector3(9,0,14));

		//roommixers.Add("Living Room",mixerlist[0]);
		//roommixers.Add("Kitchen",mixerlist[1]);
		//roommixers.Add("Bedroom",mixerlist[2]);
		//roommixers.Add("Bathroom",mixerlist[3]);

		//SETUP AUDIO OBJECTS
		foreach(GameObject g in audioObjects){
			roomAudio.Add(g.name,g.GetComponent<AudioSource>());
		}
	}
	
	// Update is called once per frame
	void Update () {
	
	}


	public void ChangeRoom(Button b){
		buttons.Find (x=>x==b).interactable = false;
		foreach(Button bu in buttons.FindAll(x=>x!=b)){
			bu.interactable = true;

			roomMixer.SetFloat(bu.GetComponentInChildren<Text>().text,1000f);
		}

		Camera.main.transform.position = rooms[buttons.Find (x=>x==b).GetComponentInChildren<Text>().text];

		//AudioMixerGroup currMix = roommixers[buttons.Find (x=>x==b).GetComponentInChildren<Text>().text];
		roomMixer.SetFloat(buttons.Find (x=>x==b).GetComponentInChildren<Text>().text,22000);


	}





}
