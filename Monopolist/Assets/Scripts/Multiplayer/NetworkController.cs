using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class NetworkController : Photon.PunBehaviour
    {
//        public override void OnConnectedToMaster()
//        {
//            Debug.Log("DemoAnimator/Launcher: OnConnectedToMaster() was called by PUN");
//        }
// 
 
        public override void OnDisconnectedFromPhoton()
        {
            Debug.LogWarning("OnDisconnectedFromPhoton() was called by PUN");        
        }
	
        
        public void OnLeftRoom()
        {
            SceneManager.LoadScene("MainMenu");
        }
 
 
        public void LeaveRoom()
        {
            PhotonNetwork.LeaveRoom();
        }
        
    }
