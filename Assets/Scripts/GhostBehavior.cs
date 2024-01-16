using UnityEngine;

[RequireComponent(typeof(Ghost))]
public abstract class GhostBehavior : MonoBehaviour
{
    public Ghost ghost { get; private set; }
    public float duration;

    //Метод получает компонент призрак
    private void Awake()
    {
        ghost = GetComponent<Ghost>();
    }

    //этот метод включает длительность какого либо поведения и передает ее в переопределенный метод
    public void Enable()
    {
        Enable(duration);
    }

    //Метод который включает поведение на особый срок с таймером но вызывает метод CancelInvoke если таймер прийдется обновить
    public virtual void Enable(float duration)
    {
        enabled = true;

        CancelInvoke();
        Invoke(nameof(Disable), duration);
    }


    //Метод который выключит состояние призрака при этом вызвав метод CancelInvoke который дезактивирует все запланированные методом Invoke действия
    public virtual void Disable()
    {
        enabled = false;

        CancelInvoke();
    }

}
