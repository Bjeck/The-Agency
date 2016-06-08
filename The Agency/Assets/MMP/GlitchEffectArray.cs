using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

public class GlitchPosition {

	/// <summary>
	/// This class is used to unify the position and scale of a glitch point.
	/// </summary>

	public Vector3 pos;
	public float scale;

	public GlitchPosition(Vector3 p, float s){
		pos = p;
		scale = s;
	}
}


public class GlitchEffectArray : MonoBehaviour {

	/// <summary>
	/// This is the main script that links the data from the audio processing to the Shader code. It directly controls the Shader values and how the visual effects look.
	/// </summary>

	public float glitchup, glitchdown, flicker;		//Variables for the glitching and audio.
	public float intensity;
	public float audioVolume;
	public float audioIntenScale;
	public float intenClamp;
	public Dictionary<GameObject,GlitchPosition> positions = new Dictionary<GameObject,GlitchPosition>();	//this dictionary has all the positions where the effect happens.
	public float[] scaleFreqModifiers = new float[10];

	float glitchupTime = 0.05f;
	float glitchdownTime = 0.05f;
	float flickerTime = 0.5f;
	Vector4 scaleRandomizer;
	float xm,ym,zm,wm;

	Material curMat;
	public Shader shader;
	public Camera cam;
	public Texture2D displacementMap;
	public RoomManager roomMan;

	Material material		//The material which has the glitch shader is set up
	{
		get
		{
			if(curMat == null)
			{
				curMat = new Material(shader);
				curMat.hideFlags = HideFlags.HideAndDontSave;
			}
			return curMat;
		}

	}

	void Start(){
		material.SetTexture("_DisplacementTex",displacementMap);
	}

	void Update(){
		intensity = audioVolume+audioIntenScale;			//The intensity of the effect is a direct consequence of the audio volume (set from AudioAnalyzer), but clamped to a set value.
		intensity = Mathf.Clamp(intensity,0,intenClamp);
	}


	void OnRenderImage(RenderTexture source, RenderTexture destination)		//This function calls every time the camera renders the image.
	{
		
		material.SetInt("_PositionsLength",positions.Count);		//The Shader arrays are set up. These have all the positions where the effect needs to happen and the scale of the effect at that position.
		for (int i = 0; i < positions.Count; i++) {
			material.SetVector("_Positions" + i.ToString(),positions.Values.ToList()[i].pos);
			material.SetVector("_Scales" + i.ToString(),new Vector2(positions.Values.ToList()[i].scale,0));
		}

		xm = scaleFreqModifiers[0]+scaleFreqModifiers[1];
		ym = scaleFreqModifiers[2]+scaleFreqModifiers[3];
		zm = scaleFreqModifiers[4]+scaleFreqModifiers[5]+scaleFreqModifiers[6];
		wm = scaleFreqModifiers[7]+scaleFreqModifiers[8]+scaleFreqModifiers[9];		//A scale randomization is added based on the frequencies, which has the effect that the spectrum can alter the shape and size of the effect.
		scaleRandomizer = new Vector4(Random.Range(-xm,xm),Random.Range(-ym,ym),
									  Random.Range(-zm,zm),Random.Range(-wm,wm));

		material.SetVector("scaleRandomizer",scaleRandomizer);		//more necessary values are set in the shader.
		material.SetFloat("_Intensity", intensity);


		//The next parts are different elements of the glitch shader, that each add to the glitch effect in their own way.

		flicker += Time.deltaTime * intensity;

		if(flicker > flickerTime){ //Color flicker
			material.SetFloat("filterRadius", Random.Range(-3f,3f) * intensity);
			flicker  = 0;
			flickerTime = Random.value;
		}


		glitchup += (Time.deltaTime * scaleFreqModifiers[0]+scaleFreqModifiers[1]+scaleFreqModifiers[2]+scaleFreqModifiers[3]+scaleFreqModifiers[4])/100f;
		glitchdown += (Time.deltaTime * scaleFreqModifiers[5]+scaleFreqModifiers[6]+scaleFreqModifiers[7]+scaleFreqModifiers[8]+scaleFreqModifiers[9])/100f;		//The time of this effect is adjusted based on the spectrum as well.

		if(glitchup > glitchupTime){
			if(Random.value < 0.1f * intensity)
				material.SetFloat("flip_up", Random.Range(0,1f) * intensity);
			else
				material.SetFloat("flip_up",0);

			glitchup = 0;
			glitchupTime = Random.value/10f;
		}

		if(glitchdown > glitchdownTime){
			if(Random.value < 0.1f * intensity)
				material.SetFloat("flip_down", 1-Random.Range(0, 1f) * intensity);
			else
				material.SetFloat("flip_down", 1);
			
			glitchdown = 0;
			glitchdownTime = Random.value/10f;
		}


		if(Random.value < 0.05 * intensity){
			material.SetFloat("displace", Random.value * intensity);
		}else
			material.SetFloat("displace", 0);
		
		Graphics.Blit(source,destination,material);

	}


	//These two functions are called from the Self_Destroy script, which adds a position to the positions dictionary.
	public void AddPosition(Vector3 poss,GameObject sourceID){	
		positions.Add(sourceID,new GlitchPosition(cam.WorldToScreenPoint(poss),0));	//the position is translated to screen space for the shader.
	}

	public void RemovePosition(GameObject sourceID){
		positions.Remove(sourceID);
	}

}
