using System;
using System.IO;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;
public class GameManager : MonoBehaviour
{
    public static GameManager Instance { get; private set; }

    [SerializeField] private Ghost[] ghosts;
    [SerializeField] private Pacman pacman;
    [SerializeField] private Transform pellets;
    [SerializeField] private Text gameOverText;
    [SerializeField] private Text NewRecord;
    [SerializeField] private Text scoreText;
    [SerializeField] private Text livesText;

    private int ghostMultiplier = 1;
    private int lives = 3;
    private int score = 0;
    private AudioSource PacmanDeath;
    private AudioSource GhostDeath;
    private AudioSource mainSound;

    public int Lives => lives;
    public int Score => score;

    //Метод который инициируется при открытии экземпляра или сцены Pacman
    private void Awake()
    {
        //if (Instance != null) {
        //    DestroyImmediate(gameObject);
        //} else {
        //    Instance = this;
        //    DontDestroyOnLoad(gameObject);
        //}
        //Если есть подобный экземпляр мы его разрушаем чтобы не вызвать ошибок или чтобы они не перекрывали друг друга
        if (Instance == this) {
        Destroy(gameObject);
        } else {
        Instance = this;
        DontDestroyOnLoad(gameObject);
        }
    }

    //Этот метод запускается вместе со сценой Pacman и запускает новую игру
    private void Start()
    {
        NewGame();
        PacmanDeath = GameObject.Find("PacmanDeath").GetComponent<AudioSource>();
        GhostDeath = GameObject.Find("GhostDeath").GetComponent<AudioSource>();
    }

    //Данный метод отслеживает динамические данные или обновляет их в данном случае проверяет если игра окончена и нажата любая клавиша игра начнется заново
    private void Update()
    {
        if (lives <= 0 && Input.anyKeyDown) {
            NewGame();
        }
    }

    //Метод который отвечает за запуск новой игры и устанавливает значения жизней и очков по умолчанию
    private void NewGame()
    {
        SetScore(0);
        SetLives(3);
        NewRound();
    }

    //Этот метод запускает новый игровой раунд
    private void NewRound()
    {
        mainSound = GameObject.Find("mainSound").GetComponent<AudioSource>();
        mainSound.Play();
        gameOverText.enabled = false;
        NewRecord.enabled = false;
        foreach (Transform pellet in pellets) {
            pellet.gameObject.SetActive(true);
        }

        ResetState();
    }

    //Данный метод отвечает за сброс объектов призрака и пакмана до первоначальных состояний
    private void ResetState()
    {
        for (int i = 0; i < ghosts.Length; i++) {
            ghosts[i].ResetState();
        }

        pacman.ResetState();
    }

    //Данный метод вызывается когда игра закончена
    private void GameOver()
    {
        mainSound = GameObject.Find("mainSound").GetComponent<AudioSource>();
        mainSound.Pause();
        gameOverText.enabled = true;
        string[] name = File.ReadAllLines("Name.json");
        DateTime Date= DateTime.Now;
        string currentDate= Date.ToString("dd.MM.yyyy");

        string[] jsonStrings = File.ReadAllLines("Scores.json");
        int[] numbers = jsonStrings.Select(s => int.Parse(s)).ToArray();
        Array.Sort(numbers);
        Array.Reverse(numbers);
        if (score > numbers[0])
        {
            NewRecord.enabled=true;
        }
        string jsonData = score.ToString();
        using (StreamWriter file = new StreamWriter("Scores.json", true))
        {
            file.WriteLine(jsonData);
        }
        using (StreamWriter file = new StreamWriter("Records.json", true))
        {
            file.WriteLine(name[0]+" "+score.ToString()+" "+currentDate);
        }
        for (int i = 0; i < ghosts.Length; i++) {
            ghosts[i].gameObject.SetActive(false);
        }

        pacman.gameObject.SetActive(false);
    }

    //этот метод устанавливает в поле текст текущие жизни пакмана
    private void SetLives(int lives)
    {
        this.lives = lives;
        livesText.text = "x" + lives.ToString();
    }

    //Этот метод устанавливает в поле текста очков текущие набранные очки
    private void SetScore(int score)
    {
        this.score = score;
        scoreText.text = score.ToString().PadLeft(2, '0');
    }

    //Этот метод вызывается тогда когда пакмана съели и проверяет количество жизней если их больше нуля тогда сбрасываем состояние пакмана если нет тогда игра окончена
    public void PacmanEaten()
    {
        pacman.DeathSequence();
        PacmanDeath.Play();
        SetLives(lives - 1);

        if (lives > 0) {
            Invoke(nameof(ResetState), 3f);
        } else {
            GameOver();
        }
    }
    //Этот метод вызывается тогда, когда призрак был съеден
    public void GhostEaten(Ghost ghost)
    {
        int points = ghost.points * ghostMultiplier;
        SetScore(score + points);
        GhostDeath.Play();  
        ghostMultiplier++;
    }

    //этот метод вызывается когда съедается каждая гранула
    public void PelletEaten(Pellet pellet)
    {
        pellet.gameObject.SetActive(false);

        SetScore(score + pellet.points);

        if (!HasRemainingPellets())
        {
            pacman.gameObject.SetActive(false);
            Invoke(nameof(NewRound), 3f);
        }
    }

    //Этот метод вызывается, если была съедена энергетическая гранула
    public void PowerPelletEaten(PowerPellet pellet)
    {
        for (int i = 0; i < ghosts.Length; i++) {
            ghosts[i].frightened.Enable(pellet.duration);
        }

        PelletEaten(pellet);
        CancelInvoke(nameof(ResetGhostMultiplier));
        Invoke(nameof(ResetGhostMultiplier), pellet.duration);
    }

    //Этот метод отслеживает остались ли на карте гранулы и в зависимости от результата вернет true либо false
    private bool HasRemainingPellets()
    {
        foreach (Transform pellet in pellets)
        {
            if (pellet.gameObject.activeSelf) {
                return true;
            }
        }

        return false;
    }

    //Этот метод используется для сброса переменной мультипликатора призрака
    private void ResetGhostMultiplier()
    {
        ghostMultiplier = 1;
    }

}
