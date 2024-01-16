using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using System.IO;

[RequireComponent(typeof(Movement))]
public class Control : MonoBehaviour
{
    [SerializeField]
    private AnimatedSprite deathSequence;
    private SpriteRenderer spriteRenderer;
    private Movement movement;
    private new Collider2D collider;


    public float PacmanSpeed;
    public float GhostSpeed;
    public void JsonRead()
    {
        string jsonFilePath = "UserSettings.json";
        string[] lines = File.ReadAllLines(jsonFilePath);
        PacmanSpeed = float.Parse(lines[1]);
        GhostSpeed = float.Parse(lines[2]);
        Debug.Log(GhostSpeed);
    }


    public Button Up;
    public Button Down;
    public Button Left;
    public Button Right;
    private void Awake()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
        movement = GetComponent<Movement>();
        collider = GetComponent<Collider2D>();
    }
    public void goUp()
    {
        movement.SetDirection(Vector2.up);
        Debug.Log("Нажата кнопка вверх");
    }
    public void goDown()
    {
        movement.SetDirection(Vector2.down);
        Debug.Log("Нажата кнопка вниз");
    }
    public void goLeft()
    {
        movement.SetDirection(Vector2.left);
        Debug.Log("Нажата кнопка влево");
    }
    public void goRight()
    {
        movement.SetDirection(Vector2.right);
        Debug.Log("Нажата кнопка вправо");
    }
    private void Update()
    {

        if (Input.GetKeyDown(KeyCode.W) || Input.GetKeyDown(KeyCode.UpArrow))
        {
            goUp();
        }
        else if (Input.GetKeyDown(KeyCode.S) || Input.GetKeyDown(KeyCode.DownArrow))
        {
            goDown();
        }
        else if (Input.GetKeyDown(KeyCode.A) || Input.GetKeyDown(KeyCode.LeftArrow))
        {
            goLeft();
        }
        else if (Input.GetKeyDown(KeyCode.D) || Input.GetKeyDown(KeyCode.RightArrow))
        {
            goRight();
        }

        float angle = Mathf.Atan2(movement.direction.y, movement.direction.x);
        transform.rotation = Quaternion.AngleAxis(angle * Mathf.Rad2Deg, Vector3.forward);
    }

    public void ResetState()
    {
        enabled = true;
        spriteRenderer.enabled = true;
        collider.enabled = true;
        deathSequence.enabled = false;
        movement.ResetState();
        gameObject.SetActive(true);
    }

    public void DeathSequence()
    {
        enabled = false;
        spriteRenderer.enabled = false;
        collider.enabled = false;
        movement.enabled = false;
        deathSequence.enabled = true;
        deathSequence.Restart();
    }

}