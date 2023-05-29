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
        if (Input.GetKeyDown(KeyCode.K))
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
            currentSlide++;
            ShowSlide(currentSlide);
        }
        else
        {
            // All slides have been shown, load the next scene
            SceneManager.LoadScene("MainGame");
        }
    }
}