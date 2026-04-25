using UnityEngine;

public class Spaceship : MonoBehaviour
{
    public AudioSource engineAudio;

    void Start()
    {
        GameManager.Instance.RegisterSpaceship(engineAudio);
    }
}
