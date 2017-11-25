using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameController : MonoBehaviour
{

	public Player CurrentPlayer;
	private DBwork _dBwork;

	void Start()
	{
		_dBwork = Camera.main.GetComponent<DBwork>();
	}
	public void nextStep()
	{
		//foreach (Player player in _dBwork.GetAllPlayers())
		//{
			//if(player.IdPlayer != 0)
			_dBwork.GetPlayerbyId(1).NextStep();
		//}
	}

	public void cheet()
	{
		
	}

	void checkPlayer()
	{
		
	}
	
}
