using UnityEngine;
using System.Collections;

public class HorseMoving : MonoBehaviour {

	public float speed = 10f;
	public float turnSpeed = 10f;
	int count = 0;
	Animator anim;
	public bool isFlying = false;
	 public Transform tr;
	// Use this for initialization
	void Start () {
		anim = GetComponent<Animator> ();
	}
	
	// Update is called once per frame


	void Update () {


		if (Input.GetKey (KeyCode.W)) {
			tr.Translate (Vector3.forward * speed * Time.deltaTime);
			//tr.Translate (tr.forward * 0.2f);

		}
		if (Input.GetKey (KeyCode.D)) {
			//tr.Translate (tr.right * 0.2f);
			transform.Rotate(Vector3.up, turnSpeed * Time.deltaTime);
		}
		if (Input.GetKey (KeyCode.A)) {
			//tr.Translate (tr.right * (-0.2f));
			tr.Rotate(Vector3.up, -turnSpeed * Time.deltaTime);
		}
		if (Input.GetKey (KeyCode.S)) {
			tr.Translate (-Vector3.forward * speed * Time.deltaTime);
			//tr.Translate (tr.forward * (-0.2f));
		} 

		if (Input.GetKeyDown (KeyCode.Space) && !isFlying) {
			tr.Translate (tr.up);
			isFlying = true;
		} 
		if (tr.position.y < 0) {
			isFlying = false;

		}


	}
	void OnCollisionEnter(Collision coll) {
		isFlying = false;
	}
}
