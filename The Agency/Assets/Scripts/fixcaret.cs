using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class fixcaret : MonoBehaviour {

	Text text;

	// Use this for initialization
	void Start () {
		text = GetComponent<Text> ();
		text.rectTransform.pivot = new Vector3 (0.5f, 0.5f);
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}
}
