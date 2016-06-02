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
	public string roomIAmIn;
	public TextManager txtM;

	public List<GameObject> objectLists = new List<GameObject>();
	public Dictionary<string, AudioObject> roomAudio = new Dictionary<string,AudioObject>();

	public List<Vector3> positionsSoundsArePlayingIn = new List<Vector3>();

	public GameObject soundIsPlayingPrefab;

	// Use this for initialization
	void Start () {
		rooms.Add("Living Room",micPoss[0]);
		rooms.Add("Kitchen",micPoss[1]);
		rooms.Add("Bedroom",micPoss[2]);
		rooms.Add("Bathroom",micPoss[3]);

		//SETUP AUDIO OBJECTS
		foreach(GameObject g in objectLists){
			foreach(Transform c in g.transform){
				roomAudio.Add(c.gameObject.name,c.gameObject.GetComponent<AudioObject>());
				foreach(Transform cc in c){
					roomAudio.Add(cc.gameObject.name,cc.gameObject.GetComponent<AudioObject>());
				}
			}
		}

		//ChangeRoom("Living Room");

		//foreach(AudioObject g in audioObjects){
			//roomAudio.Add(g.gameObject.name,g.GetComponent<AudioObject>());
		//}

	}

	//Takes care of changing sound mixing when changing rooms. SHOULD ALSO SEND TO room switching for text. Eventually.
	public void ChangeRoom(Button b){
		buttons.Find (x=>x==b).interactable = false;
		foreach(Button bu in buttons.FindAll(x=>x!=b)){
			bu.interactable = true;
			roomMixer.SetFloat(bu.GetComponentInChildren<Text>().text,600f);
		}

		roomIAmIn = buttons.Find (x=>x==b).GetComponentInChildren<Text>().text;

		Camera.main.transform.position = rooms[roomIAmIn].position;
		Camera.main.transform.rotation = rooms[roomIAmIn].rotation;

		roomMixer.SetFloat(buttons.Find (x=>x==b).GetComponentInChildren<Text>().text,22000);
		
		txtM.AddSpace();

	}


	public void ChangeRoom(string room){
		print (room);
		print (buttons[0].GetComponentInChildren<Text>().text);
		print (buttons[1].GetComponentInChildren<Text>().text);
		print (buttons[2].GetComponentInChildren<Text>().text);
		print (buttons[3].GetComponentInChildren<Text>().text);

		ChangeRoom(buttons.Find(x=>x.GetComponentInChildren<Text>().text==room));
	}



	public void PlaySoundInRoom(AudioEvent s){
		roomAudio[s.sound].PlayAudio(soundIsPlayingPrefab);
//		positionsSoundsArePlayingIn.Add(roomAudio[s.sound].gameObject.transform.position);
	}





}
