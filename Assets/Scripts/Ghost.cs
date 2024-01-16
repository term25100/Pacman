using UnityEngine;

[DefaultExecutionOrder(-10)]
[RequireComponent(typeof(Movement))]
public class Ghost : MonoBehaviour
{
    public Movement movement { get; private set; }
    public GhostHome home { get; private set; }
    public GhostScatter scatter { get; private set; }
    public GhostChase chase { get; private set; }
    public GhostFrightened frightened { get; private set; }
    public GhostBehavior initialBehavior;
    public Transform target;
    public int points = 200;

    //Метод который запускается одновременно с экземпляром, здесь он получает компоненты движения и поведений призраков
    private void Awake()
    {
        movement = GetComponent<Movement>();
        home = GetComponent<GhostHome>();
        scatter = GetComponent<GhostScatter>();
        chase = GetComponent<GhostChase>();
        frightened = GetComponent<GhostFrightened>();
    }

    //Данный метод срабатыват при запуске данного класса и производит сброс призрака до первоначального состояния
    private void Start()
    {
        ResetState();
    }

    public void ResetState()
    {   
        //Метод который инициирует сброс объекта до первоначального состояния
        gameObject.SetActive(true);
        movement.GhostResetState();

        frightened.Disable();
        chase.Disable();
        scatter.Enable();

        if (home != initialBehavior) {
            home.Disable();
        }

        if (initialBehavior != null) {
            initialBehavior.Enable();
        }
    }

    //Метод который предназначен для установки новой позиции призрака
    public void SetPosition(Vector3 position)
    {
        position.z = transform.position.z;
        transform.position = position;
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        //Отслеживаем если объект с которым мы столкнулись Pacman если да и наш призрак испуган то Pacman съел призрака если наоборот то съели Pacmanа
        if (collision.gameObject.layer == LayerMask.NameToLayer("Pacman"))
        {
            if (frightened.enabled) {
                GameManager.Instance.GhostEaten(this);
            } else {
                GameManager.Instance.PacmanEaten();
            }
        }
    }

}
