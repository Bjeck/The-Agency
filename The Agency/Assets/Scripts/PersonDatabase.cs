﻿using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;

public class PersonDatabase : MonoBehaviour {

	public InputField ipf;
	public Image img;
	public Image bImg;
	public Text output;


	public List<Person> people = new List<Person>();

	public GameObject peopleObj;

	// Use this for initialization
	void Start () {

		people.AddRange(peopleObj.transform.GetComponentsInChildren<Person>());
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}




	public void Search(){
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
		print("showing "+p.name);

		img.color = Color.white;
		bImg.color = Color.white;

		img.sprite = p.img;
		string put = "Name: "+p.pName+"\nID: "+p.ID+"\nJob: "+p.job+"\nEducation: "+p.education+"\nSSN: "+p.socialsecurityNr+"\nHobbies: "+p.hobbies;


		//recent transactions
		put += "\n\nRecent Credit Card Transactions: ";
		if(p.recentCreditCardTransactions.Count > 0){
			foreach(string s in p.recentCreditCardTransactions){
				put += "\n"+s;
			}
		}


		//relations
		put += "\n\nRelations: ";
		if(p.relations.Count > 0){
			foreach(Person r in p.relations){
				put += "\n"+r.name;
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
