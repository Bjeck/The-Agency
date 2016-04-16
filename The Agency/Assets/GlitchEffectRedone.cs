using UnityEngine;
using System.Collections;

public class GlitchEffectRedone : MonoBehaviour {
	public Texture2D displacementMap;
	public float glitchup, glitchdown, flicker;
	float glitchupTime = 0.05f;
	float glitchdownTime = 0.05f;
	float flickerTime = 0.5f;
	public float intensity;
	public float scaleIntensity;

	[Range(0,2000)]
	public float screenHeightLow = 0;
	[Range(0,2000)]
	public float screenHeightHigh = 0;
	[Range(0,2000)]
	public float screenWidthLeft = 0;
	[Range(0,2000)]
	public float screenWidthRight = 0;

	float timeToChangePos = 1f;

	public Transform bla;


	public float audioVolume;
	public float audioIntenScale;
	public float intenClamp;

	Vector3 oPos = new Vector3();
	Vector3 pos = new Vector3();


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
		scaleIntensity = Mathf.Clamp(intensity,1,intenClamp);

		//if(Input.GetMouseButton(0)){
			//oPos = Input.mousePosition;
			oPos = cam.WorldToScreenPoint(bla.position);

		//}
	}


	void OnRenderImage(RenderTexture source, RenderTexture destination)
	{
		pos = new Vector3(Random.Range(oPos.x-intensity,oPos.x+intensity),Random.Range(oPos.y-intensity,oPos.y+intensity),oPos.z);

		screenHeightHigh = pos.y + ((Random.Range(5f,20f))*scaleIntensity);
		screenHeightLow = pos.y - ((Random.Range(5f,20f))*scaleIntensity);
		screenWidthLeft = pos.x - ((Random.Range(5f,20f))*scaleIntensity);
		screenWidthRight = pos.x + ((Random.Range(5f,20f))*scaleIntensity);


	//	if(timeToChangePos > 0){
	//		timeToChangePos -= Time.deltaTime;
	//	}
	//	else{
	//		
			//screenHeightHigh = Random.Range(0,Camera.main.pixelHeight);
			//screenHeightLow = Random.Range(0,screenHeightHigh);
			//screenWidthRight = Random.Range(0,Camera.main.pixelWidth);
			//screenWidthLeft = Random.Range(0,screenWidthRight);

			material.SetFloat("_ScreenHeightLow",screenHeightLow);
			material.SetFloat("_ScreenHeightHigh",screenHeightHigh);
			material.SetFloat("_ScreenWidthLeft",screenWidthLeft);
			material.SetFloat("_ScreenWidthRight",screenWidthRight);

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
