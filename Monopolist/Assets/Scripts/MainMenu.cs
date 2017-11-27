using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine;
using System.Linq;
using UnityEngine.Experimental.UIElements;
using UnityEngine.SceneManagement;
using Button = UnityEngine.UI.Button;
using Slider = UnityEngine.UI.Slider;
using Toggle = UnityEngine.UI.Toggle;


public class MainMenu : MonoBehaviour
{
    public GameObject MainMenuObj;
    public GameObject CreateNewGameObj;
    public GameObject ContinueObj;
    public GameObject BackButton;
    public GameObject SettingsMenu;
    public Transform button;
    public InputField InputFieldNameOfGame;
    public Slider sliderCountOfPlayers;
    public Slider sliderSoundVolume;
    public Slider sliderMusicVolume;
    public Text countOfPlayersText;
    public Text NameOfGameText;
    public Text NameOfTown;
    public ScrollRect scrollSavedGames;
    public ScrollRect scrollTowns;
    private int countOfPlayers;
    private int maxcountOfPlayers = 4;
    private int mincountOfPlayers = 1;
    private string newNameGame = "Monopolist.db";
    private int startMoney =  2000;
    private string nameTownForNewGame;
    private bool online = false;


    private void Start()
    {
        sliderCountOfPlayers.minValue = mincountOfPlayers;
        sliderCountOfPlayers.maxValue = maxcountOfPlayers;

        CreateButtonsSaves();
        CreateButtonTowns();
    }

    private void Update()
    {
        if (!sliderCountOfPlayers.IsActive()) return;
        countOfPlayers = (int) sliderCountOfPlayers.value;
        countOfPlayersText.text = "Количество игроков: " + (int) sliderCountOfPlayers.value;
    }

    private void CreateButtonsSaves()
    {
        List<string> namesSavedGames = SaveLoad.loadGamesList("SavedGames");
        foreach (string dbName in namesSavedGames)
        {
            Transform but = Instantiate(button) as Transform;
            but.SetParent(scrollSavedGames.content, false);
            RectTransform tr = but.GetComponent<RectTransform>();
            but.GetComponentInChildren<Text>().text = dbName;
            Button b = but.GetComponent<Button>();
            b.onClick.AddListener(() => onButtonClickLoadGame(dbName));
        }
    }

    private void CreateButtonTowns()
    {
        List<string> townsList = SaveLoad.loadGamesList("StreamingAssets");
        foreach (string nameTown in townsList)
        {
            Transform but = Instantiate(button) as Transform;
            but.SetParent(scrollTowns.content, false);
            RectTransform tr = but.GetComponent<RectTransform>();
            but.GetComponentInChildren<Text>().text = nameTown;
            Button b = but.GetComponent<Button>();
            b.onClick.AddListener(() => onButtonClickChoseTown(nameTown));
        }
    }

    public void OpenMenuNewGame()
    {
        ChangeMenuObject(1);
    }

    public void OpenMenuLoadGame()
    {
        ChangeMenuObject(2);
    }

    public void OpenSettings()
    {
        ChangeMenuObject(3);
    }

    private void onButtonClickLoadGame(string dbName)
    {
        SaveLoad.loadGame(dbName);
        SceneManager.LoadScene("Game");
    }

    private void onButtonClickChoseTown(string nameTown)
    {
        NameOfTown.text = "Город: " + nameTown;
        nameTownForNewGame = nameTown;
    }


    public void BackToMainMenu()
    {
        ChangeMenuObject(4);
    }

    public void QuitGame()
    {
        Application.Quit();
    }

    public void StartNewGame()
    {
        // Debug.Log(countOfPlayers + ", " + startMoney + ", " + newNameGame + ", " + online + ", " + nameTownForNewGame);
        Camera.main.GetComponent<DBwork>()
            .CreateNewGame(countOfPlayers, startMoney, newNameGame, online, nameTownForNewGame);
        SceneManager.LoadScene("Game");
    }

    public void ChangeNameOfGame()
    {
        if (InputFieldNameOfGame.text.Length != 0)
        {
            newNameGame = InputFieldNameOfGame.text;
        }

        NameOfGameText.text = "Название игры: " + newNameGame;
    }

    private void ChangeMenuObject(int state)
    {
        switch (state)
        {
            //открыть меню настройки новой игры
            case 1:
                MainMenuObj.SetActive(false);
                ContinueObj.SetActive(false);
                BackButton.SetActive(true);
                CreateNewGameObj.SetActive(true);
                SettingsMenu.SetActive(false);
                break;
            //открыть меню загрузки игры
            case 2:
                MainMenuObj.SetActive(false);
                ContinueObj.SetActive(true);
                BackButton.SetActive(true);
                CreateNewGameObj.SetActive(false);
                SettingsMenu.SetActive(false);
                break;
            //открыть настройки        
            case 3:
                MainMenuObj.SetActive(false);
                ContinueObj.SetActive(false);
                BackButton.SetActive(true);
                CreateNewGameObj.SetActive(false);
                SettingsMenu.SetActive(true);
                break;
            //Вернуться в глввное меню
            case 4:
                MainMenuObj.SetActive(true);
                ContinueObj.SetActive(false);
                BackButton.SetActive(false);
                CreateNewGameObj.SetActive(false);
                SettingsMenu.SetActive(false);
                break;
            default:
            {
                MainMenuObj.SetActive(true);
                ContinueObj.SetActive(false);
                BackButton.SetActive(false);
                CreateNewGameObj.SetActive(false);
                SettingsMenu.SetActive(false);
                break;
            }
        }
    }

    public void ChangeToggles(ToggleGroup group)
    {
        Toggle active = GetActive(group);

        switch (group.name)
        {
            case "ToggleGroupTypeOfGame":
                ChangeTypeOfGame(active);
                break;
            case "ToggleGroupMoney":
                ChangeStartMoney(active);
                break;
        }
    }

    Toggle GetActive(ToggleGroup aGroup)
    {
        return aGroup.ActiveToggles().FirstOrDefault();
    }


    private void ChangeTypeOfGame(Toggle active)
    {
        switch (active.name)
        {
            case "Offlane":
                online = false;
                break;
            case "Online":
                online = true;
                break;
        }
    }

    private void ChangeStartMoney(Toggle active)
    {
        switch (active.name)
        {
            case "FirstMoney":
                startMoney = 2000;
                break;
            case "SecondMoney":
                startMoney = 2500;
                break;
            case "ThirdMoney":
                startMoney = 3500;
                break;
            case "FourthMoney":
                startMoney = 4000;
                break;
        }
    }
}