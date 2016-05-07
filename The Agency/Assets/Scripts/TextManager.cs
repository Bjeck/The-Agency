using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;


public class TextManager : MonoBehaviour {

	public Text txt;
	public Scrollbar scrb;

	public string masterString;
	public string toAddLiving;
	public string toAddKitchen;
	public string toAddBedroom;
	public string toAddBathroom;
	public string toAddMaster;

	public bool isRollingLiving = false;
	public bool isRollingKitchen = false;
	public bool isRollingBedroom = false;
	public bool isRollingBathroom = false;
	public bool isRollingMaster = false;

	public AudioSource currentTextSound;

	public AudioSource masterTextSound;
	public AudioSource alyvTextSound;
	public AudioSource tromaTextSound;

	public float timeTilDone = 0;

	public float del = 0.1f;

	public bool addText = false;
	public RoomManager roomM;


	public Dictionary<string,Color> personColors = new Dictionary<string, Color>()
	{
		{ "NA"	,Color.white },
		{ "Troma"	,new Color(0.8f,0.2f,0.2f) },
		{ "Alyv"	,new Color(0.8f,0.7f,0.2f) }
	};

	public Dictionary<string,string> personSus = new Dictionary<string, string>()
	{
		{ "NA"	, "" },
		{ "Troma"	, "SUSPECT" },
		{ "Alyv"	, "SPOUSE" }
	};


	// Use this for initialization
	void Start () {
		currentTextSound = masterTextSound;
	}
	
	// Update is called once per frame
	void Update () {

		//if(addText){
			//AddToText(toAddLiving, true);
			//addText = false;
		//}




		txt.text = masterString;
	
	}


	public void AddSpace(){
		if(roomM.roomIAmIn == "Living Room" && isRollingLiving){
			masterString += "/\n/";
		}
		else if(roomM.roomIAmIn == "Kitchen" && isRollingKitchen){
			masterString += "/\n/";
		}
		else if(roomM.roomIAmIn == "Bedroom" && isRollingBedroom){
			masterString += "/\n/";
		}
		else if(roomM.roomIAmIn == "Bathroom" && isRollingBathroom){
			masterString += "/\n/";
		}

	}

	public void AddToText(TextEvent e, bool addspace){

//		print ("ADDING TEXT "+e.text+" "+toAddBedroom);

		switch(e.room){
		case "Living Room":
			if(addspace){
				toAddLiving += "\n";
			}
			toAddLiving += personSus[e.person]+": "+e.text;
			if(!isRollingLiving){
				StartCoroutine(LivingRoomRoll(e));
			}
			break;
		case "Kitchen":
			if(addspace){
				toAddKitchen += "\n";
			}
			toAddKitchen += personSus[e.person]+": "+e.text;
			if(!isRollingKitchen){
				StartCoroutine(KitchenRoomRoll(e));
			}
			break;
		case "Bedroom":
			if(addspace){
				toAddBedroom += "\n";
			}
			toAddBedroom += personSus[e.person]+": "+e.text;
			if(!isRollingBedroom){
				StartCoroutine(BedroomRoomRoll(e));
			}
			break;
		case "Bathroom":
			if(addspace){
				toAddBathroom += "\n";
			}
			toAddBathroom += personSus[e.person]+": "+e.text;
			if(!isRollingBathroom){
				StartCoroutine(BathroomRoll(e));
			}
			break;
		}
	}

	public void AddToMaster(string s){
		toAddMaster += s;
		StartCoroutine(MasterRoll());
	}




	/*public IEnumerator RollText(){
		int i = 0;
		isRolling = true;
		while (i< toAdd.Length) {

			//if in right room)
			masterString += toAdd[i];


			txt.text = masterString;
			i++;
			scrb.value = 0;
			timeTilDone = ((toAdd.Length-i)*del);
			yield return new WaitForSeconds(del);

		}
		isRolling = false;
		toAdd = "";
	}*/


	public IEnumerator MasterRoll(){
		int i = 0;
		isRollingMaster = true;
		
		while(i < toAddMaster.Length){

			masterString += toAddMaster[i];

			if(toAddMaster[i] != ' '){
			//	masterTextSound.pitch = Random.Range(0.99f,1.01f);
				masterTextSound.Play();
			}



			i++;
			scrb.value = 0;
			timeTilDone = ((toAddMaster.Length-i)*del);


			yield return new WaitForSeconds(del);
		}
		toAddMaster = "";
		isRollingMaster = false;
	}


	public IEnumerator LivingRoomRoll(TextEvent e){
		int i = 0;
		isRollingLiving = true;

		while(i< toAddLiving.Length){

			if(roomM.roomIAmIn == "Living Room"){
			//	if(toAddLiving[i] == '¤'){
			//		masterString += ParsePerson(e);
			//	}


				masterString += toAddLiving[i];

				if(toAddLiving[i] != ' '){
				//	currentTextSound.pitch = Random.Range(0.99f,1.01f);
					currentTextSound.Play();
				}
			}

			i++;
			scrb.value = 0;
			timeTilDone = ((toAddLiving.Length-i)*del);

			yield return new WaitForSeconds(del);
		}
		isRollingLiving = false;
		toAddLiving = "";
	}

	public IEnumerator KitchenRoomRoll(TextEvent e){
		int i = 0;
		isRollingKitchen = true;
		while(i< toAddKitchen.Length){
			
			if(roomM.roomIAmIn == "Kitchen"){
				masterString += toAddKitchen[i];

				if(toAddKitchen[i] != ' '){
				//	currentTextSound.pitch = Random.Range(0.99f,1.01f);
					currentTextSound.Play();
				}
			}


			
			i++;
			scrb.value = 0;
			timeTilDone = ((toAddKitchen.Length-i)*del);
			
			yield return new WaitForSeconds(del);
		}
		isRollingKitchen = false;
		toAddKitchen = "";
	}

	public IEnumerator BedroomRoomRoll(TextEvent e){
		int i = 0;
		isRollingBedroom = true;
		//print ("STARTING BEDROOM ROLL");

		while(i< toAddBedroom.Length){
			
			if(roomM.roomIAmIn == "Bedroom"){
				//if(toAddBedroom[i] == '¤'){
				//	print("found Person Marker");
				//	masterString += ParsePerson(e);
				//}
				//ParsePerson(e);
				masterString += toAddBedroom[i];

				if(toAddBedroom[i] != ' '){
				//	currentTextSound.pitch = Random.Range(0.99f,1.01f);
					currentTextSound.Play();
				}
			}



			i++;
			scrb.value = 0;
			timeTilDone = ((toAddBedroom.Length-i)*del);
			
			yield return new WaitForSeconds(del);
		}
		isRollingBedroom = false;
		toAddBedroom = "";
	}

	public IEnumerator BathroomRoll(TextEvent e){
		int i = 0;
		isRollingBathroom = true;

		while(i< toAddBathroom.Length){
			
			if(roomM.roomIAmIn == "Bathroom"){
				masterString += toAddBathroom[i];

				if(toAddBathroom[i] != ' '){
				//	currentTextSound.pitch = Random.Range(0.99f,1.01f);
					currentTextSound.Play();
				}
			}



			i++;
			scrb.value = 0;
			timeTilDone = ((toAddBathroom.Length-i)*del);
			
			yield return new WaitForSeconds(del);
		}
		toAddBathroom = "";
		isRollingBathroom = false;
	}









	public string ColorToHex(Color32 color)
	{
		string hex = color.r.ToString("X2") + color.g.ToString("X2") + color.b.ToString("X2");
		return hex;
	}


	public string ParsePerson(TextEvent e){

		print("parsing");
		switch(e.person){
		case "Alyv":
			print("Alyv detected");
			currentTextSound = alyvTextSound;
			break;
		case "Troma":
			print("Troma detected");
			currentTextSound = tromaTextSound;
			break;
		default:
			print("NO ONE detected");
			currentTextSound = masterTextSound;
			break;
		};

		return personSus[e.person];
	}





}
