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
    //объект с кнопками самого главного меню
    public GameObject MainMenuObj;

    //объект с кнопками для создания новой игры
    public GameObject CreateNewGameObj;

    //объект с кнопками уже существующих игр
    public GameObject ContinueObj;

    //кнопка возврата
    public GameObject BackButton;

    //объект с кнопками настроек
    public GameObject SettingsMenu;

    //префаб кнопки для создания меню с сохраненными играми
    public Transform button;

    //поле для ввода нового названия игры
    public InputField InputFieldNameOfGame;

    //слайдер количества игроков
    public Slider sliderCountOfPlayers;

    //слайдер громкости звука
    public Slider sliderSoundVolume;

    //слайдер громкости музыки
    public Slider sliderMusicVolume;

    //вывод количества игроков
    public Text countOfPlayersText;

    //вывод нового названия игры
    public Text NameOfGameText;

    //вывод названия выбранного города
    public Text NameOfTown;

    //вьюха для отображения кнопок с сохранениями
    public ScrollRect scrollSavedGames;

    //вьюха для отображения доступных городов
    public ScrollRect scrollTowns;

    //количество игроков
    private int countOfPlayers;

    //максимальное количество игроков
    private int maxcountOfPlayers = 4;

    //минимальное количество игроков
    private int mincountOfPlayers = 1;

    //название игры
    private string newNameGame = "Monopolist.db";

    //стартовый капитал
    private int startMoney = 2000;

    //название города
    private string nameTownForNewGame;

    //онлайн или оффлайн игра
    private bool online = false;

    //инициализация главного меню
    private void Start()
    {
        //задаем значения для слайдера количества игроков
        sliderCountOfPlayers.minValue = mincountOfPlayers;
        sliderCountOfPlayers.maxValue = maxcountOfPlayers;

        //создаем кнопки сохранений и городов
        CreateButtonsSaves();
        CreateButtonTowns();
    }

    //выводим значения слайдеров на экран, если они активны
    private void Update()
    {
        if (!sliderCountOfPlayers.IsActive()) return;
        countOfPlayers = (int) sliderCountOfPlayers.value;
        countOfPlayersText.text = "Количество игроков: " + (int) sliderCountOfPlayers.value;
    }

    //создание кнопок сохранений
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

    //создание кнопок городов
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

    //открыть меню создания новой игры
    public void OpenMenuNewGame()
    {
        ChangeMenuObject(1);
    }

    //открыть меню загрузок
    public void OpenMenuLoadGame()
    {
        ChangeMenuObject(2);
    }

    //открыть меню настроек
    public void OpenSettings()
    {
        ChangeMenuObject(3);
    }

    //действия при клике кнопки сохранения
    private void onButtonClickLoadGame(string dbName)
    {
        SaveLoad.loadGame(dbName);
        SceneManager.LoadScene("Game");
    }

    //действия при клике кнопки города
    private void onButtonClickChoseTown(string nameTown)
    {
        NameOfTown.text = "Город: " + nameTown;
        nameTownForNewGame = nameTown;
    }

    //возврат в самое главное меню
    public void BackToMainMenu()
    {
        ChangeMenuObject(4);
    }

    //выйти из игры
    public void QuitGame()
    {
        Application.Quit();
    }

    //начать новую игру
    public void StartNewGame()
    {
        Camera.main.GetComponent<DBwork>()
            .CreateNewGame(countOfPlayers, startMoney, newNameGame, online, nameTownForNewGame, "");
        SceneManager.LoadScene("Game");
    }

    //изменить название игры
    public void ChangeNameOfGame()
    {
        if (InputFieldNameOfGame.text.Length != 0)
        {
            newNameGame = InputFieldNameOfGame.text;
        }

        NameOfGameText.text = "Название игры: " + newNameGame;
    }

    //переключение между объектами меню
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

    //действия при изменении галочек
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

    //определение активной галочки
    Toggle GetActive(ToggleGroup aGroup)
    {
        return aGroup.ActiveToggles().FirstOrDefault();
    }

    //действия при изменении типа игры
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

    //действия при изменении значения стартового капитала
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