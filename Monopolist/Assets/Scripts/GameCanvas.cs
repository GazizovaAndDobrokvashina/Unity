using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class GameCanvas : MonoBehaviour
{
	public GameObject playMenu;
	public GameObject pauseMenu;
	public GameObject returnButton;
	public Text stepsText;
	public Text moneyText;
	public InputField inputField;
	[SerializeField]
	public static int currentSteps;
	[SerializeField]
	public static int maxSteps;
	[SerializeField]
	public static int money;
	public string newName;

	void Update()
	{
		moneyText.text = "Капитал: " + money;
		stepsText.text = "Сделано ходов: " + currentSteps + "/" + maxSteps;
	}

	public void OpenGameMenu()
	{
		ChangeMenu(2);
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
		ChangeMenu(3);
		
	}

	public void SaveGameAsNewInputField()
	{
		if (inputField.text.Length != 0)
		{
		newName = inputField.text;
		Camera.main.GetComponent<DBwork>().SaveGameAsNewFile(newName);
			ChangeMenu(2);
		}
	}

	public void OpenMainMenu()
	{
		Destroy(Camera.main);
		SceneManager.LoadScene("MainMenu");
	}


	public void returnToGame()
	{
		ChangeMenu(1);
	}

	private void ChangeMenu(int status)
	{
		switch (status)
		{		
			//игровая канва
				case 1: 
					inputField.gameObject.SetActive(false);
					playMenu.SetActive(true);
					pauseMenu.SetActive(false);
					returnButton.SetActive(false);
					break;
					//меню паузы
				case 2: 
					inputField.gameObject.SetActive(false);
					playMenu.SetActive(false);
					pauseMenu.SetActive(true);
					returnButton.SetActive(false);
					break;
					//меню записи
				case 3: 
					inputField.gameObject.SetActive(true);
					playMenu.SetActive(false);
					pauseMenu.SetActive(false);
					returnButton.SetActive(true);
					break;
					//если че, игровая канва
				default:
			{
				inputField.gameObject.SetActive(false);
				playMenu.SetActive(true);
				pauseMenu.SetActive(false);
				returnButton.SetActive(false);
				break;
			}
		}
	}
}
