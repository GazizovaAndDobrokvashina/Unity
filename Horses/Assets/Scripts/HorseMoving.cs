using UnityEngine;
using System.Collections;

public class HorseMoving : MonoBehaviour {

	float userSpeed = 10f;
	float speed = 10f;
	float turnSpeed = 50f;
	int count = 0;
	Animator anim;
	bool isFlying = false;
	public Transform tr;
	public Rigidbody rig;
	float valueMove = 0;
	float valueTurn = 0;
	private bool isAxisInUse = false;


	void Start () {
		anim = GetComponent<Animator> ();
		rig = GetComponent<Rigidbody> ();
	}

	void FixedUpdate() {

		if (Input.GetKeyDown (KeyCode.Space) && !isFlying) {
			rig.AddForce(new Vector3(0,7), ForceMode.Impulse);
			isFlying = true;
			anim.SetBool ("Jump",true);
		} 

	}

	void Update () {
		valueMove = Input.GetAxis("Vertical");
		valueTurn = Input.GetAxis("Horizontal");

		if(valueMove != 0 || valueTurn != 0) {
			anim.SetBool ("Move",true);
		} else{
			anim.SetBool ("Move",false);
		}

		if (Input.GetAxisRaw ("Vertical") != 0 ) {
			if (valueMove > 0) {

				if (isAxisInUse == false) {
					if (count == 0 || count == 1) {
						count++;
					}
					isAxisInUse = true;
				}
				ControlSpeed ();
				tr.Translate (Vector3.forward * speed * Time.deltaTime);
			} else {
				if (isAxisInUse == false) {
					if (count == 2) {
						count--;
						ControlSpeed ();
						tr.Translate (Vector3.forward * speed * Time.deltaTime);
					}
					if (count == 1) {
						count--;
						tr.Translate (Vector3.forward * speed * Time.deltaTime);
					}
					isAxisInUse = true;
				}

				ControlSpeed ();
				tr.Translate (-Vector3.forward * speed * Time.deltaTime);
			}
		} else {
			isAxisInUse = false;
			tr.Translate (-Vector3.zero);
		}


		if(valueTurn > 0) {
			transform.Rotate(Vector3.up, turnSpeed * Time.deltaTime);
		} else if(valueTurn < 0){
			tr.Rotate(Vector3.up, -turnSpeed * Time.deltaTime);
		} else {
			tr.Translate (-Vector3.zero);
		}


	}

	void OnCollisionEnter(Collision coll) {
		if (coll.gameObject.tag == "Zemlya") { 
			isFlying = false;
			anim.SetBool ("Jump", false);
		}
	}

	void ControlSpeed() {
		if (count == 1) {
			speed = 5f;
			anim.SetFloat ("vSpeed", speed);
		} else if (count == 2) {
			speed = userSpeed;
			anim.SetFloat ("vSpeed", speed);
		}
	}
}
