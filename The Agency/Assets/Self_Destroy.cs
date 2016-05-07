using UnityEngine;
using System.Collections;

public class Self_Destroy : MonoBehaviour {

	public float timeTilDestroy = 0;

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {

		timeTilDestroy -= Time.deltaTime;

		if(timeTilDestroy <= 0){
			Destroy(gameObject);
		}
	
	}
}
