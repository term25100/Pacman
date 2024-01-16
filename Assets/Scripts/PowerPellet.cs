using UnityEngine;

public class PowerPellet : Pellet
{
    public float duration = 8f;
    private AudioSource bonusAudioSource;
    private void Start()
    {
        bonusAudioSource = GameObject.Find("Bonus").GetComponent<AudioSource>();
    }
    protected override void Eat()
    {
        GameManager.Instance.PowerPelletEaten(this);
        bonusAudioSource.Play();
    }

}
