using System.Collections;
using UnityEngine;

public class GhostHome : GhostBehavior
{
    public Transform inside;
    public Transform outside;

    //Этот метод отключает все корутины если активно домашнее состояние
    private void OnEnable()
    {
        StopAllCoroutines();
    }

    //Этот метод проверяет если игровой объект является активным объектом иерархии тогда он запускает корутину ExitTransition()
    private void OnDisable()
    {

        if (gameObject.activeInHierarchy) {
            StartCoroutine(ExitTransition());
        }
    }
    //Этот метод срабатывает когда объект призрака сталкивается с препятствием в виде стены и меняет его направление на обратное
    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (enabled && collision.gameObject.layer == LayerMask.NameToLayer("Obstacle")) {
            ghost.movement.SetDirection(-ghost.movement.direction);
        }
    }

    //Этот метод является корутиной которая при помощи линейной интерполяции плавно перемещает нашего призрака  из одной точки в другую
    private IEnumerator ExitTransition()
    {

        ghost.movement.SetDirection(Vector2.up, true);
        ghost.movement.rigidbody.isKinematic = true;
        ghost.movement.enabled = false;

        Vector3 position = transform.position;

        float duration = 0.5f;
        float elapsed = 0f;

        while (elapsed < duration)
        {
            ghost.SetPosition(Vector3.Lerp(position, inside.position, elapsed / duration));
            elapsed += Time.deltaTime;
            yield return null;
        }

        elapsed = 0f;

        while (elapsed < duration)
        {
            ghost.SetPosition(Vector3.Lerp(inside.position, outside.position, elapsed / duration));
            elapsed += Time.deltaTime;
            yield return null;
        }

        ghost.movement.SetDirection(new Vector2(Random.value < 0.5f ? -1f : 1f, 0f), true);
        ghost.movement.rigidbody.isKinematic = false;
        ghost.movement.enabled = true;
    }

}
