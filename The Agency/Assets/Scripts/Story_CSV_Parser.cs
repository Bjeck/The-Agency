using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Text;
using System.IO;  
using System.Linq;
//using System;



public class Story_CSV_Parser : MonoBehaviour {

	public string textfileName;
	
	List<string> instances = new List<string>();
	List<string> e = new List<string>(); 
	public List<Event> eventsParsed = new List<Event>();
	
	bool firstTime = true;

	
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
		//print("WHAT");
		Load (textfileName);
	}
	

	private bool Load(string fileName)
	{
		//print("LOADING");
		// Handle any problems that might arise when reading the text
		
		string line;
		line = "";
		// Create a new StreamReader, tell it which file to read and what encoding the file
		// was saved as
		StreamReader theReader = new StreamReader(fileName, Encoding.Default);
		// Immediately clean up the reader after this block of code is done.
		// You generally use the "using" statement for potentially memory-intensive objects
		// instead of relying on garbage collection.
		// (Do not confuse this with the using directive for namespace at the 
		// beginning of a class!)
		using (theReader)
		{
			// While there's lines left in the text file, do this:
			int lineCounter = 0;
			while(line != null){
				instances.Clear();
				e.Clear();
				//print("reading line");
				line = theReader.ReadLine();
				
				if(line != null && !firstTime && line.Substring(0,4) != "SKIP"){
					//print ("LINE: "+line);
					e = (line.Split(';').ToList()); //U+03B1 THAT SUCKS

					//foreach(string s in e){
					//	print (s);
					//}

					Event ev = null;
					//print(e[0]);
					if(e[0] == "TEXT"){
						//CREATE TEXT EVENT
						ev = new TextEvent(e[1],int.Parse(e[5]),(personSus[e[4]]+": "+e[2]),e[3]); // ("<"+e[2]+">")
					}
					else if(e[0] == "AUDIO"){
						ev = new AudioEvent(e[1],int.Parse(e[5]),e[2],e[3]);
					}

					//print("parsed "+ev.name);

					if(ev != null){

						switch(e[3]){
						case "L":
							ev.room = "Living Room";
							break;
						case "K":
							ev.room = "Kitchen";
							break;
						case "B":
							ev.room = "Bedroom";
							break;
						case "T":
							ev.room = "Bathroom";
							break;
						}

						eventsParsed.Add(ev);
						//print("parsed room "+ev.name);
					}

					//print ("EVENT PARSED: "+eventsParsed[lineCounter].name+" "+eventsParsed[lineCounter].time+" "+eventsParsed[lineCounter].room);
					lineCounter++;
				}
				if(firstTime){
					firstTime = false;
				}
			}
			// Done reading, close the reader and return true to broadcast success    
			theReader.Close();
			return true;
		}
		
	}



}
