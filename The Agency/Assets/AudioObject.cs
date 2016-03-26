﻿using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

public class AudioObject : MonoBehaviour {
	AudioSource source = new AudioSource();

	public List<AudioClip> audios = new List<AudioClip>();


	// Use this for initialization
	void Start () {
		source = GetComponent<AudioSource>();
	}
	

	//PLAY AUDIO. Picks a random one of several audioclips on this audioobject, and plays it at the source.
	public void PlayAudio(){
		int r = Random.Range(0,audios.Count);
		source.clip = audios[r];
		source.Play();
	}


}