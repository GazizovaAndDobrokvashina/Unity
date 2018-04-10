
using UnityEngine;
using UnityEngine.Networking;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Runtime.Serialization.Formatters;
using System.Text;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class NetworkGameCanvas : NetworkBehaviour
{
     //кнопка броска кубиков
    public GameObject buttonThrowDice;
    
    public CanvasGroup saveButton;

    public CanvasGroup saveAsNewButton;

    //объект с енопками меню торговли
    public GameObject TradeMenu;

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
    private NetworkDBwork _dBwork;

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

    //текущий активный игрок
    private NetworkPlayer currentPlayer;

    //ответ игрока
    public bool response;

    //аудиомикшер на сцене
    public AudioMixer GameMixer;

    //скрипт камер
    public Cameras camerasScript;

    //кнопка с именем первого игрока в торговле
    public GameObject firstPlayer;

    //кнопка с именем второго игрока в торговле
    public GameObject secondPlayer;

    //имеющиеся у первого игрока улицы
    public ScrollRect scrollFirstPlayerStreets;

    //имеющиеся у второго игрока улицы
    public ScrollRect scrollSecondPlayerStreets;

    //предложение первого игрока
    public ScrollRect scrollFirstPlayerOffer;

    //предложение второго игрока
    public ScrollRect scrollSecondPlayerOffer;

    //префаб кнопочки с названием удицы для торговли
    public GameObject prefButStreetForTrade;

    //кнопка предложения обмена
    public Button ApplyTrade;

    //слайдер денег первого игрока
    public Slider sliderMoneyFirst;

    //слайдер денег второго игрока
    public Slider sliderMoneySecond;

    //ИнпутФилд денег первого игрока
    public InputField InputFieldMoneyFirst;

    //ИнпутФилд денег второго игрока
    public InputField InputFieldMoneySecond;

    //сумма денег первого игрока (используется в торговле)
    private int moneyFirstPlayer;

    //сумма денег второго игрока (используется в торговле)
    private int moneySecondPlayer;

    //идентификатор улицы, информацию о которой нужно показать
    private int idStreetWhichOPened;

    private GameController _gameController;

    public void ThrowDice()
    {
        if (_gameController == null)
            _gameController = gameObject.GetComponent<GameController>();
        StartCoroutine(_gameController.Dices());
        buttonThrowDice.SetActive(false);
    }

    public void OpenThrowDiceButton()
    {
        buttonThrowDice.SetActive(true);
    }

    public void OpenBuilds()
    {
        OpenBuildsList(idStreetWhichOPened);
    }

    //переключение между видом от первого и от третьего лица
    public void ChangeCamera()
    {
        camerasScript.ChangeCamera();
    }

    //переключение между орто и перспективой верхней камеры
    public void ChangeTypeOfCamera()
    {
        camerasScript.ChangeTypeOfCamera();
    }

    //открыть окно с предупреждением
    public void OpenWarningWindow(NetworkPlayer player)
    {
        currentPlayer = player;
        warningWindow.SetActive(true);
    }

    //отправить ответ игрока и закрыть окно предупреждения
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

        //если открыто меню торговли и слайдеры игроков, то синхронизируем данные о деньгах игроков на слайдерах и инпутфилдах
        if (TradeMenu.active &&sliderMoneyFirst.gameObject.activeInHierarchy)
        {
            if (moneyFirstPlayer != (int) sliderMoneyFirst.value)
            {
                moneyFirstPlayer = (int) sliderMoneyFirst.value;
                InputFieldMoneyFirst.text = moneyFirstPlayer.ToString();
            }

            if (moneySecondPlayer != (int) sliderMoneySecond.value)
            {
                moneySecondPlayer = (int) sliderMoneySecond.value;
                InputFieldMoneySecond.text = moneySecondPlayer.ToString();
            }
        }
    }


    //изменение значения поля ввода суммы денег первого игрока
    public void OnValueChangedFirstInputFielid(string price)
    {
        if (!int.TryParse(price, out moneyFirstPlayer))
        {
            moneyFirstPlayer = 0;
            sliderMoneyFirst.value = 0;
            InputFieldMoneyFirst.text = "0";
        }
        else
        {
            moneyFirstPlayer = int.Parse(price);
            if(sliderMoneyFirst.gameObject.activeInHierarchy)
            sliderMoneyFirst.value = (float) moneyFirstPlayer;
            InputFieldMoneyFirst.text = moneyFirstPlayer.ToString();
        }
    }

    //изменнение значения поля ввода суммы денег второго игрока
    public void OnValueChangedSecondInputFielid(string price)
    {
        if (!int.TryParse(price, out moneySecondPlayer))
        {
            moneySecondPlayer = 0;
            sliderMoneySecond.value = 0;
            InputFieldMoneySecond.text = "0";
        }
        else
        {
            moneySecondPlayer = int.Parse(price);
            if(sliderMoneySecond.gameObject.activeInHierarchy)
            sliderMoneySecond.value = (float) moneySecondPlayer;
            InputFieldMoneySecond.text = moneySecondPlayer.ToString();
        }
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

    public void CloseScroll(int idScroll)
    {
        if (idScroll == 1)
        {
            ChooseScrollView(ScrollRectFirst, 1, -1);
            resetOpenParametr(1);
        }
        else if (idScroll == 2)
        {
            ChooseScrollView(ScrollRectSecond, 1, -1);
            resetOpenParametr(2);
        }
        else
        {
            ChooseScrollView(ScrollRectThird, 1, -1);
            resetOpenParametr(3);
        }
    }

    private void resetOpenParametr(int number)
    {
        if (openedStreets == number)
        {
            openedStreets = 0;
        }
        else if (openedPlayers == number)
        {
            openedPlayers = 0;
        }
        else
        {
            openedBuilds = 0;
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
        if (Camera.main.GetComponent<NetworkDBwork>().GetBuildsForThisPath(idPath).Length == 0)
            return;

        if (openedBuilds == 0)
        {
            if (!ScrollRectFirst.IsActive())
            {
                ChooseScrollView(ScrollRectFirst, 3, idPath);
                openedBuilds = 1;
                currentPathB = idPath;
            }
            else if (!ScrollRectSecond.IsActive())
            {
                ChooseScrollView(ScrollRectSecond, 3, idPath);
                openedBuilds = 2;
                currentPathB = idPath;
            }
            else if (!ScrollRectThird.IsActive())
            {
                ChooseScrollView(ScrollRectThird, 3, idPath);
                openedBuilds = 3;
                currentPathB = idPath;
            }
        }
        else
        {
            if (openedBuilds == 1)
            {
                if (currentPathB == idPath)
                {
                    ChooseScrollView(ScrollRectFirst, 3, idPath);
                    openedBuilds = 0;
                }
                else
                {
                    ChooseScrollView(ScrollRectFirst, 3, idPath);
                    currentPathB = idPath;
                }
            }
            else if (openedBuilds == 2)
            {
                if (currentPathB == idPath)
                {
                    ChooseScrollView(ScrollRectSecond, 3, idPath);
                    openedBuilds = 0;
                }
                else
                {
                    ChooseScrollView(ScrollRectSecond, 3, idPath);
                    currentPathB = idPath;
                }
            }
            else if (openedBuilds == 3)
            {
                if (currentPathB == idPath)
                {
                    ChooseScrollView(ScrollRectThird, 3, idPath);
                    openedBuilds = 0;
                }
                else
                {
                    ChooseScrollView(ScrollRectThird, 3, idPath);
                    currentPathB = idPath;
                }
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
        Time.timeScale = 1;
        SceneManager.LoadScene("MainMenu", LoadSceneMode.Single);
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
                TradeMenu.SetActive(false);
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
                TradeMenu.SetActive(false);
                CloseViews();
                break;
            //меню записи
            case 3:
                buildsButton.SetActive(false);
                inputField.gameObject.SetActive(true);
                playMenu.SetActive(false);
                pauseMenu.SetActive(false);
                returnButton.SetActive(true);
                TradeMenu.SetActive(false);
                CloseViews();
                break;
            //меню торговли
            case 4:
                buildsButton.SetActive(false);
                inputField.gameObject.SetActive(false);
                playMenu.SetActive(false);
                pauseMenu.SetActive(false);
                returnButton.SetActive(false);
                TradeMenu.SetActive(true);
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
                TradeMenu.SetActive(false);
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
        NetworkStreetPath[] streetsPaths = getDbWork().GetAllPaths();
        streetsPathsRectTransforms = new RectTransform[streetsPaths.Length];
        foreach (NetworkStreetPath path in streetsPaths)
        {
            if (path.GetIdStreetPath() == 0)
            {
                continue;
            }

            var prefButtons = Instantiate(prefabButtonsinScrolls);
            
            streetsPathsRectTransforms[path.GetIdStreetPath()] = prefButtons;

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
        NetworkPlayer[] Players = getDbWork().GetAllPlayers();
        playersRectTransforms = new RectTransform[Players.Length];

        foreach (NetworkPlayer player in Players)
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
        }
    }

    //создание кнопок со зданиями конкретной улицы
    private void CreateBuildsButtons(int idPath)
    {
        NetworkBuild[] builds = getDbWork().GetBuildsForThisPath(idPath);
        if (builds.Length > 0)
        {
            buildsRectTransforms = new RectTransform[builds.Length];
            int i = 0;
            foreach (NetworkBuild build in builds)
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

    //ID улицы, на которой сейчас игрок
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
            idStreetWhichOPened = idPath;
            ImportantInfoAboutStreetText.transform.parent.gameObject.SetActive(true);
            ImportantInfoAboutStreetText.gameObject.SetActive(true);

            NetworkPathForBuy pathForBuy = getDbWork().GetPathForBuy(idPath);
            
            camerasScript.SetActiveFirstCamera();
          //  if (camerasScript.isActiveOrtoCamera())
          //  {
                camerasScript.moveOrtoCamera(getDbWork().GetPathById(idPath).transform.position);
           // }


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

    //показать улицы в списке, принадлежащие только этому игроку
    public void ShowJustMineStreet(bool activeTogle)
    {
        if (activeTogle)
        {
            List<int> paths = getDbWork().GetMyPathes(getCurrentPlayer().IdPlayer);
            
            List<RectTransform> offThem = new List<RectTransform>();

            foreach (RectTransform rectTransform in streetsPathsRectTransforms)
            {
                if (rectTransform == null)
                {
                    continue;
                }
                //Debug.Log(rectTransform.GetSiblingIndex() + "     " + _dBwork.GetPathById(rectTransform.GetSiblingIndex()).NamePath);
                if (paths.Contains(rectTransform.GetSiblingIndex()+1))
                {
                    //Debug.Log("MINE");
                    paths.Remove(rectTransform.GetSiblingIndex()+1);
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
    private NetworkDBwork getDbWork()
    {
        if (_dBwork == null)
            _dBwork = Camera.main.GetComponent<NetworkDBwork>();

        return _dBwork;
    }

    //найти текущего игрока (работает только для игрока с ID = 1, для мультиплеера не подходит)
    private NetworkPlayer getCurrentPlayer()
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
        }
    }

    //ожидание ответа от игрока
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
                NetworkPathForBuy pathForBuy = getDbWork().GetPathForBuy(id);
                if (pathForBuy != null)
                {
                    info = "Название: " + pathForBuy.namePath + "\n" +
                           "Владелец: " + getDbWork().GetPlayerbyId(pathForBuy.IdPlayer).NickName +
                           "\n" + "Рента: " + pathForBuy.GetRenta() + "\n" + "Здания: " + pathForBuy.GetBuildsName()
                           + "\n\n Информация об улице: " +
                           getDbWork().getStreetById(pathForBuy.GetIdStreetParent()).AboutStreet;
                }
                else
                {
                    NetworkStreetPath path = getDbWork().GetPathById(id);
                    info = "Название: " + path.namePath + "\n" +
                           "Гос. учереждение \n\n Информация об улице: " +
                           getDbWork().getStreetById(path.GetIdStreetParent()).AboutStreet;
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

    //показать информацию о событии
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
        camerasScript.SetActiveFirstCamera();
        camerasScript.moveOrtoCamera(getDbWork()
            .GetPathById(getDbWork().GetPlayerbyId(idPlayer).GetCurrentStreetPath().GetIdStreetPath()).transform
            .position);

       
    }

    //открыть окно торговли с этим игроком
    public void onButtonClickTrade(int idPlayerSecond)
    {
        //открываем меню торговли
        ChangeMenu(4);
        //обновляем ссылку на дбворк
        _dBwork = getDbWork();
        //создаем список товаров на продажу
        NetworkTrade.CreateListThings(getCurrentPlayer(), _dBwork.GetPlayerbyId(idPlayerSecond));
        //вешаем скрипт на кнопку подтверждения предложения
        ApplyTrade.onClick.AddListener(() =>
            NetworkTrade.TradeApply(getCurrentPlayer(), _dBwork.GetPlayerbyId(idPlayerSecond), this, moneyFirstPlayer,
                moneySecondPlayer));
        //назначаем имя первого игрока
        firstPlayer.GetComponentInChildren<Text>().text = getCurrentPlayer().NickName;
        //узнаем улицы первого игрока
        List<int> pathsFirstPlayer = _dBwork.GetMyPathes(getCurrentPlayer().IdPlayer);

        //если торгуем с другим игроком
        if (idPlayerSecond != getCurrentPlayer().IdPlayer)
        {
            //назначаем имя второго игрока
            secondPlayer.GetComponentInChildren<Text>().text = _dBwork.GetPlayerbyId(idPlayerSecond).NickName;

            //собираем список улиц первого игрока в первом скроле
            foreach (var path in pathsFirstPlayer)
            {
                //если улица заложена, то она не отображается доступной к продаже
                if (_dBwork.GetPathForBuy(path).IsBlocked)
                    continue;

                GameObject prefButton = Instantiate(prefButStreetForTrade);
                prefButton.GetComponentInChildren<Text>().text = _dBwork.GetPathById(path).namePath;
                prefButton.GetComponent<RectTransform>().SetParent(scrollFirstPlayerStreets.content, false);
                prefButton.GetComponent<Button>().onClick
                    .AddListener(() =>
                        onButtonAddOrDeleteOfferStreet(prefButton, getCurrentPlayer(),
                            _dBwork.GetPlayerbyId(idPlayerSecond), path));
            }

            //создаем список улиц второго игрока
            List<int> pathsSecondPlayer = _dBwork.GetMyPathes(idPlayerSecond);

            //собираем список улиц второго игрока во втором скроле
            foreach (var path in pathsSecondPlayer)
            {
                //если улица заложена, то она не отображается доступной к продаже
                if (_dBwork.GetPathForBuy(path).IsBlocked)
                    continue;

                GameObject prefButton = Instantiate(prefButStreetForTrade);
                prefButton.GetComponentInChildren<Text>().text = _dBwork.GetPathById(path).namePath;
                prefButton.GetComponent<RectTransform>().SetParent(scrollSecondPlayerStreets.content, false);
                prefButton.GetComponent<Button>().onClick
                    .AddListener(() =>
                        onButtonAddOrDeleteOfferStreet(prefButton, getCurrentPlayer(),
                            _dBwork.GetPlayerbyId(idPlayerSecond), path));
            }

            //отражаем максимальное количество денег первого игрока 
            sliderMoneyFirst.maxValue = getCurrentPlayer().Money;
            //включаем слайдер первого игрока
            sliderMoneyFirst.gameObject.SetActive(true);
            //отражаем максимальное количество денег второго игрока 
            sliderMoneySecond.maxValue = _dBwork.GetPlayerbyId(idPlayerSecond).Money;
            //включаем слайдер второго игрока
            sliderMoneySecond.gameObject.SetActive(true);
            
            //разрешаем заполнение игроком полей денег
            InputFieldMoneyFirst.gameObject.GetComponent<CanvasGroup>().interactable = true;
            InputFieldMoneySecond.gameObject.GetComponent<CanvasGroup>().interactable = true;
        }
        //если открыли меню закладывания улиц
        else
        {
            //назначаем город вторым участником обмена
            secondPlayer.GetComponentInChildren<Text>().text = "Город";
            //Выключаем слайдеры
            sliderMoneyFirst.gameObject.SetActive(false);
            sliderMoneySecond.gameObject.SetActive(false);
            //запрещаем заполнение игроком полей денег
            InputFieldMoneyFirst.gameObject.GetComponent<CanvasGroup>().interactable = false;
            InputFieldMoneySecond.gameObject.GetComponent<CanvasGroup>().interactable = false;

            //раскидываем улицы по скролам в зависимости от того заложены они или нет
            foreach (var path in pathsFirstPlayer)
            {
                GameObject prefButton = Instantiate(prefButStreetForTrade);
                prefButton.GetComponentInChildren<Text>().text = _dBwork.GetPathById(path).namePath;            
                
                if (_dBwork.GetPathForBuy(path).IsBlocked)
                {
                    prefButton.GetComponent<RectTransform>().SetParent(scrollSecondPlayerStreets.content, false);
                }
                else
                {
                    prefButton.GetComponent<RectTransform>().SetParent(scrollFirstPlayerStreets.content, false);
                }

                prefButton.GetComponent<Button>().onClick
                    .AddListener(() =>
                        onButtonAddOrDeleteOfferStreet(prefButton, getCurrentPlayer(),
                            getCurrentPlayer(), path));
            }
        }
    }


    private void Start()
    {
        getDbWork().SetGameCanvas(this);
    }


    //очистить канву торговли
    public void ClearTradeMenu()
    {
        if (scrollFirstPlayerStreets.content.childCount != 0)
        {
            for (int i = scrollFirstPlayerStreets.content.childCount - 1; i >= 0; i--)
            {
                Destroy(scrollFirstPlayerStreets.content.GetChild(i).gameObject);
            }
        }

        if (scrollFirstPlayerOffer.content.childCount != 0)
        {
            for (int i = scrollFirstPlayerOffer.content.childCount - 1; i >= 0; i--)
            {
                Destroy(scrollFirstPlayerOffer.content.GetChild(i).gameObject);
            }
        }

        if (scrollSecondPlayerOffer.content.childCount != 0)
        {
            for (int i = scrollSecondPlayerOffer.content.childCount - 1; i >= 0; i--)
            {
                Destroy(scrollSecondPlayerOffer.content.GetChild(i).gameObject);
            }
        }

        if (scrollSecondPlayerStreets.content.childCount != 0)
        {
            for (int i = scrollSecondPlayerStreets.content.childCount - 1; i >= 0; i--)
            {
                Destroy(scrollSecondPlayerStreets.content.GetChild(i).gameObject);
            }
        }
        
        //сбрасываем параметры слайдер и полей для ввода
        moneyFirstPlayer = 0;
        moneySecondPlayer = 0;
        InputFieldMoneyFirst.text = "0";
        InputFieldMoneySecond.text = "0";
        ChangeMenu(1);
    }

    //добавить или убрать кнопку в список предложений от игрока
    private void onButtonAddOrDeleteOfferStreet(GameObject button, NetworkPlayer playerOne, NetworkPlayer playerTwo, int idPath)
    {
        string nameParent = button.transform.parent.parent.parent.gameObject.name;

        switch (nameParent)
        {
            case "FirstPlayerItem":
                button.GetComponent<RectTransform>().SetParent(scrollFirstPlayerOffer.content, false);
                NetworkTrade.AddItemToList(playerOne, playerTwo, getDbWork().GetPathForBuy(idPath));
                //если слайдеры не активны, то происходит закладывание улиц
                if (!sliderMoneyFirst.gameObject.activeInHierarchy)
                {
                    moneySecondPlayer += _dBwork.GetPathForBuy(idPath).PriceStreetPath;
                    OnValueChangedSecondInputFielid(moneySecondPlayer.ToString());
                }
                break;
            case "FirstPlayerOffer":
                button.GetComponent<RectTransform>().SetParent(scrollFirstPlayerStreets.content, false);
                NetworkTrade.RemoveItemFromList(playerOne, playerTwo, getDbWork().GetPathForBuy(idPath));
                if (!sliderMoneyFirst.gameObject.activeInHierarchy)
                {
                    moneySecondPlayer -= _dBwork.GetPathForBuy(idPath).PriceStreetPath;
                    OnValueChangedSecondInputFielid(moneySecondPlayer.ToString());
                }
                break;
            case "SecondPlayerItem":
                button.GetComponent<RectTransform>().SetParent(scrollSecondPlayerOffer.content, false);
                NetworkTrade.AddItemToList(playerTwo, playerOne, getDbWork().GetPathForBuy(idPath));
                if (!sliderMoneyFirst.gameObject.activeInHierarchy)
                {
                    moneyFirstPlayer += _dBwork.GetPathForBuy(idPath).PriceStreetPath;
                    OnValueChangedFirstInputFielid(moneyFirstPlayer.ToString());
                }
                break;
            case "SecondPlayerOffer":
                button.GetComponent<RectTransform>().SetParent(scrollSecondPlayerStreets.content, false);
                NetworkTrade.RemoveItemFromList(playerTwo, playerOne, getDbWork().GetPathForBuy(idPath));
                if (!sliderMoneyFirst.gameObject.activeInHierarchy)
                {
                    moneyFirstPlayer -= _dBwork.GetPathForBuy(idPath).PriceStreetPath;
                    OnValueChangedFirstInputFielid(moneyFirstPlayer.ToString());
                }
                break;
        }
    }

    //отменить предложение торговли
    public void CancelTrade()
    {
        ClearTradeMenu();
        ChangeMenu(1);
    }

    //улица, для которой открыты здания
    private int currentPathB;

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
                        
                        Debug.Log(rectTransform.GetSiblingIndex() + "     " + _dBwork.GetPathById(rectTransform.GetSiblingIndex()).NamePath);
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
            if (type == 3 && currentPathB != idPath)
            {
                for (int i = scroll.content.childCount - 1; i >= 0; i--)
                    Destroy(scroll.content.GetChild(i).gameObject);

                CreateBuildsButtons(idPath);
                foreach (RectTransform rectTransform in buildsRectTransforms)
                {
                    rectTransform.SetParent(scroll.content, false);
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
                    ImportantInfoAboutStreetText.transform.parent.gameObject.SetActive(false);
                    buildsButton.gameObject.SetActive(false);
                }
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
        camerasScript.SetActiveFirstCamera();
    }

    public void ChangeSoundLevel(float input)
    {
        GameMixer.SetFloat("Sound", input);
    }

    public void ChangeMusicLevel(float input)
    {
        GameMixer.SetFloat("Music", input);
    }

    public void OnOffSavedButtons()
    {
        saveAsNewButton.interactable = !saveAsNewButton.interactable;
        saveButton.interactable = !saveButton.interactable;
    }
    
}