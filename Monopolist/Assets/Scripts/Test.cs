using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Test : MonoBehaviour {

	
	//скрипт первого кубика
	public Dice firstDice;

	//скрипт второго кубика
	public Dice secondDice;
	
	// Use this for initialization
	void Start ()
	{
		StartCoroutine(Dices());
	}
	
	private IEnumerator Dices()
	{
		//сбрасываем индекс первого кубика
		firstDice.resetIndex();
		//сбрасываем индекс второго кубика
		secondDice.resetIndex();
		//дожидаемся ответа от первого кубика
		yield return StartCoroutine(firstDice.WaitForAllSurfaces());
		//дожидаемся ответа от второго кубика
		yield return StartCoroutine(secondDice.WaitForAllSurfaces());

		if (firstDice.GetIndexOfSurface() > -1)
			Debug.Log("First Dice: " + firstDice.GetIndexOfSurface());

		if (secondDice.GetIndexOfSurface() > -1)
			Debug.Log("Second Dice: " + secondDice.GetIndexOfSurface());
	}
}
