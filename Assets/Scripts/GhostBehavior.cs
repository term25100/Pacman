using UnityEngine;

[RequireComponent(typeof(Ghost))]
public abstract class GhostBehavior : MonoBehaviour
{
    public Ghost ghost { get; private set; }
    public float duration;

    //����� �������� ��������� �������
    private void Awake()
    {
        ghost = GetComponent<Ghost>();
    }

    //���� ����� �������� ������������ ������ ���� ��������� � �������� �� � ���������������� �����
    public void Enable()
    {
        Enable(duration);
    }

    //����� ������� �������� ��������� �� ������ ���� � �������� �� �������� ����� CancelInvoke ���� ������ ��������� ��������
    public virtual void Enable(float duration)
    {
        enabled = true;

        CancelInvoke();
        Invoke(nameof(Disable), duration);
    }


    //����� ������� �������� ��������� �������� ��� ���� ������ ����� CancelInvoke ������� ������������� ��� ��������������� ������� Invoke ��������
    public virtual void Disable()
    {
        enabled = false;

        CancelInvoke();
    }

}
