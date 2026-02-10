using UnityEngine;
using UnityEngine.SceneManagement;

public class GamePauseManager : MonoBehaviour
{
    public GameObject PauseMenu;
    public DialogueLevels dialogueManager; // Référence au DialogueManager
    public DialogueTutorialManager dialogueTutorialManager; // Référence au DialogueTutorialManager

    void Awake()
    {
        PauseMenu.SetActive(false);
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            if (PauseMenu.activeSelf)
            {
                Resume();
            }
            else
            {
                Pause();
            }
        }
    }

    public void Resume()
    {
        PauseMenu.SetActive(false);
        Time.timeScale = 1f;
        
        // Réactiver le DialogueManager
        if (dialogueManager != null)
        {
            dialogueManager.enabled = true;
        }
        if (dialogueTutorialManager != null)
        {
            dialogueTutorialManager.enabled = true;
        }
    }

    public void Pause()
    {
        PauseMenu.SetActive(true);
        Time.timeScale = 0f;
        
        // Désactiver le DialogueManager
        if (dialogueManager != null)
        {
            dialogueManager.enabled = false;
        }
        if (dialogueTutorialManager != null)
        {
            dialogueTutorialManager.enabled = false;
        }
    }

    public void QuitGame()
    {
        Time.timeScale = 1f;
        SceneManager.LoadScene("MainMenu");
    }
}