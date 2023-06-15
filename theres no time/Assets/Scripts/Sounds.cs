using UnityEngine;

public class Sounds : MonoBehaviour
{
    public AudioClip clickSound;
    public AudioClip backgroundMusic;

    private AudioSource audioSource;
    private AudioSource musicSource;

    void Start()
    {
        // audiosource = sound effect, musicsource = bg music
        audioSource = GetComponent<AudioSource>();
        musicSource = gameObject.AddComponent<AudioSource>();

        // loop & play bg music
        musicSource.clip = backgroundMusic;
        musicSource.loop = true;
        musicSource.Play();
    }

    void Update()
    {
        // play sound effect when keypad 4 pressed
        if (Input.GetKeyDown(KeyCode.Keypad4))
        {
            audioSource.PlayOneShot(clickSound);
        }

        // pause music if game is paused
        if (PauseController.isPaused)
        {
            musicSource.Pause();
        }
        else
        {
            // unpause if game is played, ensure that song isn't restarted
            if (!musicSource.isPlaying)
            {
                musicSource.UnPause();
            }
        }
    }
}
