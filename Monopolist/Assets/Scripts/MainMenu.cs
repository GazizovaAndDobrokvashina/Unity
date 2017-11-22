using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine;
using System.Linq;
using UnityEngine.SceneManagement;


public class MainMenu : MonoBehaviour {

    public GameObject MainMenuObj;
    public GameObject CreateNewGameObj;
    public GameObject ContinueObj;
    public GameObject BackButton;
    public GameObject SettingsMenu;
    public Transform button;
    public Slider sliderCountOfPlayers;
    public Slider sliderSoundVolume;
    public Slider sliderMusicVolume;
    public Text countOfPlayersText;
    public ScrollRect scroll;
    private int countOfPlayers;
    private int maxcountOfPlayers = 4;
    private int mincountOfPlayers = 1;
    

    private void Start()
    {
        sliderCountOfPlayers.minValue = mincountOfPlayers;
        sliderCountOfPlayers.maxValue = maxcountOfPlayers;
        
        int gap = 0;
        List<string> names = SaveLoad.loadGamesList();
        foreach(string dbName in names) {
			
            Transform but = Instantiate (button) as Transform;
            //but.SetParent (ContinueObj.transform);
            but.SetParent (scroll.content,false);
            RectTransform tr = but.GetComponent<RectTransform> ();
            //tr.anchoredPosition = new Vector2 (0,gap);
            //gap -= -39;
            but.GetComponentInChildren<Text> ().text = dbName;
            Button b = but.GetComponent<Button> ();
            b.onClick.AddListener (() => onButtonClick(dbName));

        }
    }

    private void Update()
    {
        if (!sliderCountOfPlayers.IsActive()) return;
        countOfPlayers = (int) sliderCountOfPlayers.value;
        countOfPlayersText.text = "Количество игроков: " + (int) sliderCountOfPlayers.value;
    }


    public void OpenMenuNewGame()
    {
        MainMenuObj.SetActive (false);
        ContinueObj.SetActive (false);
        BackButton.SetActive(true);
        CreateNewGameObj.SetActive (true);

    }

    public void OpenMenuLoadGame()
    {
        MainMenuObj.SetActive (false);
        ContinueObj.SetActive (true);
        BackButton.SetActive(true);
        CreateNewGameObj.SetActive (false);
        
        

    }

    public void OpenSettings()
    {
        MainMenuObj.SetActive (false);
        ContinueObj.SetActive (false);
        BackButton.SetActive(true);
        SettingsMenu.SetActive(true);
    }

    private void onButtonClick(string dbName){
       SaveLoad.loadGame(dbName);
        SceneManager.LoadScene("Game");

    }

    
    public void BackToMainMenu()
    {
        MainMenuObj.SetActive (true);
        ContinueObj.SetActive (false);
        BackButton.SetActive(false);
        CreateNewGameObj.SetActive (false);  
        SettingsMenu.SetActive(false);
    }

    public void QuitGame()
    {
        Application.Quit();
    }

    public void StartNewGame()
    {
        SceneManager.LoadScene("Game");
    }
}
