using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameCanvas : MonoBehaviour
{
	public GameObject playMenu;
	public GameObject pauseMenu;
	public Text stepsText;
	public Text moneyText;
	public static int currentSteps;
	public static int maxSteps;
	public static int money;
	public string newName;

	void Update()
	{
		moneyText.text = "Капитал: " + money;
		stepsText.text = "Сделано ходов: " + currentSteps + "/" + maxSteps;
	}

	public void OpenGameMenu()
	{
		playMenu.SetActive(false);
		pauseMenu.SetActive(true);
	}

	public void OpenStreetsList()
	{
		
	}

	public void OpenPlayersList()
	{
		
	}

	public void OpenBuildsList()
	{
		
	}

	public void SaveGame()
	{
		Camera.main.GetComponent<DBwork>().SaveGame();
	}

	public void SaveGameAsNew()
	{	
		//перенести на метод подтверждения, добавить InputFild
		Camera.main.GetComponent<DBwork>().SaveGameAsNewFile(newName);
	}

	public void OpenMainMenu()
	{
		
	}

	public void ClosePauseMenu()
	{
		playMenu.SetActive(true);
		pauseMenu.SetActive(false);
	}

	public void NextStep()
	{
		
	}
}
