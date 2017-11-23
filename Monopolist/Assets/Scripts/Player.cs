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
	public float speed = 1f;
	

	private void Update()
	{
		if (!transform.position.Equals(destination)) {
			transform.position = Vector3.Lerp(transform.position, destination, speed);
		}
		GameCanvas.money = money;
		GameCanvas.currentSteps = currentSteps;
		GameCanvas.maxSteps = maxSteps;
	}

	public void move(StreetPath path)
	{
		destination =  path.end;
		
	}

	public Vector3 getDestination()
	{
		return destination;
	}

	public Player(int idPlayer, int money, bool isBankrupt, Vector3 destination)
	{
		this.idPlayer = idPlayer;
		this.money = money;
		this.isBankrupt = isBankrupt;
		this.destination = destination;
	}

	public Players GetPlayers()
	{
		Players players = new Players();
		players.IdPlayer = idPlayer;
		players.CoordinateX = destination.x;
		players.CoordinateY = destination.z;
		players.IsBankrupt = isBankrupt;
		players.Money = money;
		return players;
	}

	public int IdPlayer
	{
		get { return idPlayer; }
	}

	public int Money
	{
		get { return money; }
	}

	public int MaxSteps
	{
		get { return maxSteps; }
	}

	public int CurrentSteps
	{
		get { return currentSteps; }
	}

	public bool IsBankrupt
	{
		get { return isBankrupt; }
	}

	public Vector3 Destination
	{
		get { return destination; }
	}

	public float Speed
	{
		get { return speed; }
	}

	public void GetData(Player player)
	{
		this.currentSteps = player.CurrentSteps;
		this.destination = player.Destination;
		this.idPlayer = player.IdPlayer;
		this.isBankrupt = player.IsBankrupt;
		this.maxSteps = player.MaxSteps;
		this.money = player.Money;
		this.speed = player.Speed;
	}
}
