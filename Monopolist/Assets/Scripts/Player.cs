using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
	private int idPlayer;
	private int money;
	[SerializeField]
	private int maxSteps;
	[SerializeField]
	private int currentSteps;
	private bool isBankrupt;
	private Vector3 destination;
	public float speed = 1f;
	private DBwork db;
	private List<Queue<int>> _list;


	private void Update()
	{	
		Debug.Log(transform.position);
		if (!transform.position.Equals(destination))
		{
			transform.position = Vector3.Lerp(transform.position, destination, Time.deltaTime);
		}
		if (idPlayer != 1) return;
		GameCanvas.money = money;
		GameCanvas.currentSteps = currentSteps;
		GameCanvas.maxSteps = maxSteps;
	}

	public void move(StreetPath path)
	{

		destination = path.end;
		/*int min = maxSteps;
		int number = -1;
		for(int i = 0; i < _list.Count; i++)
		{
			if (_list[i].Contains(path.GetIdStreetPath()))
			{
				int[] array = _list[i].ToArray();
				for (int j = 0; j < array.Length; j++)
				{
					if (array[j] == path.GetIdStreetPath() && j+1 < min)
					{
						min = j + 1;
						number = i;
					}
				}
			}
		}

		if (number != -1)
			while (_list[number].Count > 0)
			{
				destination = db.GetPathById(_list[number].Dequeue()).end;
			}
		else
		{
			while (_list[0].Count > 0)
			{
				destination = db.GetPathById(_list[0].Dequeue()).end;
			}
		}*/
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
		_list = new List<Queue<int>>();
	}

	public void NextStep()
	{
		maxSteps = Random.Range(2, 13);
		currentSteps = 0;

		db = Camera.main.GetComponent<DBwork>();
		Debug.Log(transform.position);
		FindAllTheWays(db.GetPathByCoordinates(transform.position),maxSteps);
	}

	private void FindAllTheWays(StreetPath path, int stepsMax)
	{
		List<Queue<int>> list = new List<Queue<int>>();
		Debug.Log(path.NeighborsId);
		foreach (int pathId in path.NeighborsId)
		{
			Debug.Log("Here");
			StreetPath pathes = db.GetPathById(pathId);
			if (pathes.isBridge)
			{
				list.AddRange(findWay(pathes, stepsMax - 1, true, GetCenter(path.start, path.end)));
			}
			else if (pathes.start.Equals(path.end))
			{
				list.AddRange(findWay(pathes, stepsMax - 1, false, Vector3.zero));
			}
		}
		_list = list;

	}

	private List<Queue<int>> findWay(StreetPath path, int stepsLeft, bool wasBridge, Vector3 centerBridge)
	{
		
		List<Queue<int>> shit = new List<Queue<int>>();
		if (stepsLeft > 0)
		{
			if (wasBridge)
			{
				foreach (int i in path.NeighborsId)
				{
					StreetPath pathes = db.GetPathById(i);
					if (pathes.isBridge && !centerBridge.Equals(GetCenter(pathes.start, pathes.end)))
					{
						shit.AddRange(findWay(pathes, stepsLeft - 1, true, GetCenter(path.start, path.end)));
						
						
					}
					else if (pathes.start.Equals(path.end))
					{
						shit.AddRange(findWay(pathes, stepsLeft - 1, false, Vector3.zero));
					}
				}
			}
			else
			{
				foreach (int i in path.NeighborsId)
				{
					StreetPath pathes = db.GetPathById(i);
					if (pathes.isBridge)
					{
						shit.AddRange(findWay(pathes, stepsLeft - 1,  true, GetCenter(path.start, path.end)));
						
						
					}
					else if (pathes.start.Equals(path.end))
					{
						shit.AddRange(findWay(pathes, stepsLeft - 1, false, Vector3.zero));
					}
				}

			}

			foreach (Queue<int> queue in shit)
			{
				queue.Enqueue(path.GetIdStreetPath());
			}
			return shit;
		}
		else
		{
			shit.Add(new Queue<int>());
			shit[0].Enqueue(path.GetIdStreetPath());
			return shit;
		}
	}
	
	Vector3 GetCenter(Vector3 start, Vector3 end)
	{
		Vector3 vec = new Vector3(start.x +((end.x - start.x)/2), start.y +((end.y - start.y)/2), start.z +((end.z - start.z)/2) );

		return vec;
	}
}
