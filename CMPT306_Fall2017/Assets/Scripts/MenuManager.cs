﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class MenuManager : MonoBehaviour
{

    public InputManagerScript curInputManager;

    public string mainSceneName;
    public string menuSceneName;
    public string easyScene;
	public string splashScreenName;

    public Canvas MainMenu;
    public Canvas Credits;
    public Canvas PlayerHUD;
    public Canvas DeathPanel;

    public void LoadMainScene()
    {
        SceneManager.LoadScene(mainSceneName);
    }
    
    public void LoadEasyScene()
    {
        SceneManager.LoadScene(easyScene);
    }

    public void LoadMainMenu()
    {
        SceneManager.LoadScene(menuSceneName);
    }

	public void LoadSplashScreen(){
		SceneManager.LoadScene(splashScreenName);
	}

    public void ExitGame()
    {
        Application.Quit();
    }

    public void LoadCreditCanvas()
    {
        MainMenu.enabled = false;
        Credits.enabled = true;
    }

    public void LoadMainMenuCanvas()
    {
        Credits.enabled = false;
        MainMenu.enabled = true;
    }

    public void ShowDeathPanel()
    {
        PlayerHUD.enabled = false;
        DeathPanel.enabled = true;

        //curInputManager.inputEnabled = false;
    }

    // Use this for initialization
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }
}
