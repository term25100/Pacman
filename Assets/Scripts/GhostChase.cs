using UnityEngine;

public class GhostChase : GhostBehavior
{
    //Данный метод включит состояние разброса если призракам не удастца поймать пакмана за отведенное время
    private void OnDisable()
    {
        ghost.scatter.Enable();
    }

    //Этот метод проверяет столкновения призрака с узлами и ищет самый короткий путь
    private void OnTriggerEnter2D(Collider2D other)
    {
        Node node = other.GetComponent<Node>();

        if (node != null && enabled && !ghost.frightened.enabled)
        {
            Vector2 direction = Vector2.zero;
            float minDistance = float.MaxValue;

            foreach (Vector2 availableDirection in node.availableDirections)
            {
                Vector3 newPosition = transform.position + new Vector3(availableDirection.x, availableDirection.y);
                float distance = (ghost.target.position - newPosition).sqrMagnitude;

                if (distance < minDistance)
                {
                    direction = availableDirection;
                    minDistance = distance;
                }
            }

            ghost.movement.SetDirection(direction);
        }
    }

}
