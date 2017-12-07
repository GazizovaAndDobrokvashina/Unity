using System.Collections;
using System.Collections.Generic;
using System.Security.Principal;
using UnityEngine;

public class GameController : MonoBehaviour
{

	public Player CurrentPlayer;
	private DBwork _dBwork;
	private int CountStepsInAllGame;
	private int salary = 1000;

	void Start()
	{
		_dBwork = Camera.main.GetComponent<DBwork>();
	}
	public void nextStep()
	{
		CountStepsInAllGame++;
		if (CountStepsInAllGame % 10 == 0)
			_dBwork.GetPlayerbyId(1).Money += salary;
		//foreach (Player player in _dBwork.GetAllPlayers())
		//{
			//if(player.IdPlayer != 0)
			_dBwork.GetPlayerbyId(1).NextStep();
		//}
	}

	public static void cathedPlayer()
	{
		//перевести плеера в суд, так как он пойман
		Debug.Log("попался");
	}

	void checkPlayer()
	{
		
	}
	
}
