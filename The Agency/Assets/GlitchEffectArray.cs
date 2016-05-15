﻿using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class GlitchEffectArray : MonoBehaviour {
	public Texture2D displacementMap;
	public float glitchup, glitchdown, flicker;
	float glitchupTime = 0.05f;
	float glitchdownTime = 0.05f;
	float flickerTime = 0.5f;
	public float intensity;
	public float scaleIntensity;
	public float scaleModifier = 1f;
	public float scaleMin;
	public float scaleMax;

//	[Range(0,2000)]
	public Vector3 screenPoint;
//	[Range(0,100)]
	//public float PointScale = 0;
	[Range(0,100)]
	public float randomness = 0;

	float timeToChangePos = 1f;

	public Transform bla;


	public float audioVolume;
	public float audioIntenScale;
	public float intenClamp;

	Vector3 oPos = new Vector3();
	Vector3 pos = new Vector3();

	public List<Vector3> positions = new List<Vector3>();
	public List<float> scales = new List<float>();




	Material curMat;
	public Shader shader;

	public Camera cam;

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

	void Update(){
		intensity = audioVolume*audioIntenScale;
		intensity = Mathf.Clamp(intensity,0,intenClamp);
		scaleIntensity = intensity*scaleModifier;
		scaleIntensity = Mathf.Clamp(scaleIntensity,scaleMin,scaleMax);

		//if(Input.GetMouseButton(0)){
			//oPos = Input.mousePosition;
			oPos = cam.WorldToScreenPoint(bla.position);

		//}
	}


	void OnRenderImage(RenderTexture source, RenderTexture destination)
	{
		pos = new Vector3(Random.Range(oPos.x-intensity,oPos.x+intensity),Random.Range(oPos.y-intensity,oPos.y+intensity),oPos.z);

		//screenPoint = pos;

		//screenHeightHigh = pos.y + ((Random.Range(5f,20f))*scaleIntensity);
		//screenHeightLow = pos.y - ((Random.Range(5f,20f))*scaleIntensity);
		//screenWidthLeft = pos.x - ((Random.Range(5f,20f))*scaleIntensity);
		//screenWidthRight = pos.x + ((Random.Range(5f,20f))*scaleIntensity);

	//	PointScale = intensity*25f;


	//	if(timeToChangePos > 0){
	//		timeToChangePos -= Time.deltaTime;
	//	}
	//	else{
	//		
			//screenHeightHigh = Random.Range(0,Camera.main.pixelHeight);
			//screenHeightLow = Random.Range(0,screenHeightHigh);
			//screenWidthRight = Random.Range(0,Camera.main.pixelWidth);
			//screenWidthLeft = Random.Range(0,screenWidthRight);

		positions[0] = pos;
	//	scales[0]=PointScale;
	//	scales[1]=PointScale;

		//material.SetVector("_ScreenPoint",screenPoint);
	//	material.SetFloat("_PointScale",PointScale);
		material.SetInt("_PositionsLength",positions.Count);
		material.SetFloat("_Randomness",randomness);

	//	material.SetVector("_Positions" + 0,screenPoint);
	//	material.SetVector("_Positions" + 1,screenPoint+(Vector3.up*200f));

		for (int i = 0; i < 1; i++) {
			material.SetVector("_Positions" + i.ToString(),positions[i]);
			material.SetVector("_Scales" + i.ToString(),new Vector2(scaleIntensity,0)   );
		}


		//	material.SetFloat("_ScreenHeightHigh",screenHeightHigh);
		//	material.SetFloat("_ScreenWidthLeft",screenWidthLeft);
		//	material.SetFloat("_ScreenWidthRight",screenWidthRight);

	//		timeToChangePos = 1f;
	//	}


		material.SetFloat("_Intensity", intensity);
		material.SetTexture("_DisplacementTex",displacementMap);

		//material.SetFloat("filterRadius", Random.Range(-3f,3f) * intensity); //BLINKING
		//material.SetFloat("flip_up", glitchup);
		//material.SetFloat("flip_down", glitchdown);

		flicker += Time.deltaTime * intensity;

		if(flicker > flickerTime){
			material.SetFloat("filterRadius", Random.Range(-3f,3f) * intensity);
			flicker  = 0;
			flickerTime = Random.value;
		}


		glitchup += Time.deltaTime * intensity;
		glitchdown += Time.deltaTime * intensity;



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
			material.SetFloat("scale", 1-Random.value * intensity);
			//			GlitchManager.instance.PlayGlitchSound(0);
		}else
			material.SetFloat("displace", 0);







		Graphics.Blit(source,destination,material);

	}
}
