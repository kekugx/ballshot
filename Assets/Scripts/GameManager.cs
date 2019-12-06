using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public static GameManager instance = null;
  
    public Material[] PlayerColors, WallColors;
    public GameObject AllSound;

    public event Action GameOverEvent;
    public event Action SectionClearedEvent;
    public event Action LevelClearedEvent;

    private int gem, level = 1;
    public float Max = 10f, Min = -10f;

    public GameObject[] Sections;
    private int _currentSection;

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
        else if (instance != this)
        {
            gameObject.SetActive(false);
            Destroy(gameObject);
        }
    }

    void Start()
    {
        for (int i = 1; i < Sections.Length; i++)
        {
            Sections[i].SetActive(false);
        }
        GetDatas();
        UIManage.instance.WriteUI();
        GameOverEvent += GameOver;
        SectionClearedEvent += OnSectionClearedEvent;
        LevelClearedEvent += WinLevel;
    }

    private void OnSectionClearedEvent()
    {
        _currentSection++;
        if (_currentSection == Sections.Length)
        {
            LevelClearedEvent?.Invoke();
            return;
        }

        Sections[_currentSection].SetActive(true);
        Invoke("DeactiveOldSection", 1.5f);
    }

    private void DeactiveOldSection()
    {
        Sections[_currentSection - 1].SetActive(false);
    }

    public void GetDatas()
    {
        // LEVEL
        if (PlayerPrefs.HasKey("level"))
        {
            level = PlayerPrefs.GetInt("level");
        }
        else
        {
            PlayerPrefs.SetInt("level", 1);
        }

        // GEM
        if (PlayerPrefs.HasKey("gem"))
        {
            gem = PlayerPrefs.GetInt("gem");
        }
        else
        {
            PlayerPrefs.SetInt("gem", 10);
        }

        // SOUND
        if (!PlayerPrefs.HasKey("sound"))
        {
            PlayerPrefs.SetInt("sound", 1);
        }
    }

    public void NextLevel()
    {
        UIManage.instance.WinPanel.SetActive(false);
        // Load New Level
    }

    /// <summary>
    /// Oyunun restartı
    /// </summary>
    public void RestartGame()
    {
        
        UIManage.instance.menuPanel.SetActive(true);
        UIManage.instance.GameOverPanel.SetActive(false);
        UIManage.instance.WriteUI();
        Application.LoadLevel("Demo Day Texas");
    }

    public void WinLevel()
    {
        
        LevelUp();
        AddGem(30);
        StartCoroutine(openWinPanel());
    }

    IEnumerator openWinPanel()
    {
        yield return new WaitForSeconds(2);
        UIManage.instance.WinPanel.SetActive(true);
    }

    public void RequestGameOverEvent()
    {
        GameOverEvent?.Invoke();
    }

    /// <summary>
    /// This method requests a SectionClearedEvent. 
    /// </summary>
    public void RequestSectionClearedEvent()
    {
        SectionClearedEvent?.Invoke();
    }

    public void GameOver()
    {
        UIManage.instance.GameOverPanel.SetActive(true);
        
    }

    public void LevelUp()
    {
        level++;
        int prevLevel = PlayerPrefs.GetInt("level");
        PlayerPrefs.SetInt("level", prevLevel + 1);
    }

    public void AddGem(int newGem)
    {
        int prevGem = PlayerPrefs.GetInt("gem");
        PlayerPrefs.SetInt("gem", prevGem + newGem);
        gem = newGem;
    }
}