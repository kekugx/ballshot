using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIManage : MonoBehaviour
{
    public static UIManage instance = null;

    public GameObject menuPanel, soundButton, WinPanel, GameOverPanel;
    public Text GemText, LevelText;
    public Sprite[] soundIcons;
    private InputManager _inputManager;

    private void Awake()
    {
        Application.targetFrameRate = 60;
        QualitySettings.vSyncCount = 0;
        
//        Screen.SetResolution(720, 1280, true);
        
        if (instance == null)

            instance = this;

        else if (instance != this)

            Destroy(gameObject);
        
        
    }

    private void Start()
    {
        WriteUI();
        _inputManager = FindObjectOfType<InputManager>();
        _inputManager.TouchStartEvent += CloseTutorialOnStart;
    }

    private void CloseTutorialOnStart(Vector2 obj)
    {
        menuPanel.SetActive(false);
        _inputManager.TouchStartEvent -= CloseTutorialOnStart;
    }

    public void WriteUI()
    {
        GemText.text = PlayerPrefs.GetInt("gem").ToString();
        LevelText.text = "LEVEL " + PlayerPrefs.GetInt("level").ToString();
    }

    public void soundControl()
    {
        int soundBool = PlayerPrefs.GetInt("sound");
        // Sound Open
        if (soundBool == 1)
        {
            GameManager.instance.AllSound.SetActive(true);
            soundButton.GetComponent<Image>().sprite = soundIcons[0];
        }
        // Sound Mute
        else
        {
            GameManager.instance.AllSound.SetActive(false);
            soundButton.GetComponent<Image>().sprite = soundIcons[1];
        }
    }

    public void SoundButton()
    {
        int soundBool = PlayerPrefs.GetInt("sound");

        // Sound OFF
        if (soundBool == 1)
        {
            PlayerPrefs.SetInt("sound", 0);
            GameManager.instance.AllSound.SetActive(false);
            soundButton.GetComponent<Image>().sprite = soundIcons[1];
        }
        // Sound ON
        else
        {
            PlayerPrefs.SetInt("sound", 1);
            GameManager.instance.AllSound.SetActive(true);
            soundButton.GetComponent<Image>().sprite = soundIcons[0];
        }
    }
}
