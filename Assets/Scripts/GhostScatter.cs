using UnityEngine;

public class GhostScatter : GhostBehavior
{
    //Если класс перестает быть активным он включит поведение погони
    private void OnDisable()
    {
        ghost.chase.Enable();
    }

    //Этот метод проверяет если компонент узла с которым столкнулся призрак не пустой, призрак активен и не напуган тогда случайным образом выбирается любое доступное направление из списка направлений узла далее мы выполняем проверку что направление, выбранное случайным образом, не является обратным к текущему направлению движения призрака, если оно оказывается обратным, выбирается другое доступное направление, чтобы предотвратить разворот на месте
    private void OnTriggerEnter2D(Collider2D other)
    {
        Node node = other.GetComponent<Node>();

        if (node != null && enabled && !ghost.frightened.enabled)
        {

            int index = Random.Range(0, node.availableDirections.Count);


            if (node.availableDirections.Count > 1 && node.availableDirections[index] == -ghost.movement.direction)
            {
                index++;

                if (index >= node.availableDirections.Count) {
                    index = 0;
                }
            }

            ghost.movement.SetDirection(node.availableDirections[index]);
        }
    }

}
