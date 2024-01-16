using System.IO;
using System.Linq;
using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
public class Movement : MonoBehaviour
{
   

    public float speed = 8f;
    public float speedMultiplier = 1f;
    public Vector2 initialDirection;
    public LayerMask obstacleLayer;

    public new Rigidbody2D rigidbody { get; private set; }
    public Vector2 direction { get; private set; }
    public Vector2 nextDirection { get; private set; }
    public Vector3 startingPosition { get; private set; }


    //Метод который принимает 2d тело и приравнивает стартовой позиции текущее положение
    private void Awake()
    {
        rigidbody = GetComponent<Rigidbody2D>();
        startingPosition = transform.position;
    }

    //Данный метод сбрасывает состояния призраков и Пакмана до первоначальных
    private void Start()
    {
        ResetState();
        GhostResetState();
    }

    //Метод сброса Пакмана
    public void ResetState()
    {
        string[] jsonStrings = File.ReadAllLines("UserSettings.json");
        float[] numbers = jsonStrings.Select(s => float.Parse(s)).ToArray();
        speed = numbers[1];
        speedMultiplier = 1f;
        direction = initialDirection;
        nextDirection = Vector2.zero;
        transform.position = startingPosition;
        rigidbody.isKinematic = false;
        enabled = true;
    }
    
    //Метод сброса призраков
    public void GhostResetState()
    {
        string[] jsonStrings = File.ReadAllLines("UserSettings.json");
        float[] numbers = jsonStrings.Select(s => float.Parse(s)).ToArray();
        speed = numbers[2];
        speedMultiplier = 1f;
        direction = initialDirection;
        nextDirection = Vector2.zero;
        transform.position = startingPosition;
        rigidbody.isKinematic = false;
        enabled = true;
    }

    //Метод который динамически проверяет, если следующее направление не пустое тогда направлению установится следующее направление
    private void Update()
    {

        if (nextDirection != Vector2.zero) {
            SetDirection(nextDirection);
        }
    }

    //Этот метод используется для перемещение объектов на определенное растояние за определенное время
    private void FixedUpdate()
    {
        Vector2 position = rigidbody.position;
        Vector2 translation = direction * speed * speedMultiplier * Time.fixedDeltaTime;
        rigidbody.MovePosition(position + translation);
    }

    //Этот метод проверяет, если принудительное направление движения ложное или направление не занято тогда выполняется это направление а следующее направление становится нулевым, если же нет тогда следующее направление приравнивается текущему
    public void SetDirection(Vector2 direction, bool forced = false)
    {

        if (forced || !Occupied(direction))
        {
            this.direction = direction;
            nextDirection = Vector2.zero;
        }
        else
        {
            nextDirection = direction;
        }
    }
    //Этот метод отслеживает при помощи ударов лучей не занято ли текущее направление
    public bool Occupied(Vector2 direction)
    {
        RaycastHit2D hit = Physics2D.BoxCast(transform.position, Vector2.one * 0.75f, 0f, direction, 1.5f, obstacleLayer);
        return hit.collider != null;
    }

}
