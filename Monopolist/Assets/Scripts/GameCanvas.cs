using System.Collections;
using System.Collections.Generic;
using System.Text;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class GameCanvas : MonoBehaviour
{
    //Объект с кнопками, присутвующими на экране во время игры
    public GameObject playMenu;

    //объект, с кнопкаи, появляющимися при выходе в меню паузы
    public GameObject pauseMenu;

    //кнопка отмены сохранения
    public GameObject returnButton;
    
    //кнопка включения/выключения списка зданий
    public GameObject buildsButton; 

    //Префаб кнопок, появляющихся в контекте скроллов
    public RectTransform prefabButtonsinScrolls;

    //Вывод шагов игрока
    public Text stepsText;

    //Вывод денег игрока
    public Text moneyText;

    //Первый скроллВью
    public ScrollRect ScrollRectFirst;

    //Второй  скроллВью
    public ScrollRect ScrollRectSecond;

    //Третий скроллВью
    public ScrollRect ScrollRectThird;

    //Поле для ввода нового названия игры
    public InputField inputField;

    //Сколько ходов игрок сделал
    public static int currentSteps;

    //Количество ходов, выпавших игроку на кубиках
    public static int maxSteps;

    //Деньги игрока
    public static int money;

    //Строка для сохранения нового названия игры
    private string newName;

    //Класс для работы с базой данных
    private DBwork _dBwork;

    //массив кнопок с улицами
    private RectTransform[] streetsPathsRectTransforms;

    //массив кнопок с игроками
    private RectTransform[] playersRectTransforms;

    //массив кнопок со зданиями конкретной улицы
    private RectTransform[] buildsRectTransforms;

    //открыто ли уже онкно с улицами и на какой вьюхе
    private int openedStreets = 0;

    //открыто ли уже онкно с игроками и на какой вьюхе
    private int openedPlayers = 0;

    //открыто ли уже онкно со зданиями и на какой вьюхе
    private int openedBuilds = 0;

    //вывод данных об игроке
    void Update()
    {
        //вывод количества денег игрока на экран
        moneyText.text = "Капитал: " + money;
        //вывод ходов игрока на экран
        stepsText.text = "Сделано ходов: " + currentSteps + "/" + maxSteps;
    }

    //открыть меню паузы
    public void OpenGameMenu()
    {
        ChangeMenu(2);
    }

    //открыть список улиц
    public void OpenStreetsList()
    {
        if (openedStreets == 0)
        {
            if (!ScrollRectFirst.IsActive())
            {
                ChooseScrollView(ScrollRectFirst, 1, -1);
                openedStreets = 1;
            }
            else if (!ScrollRectSecond.IsActive())
            {
                ChooseScrollView(ScrollRectSecond, 1, -1);
                openedStreets = 2;
            }
            else if (!ScrollRectThird.IsActive())
            {
                ChooseScrollView(ScrollRectThird, 1, -1);
                openedStreets = 3;
            }
        }
        else
        {
            if (openedStreets == 1)
            {
                ChooseScrollView(ScrollRectFirst, 1, -1);
                openedStreets = 0;
            }
            else if (openedStreets == 2)
            {
                ChooseScrollView(ScrollRectSecond, 1, -1);
                openedStreets = 0;
            }
            else if (openedStreets == 3)
            {
                ChooseScrollView(ScrollRectThird, 1, -1);
                openedStreets = 0;
            }
        }
    }

    //открыть список игроков
    public void OpenPlayersList()
    {
        if (openedPlayers == 0)
        {
            if (!ScrollRectFirst.IsActive())
            {
                ChooseScrollView(ScrollRectFirst, 2, -1);
                openedPlayers = 1;
            }
            else if (!ScrollRectSecond.IsActive())
            {
                ChooseScrollView(ScrollRectSecond, 2, -1);
                openedPlayers = 2;
            }
            else if (!ScrollRectThird.IsActive())
            {
                ChooseScrollView(ScrollRectThird, 2, -1);
                openedPlayers = 3;
            }
        }
        else
        {
            if (openedPlayers == 1)
            {
                ChooseScrollView(ScrollRectFirst, 2, -1);
                openedPlayers = 0;
            }
            else if (openedPlayers == 2)
            {
                ChooseScrollView(ScrollRectSecond, 2, -1);
                openedPlayers = 0;
            }
            else if (openedPlayers == 3)
            {
                ChooseScrollView(ScrollRectThird, 2, -1);
                openedPlayers = 0;
            }
        }
    }

    //открыть список зданий на конкретной улице
    public void OpenBuildsList(int idPath)
    {
        if (openedBuilds == 0)
        {
            if (!ScrollRectFirst.IsActive())
            {
                ChooseScrollView(ScrollRectFirst, 3, idPath);
                openedBuilds = 1;
            }
            else if (!ScrollRectSecond.IsActive())
            {
                ChooseScrollView(ScrollRectSecond, 3, idPath);
                openedBuilds = 2;
            }
            else if (!ScrollRectThird.IsActive())
            {
                ChooseScrollView(ScrollRectThird, 3, idPath);
                openedBuilds = 3;
            }
        }
        else
        {
            if (openedBuilds == 1)
            {
                ChooseScrollView(ScrollRectFirst, 3, idPath);
                openedBuilds = 0;
            }
            else if (openedBuilds == 2)
            {
                ChooseScrollView(ScrollRectSecond, 3, idPath);
                openedBuilds = 0;
            }
            else if (openedBuilds == 3)
            {
                ChooseScrollView(ScrollRectThird, 3, idPath);
                openedBuilds = 0;
            }
        }
    }

    //сохранить игру
    public void SaveGame()
    {
        _dBwork.SaveGame();
    }

    //открыть меню для сохранения игры как новый файл
    public void SaveGameAsNew()
    {
        ChangeMenu(3);
    }

    //сохранить игру как новый файл
    public void SaveGameAsNewInputField()
    {
        if (inputField.text.Length != 0)
        {
            newName = inputField.text;
            _dBwork.SaveGameAsNewFile(newName);
            ChangeMenu(2);
        }
    }

    //открыть главное меню
    public void OpenMainMenu()
    {
        //Destroy(Camera.main);
        SceneManager.LoadScene("MainMenu");
    }

    //вернуться в игру
    public void returnToGame()
    {
        ChangeMenu(1);
    }

    //метод переключения меню между собой
    private void ChangeMenu(int status)
    {
        switch (status)
        {
            //игровая канва
            case 1:
                buildsButton.SetActive(false);
                inputField.gameObject.SetActive(false);
                playMenu.SetActive(true);
                pauseMenu.SetActive(false);
                returnButton.SetActive(false);
                if (openedBuilds == 1 || openedPlayers == 1 || openedStreets == 1)
                {
                    ScrollRectFirst.gameObject.SetActive(true);
                }
                if (openedBuilds == 2 || openedPlayers == 2 || openedStreets == 2)
                {
                    ScrollRectSecond.gameObject.SetActive(true);
                }
                if (openedBuilds == 3 || openedPlayers == 3 || openedStreets == 3)
                {
                    ScrollRectThird.gameObject.SetActive(true);
                }
                break;
            //меню паузы
            case 2:
                buildsButton.SetActive(false);
                inputField.gameObject.SetActive(false);
                playMenu.SetActive(false);
                pauseMenu.SetActive(true);
                returnButton.SetActive(false);
                CloseViews();
                break;
            //меню записи
            case 3:
                buildsButton.SetActive(false);
                inputField.gameObject.SetActive(true);
                playMenu.SetActive(false);
                pauseMenu.SetActive(false);
                returnButton.SetActive(true);
                CloseViews();
                break;
            //если че, игровая канва
            default:
            {    
                buildsButton.SetActive(false);
                inputField.gameObject.SetActive(false);
                playMenu.SetActive(true);
                pauseMenu.SetActive(false);
                returnButton.SetActive(false);
                CloseViews();
                break;
            }
        }
    }

    //закрыть активные вьюхи
    private void CloseViews()
    {
        ScrollRectFirst.gameObject.SetActive(false);
        ScrollRectSecond.gameObject.SetActive(false);
        ScrollRectThird.gameObject.SetActive(false);
    }

    //создание кнопок с улицами
    private void CreateStreetsButtons()
    {
        _dBwork = Camera.main.GetComponent<DBwork>();
        StreetPath[] streetsPaths = _dBwork.GetAllPaths();
        streetsPathsRectTransforms = new RectTransform[streetsPaths.Length];
        foreach (StreetPath path in streetsPaths)
        {
            var prefButtons = Instantiate(prefabButtonsinScrolls);
            streetsPathsRectTransforms[path.GetIdStreetPath()] = prefButtons;
            // prefButtons.SetParent(ScrollRectFirst.content, false);

            prefButtons.GetChild(0).GetComponent<Button>().GetComponentInChildren<Text>().text =
                path.GetIdStreetPath().ToString();
            prefButtons.GetChild(0).GetComponent<Button>().onClick
                .AddListener(() => onButtonStreetClick(path.GetIdStreetPath()));

            prefButtons.GetChild(1).GetComponent<Button>().GetComponentInChildren<Text>().text = "Buy";
            prefButtons.GetChild(1).GetComponent<Button>().onClick
                .AddListener(() => onButtonBuyClick(path.GetIdStreetPath()));

            prefButtons.GetChild(2).GetComponent<Button>().GetComponentInChildren<Text>().text = "Info";
            prefButtons.GetChild(2).GetComponent<Button>().onClick
                .AddListener(() => onButtonInfoClick(path.GetIdStreetPath()));

            prefButtons.GetChild(3).GetComponent<Button>().GetComponentInChildren<Text>().text = "Builds";
            prefButtons.GetChild(3).GetComponent<Button>().onClick
                .AddListener(() => onButtonBuildsClick(path.GetIdStreetPath()));
        }
    }

    //создание кнопок с игроками
    private void CreatePlayersButtons()
    {
        _dBwork = Camera.main.GetComponent<DBwork>();
        Player[] Players = _dBwork.GetAllPlayers();
        playersRectTransforms = new RectTransform[Players.Length];

        foreach (Player player in Players)
        {
            var prefButtons = Instantiate(prefabButtonsinScrolls);
            playersRectTransforms[player.IdPlayer] = prefButtons;

            prefButtons.GetChild(0).GetComponent<Button>().GetComponentInChildren<Text>().text =
                player.IdPlayer.ToString();
            prefButtons.GetChild(0).GetComponent<Button>().onClick
                .AddListener(() => onButtonClickPlayer(player.IdPlayer));

            prefButtons.GetChild(1).GetComponent<Button>().GetComponentInChildren<Text>().text = "Trade";
            prefButtons.GetChild(1).GetComponent<Button>().onClick
                .AddListener(() => onButtonClickTrade(player.IdPlayer));

            prefButtons.GetChild(2).GetComponent<Button>().GetComponentInChildren<Text>().text = "Info";
            prefButtons.GetChild(2).GetComponent<Button>().onClick
                .AddListener(() => onButtonInfoClick(player.IdPlayer));

            prefButtons.GetChild(3).GetComponent<Button>().GetComponentInChildren<Text>().text = "none";
            prefButtons.GetChild(3).gameObject.SetActive(false);
            //prefButtons.GetChild(3).GetComponent<Button>().onClick
            //    .AddListener(() => onButtonBuildsClick(player.GetIdStreetPath()));
        }
    }

    //создание кнопок со зданиями конкретной улицы
    private void CreateBuildsButtons(int idPath)
    {
        _dBwork = Camera.main.GetComponent<DBwork>();
        Build[] builds = _dBwork.GetBuildsForThisPath(idPath);
        buildsRectTransforms = new RectTransform[builds.Length];
        foreach (Build build in builds)
        {
            var prefButtons = Instantiate(prefabButtonsinScrolls);
            streetsPathsRectTransforms[build.IdBuild] = prefButtons;

            prefButtons.GetChild(0).GetComponent<Button>().GetComponentInChildren<Text>().text =
                build.IdBuild.ToString();
            //prefButtons.GetChild(0).GetComponent<Button>().onClick
            //    .AddListener(() => onButtonStreetClick(build.GetIdStreetPath()));

            prefButtons.GetChild(1).GetComponent<Button>().GetComponentInChildren<Text>().text = "Buy";
            //prefButtons.GetChild(1).GetComponent<Button>().onClick
            //    .AddListener(() => onButtonBuyClick(build.GetIdStreetPath()));

            prefButtons.GetChild(2).GetComponent<Button>().GetComponentInChildren<Text>().text = "Info";
            //prefButtons.GetChild(2).GetComponent<Button>().onClick
            //   .AddListener(() => onButtonInfoClick(build.GetIdStreetPath()));

            prefButtons.GetChild(3).GetComponent<Button>().GetComponentInChildren<Text>().text = "none";
            //prefButtons.GetChild(3).GetComponent<Button>().onClick
            //    .AddListener(() => onButtonBuildsClick(build.GetIdStreetPath()));
        }
    }

    //перемещение к выбранной улице, включение кнопки зданий на этой улице
    private void onButtonStreetClick(int idPath)
    {
        buildsButton.SetActive(true);
    }

    //окно покупки улиц
    private void onButtonBuyClick(int idPath)
    {
    }

    //показать информацию об объекте
    private void onButtonInfoClick(int id)
    {
    }

    //показать список зданий этой улицы
    private void onButtonBuildsClick(int idPath)
    {
        OpenBuildsList(idPath);
    }

    //перемещает к этому игроку на карте
    private void onButtonClickPlayer(int idPlayer)
    {
    }

    //открыть окно торговли с этим игроком
    private void onButtonClickTrade(int idPlayer)
    {
    }

    //выбор ещё не активной вьюхи для отображения информации
    private void ChooseScrollView(ScrollRect scroll, int type, int idPath)
    {
        //idPath для зданий, для остальных он -1

        if (!scroll.IsActive())
        {
            scroll.gameObject.SetActive(true);
            //тип 1 - улицы
            switch (type)
            {
                case 1:
                    CreateStreetsButtons();
                    foreach (RectTransform rectTransform in streetsPathsRectTransforms)
                    {
                        rectTransform.SetParent(scroll.content, false);
                    }
                    //тип 2 - игроки
                    break;
                case 2:
                    CreatePlayersButtons();
                    foreach (RectTransform rectTransform in playersRectTransforms)
                    {
                        rectTransform.SetParent(scroll.content, false);
                    }
                    //тип 3 - здания
                    break;
                case 3:
                    CreateBuildsButtons(idPath);
                    foreach (RectTransform rectTransform in buildsRectTransforms)
                    {
                        rectTransform.SetParent(scroll.content, false);
                    }
                    break;
            }
        }
        else
        {
            scroll.gameObject.SetActive(false);
            for (int i = scroll.content.childCount - 1; i >= 0; i--)
                Destroy(scroll.content.GetChild(i).gameObject);
        }
    }

//    private void OpenLists(int type)
//    {
//        
//        /**
//         * Тип это что конкретно открываем: улицы, игроков или здания
//         * i - это проверка открыта ли уже вьюха с этими данными
//         */
//        int i = 0;
//        switch (type)
//        {
//            case 1:
//                i = openedStreets;
//                break;
//            case 2:
//                i = openedPlayers;
//                break;
//            case 3:
//                i = openedBuilds;
//                break;
//        }
//        
//        
//        if (i == 0)
//        {
//            if (!ScrollRectFirst.IsActive())
//            {
//                ChooseScrollView(ScrollRectFirst,type);
//                openedStreets = 1;
//            }
//            else if (!ScrollRectSecond.IsActive())
//            {
//                ChooseScrollView(ScrollRectSecond,type);
//                openedStreets = 2;
//            }
//            else if (!ScrollRectThird.IsActive())
//            {
//                ChooseScrollView(ScrollRectThird,type);
//                openedStreets = 3;
//            }
//        }
//        else
//        {
//            if (i == 1)
//            {
//                ChooseScrollView(ScrollRectFirst,type);
//                openedStreets = 0;
//            }
//            else if (i == 2)
//            {
//                ChooseScrollView(ScrollRectSecond,type);
//                openedStreets = 0;
//            }
//            else if (i == 3)
//            {
//                ChooseScrollView(ScrollRectThird,type);
//                openedStreets = 0;
//            }
//        }
//    }
}