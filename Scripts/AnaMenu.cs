using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class AnaMenu : MonoBehaviour
{
    [SerializeField] private GameObject creditsPanel;
    [SerializeField] private GameObject mainMenuPanel;
    [SerializeField] private GameObject storyPanel;
    [SerializeField] private Animator animator;

    private void Start()
    {
        Time.timeScale = 1.0f;
    }

    public void StartGame()
    {
        StartCoroutine(WaitForStory());
    }

    public void ExitGame()
    {
        Application.Quit();
    }

    public void OpenCredits()
    {
        creditsPanel.SetActive(true);
        mainMenuPanel.SetActive(false);
    }

    public void ExitCredits()
    {
        creditsPanel.SetActive(false);
        mainMenuPanel.SetActive(true);
    }

    private IEnumerator WaitForStory()
    {
        storyPanel.SetActive(true);
        animator.SetTrigger("isStoryOn");

        yield return new WaitForSeconds(18);

        SceneManager.LoadScene("Oyun Ekraný");
    }
}