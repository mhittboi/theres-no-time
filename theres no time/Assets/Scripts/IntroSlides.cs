using UnityEngine;
using UnityEngine.SceneManagement;

public class IntroSlides : MonoBehaviour
{
    public GameObject[] slides;
    private int currentSlide = 0;

    private void Start()
    {
        ShowSlide(currentSlide);
    }

    private void Update()
    {
        // if player presses 4, show next slide
        if (Input.GetKeyDown(KeyCode.Keypad4))
        {
            NextSlide();
        }
    }

    private void ShowSlide(int slideIndex)
    {
        for (int i = 0; i < slides.Length; i++)
        {
            slides[i].SetActive(i == slideIndex);
        }
    }

    private void NextSlide()
    {
        if (currentSlide < slides.Length - 1)
        {
            // show next slide for as long as they are available
            currentSlide++;
            ShowSlide(currentSlide);
        }
        else
        {
            // next scene
            SceneManager.LoadScene("MainGame");
        }
    }
}