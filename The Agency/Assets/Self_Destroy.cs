using UnityEngine;
using System.Collections;

public class Self_Destroy : MonoBehaviour {

	public float timeTilDestroy = 0;

	public GlitchEffectArray gle;

	// Use this for initialization
	void Start () {

		gle = GameObject.FindGameObjectWithTag("GLITCH").GetComponent<GlitchEffectArray>();
		gle.AddPosition(transform.position);
	
	}
	
	// Update is called once per frame
	void Update () {

		timeTilDestroy -= Time.deltaTime;

		if(timeTilDestroy <= 0){
			print("DESTROY");
			gle.RemovePosition(transform.position);
			Destroy(gameObject);
		}
	
	}
}
