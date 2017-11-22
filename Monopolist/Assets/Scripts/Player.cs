using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
	private int idPlayer;
	private int money;
	private int maxSteps;
	private int currentSteps;
	private bool isBankrupt;
	private Vector3 destination;
	public float speed;
	

	private void Update()
	{
		if (transform.position != destination) {
			transform.position = Vector3.Lerp(transform.position, destination, speed);
		}
	}

	public void move(StreetPath path)
	{
		destination =  new Vector3(path.end.x, path.end.y);
		
	}


	public Player(int idPlayer, int money, bool isBankrupt, Vector3 destination)
	{
		this.idPlayer = idPlayer;
		this.money = money;
		this.isBankrupt = isBankrupt;
		this.destination = destination;
	}
}
