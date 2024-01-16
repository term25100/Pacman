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


    //����� ������� ��������� 2d ���� � ������������ ��������� ������� ������� ���������
    private void Awake()
    {
        rigidbody = GetComponent<Rigidbody2D>();
        startingPosition = transform.position;
    }

    //������ ����� ���������� ��������� ��������� � ������� �� ��������������
    private void Start()
    {
        ResetState();
        GhostResetState();
    }

    //����� ������ �������
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
    
    //����� ������ ���������
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

    //����� ������� ����������� ���������, ���� ��������� ����������� �� ������ ����� ����������� ����������� ��������� �����������
    private void Update()
    {

        if (nextDirection != Vector2.zero) {
            SetDirection(nextDirection);
        }
    }

    //���� ����� ������������ ��� ����������� �������� �� ������������ ��������� �� ������������ �����
    private void FixedUpdate()
    {
        Vector2 position = rigidbody.position;
        Vector2 translation = direction * speed * speedMultiplier * Time.fixedDeltaTime;
        rigidbody.MovePosition(position + translation);
    }

    //���� ����� ���������, ���� �������������� ����������� �������� ������ ��� ����������� �� ������ ����� ����������� ��� ����������� � ��������� ����������� ���������� �������, ���� �� ��� ����� ��������� ����������� �������������� ��������
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
    //���� ����� ����������� ��� ������ ������ ����� �� ������ �� ������� �����������
    public bool Occupied(Vector2 direction)
    {
        RaycastHit2D hit = Physics2D.BoxCast(transform.position, Vector2.one * 0.75f, 0f, direction, 1.5f, obstacleLayer);
        return hit.collider != null;
    }

}
