using UnityEngine;

[RequireComponent(typeof(SpriteRenderer))]
public class AnimatedSprite : MonoBehaviour
{
    public Sprite[] sprites = new Sprite[0];
    public float animationTime = 0.25f;
    public bool loop = true;

    private SpriteRenderer spriteRenderer;
    private int animationFrame;

    //Данный метод получает компонент рендера спрайтов 
    private void Awake()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
    }

    //Если данный класс активен то запускается рендер спрайтов
    private void OnEnable()
    {
        spriteRenderer.enabled = true;
    }

    //Если данный класс перестанет быть активным тогда рендер спрайтов будет выключен
    private void OnDisable()
    {
        spriteRenderer.enabled = false;
    }

    //При выполнении данного метода метод Invoke будет повторят метод адванс с определенным временем анимации
    private void Start()
    {
        InvokeRepeating(nameof(Advance), animationTime, animationTime);
    }

    //Данный метод выполняет логику обновления кадров анимации
    private void Advance()
    {
        if (!spriteRenderer.enabled) {
            return;
        }

        animationFrame++;

        if (animationFrame >= sprites.Length && loop) {
            animationFrame = 0;
        }

        if (animationFrame >= 0 && animationFrame < sprites.Length) {
            spriteRenderer.sprite = sprites[animationFrame];
        }
    }

    //этот метод сбрасывает переменную кадра до -1 чтобы при повторной анимации она началась с первого элемента и выполняет запуск метода Advance
    public void Restart()
    {
        animationFrame = -1;

        Advance();
    }

}
