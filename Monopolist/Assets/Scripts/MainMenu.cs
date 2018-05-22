using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine;
using System.Linq;
using ExitGames.Client.Photon;
using UnityEngine.Audio;
using UnityEngine.SceneManagement;
using Button = UnityEngine.UI.Button;
using Hashtable = ExitGames.Client.Photon.Hashtable;
using Slider = UnityEngine.UI.Slider;
using Toggle = UnityEngine.UI.Toggle;


public class MainMenu : MonoBehaviour


{
    public ScrollRect scrollActiveNetworkGames;

    public ScrollRect scrollSavedNetworkGames;

    public Text textTypeOfNewGame;

    public GameObject NetworkSettings;

    public GameObject SavedNetworkGames;

    public GameObject ActiveNetworkGames;

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
    public RectTransform buttonsSaves;

    //поле для ввода нового названия игры
    public InputField InputFieldNameOfGame;

    //поле для ввода имени игрока
    public InputField InputFieldPlayerName;

    //слайдер количества игроков
    public Slider sliderCountOfPlayers;

    //
    public Transform buttonTown;

    //слайдер громкости музыки
    public Slider sliderMusicVolume;

    //вывод количества игроков
    public Text countOfPlayersText;

    //вывод нового названия игры
    public Text NameOfGameText;

    //вывод названия выбранного города
    public Text NameOfTown;

    //поле для вывода имени игрока
    public Text NamePlayerText;

    //вьюха для отображения кнопок с сохранениями
    public ScrollRect scrollSavedGames;

    //вьюха для отображения доступных городов
    public ScrollRect scrollTowns;

    //количество игроков
    private int countOfPlayers;

    //максимальное количество игроков
    private int maxcountOfPlayers = 6;

    //минимальное количество игроков
    private int mincountOfPlayers = 1;

    //аудиомикшер звука
    public AudioMixer MainMenuMixer;

    //название игры
    private string newNameGame = "Monopolist.db";

    //стартовый капитал
    private int startMoney = 2000;

    //название города
    private string nameTownForNewGame;

    //онлайн или оффлайн игра
    private bool online = false;

    //имя игрока
    private string namePlayer = "Jonny";

    public int CountOfPlayers
    {
        get { return countOfPlayers; }
    }

    public string NewNameGame
    {
        get { return newNameGame; }
    }

    public int StartMoney
    {
        get { return startMoney; }
    }

    public string NameTownForNewGame
    {
        get { return nameTownForNewGame; }
    }

    public string NamePlayer
    {
        get { return namePlayer; }
    }

    //инициализация главного меню
    private void Start()
    {
        if (GameObject.FindGameObjectsWithTag("MainCamera").Length > 1)
        {
            Destroy(GameObject.FindGameObjectWithTag("MainCamera"));
        }

        //задаем значения для слайдера количества игроков
        sliderCountOfPlayers.minValue = mincountOfPlayers;
        sliderCountOfPlayers.maxValue = maxcountOfPlayers;

        //создаем кнопки сохранений и городов
        CreateButtonTowns();
        CreateButtonsSaves();
        CreateButtonsNetworkSaves();
    }

    //выводим значения слайдеров на экран, если они активны
    private void Update()
    {
        if (!sliderCountOfPlayers.IsActive()) return;
        countOfPlayers = (int) sliderCountOfPlayers.value;
        countOfPlayersText.text = "Количество игроков: " + (int) sliderCountOfPlayers.value;
    }

    public void openSavedNetworkGames()
    {
        ChangeMenuObject(7);
    }

    public void openActiveNetworkGames()
    {
        CreateButtonsNetworkActive();
        ChangeMenuObject(6);
    }

    public void openNetworkSettings()
    {
        ChangeMenuObject(5);
    }

    private void ClearScroll(ScrollRect scroll)
    {
        int countOfChild = scroll.content.childCount;
        if (countOfChild != 0)
            for (int i = countOfChild - 1; i >= 0; i--)
            {
                Destroy(scroll.content.GetChild(i));
            }
    }

    public void openNetworkStartNewGame()
    {
        textTypeOfNewGame.text = "Новая сетевая игра";
        online = true;
        ChangeMenuObject(1);
    }

    //изменение названия игры
    public void ChangeNameOfGPlyer()
    {
        if (InputFieldPlayerName.text.Length != 0)
        {
            namePlayer = InputFieldPlayerName.text;
        }

        NamePlayerText.text = "Ваше имя: " + namePlayer;
    }

    //создание кнопок сохранений
    private void CreateButtonsSaves()
    {
        List<string> namesSavedGames = SaveLoad.loadGamesList("SavedGames");

        foreach (string dbName in namesSavedGames)
        {
            var prefButtons = Instantiate(buttonsSaves);
            prefButtons.SetParent(scrollSavedGames.content, false);
            prefButtons.GetChild(0).GetComponent<Button>().GetComponentInChildren<Text>().text = dbName;
            Button b = prefButtons.GetChild(0).GetComponent<Button>();
            b.onClick.AddListener(() => onButtonClickLoadGame(dbName));

            prefButtons.GetChild(1).GetComponent<Button>().GetComponentInChildren<Text>().text = "X";
            b = prefButtons.GetChild(1).GetComponent<Button>();
            b.onClick.AddListener(() => DeleteGame(dbName, prefButtons.gameObject));
        }
    }

    //создание кнопок сохранений сетевых игр
    private void CreateButtonsNetworkSaves()
    {
        List<string> namesSavedGames = SaveLoad.loadGamesList("SavedGames/Network");

        foreach (string dbName in namesSavedGames)
        {
            var prefButtons = Instantiate(buttonsSaves);
            prefButtons.SetParent(scrollSavedNetworkGames.content, false);
            prefButtons.GetChild(0).GetComponent<Button>().GetComponentInChildren<Text>().text = dbName;
            Button b = prefButtons.GetChild(0).GetComponent<Button>();
            // b.onClick.AddListener(() => onButtonClickLoadGame(dbName));

            prefButtons.GetChild(1).GetComponent<Button>().GetComponentInChildren<Text>().text = "X";
            b = prefButtons.GetChild(1).GetComponent<Button>();
            b.onClick.AddListener(() => DeleteGame(dbName, prefButtons.gameObject));
        }
    }

    //создание кнопок активных сетевых игр
    private void CreateButtonsNetworkActive()
    {
        Lobby lobby = GetComponent<Lobby>();
        RoomInfo[] rooms = lobby.getRooms();
        //List<string> namesActiveGames = new List<string>();
        object nameOfGame = "";
        object nameOfTown = "";

        foreach (RoomInfo room in rooms)
        {
            room.CustomProperties.TryGetValue("ngame", out nameOfGame);
            room.CustomProperties.TryGetValue("ntown", out nameOfTown);

            string town = nameOfTown.ToString();

            var prefButtons = Instantiate(buttonsSaves);
            prefButtons.SetParent(scrollActiveNetworkGames.content, false);
            prefButtons.GetChild(0).GetComponent<Button>().GetComponentInChildren<Text>().text =
                (string) nameOfGame + " " + town;
            Button b = prefButtons.GetChild(0).GetComponent<Button>();
            b.onClick.AddListener(() => lobby.ConnectToRoom(room.Name));

            prefButtons.GetChild(1).gameObject.SetActive(false);
        }
    }


    //создание кнопок городов
    private void CreateButtonTowns()
    {
        List<string> townsList = SaveLoad.loadGamesList("StreamingAssets");
        foreach (string nameTown in townsList)
        {
            Transform but = Instantiate(buttonTown) as Transform;
            but.SetParent(scrollTowns.content, false);
            but.GetComponentInChildren<Text>().text = nameTown;
            Button b = but.GetComponent<Button>();
            b.onClick.AddListener(() => onButtonClickChoseTown(nameTown));
        }
    }

    //открыть меню создания новой игры
    public void OpenMenuNewGame()
    {
        textTypeOfNewGame.text = "Новая одиночная игра";
        online = false;
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

        SceneManager.LoadScene("Game", LoadSceneMode.Single);
    }

    private void DeleteGame(string dbName, GameObject pref)
    {
        SaveLoad.deleteGame(dbName);
        Destroy(pref);
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
        if (ActiveNetworkGames.activeInHierarchy)
            ClearScroll(scrollActiveNetworkGames);
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
        if (!online)
        {
            Camera.main.GetComponent<DBwork>()
                .CreateNewGame(countOfPlayers, startMoney, newNameGame, online, nameTownForNewGame, namePlayer);
            if (Trade.things == null)
            {
                Trade.things = new List<ThingForTrade>[countOfPlayers + 1, countOfPlayers + 1];
            }

            SceneManager.LoadScene("Game", LoadSceneMode.Single);
        }
        else
        {
            Lobby lobby = GetComponent<Lobby>();
            lobby.StartNewNetworkGame(countOfPlayers, startMoney, newNameGame, online, nameTownForNewGame, namePlayer);
        }
    }

    public void OpenListOfNetworksGames()
    {
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
                NetworkSettings.SetActive(false);
                SavedNetworkGames.SetActive(false);
                ActiveNetworkGames.SetActive(false);
                break;
            //открыть меню загрузки игры
            case 2:
                MainMenuObj.SetActive(false);
                ContinueObj.SetActive(true);
                BackButton.SetActive(true);
                CreateNewGameObj.SetActive(false);
                SettingsMenu.SetActive(false);
                NetworkSettings.SetActive(false);
                SavedNetworkGames.SetActive(false);
                ActiveNetworkGames.SetActive(false);
                break;
            //открыть настройки        
            case 3:
                MainMenuObj.SetActive(false);
                ContinueObj.SetActive(false);
                BackButton.SetActive(true);
                CreateNewGameObj.SetActive(false);
                SettingsMenu.SetActive(true);
                NetworkSettings.SetActive(false);
                SavedNetworkGames.SetActive(false);
                ActiveNetworkGames.SetActive(false);
                break;
            //Вернуться в глввное меню
            case 4:
                MainMenuObj.SetActive(true);
                ContinueObj.SetActive(false);
                BackButton.SetActive(false);
                CreateNewGameObj.SetActive(false);
                SettingsMenu.SetActive(false);
                NetworkSettings.SetActive(false);
                SavedNetworkGames.SetActive(false);
                ActiveNetworkGames.SetActive(false);
                break;
            //открыть первое меню нетворка 
            case 5:
                MainMenuObj.SetActive(false);
                ContinueObj.SetActive(false);
                BackButton.SetActive(true);
                CreateNewGameObj.SetActive(false);
                SettingsMenu.SetActive(false);
                NetworkSettings.SetActive(true);
                SavedNetworkGames.SetActive(false);
                ActiveNetworkGames.SetActive(false);
                break;
            //открыть список доступных игровых лобби
            case 6:
                MainMenuObj.SetActive(false);
                ContinueObj.SetActive(false);
                BackButton.SetActive(true);
                CreateNewGameObj.SetActive(false);
                SettingsMenu.SetActive(false);
                NetworkSettings.SetActive(false);
                SavedNetworkGames.SetActive(false);
                ActiveNetworkGames.SetActive(true);
                break;
            //открыть список сохраненных сетевых игр
            case 7:
                MainMenuObj.SetActive(false);
                ContinueObj.SetActive(false);
                BackButton.SetActive(true);
                CreateNewGameObj.SetActive(false);
                SettingsMenu.SetActive(false);
                NetworkSettings.SetActive(false);
                SavedNetworkGames.SetActive(true);
                ActiveNetworkGames.SetActive(false);
                break;
            default:
            {
                MainMenuObj.SetActive(true);
                ContinueObj.SetActive(false);
                BackButton.SetActive(false);
                CreateNewGameObj.SetActive(false);
                SettingsMenu.SetActive(false);
                NetworkSettings.SetActive(false);
                SavedNetworkGames.SetActive(false);
                ActiveNetworkGames.SetActive(false);
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

    //изменение уровня громкости звуков в главном меню
    public void ChangeVolumeLevel(float input)
    {
        MainMenuMixer.SetFloat("Volume", input);
    }
}