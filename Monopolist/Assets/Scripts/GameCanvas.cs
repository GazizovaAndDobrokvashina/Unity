using System.Collections;
using System.Collections.Generic;
using System.Text;
using UnityEngine;
using UnityEngine.Audio;
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

    //окно с предупреждениями
    public GameObject warningWindow;

    //кнопка с информацией
    public GameObject ButtonWithInfo;
    
    //галочка "Только мои улицы"
    public GameObject MineTogle;

    //Префаб кнопок, появляющихся в контекте скроллов
    public RectTransform prefabButtonsinScrolls;

    //Вывод шагов игрока
    public Text stepsText;

    //Вывод денег игрока
    public Text moneyText;

    //улица, на которой стоит игрок
    public Text destinationText;

    //рабочая информация об улице
    public Text ImportantInfoAboutStreetText;

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

    //месторасположение игрока
    public static string destination;

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

    private Player currentPlayer;

    public Cameras cameras;

    public bool response;

    public AudioMixer GameMixer;

    public Cameras camerasScript;

    public void ChangeCamera()
    {
        camerasScript.ChangeCamera();
    }

    public void ChangeTypeOfCamera()
    {
        camerasScript.ChangeTypeOfCamera();
    }

    public void OpenWarningWindow(Player player)
    {
        currentPlayer = player;
        warningWindow.SetActive(true);
    }

    public void GetRespons(bool response)
    {
        getCurrentPlayer().takeResponse(response);
        this.response = response;
        warningWindow.SetActive(false);
    }

    //вывод данных об игроке
    private void Update()
    {
        //вывод количества денег игрока на экран
        moneyText.text = "Капитал: " + money;
        //вывод ходов игрока на экран
        stepsText.text = "Сделано ходов: " + currentSteps + "/" + maxSteps;
        //вывод информации, где находится игрок
        destinationText.text = "Улица: " + destination;
    }

    //открыть меню паузы
    public void OpenGameMenu()
    {
        ChangeMenu(2);
        Time.timeScale = 0;
    }

    //скрыть информацию
    public void CloseInformation()
    {
        ButtonWithInfo.SetActive(false);
    }

    //открыть список улиц
    public void OpenStreetsList()
    {
        if (openedStreets == 0)
        {
            MineTogle.SetActive(true);
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
            MineTogle.SetActive(false);
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
        if (Camera.main.GetComponent<DBwork>().GetBuildsForThisPath(idPath).Length == 0)
            return;

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
        getDbWork().SaveGame();
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
            getDbWork().SaveGameAsNewFile(newName);
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
        Time.timeScale = 1;
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
        StreetPath[] streetsPaths = getDbWork().GetAllPaths();
        streetsPathsRectTransforms = new RectTransform[streetsPaths.Length];
        foreach (StreetPath path in streetsPaths)
        {
            if (path.GetIdStreetPath() == 0)
            {
                continue;
            }
            var prefButtons = Instantiate(prefabButtonsinScrolls);
            streetsPathsRectTransforms[path.GetIdStreetPath()] = prefButtons;
            // prefButtons.SetParent(ScrollRectFirst.content, false);

            prefButtons.GetChild(0).GetComponent<Button>().GetComponentInChildren<Text>().text =
                path.NamePath;
            prefButtons.SetSiblingIndex(path.GetIdStreetPath());
            prefButtons.GetChild(0).GetComponent<Button>().onClick
                .AddListener(() => onButtonStreetClick(path.GetIdStreetPath()));

            prefButtons.GetChild(1).GetComponent<Button>().GetComponentInChildren<Text>().text = "Buy";
            if (path.CanBuy)
            {
                prefButtons.GetChild(1).GetComponent<Button>().onClick
                    .AddListener(() => onButtonBuyClick(path.GetIdStreetPath()));
            }
            else
            {
                prefButtons.GetChild(1).gameObject.SetActive(false);
            }

            prefButtons.GetChild(2).GetComponent<Button>().GetComponentInChildren<Text>().text = "Info";
            prefButtons.GetChild(2).GetComponent<Button>().onClick
                .AddListener(() => onButtonInfoClick(path.GetIdStreetPath(), 1));

            prefButtons.GetChild(3).GetComponent<Button>().GetComponentInChildren<Text>().text = "Builds";
            if (path.CanBuy)
            {
                prefButtons.GetChild(3).GetComponent<Button>().onClick
                    .AddListener(() => onButtonBuildsClick(path.GetIdStreetPath()));
            }
            else
            {
                prefButtons.GetChild(3).gameObject.SetActive(false);
            }
        }
    }

    //создание кнопок с игроками
    private void CreatePlayersButtons()
    {
        //переделать с айдишников на нормальные названия когда появятся
        Player[] Players = getDbWork().GetAllPlayers();
        playersRectTransforms = new RectTransform[Players.Length];

        foreach (Player player in Players)
        {
            if (player.IdPlayer == 0)
            {
                continue;
            }
            var prefButtons = Instantiate(prefabButtonsinScrolls);
            playersRectTransforms[player.IdPlayer] = prefButtons;
            prefButtons.GetChild(0).GetComponent<Button>().GetComponentInChildren<Text>().text =
                player.NickName;
            prefButtons.GetChild(0).GetComponent<Button>().onClick
                .AddListener(() => onButtonClickPlayer(player.IdPlayer));

            prefButtons.GetChild(1).GetComponent<Button>().GetComponentInChildren<Text>().text = "Trade";
            prefButtons.GetChild(1).GetComponent<Button>().onClick
                .AddListener(() => onButtonClickTrade(player.IdPlayer));

            prefButtons.GetChild(2).GetComponent<Button>().GetComponentInChildren<Text>().text = "Info";
            prefButtons.GetChild(2).GetComponent<Button>().onClick
                .AddListener(() => onButtonInfoClick(player.IdPlayer, 2));

            prefButtons.GetChild(3).GetComponent<Button>().GetComponentInChildren<Text>().text = "none";
            prefButtons.GetChild(3).gameObject.SetActive(false);
            //prefButtons.GetChild(3).GetComponent<Button>().onClick
            //    .AddListener(() => onButtonBuildsClick(player.GetIdStreetPath()));
        }
    }

    //создание кнопок со зданиями конкретной улицы
    private void CreateBuildsButtons(int idPath)
    {
        Build[] builds = getDbWork().GetBuildsForThisPath(idPath);
        if (builds.Length > 0)
        {
            buildsRectTransforms = new RectTransform[builds.Length];
            int i = 0;
            foreach (Build build in builds)
            {
                var prefButtons = Instantiate(prefabButtonsinScrolls);
                buildsRectTransforms[i] = prefButtons;
                i++;
                prefButtons.GetChild(0).GetComponent<Button>().GetComponentInChildren<Text>().text =
                    build.NameBuild;

                prefButtons.GetChild(1).GetComponent<Button>().GetComponentInChildren<Text>().text = "Buy";
                prefButtons.GetChild(1).GetComponent<Button>().onClick
                    .AddListener(() => OnButtonClickBuyBild(build.IdBuild));

                prefButtons.GetChild(2).GetComponent<Button>().GetComponentInChildren<Text>().text = "Info";
                prefButtons.GetChild(2).GetComponent<Button>().onClick
                    .AddListener(() => onButtonInfoClick(build.IdBuild, 3));

                prefButtons.GetChild(3).GetComponent<Button>().GetComponentInChildren<Text>().text = "none";
                prefButtons.GetChild(3).gameObject.SetActive(false);
            }
        }
    }

    //купить здание 
    private void OnButtonClickBuyBild(int idBuild)
    {
        if (!getDbWork().GetBuild(idBuild).Enable && getDbWork().isAllPathsMine(idBuild, getCurrentPlayer().IdPlayer) &&
            getCurrentPlayer().Money >= getDbWork().GetBuild(idBuild).PriceBuild)
        {
            getDbWork().GetBuild(idBuild).build(getCurrentPlayer());
        }
    }

    private int currentIdPath;
    //перемещение к выбранной улице, включение кнопки зданий на этой улице и важной информации об улице
    private void onButtonStreetClick(int idPath)
    {
        
        if (buildsButton.activeInHierarchy && idPath == currentIdPath)
        {
            buildsButton.SetActive(false);
            ImportantInfoAboutStreetText.transform.parent.gameObject.SetActive(false);
        }
        else
        {
            currentIdPath = idPath;
            buildsButton.SetActive(true);
            ImportantInfoAboutStreetText.transform.parent.gameObject.SetActive(true);

            PathForBuy pathForBuy = getDbWork().GetPathForBuy(idPath);

            if (camerasScript.isActiveOrtoCamera())
            {
                camerasScript.moveOrtoCamera(getDbWork().GetPathById(idPath).transform.position);
            }


            if (pathForBuy != null)
            {
                ImportantInfoAboutStreetText.text = "Название: " + pathForBuy.namePath + "\n" +
                                                    "Владелец: " + getDbWork().GetPlayerbyId(pathForBuy.IdPlayer)
                                                        .NickName +
                                                    "\n" + "Рента: " + pathForBuy.GetRenta() + "\n" + "Здания: " +
                                                    pathForBuy.GetBuildsName();
            }
            else
            {
                ImportantInfoAboutStreetText.text = "Название: " + getDbWork().GetPathById(idPath).namePath + "\n" +
                                                    "Гос. учереждение";
            }
        }
    }

    public void ShowJustMineStreet(bool activeTogle)
    {
        if (activeTogle)
        {
            List<int> paths = getDbWork().GetMyPathes(1);

            foreach (RectTransform rectTransform in streetsPathsRectTransforms)
            {
                
                if (rectTransform == null)
                {
                    continue;
                }
                if (paths.Contains(rectTransform.GetSiblingIndex()))
                {
                    paths.Remove(rectTransform.GetSiblingIndex());
                }
                else
                {
                    rectTransform.gameObject.SetActive(false);
                }
            }
                                                                                                                                                         
        }
        else
        {
            foreach (RectTransform rectTransform in streetsPathsRectTransforms)
            {
                
                if (rectTransform == null)
                {
                    continue;
                }
                rectTransform.gameObject.SetActive(true);
            }
        }
    }

    //получить экземпляр текущего DBWork
    private DBwork getDbWork()
    {
        if (_dBwork == null)
            _dBwork = Camera.main.GetComponent<DBwork>();

        return _dBwork;
    }

    //найти текущего игрока
    private Player getCurrentPlayer()
    {
        if (currentPlayer == null)
            currentPlayer = getDbWork().GetPlayerbyId(1);
        return currentPlayer;
    }

    //окно покупки улиц
    private void onButtonBuyClick(int idPath)
    {
        if (getCurrentPlayer().CurrentStreetPath.GetIdStreetPath() == idPath &&
            getDbWork().GetPathById(idPath).CanBuy && getDbWork().GetPathForBuy(idPath).IdPlayer == 0 &&
            getDbWork().GetPathForBuy(idPath).PriceStreetPath < getCurrentPlayer().Money)
        {
            if (currentSteps < maxSteps)
            {
                OpenWarningWindow(getCurrentPlayer());
                StartCoroutine(WaitForAnswer(idPath));
                return;
            }


            getDbWork().GetPathForBuy(idPath).Buy(getCurrentPlayer());
            //gameObject.GetComponent<GameController>().nextStep();
        }
    }

    private IEnumerator WaitForAnswer(int idPath)
    {
        yield return new WaitForSeconds(0.5f);
        yield return new WaitWhile(() => warningWindow.activeInHierarchy);

        if (response)
        {
            getDbWork().GetPathForBuy(idPath).Buy(getCurrentPlayer());
            gameObject.GetComponent<GameController>().nextStep();
        }
        else
        {
            yield break;
        }
    }

    //показать информацию об объекте
    private void onButtonInfoClick(int id, int type)
    {
        //type = 1 - streetspaths; 2 - players; 3 - builds

        string info = "";
        switch (type)
        {
            case 1:
                PathForBuy pathForBuy = getDbWork().GetPathForBuy(id);
                if (pathForBuy != null)
                {
                    info = "Название: " + pathForBuy.namePath + "\n" +
                           "Владелец: " + getDbWork().GetPlayerbyId(pathForBuy.IdPlayer).NickName +
                           "\n" + "Рента: " + pathForBuy.GetRenta() + "\n" + "Здания: " + pathForBuy.GetBuildsName()
                           + "\n\n Информация об улице: " +
                           getDbWork().getStreetById(pathForBuy.GetIdStreetParent()).AboutStreet1;
                }
                else
                {
                    StreetPath path = getDbWork().GetPathById(id);
                    info = "Название: " + path.namePath + "\n" +
                           "Гос. учереждение \n\n Информация об улице: " +
                           getDbWork().getStreetById(path.GetIdStreetParent()).AboutStreet1;
                }
                break;
            case 2:
                info = getDbWork().GetPlayerbyId(id).NickName + " " + getDbWork().GetPlayerbyId(id).Money;
                break;
            case 3:
                info = getDbWork().GetBuild(id).NameBuild + "\n" + getDbWork().GetBuild(id).AboutBuild;
                break;
        }


        ButtonWithInfo.GetComponentInChildren<Text>().text = info + "\n\n" + "(нажмите, чтобы закрыть)";
        ButtonWithInfo.SetActive(true);
    }

    public void ShowInfoAboutEvent(string info)
    {
        ButtonWithInfo.GetComponentInChildren<Text>().text = info + "\n\n" + "(нажмите, чтобы закрыть)";
        ButtonWithInfo.SetActive(true);
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
                    for (int index = 1; index < streetsPathsRectTransforms.Length; index++)
                    {
                        RectTransform rectTransform = streetsPathsRectTransforms[index];
                        rectTransform.SetParent(scroll.content, false);
                    }
                    //тип 2 - игроки
                    break;
                case 2:
                    CreatePlayersButtons();
                    for (int index = 1; index < playersRectTransforms.Length; index++)
                    {
                        RectTransform rectTransform = playersRectTransforms[index];
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
            //пока что корявнько так
            if (type == 1)
            {
                ImportantInfoAboutStreetText.gameObject.SetActive(false);
                buildsButton.gameObject.SetActive(false);
            }


            CheckScrolls();
        }
    }

    //при закрытии одной из вьюх, перемещение информации из других влево, если есть место
    private void CheckScrolls()
    {
        //если первый неактивен, а второй активен
        if (!ScrollRectFirst.IsActive() && ScrollRectSecond.IsActive())
        {
            ChangeTypesScrolls(2, 1);
            for (int i = ScrollRectSecond.content.childCount - 1; i >= 0; i--)
            {
                ScrollRectSecond.content.GetChild(0).SetParent(ScrollRectFirst.content, false);
            }
            ScrollRectFirst.gameObject.SetActive(true);
            ScrollRectSecond.gameObject.SetActive(false);
        }

        //Если третий активен
        if (ScrollRectThird.IsActive())
        {
            //и первый не активен
            if (!ScrollRectFirst.IsActive())
            {
                for (int i = ScrollRectThird.content.childCount - 1; i >= 0; i--)
                {
                    ScrollRectThird.content.GetChild(0).SetParent(ScrollRectFirst.content, false);
                }
                ScrollRectFirst.gameObject.SetActive(true);
                ScrollRectThird.gameObject.SetActive(false);
                ChangeTypesScrolls(3, 1);
                //и второй не активен
            }
            else if (!ScrollRectSecond.IsActive())
            {
                for (int i = ScrollRectThird.content.childCount - 1; i >= 0; i--)
                {
                    ScrollRectThird.content.GetChild(0).SetParent(ScrollRectSecond.content, false);
                }
                ScrollRectSecond.gameObject.SetActive(true);
                ScrollRectThird.gameObject.SetActive(false);
                ChangeTypesScrolls(3, 2);
            }
        }
    }

    //Вспомогательный метод для смещения кнопок в скроллах
    private void ChangeTypesScrolls(int start, int end)
    {
        if (openedBuilds == start)
        {
            openedBuilds = end;
        }
        if (openedPlayers == start)
        {
            openedPlayers = end;
        }
        if (openedStreets == start)
        {
            openedStreets = end;
        }
    }

    //перемещение камеры к улице, где стоит игрок 
    public void GoToStreetUpButton()
    {
        
        onButtonStreetClick(getCurrentPlayer().GetCurrentStreetPath().GetIdStreetPath());
        cameras.SetActiveFirstCamera();
    }

    public void ChangeSoundLevel(float input)
    {
        GameMixer.SetFloat("Sound", input);
    }

    public void ChangeMusicLevel(float input)
    {
        GameMixer.SetFloat("Music", input);
    }
}