using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIManager : MonoBehaviour
{
    public GameObject mainMenuPanel;
    public GameObject worldMapPanel;
    public GameObject levelSelectPanel;
    public GameObject gamePlayPanel;
    public GameObject pausePanel;
    public GameObject settingsPanel;
    public GameObject levelCompletePanel;
    public GameObject loadingPanel;
    void Start()
    {
        ShowMainMenu();
    }
    void HideAllPanels()
    {
        mainMenuPanel.SetActive(false);
        worldMapPanel.SetActive(false);
        levelSelectPanel.SetActive(false);
        gamePlayPanel.SetActive(false);
        pausePanel.SetActive(false);
        settingsPanel.SetActive(false);
        levelCompletePanel.SetActive(false);
        loadingPanel.SetActive(false);
    }
    public void ShowMainMenu()
    {
        HideAllPanels();
        mainMenuPanel.SetActive(true);
    }
    public void ShowWorldMap()
    {
        HideAllPanels();
        worldMapPanel.SetActive(true);
    }
    public void BackToMainMenu()
    {
        HideAllPanels();
        mainMenuPanel.SetActive(true);
    }
    public void ShowLevelSelect()
    {
        HideAllPanels();
        levelSelectPanel.SetActive(true);
    }
    public void BackToWorldMap()
    {
        HideAllPanels();
        worldMapPanel.SetActive(true);
    }
    public void ShowGamePlay()
    {
        HideAllPanels();
        gamePlayPanel.SetActive(true);
    }
    
}
