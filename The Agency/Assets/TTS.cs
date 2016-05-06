using UnityEngine;
using System.Collections;
//using System.


public class TTS : MonoBehaviour {

	public string s = "Hello";

	// Use this for initialization
	void Start () {
		System.Diagnostics.Process.Start("speak");
//		System.Diagnostics.Process.Start(
	}
	
	// Update is called once per frame
	void Update () {
	
	}
}
