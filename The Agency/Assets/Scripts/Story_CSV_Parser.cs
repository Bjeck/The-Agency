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

	



	// Use this for initialization
	void Start () {

		TextAsset txtfile = (TextAsset)Resources.Load(textfileName);
		print(txtfile.text);
		Load(txtfile.text);

	}
	

	private bool Load(string fileName)
	{

		string line;
		line = "";


		StringReader theReader = new StringReader(fileName);

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
						ev = new TextEvent(e[1],int.Parse(e[5]),e[2],e[3],e[4]); // ("<"+e[2]+">")
						print(e[2]);
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

/*
using System.Text;
using System.IO;
using System.Linq;
​
​
public class CSVParser : MonoBehaviour {
​
	public string textfileName;
​
	List<string> instances = new List<string>();
	List<string> e = new List<string>(); 
	List<LeakEvent> eventsParsed = new List<LeakEvent>();
	public CrisisManager crisis;
​
	bool firstTime = true;
​
	// Use this for initialization
	void Start () {
		print("loading");
​
		TextAsset txtfile = (TextAsset)Resources.Load("csvsheet");
		print(" HELLO "+txtfile.text);
		Load(txtfile.text);
​
		crisis.LoadEventList(eventsParsed);
	}
	
	private bool Load(string fileText)
	{
		
		string line;
		line = "";
​
		StringReader theReader = new StringReader(fileText);
​
		using (theReader)
		{
			// While there's lines left in the text file, do this:
			int lineCounter = 0;
			while(line != null){
				instances.Clear();
				e.Clear();
				//print("reading line");
				line = theReader.ReadLine();
				//print(line+" "+firstTime);
				if(line != null && !firstTime){
					//print ("LINE: "+line);
					e = (line.Split(',').ToList());
​
					LeakEvent l = new LeakEvent();
​
					l.text = e[0];
					l.duration = float.Parse(e[2]);
					l.publicRelation.leakMitigation = float.Parse(e[3]);
					l.publicRelation.moleDiscovery = float.Parse(e[4]);
					l.it.leakMitigation = float.Parse(e[5]);
					l.it.moleDiscovery = float.Parse(e[6]);
					l.accounting.leakMitigation = float.Parse(e[7]);
					l.accounting.moleDiscovery = float.Parse(e[8]);
					l.humanResources.leakMitigation = float.Parse(e[9]);
					l.humanResources.moleDiscovery = float.Parse(e[10]);
​
					print("parsed "+l.text+" "+e.Count+" "+l.duration+" "+l.it.leakMitigation+" "+l.humanResources.leakMitigation);
					//l.hacker.leakMitigation
					//	ev = new TextEvent(e[1],int.Parse(e[2]),e[3],e[4]);
​
					eventsParsed.Add(l);
​
				}
​
					lineCounter++;
				if(firstTime){
					firstTime = false;
				}
			}
		}
		// Done reading, close the reader and return true to broadcast success    
		theReader.Close();
		return true;
​
	}
}
*/
