using UnityEngine;
using UnityEngine.SceneManagement;

public class PauseMenu : MonoBehaviour
{
    [SerializeField] private GameObject tutorialPanel;
    [SerializeField] private AudioManager audioManager;

    public void OpenMainMenu()
    {
        audioManager.PlaySFX(audioManager.butonBasma);
        SceneManager.LoadScene("Ana Menu", LoadSceneMode.Single);
    }

    public void RestartGame()
    {
        audioManager.PlaySFX(audioManager.butonBasma);
        SceneManager.LoadScene("Restart Ekraný", LoadSceneMode.Single);
    }

    public void ExitTutorial()
    {
        audioManager.PlaySFX(audioManager.butonBasma);
        Cursor.lockState = CursorLockMode.Locked;
        Time.timeScale = 1;
        tutorialPanel.SetActive(false);
    }
}