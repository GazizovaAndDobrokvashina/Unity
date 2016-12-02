using UnityEngine;
using System.Collections;

public class ChangeTexture : MonoBehaviour {

	public Texture texture_1;
	public Texture texture_2;
	public Renderer render;
	Shader shader;
	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		if(Input.GetKeyDown (KeyCode.E))
			render.material.SetTexture("Horse_D",texture_2);
		if(Input.GetKeyDown (KeyCode.R))
			render.material.SetTexture("Horse_D",texture_1);
	}
}
