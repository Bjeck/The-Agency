using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

public class AudioObject : MonoBehaviour {

	/// <summary>
	/// This script is a link between the RoomManager, that activates a Sound, and the sound itself. This takes care of choosing the actual sound, based on a series of (similar) Audio Sources on this script, and plays one of them.
	/// </summary>

	public List<AudioClip> audios = new List<AudioClip>();	//The list of possible AudioSources that will be played when this AudioObject is triggered.

	AudioSource source = new AudioSource();

	// Use this for initialization
	void Start () {
		source = GetComponent<AudioSource>();
	}
	

	//PLAY AUDIO. Picks a random one of several audioclips on this audioobject, and plays it at the source.
	public void PlayAudio(GameObject prefabToSpawn){
		int r = Random.Range(0,audios.Count);		//Pick a random of the sounds and play it.
		source.clip = audios[r];
		source.Play();

		//Add the Self_Destroy script, and set up the variables needed.
		GameObject g = (GameObject)Instantiate(prefabToSpawn,transform.position+(Vector3.up*2),Quaternion.identity);
		Self_Destroy sd = g.GetComponent<Self_Destroy>();
		sd.timeTilDestroy = audios[r].length;		//The object itself should only exist as long as the sound is playing.
		sd.source = source;						//Passing the audiosource so Self_Destroy knows what source to read from.	
	}


}
