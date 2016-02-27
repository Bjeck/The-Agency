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
		Load (textfileName);
	}
	

	private bool Load(string fileName)
	{
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
			while(line != null){
				instances.Clear();
				e.Clear();
				//print("reading line");
				line = theReader.ReadLine();
				
				if(line != null && !firstTime && line.Substring(0,4) != "SKIP"){
					
				//	print ("LINE: "+line);
					e = (line.Split('§').ToList());

					//foreach(string s in e){
					//	print (s);
					//}

					if(e[0] == "TEXT"){
						//CREATE TEXT EVENT
						TextEvent ev = new TextEvent(e[1],int.Parse(e[2]),e[3]);
						eventsParsed.Add (ev);
					}
					else if(e[0] == "AUDIO"){
							//CREATE AUDIO EVENT
							
					}

					print ("EVENT PARSED: "+eventsParsed[0].name+" "+eventsParsed[0].time);

					/*if(elements.Count > 3){ //arbitrary number
						DialogueInst momIntroGreet = new DialogueInst (); //need to figure out what to do with the class name.... shit. Back to lists? xD That would work.
						momIntroGreet.id = int.Parse(elements[0]);
						momIntroGreet.response = elements[1];
						momIntroGreet.thoughts = elements[2];
						momIntroGreet.thoughtsDelay = float.Parse(elements[3]);
						momIntroGreet.optionDelay = float.Parse(elements[4]);
						if(elements[6] != "NA"){	momIntroGreet.options.Add (elements[6]);	momIntroGreet.ResponseNrs.Add (int.Parse(elements[10]));	}
						if(elements[7] != "NA"){	momIntroGreet.options.Add (elements[7]);	momIntroGreet.ResponseNrs.Add (int.Parse(elements[11]));	}
						if(elements[8] != "NA"){	momIntroGreet.options.Add (elements[8]);	momIntroGreet.ResponseNrs.Add (int.Parse(elements[12]));	}
						if(elements[9] != "NA"){	momIntroGreet.options.Add (elements[9]);	momIntroGreet.ResponseNrs.Add (int.Parse(elements[13]));	}
						//Next Trigger
						if(elements[5] != "NNT"){
							string[] parseNT = elements[5].Split('/');
							if(parseNT[1] == "D"){
								momIntroGreet.NextTrigger (parseNT[0],true);
							}
							else{
								momIntroGreet.NextTrigger (parseNT[0],false);
							}
						}
						
						print ("TEST OUTPUT "+momIntroGreet.response+" "+momIntroGreet.thoughts+" "+momIntroGreet.thoughtsDelay);
					}*/
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
