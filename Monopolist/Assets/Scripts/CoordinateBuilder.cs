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
		parentOfBuilds = (Transform)EditorGUILayout.ObjectField("Parent Object For Builds", parentOfPoints , typeof(Transform));

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
				Regex regex = new Regex(@"()/()+");
				Match result =regex.Match(child.name);
				foreach (Group resultGroup in result.Groups)
				{
					regex = new Regex(@"(?<name>\w+)_(?<number>\d+)_(?<IsBridge>\w)_(?<IsEnd>\w)_(?<renta>\d+)");
					Match m = regex.Match(resultGroup.Value);
					if (!streets.Contains(m.Groups["name"].Value))
					{
						streets.Add(m.Groups["name"].Value);
					}
					// нужен иф, в котором проверяем есть ли такой кусок улицы и если есть, добавить координаты начала или конца (тоже иф на это)
					paths.Add(new StreetPaths{Renta = int.Parse(m.Groups["renta"].Value), NamePath = m.Groups["name"].Value + " " + m.Groups["nunber"].Value, IdStreetParent = 1, StartX = 5.63, StartY = -5.64, EndX = -1.57, EndY = -5.64, IsBridge = false});
				}
			}
		}
	}
	
	
}
