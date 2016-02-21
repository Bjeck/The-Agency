using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;

public class PersonDatabase : MonoBehaviour {

	public InputField ipf;
	public Image img;
	public Image bImg;
	public Text output;


	public List<Person> people = new List<Person>();

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}




	public void Search(){
		print (ipf.text);
		ipf.text = ipf.text.ToLower();
		foreach(Person p in people){
			if(ipf.text == p.pName.ToLower() || ipf.text == (p.ID).ToString()){
				ShowPerson(p);
				return;
			}
		}
		ShowNoPerson();
	}

	public void ShowPerson(Person p){
		//SHOW

		img.color = Color.white;
		bImg.color = Color.white;

		img.sprite = p.img;
		string put = "Name: "+p.pName+"\nID: "+p.ID+"\nJob: "+p.job;

		//relations
		put += "\nRelations: ";
		if(p.relations.Count > 0){
			foreach(Person r in p.relations){
				put += "\n"+r.pName;
			}
		}
		output.text = put;

	}

	public void ShowNoPerson(){
		output.text = "No Entry Found.";
		img.color = Color.black;
		bImg.color = Color.black;
	}


}
