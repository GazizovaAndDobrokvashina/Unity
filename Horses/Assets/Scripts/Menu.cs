using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using System;
using UnityEngine.Events;


public class Menu : MonoBehaviour {

	public void ChangeScene(string s){
		SceneManager.LoadScene (s);
	}
}
