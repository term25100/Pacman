using System.IO;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.SocialPlatforms.Impl;
using UnityEngine.UI;

public class Open : MonoBehaviour
{
    public Sprite audioOn;
    public Sprite audioOff;
    public Sprite Play;
    public Sprite Pause;
    public GameObject AudioButton;
    public GameObject PlayButton;
    public Text PacmanValue;
    public Text GhostsValue;
    public new Slider audio;
    public Slider PacmanSpeed;
    public Slider GhostsSpeed;

    public AudioClip clip;
    public AudioSource source;
    public AudioSource gameSound;
    public InputField inputName;

    public string previousScene;
    void Update()
    {
        source.volume = audio.value;
        PacmanValue.text=PacmanSpeed.value.ToString();
        GhostsValue.text=GhostsSpeed.value.ToString();

        //if (audio.value == 0)
        //{
        //    AudioButton.GetComponent<Image>().sprite = audioOff;
        //}else if (audio.value > 0.0)
        //{
        //    AudioButton.GetComponent<Image>().sprite = audioOn;
        //}
        
    }

    private void Awake()
    {
        string[] startSettings = File.ReadAllLines("UserSettings.json");
        audio.value = float.Parse(startSettings[0]);
        PacmanSpeed.value = float.Parse(startSettings[1]);
        GhostsSpeed.value = float.Parse(startSettings[2]);
        PacmanValue.text = startSettings[1];
        GhostsValue.text = startSettings[2];
    }
    private void Start()
    {
        string[] name = File.ReadAllLines("Name.json");
        inputName.text = name[0];
        //string[] startSettings = File.ReadAllLines("UserSettings.json");
        //audio.value = float.Parse(startSettings[0]);
        //PacmanSpeed.value = float.Parse(startSettings[1]);
        //GhostsSpeed.value = float.Parse(startSettings[2]);
        //PacmanValue.text = startSettings[1];
        //GhostsValue.text = startSettings[2];
    }
    public void GetScene()
    {
        previousScene = SceneManager.GetActiveScene().name;
        Debug.Log("Этот метод сработал!");
        File.WriteAllText("GetScene.json", previousScene);
    }
    public void OnOffAudio()
    {
        if (AudioListener.volume == 1)
        {
            AudioListener.volume = 0;
            audio.value=0;
            AudioButton.GetComponent<Image>().sprite = audioOff;
        }
        else if (AudioListener.volume == 0)
        {
            AudioListener.volume = 1;
            string[] startSettings = File.ReadAllLines("UserSettings.json");
            audio.value = float.Parse(startSettings[0]);
            AudioButton.GetComponent<Image>().sprite = audioOn;
        }
    }

    public void PlayOrPause()
    {
        if (Time.timeScale == 1)
        {
            Time.timeScale = 0;
            gameSound.Pause();
            PlayButton.GetComponent<Image>().sprite = Pause;
        }else if(Time.timeScale == 0)
        {
            Time.timeScale = 1;
            gameSound.Play();
            PlayButton.GetComponent <Image>().sprite = Play;
        }
    }
    public void StartGame()
    {
        File.WriteAllText("Name.json", string.Empty);
        using (StreamWriter file = new StreamWriter("Name.json", true))
        {
            file.WriteLine(inputName.text);
        }
        previousScene = SceneManager.GetActiveScene().name;
        Debug.Log("Этот метод сработал!");
        File.WriteAllText("GetScene.json", previousScene);
        SceneManager.LoadScene("Pacman");
    }
    public void StartLevel()
    {
        SceneManager.LoadScene("Pacman");
    }
    public void OpenSettings()
    {
        
        SceneManager.LoadScene("Settings");
    }

    public void OpenLevels()
    {
        SceneManager.LoadScene("SelectLevel");
    }

    public void OpenTable()
    {
        SceneManager.LoadScene("TableScores");
    }

    public void BackMenu()
    {
        string[] sceneName = File.ReadAllLines("GetScene.json");
        if(SceneManager.GetActiveScene().name != sceneName[0]) {
            SceneManager.LoadScene(sceneName[0]);
            Debug.Log("Нажата кнопка выхода из настроек!");
        }
        else
        {
            SceneManager.LoadScene("Menu");
        }
        
    }

    

    public void Confirm()
    {
        string Volume = source.volume.ToString();
        Debug.Log("Внесено значение звука: " + Volume);
        string speedP = PacmanSpeed.value.ToString();
        Debug.Log("Внесено значение скорости Пакмана: " + speedP);
        string speedG = GhostsSpeed.value.ToString();
        Debug.Log("Внесено значение Скорости призрака: " + speedG);
        File.WriteAllText("UserSettings.json", string.Empty);
        using (StreamWriter file = new StreamWriter("UserSettings.json", true))
        {
            file.WriteLine(Volume);
            file.WriteLine(speedP);
            file.WriteLine(speedG);
        }
    }
}


