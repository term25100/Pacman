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

    //����� ������� ������������ ��� �������� ���������� ��� ����� Pacman
    private void Awake()
    {
        //if (Instance != null) {
        //    DestroyImmediate(gameObject);
        //} else {
        //    Instance = this;
        //    DontDestroyOnLoad(gameObject);
        //}
        //���� ���� �������� ��������� �� ��� ��������� ����� �� ������� ������ ��� ����� ��� �� ����������� ���� �����
        if (Instance == this) {
        Destroy(gameObject);
        } else {
        Instance = this;
        DontDestroyOnLoad(gameObject);
        }
    }

    //���� ����� ����������� ������ �� ������ Pacman � ��������� ����� ����
    private void Start()
    {
        NewGame();
        PacmanDeath = GameObject.Find("PacmanDeath").GetComponent<AudioSource>();
        GhostDeath = GameObject.Find("GhostDeath").GetComponent<AudioSource>();
    }

    //������ ����� ����������� ������������ ������ ��� ��������� �� � ������ ������ ��������� ���� ���� �������� � ������ ����� ������� ���� �������� ������
    private void Update()
    {
        if (lives <= 0 && Input.anyKeyDown) {
            NewGame();
        }
    }

    //����� ������� �������� �� ������ ����� ���� � ������������� �������� ������ � ����� �� ���������
    private void NewGame()
    {
        SetScore(0);
        SetLives(3);
        NewRound();
    }

    //���� ����� ��������� ����� ������� �����
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

    //������ ����� �������� �� ����� �������� �������� � ������� �� �������������� ���������
    private void ResetState()
    {
        for (int i = 0; i < ghosts.Length; i++) {
            ghosts[i].ResetState();
        }

        pacman.ResetState();
    }

    //������ ����� ���������� ����� ���� ���������
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

    //���� ����� ������������� � ���� ����� ������� ����� �������
    private void SetLives(int lives)
    {
        this.lives = lives;
        livesText.text = "x" + lives.ToString();
    }

    //���� ����� ������������� � ���� ������ ����� ������� ��������� ����
    private void SetScore(int score)
    {
        this.score = score;
        scoreText.text = score.ToString().PadLeft(2, '0');
    }

    //���� ����� ���������� ����� ����� ������� ����� � ��������� ���������� ������ ���� �� ������ ���� ����� ���������� ��������� ������� ���� ��� ����� ���� ��������
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
    //���� ����� ���������� �����, ����� ������� ��� ������
    public void GhostEaten(Ghost ghost)
    {
        int points = ghost.points * ghostMultiplier;
        SetScore(score + points);
        GhostDeath.Play();  
        ghostMultiplier++;
    }

    //���� ����� ���������� ����� ��������� ������ �������
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

    //���� ����� ����������, ���� ���� ������� �������������� �������
    public void PowerPelletEaten(PowerPellet pellet)
    {
        for (int i = 0; i < ghosts.Length; i++) {
            ghosts[i].frightened.Enable(pellet.duration);
        }

        PelletEaten(pellet);
        CancelInvoke(nameof(ResetGhostMultiplier));
        Invoke(nameof(ResetGhostMultiplier), pellet.duration);
    }

    //���� ����� ����������� �������� �� �� ����� ������� � � ����������� �� ���������� ������ true ���� false
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

    //���� ����� ������������ ��� ������ ���������� ��������������� ��������
    private void ResetGhostMultiplier()
    {
        ghostMultiplier = 1;
    }

}
