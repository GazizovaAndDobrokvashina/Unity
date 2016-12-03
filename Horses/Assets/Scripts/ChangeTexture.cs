using UnityEngine;
using System.Collections;

public class ChangeTexture : MonoBehaviour {

	public Material material_1;
	public Material material_2;
	public Renderer render;
	Shader shader;
	public SkinnedMeshRenderer ren;
	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		if (Input.GetKeyDown (KeyCode.E))
			ren.material = material_2;
			//render.material.SetTexture("Horse_D",texture_2);
		if(Input.GetKeyDown (KeyCode.R))
			ren.material = material_1;
			//render.material.SetTexture("Horse_D",texture_1);
	}
}
