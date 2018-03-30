using System.Collections;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using UnityEditor;
using UnityEngine;

public class CoordinateBuilder : EditorWindow {

	
	public Transform parentOfPoints;
	public Transform parentOfBuilds;
	public string NameOfDB;

	[MenuItem("DataBase/GetCoordinates")]
	public static void ShowWindow()
	{
		GetWindow<CoordinateBuilder>(false, "Coordinates", true);
	}

	private void OnGUI()
	{
		NameOfDB = EditorGUILayout.TextField("Name Of DB", NameOfDB);
		
		parentOfPoints = (Transform)EditorGUILayout.ObjectField("Parent Object For Points", parentOfPoints , typeof(Transform));	
		parentOfBuilds = (Transform)EditorGUILayout.ObjectField("Parent Object For Builds", parentOfBuilds , typeof(Transform));

		if (GUILayout.Button("Get Coordinates"))
		{
			
			DataService service = new DataService(NameOfDB, @"Assets/Resources/");
			
			List<string> streets = new List<string>();
			List<StreetPaths> paths = new List<StreetPaths>();
			List<PathsForBuy> pathsForBuys = new List<PathsForBuy>();
			List<Builds> buildses = new List<Builds>();
			
			for (int i = 0; i < parentOfPoints.childCount; i++)
			{
				Transform child = parentOfPoints.GetChild(i);
				Regex regex = new Regex(@"(/?\w+_\d+_\w_\w_\d+_\d+/?)*");
				Match result =regex.Match(child.name);
				foreach (Group resultGroup in result.Groups)
				{
					
					Debug.Log(resultGroup.Captures.Count);	
					foreach (Capture resultGroupCapture in resultGroup.Captures)
					{
						Debug.Log(resultGroupCapture.Value);
						regex = new Regex(@"/*(?<name>\w+)_(?<number>\d+)_(?<IsBridge>\w)_(?<IsEnd>\w)_(?<renta>\d+)_(?<price>\d+)");
						Match m = regex.Match(resultGroupCapture.Value);
						if (!streets.Contains(m.Groups["name"].Value))
						{
							streets.Add(m.Groups["name"].Value);
						}
						string nameOfPath = m.Groups["name"].Value +" "+ m.Groups["number"].Value;
						int itWas = IfExist(paths, nameOfPath);
						if (itWas != -1)
						{
						
							Debug.Log(m.Groups["IsEnd"].Value+ "    " + m.Groups["IsEnd"].Value.Equals("н"));
							if (m.Groups["IsEnd"].Value.Equals("н"))
							{
								paths[itWas].StartX = child.position.x;
								paths[itWas].StartY = child.position.z;
							}
							else
							{
								paths[itWas].EndX = child.position.x;
								paths[itWas].EndY = child.position.z;
							}
						}
						else
						{
							Debug.Log(m.Groups["IsEnd"].Value+ "    " + m.Groups["IsEnd"].Value.Equals("н"));
							if (m.Groups["IsEnd"].Value.Equals("н"))
							{
								paths.Add(new StreetPaths
								{
									Renta = int.Parse(m.Groups["renta"].Value),
									NamePath = nameOfPath,
									IdStreetParent = streets.IndexOf(m.Groups["name"].Value)+1,
									StartX = child.position.x,
									StartY = child.position.z,
									IsBridge = m.Groups["IsBridge"].Value.Equals("м")
								});
							}
							else
							{
								paths.Add(new StreetPaths
								{								
									Renta = int.Parse(m.Groups["renta"].Value),
									NamePath = nameOfPath,
									IdStreetParent = streets.IndexOf(m.Groups["name"].Value)+1,
									EndX = child.position.x,
									EndY = child.position.z,
									IsBridge = m.Groups["IsBridge"].Value.Equals("м")
								});
							}
							itWas = paths.Count;

							if (int.Parse(m.Groups["price"].Value) != 0)
							{
								pathsForBuys.Add(new PathsForBuy{IdPathForBuy = itWas, IdPlayer = 0, PriceStreetPath = int.Parse(m.Groups["price"].Value)});
							}
						}
					}
					//Debug.Log(resultGroup.Captures.Count);					
					//Debug.Log(resultGroup.Value);
					
					
				}
			}
			
			
			Streets[] streetses = new Streets[streets.Count];
			for (int i = 0; i < streetses.Length; i++)
			{
				streetses[i] = new Streets {NameStreet = streets[i], AboutStreet = ""};
			}

			for (int i = 0; i < parentOfBuilds.childCount; i++)
			{
				Transform child = parentOfBuilds.GetChild(i);
				
				
				
				Regex regex = new Regex(@"(?<name>Дом на \w+ \d+.\d+)_(?<price>\d+)");
				Match result = regex.Match(child.name);
				
				buildses.Add(new Builds{
					NameBuild = result.Groups["name"].Value,
					AboutBuild = "Жилой дом",
					Enabled = false,
					IdStreetPath = GetPathForBuy(paths, result.Groups["name"].Value),
					PriceBuild = int.Parse(result.Groups["price"].Value),
					posX = child.position.x,
					posY = child.position.z
				});

			}

			StreetPaths[] path = paths.ToArray();
			PathsForBuy[] forBuy = pathsForBuys.ToArray();
			Builds[] builds = buildses.ToArray();
			
			service.FullTables(streetses,path, forBuy, builds);
		}

		if (GUILayout.Button("InsertEvents"))
		{
			
			DataService service = new DataService(NameOfDB, @"Assets/Resources/");
			
			Events[] eventses = new[]
                {
                    new Events
                    {
                        IdGovermentPath = 8,
                        Info = "Поздравляем! Вы выиграли в лотерею!",
                        NameEvent = "Лотерея",
                        Price = 100
                    }, //парк
                    new Events
                    {
                        IdGovermentPath = 8,
                        Info = "О нет! Вас обокрали!",
                        NameEvent = "Воришки!",
                        Price = -50
                    }, //парк
                    new Events
                    {
                        IdGovermentPath = 8,
                        Info = "Нужно закупить оборудование!",
                        NameEvent = "Субботник",
                        Price = -50
                    }, //парк
                    new Events
                    {
                        IdGovermentPath = 17,
                        Info = "Поздравляем! Вы выиграли в конкурсе!",
                        NameEvent = "Праздник в городе!",
                        Price = 70
                    }, //парк
                    new Events
                    {
                        IdGovermentPath = 17,
                        Info = "Вы посетили благотворительную ярмарку!",
                        NameEvent = "Благотворительная ярмарка!",
                        Price = -20
                    }, //парк
                    new Events
                    {
                        IdGovermentPath = 17,
                        Info = "Нужно закупить оборудование!",
                        NameEvent = "Субботник",
                        Price = -20
                    }, //парк
                    new Events
                    {
                        IdGovermentPath = 18,
                        Info = "Вы заключены под стражу!",
                        NameEvent = "Арест!",
                        Price = -100
                    }, //суд
                    new Events
                    {
                        IdGovermentPath = 18,
                        Info = "Ошибка в налоговой привела к увеличению вашего капитала",
                        NameEvent = "Ошибка в налоговой!",
                        Price = 50
                    }, //суд
                    new Events
                    {
                        IdGovermentPath = 18,
                        Info = "Ошибка в налоговой привела к уменьшению вашего капитала",
                        NameEvent = "Ошибка в налоговой!",
                        Price = -50
                    }, //суд
                    new Events
                    {
                        IdGovermentPath = 41,
                        Info = "Фортуна на вашей стороне! Вы выиграли крупную сумму денег!",
                        NameEvent = "Крупный Выигрыш!",
                        Price = 200
                    }, //казино
                    new Events
                    {
                        IdGovermentPath = 41,
                        Info = "Фортуна на вашей стороне! Вы выиграли небольшую сумму денег!",
                        NameEvent = "Выигрыш!",
                        Price = 50
                    }, //казино
                    new Events
                    {
                        IdGovermentPath = 41,
                        Info = "О нет! Вам не повезло и Вы проиграли большую сумму денег!",
                        NameEvent = "Проигрыш",
                        Price = -125
                    } //казино
                };
			
			service.FullEvents(eventses);
		}
	}

	private int IfExist(List<StreetPaths> paths, string name)
	{
		foreach (StreetPaths streetPath in paths)
		{
			if (streetPath.NamePath.Equals(name))
				return paths.IndexOf(streetPath);
		}

		return -1;
	}

	private int GetPathForBuy(List<StreetPaths> streetses, string name)
	{
		Regex regex = new Regex(@"Дом на (?<name>\w+) (?<number>\d+).\d+");
		Match m = regex.Match(name);
		string part = m.Groups["name"].Value;
		part = part.Substring(0, part.Length - 2);

		
		Regex find = new Regex(part + @"\w+ " + m.Groups["number"]);
		for (int i = 0; i < streetses.Count; i++)
		{
			if (find.IsMatch(streetses[i].NamePath))
				return i + 1;
		}

		return -1;
	}
	
	
}
