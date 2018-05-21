using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Test : Photon.MonoBehaviour {

	
	
	// Use this for initialization
	void Start ()
	{
		
		int id1 = PhotonNetwork.AllocateViewID();
		gameObject.AddComponent<PhotonView>().viewID = id1;
		PhotonView photonView =gameObject.GetComponent<PhotonView>();
		photonView.RPC("SendDBwork", PhotonTargets.Others);
		
		
	}
	
	[PunRPC]
	void SendDBwork()
	{
		Debug.Log("HEY ");
		PhotonView photonView = gameObject.GetComponent<PhotonView>();
		photonView.RPC("GetDBwork", PhotonTargets.All, 3);
	}

	[PunRPC]
	void GetDBwork(int n)
	{
		Debug.Log("Got " + n);
//        this.players = db.players;
//        this.streets = db.streets;
//        this.paths = db.paths;
	}
}
