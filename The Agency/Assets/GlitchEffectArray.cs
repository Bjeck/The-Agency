using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

public class GlitchPosition {
	public Vector3 pos;
	public float scale;

	public GlitchPosition(Vector3 p, float s){
		pos = p;
		scale = s;
	}
}


public class GlitchEffectArray : MonoBehaviour {
	public Texture2D displacementMap;
	public float glitchup, glitchdown, flicker;
	float glitchupTime = 0.05f;
	float glitchdownTime = 0.05f;
	float flickerTime = 0.5f;
	public float intensity;

	float timeToChangePos = 1f;

	public float audioVolume;
	public float audioIntenScale;
	public float intenClamp;

	public Dictionary<GameObject,GlitchPosition> positions = new Dictionary<GameObject,GlitchPosition>();

	Vector4 scaleRandomizer;
	public float[] scaleFreqModifiers = new float[10];
	float xm,ym,zm,wm;

	Material curMat;
	public Shader shader;
	public Camera cam;

	public RoomManager roomMan;

	Material material
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
		intensity = audioVolume*audioIntenScale;
		intensity = Mathf.Clamp(intensity,0,intenClamp);
	}


	void OnRenderImage(RenderTexture source, RenderTexture destination)
	{
		//pos = new Vector3(Random.Range(oPos.x-intensity,oPos.x+intensity),Random.Range(oPos.y-intensity,oPos.y+intensity),oPos.z);
		material.SetInt("_PositionsLength",positions.Count);

		for (int i = 0; i < positions.Count; i++) {
			material.SetVector("_Positions" + i.ToString(),positions.Values.ToList()[i].pos);
			material.SetVector("_Scales" + i.ToString(),new Vector2(positions.Values.ToList()[i].scale,0));
		}

		xm = scaleFreqModifiers[0]+scaleFreqModifiers[1];
		ym = scaleFreqModifiers[2]+scaleFreqModifiers[3];
		zm = scaleFreqModifiers[4]+scaleFreqModifiers[5]+scaleFreqModifiers[6];
		wm = scaleFreqModifiers[7]+scaleFreqModifiers[8]+scaleFreqModifiers[9];

		scaleRandomizer = new Vector4(Random.Range(-xm,xm),Random.Range(-ym,ym),
									  Random.Range(-zm,zm),Random.Range(-wm,wm));

		material.SetVector("scaleRandomizer",scaleRandomizer);
		material.SetFloat("_Intensity", intensity);


		flicker += Time.deltaTime * intensity;

		if(flicker > flickerTime){
			material.SetFloat("filterRadius", Random.Range(-3f,3f) * intensity);
			flicker  = 0;
			flickerTime = Random.value;
		}

		glitchup += Time.deltaTime * scaleFreqModifiers[0]+scaleFreqModifiers[1]+scaleFreqModifiers[2]+scaleFreqModifiers[3]+scaleFreqModifiers[4];
		glitchdown += Time.deltaTime * scaleFreqModifiers[5]+scaleFreqModifiers[6]+scaleFreqModifiers[7]+scaleFreqModifiers[8]+scaleFreqModifiers[9];

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



	public void AddPosition(Vector3 poss,GameObject sourceID){ //Does NOTHING with the Audiosource at the moment. Later, it will use it to adjust scale of the individual positions.
		positions.Add(sourceID,new GlitchPosition(cam.WorldToScreenPoint(poss),0));
	}

	public void RemovePosition(GameObject sourceID){
		positions.Remove(sourceID);
		 
	}

}
