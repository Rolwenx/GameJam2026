using UnityEngine;
using UnityEngine.SceneManagement;

public class GamePauseManager : MonoBehaviour
{
    public GameObject PauseMenu;
    public DialogueLevels dialogueManager; // Référence au DialogueManager
    public DialogueTutorialManager dialogueTutorialManager; // Référence au DialogueTutorialManager
    public GameObject ParametersMenu; // Référence au menu de paramètres

    // list des script à désactiver lors de la pause (ex: les scripts de dialogue pour éviter les dialogues qui continuent pendant la pause : souris)
    public PlayerMovement playerMovementScript;
    public PlayerAim playerAimScript;

    public PlayerTopDownController playerTopDownMovementScript;

    void Start()
    {
        PauseMenu.SetActive(false);
    }

    void Update()
    {
        if (!Input.GetKeyDown(KeyCode.Escape)) return;

        // 1) Si les paramètres sont ouverts -> on ferme tout
        if (ParametersMenu != null && ParametersMenu.activeSelf)
        {
            Resume();
            return;
        }

        // 2) Sinon toggle du menu pause
        if (PauseMenu.activeSelf) {
            Resume();

        }
        else {
            Pause();
        }
    }

    public void Resume()
    {
        PauseMenu.SetActive(false);
        ParametersMenu.SetActive(false);
        
        // Réactiver le DialogueManager
        if (dialogueManager != null)
        {
            dialogueManager.enabled = true;
            if (playerMovementScript != null && playerAimScript != null)
            {
                playerMovementScript.enabled = true;
                playerAimScript.enabled = true;
            }
            if (playerTopDownMovementScript != null)
            {
                playerTopDownMovementScript.enabled = true;
            }
        }
        if (dialogueTutorialManager != null)
        {
            dialogueTutorialManager.enabled = true;
            if (playerMovementScript != null && playerAimScript != null)
            {
                playerMovementScript.enabled = true;
                playerAimScript.enabled = true;
            }
            if (playerTopDownMovementScript != null)
            {
                playerTopDownMovementScript.enabled = true;
            }
        }

        
    }

    public void Pause()
    {
        PauseMenu.SetActive(true);        
        // Désactiver le DialogueManager
        if (dialogueManager != null)
        {
            dialogueManager.enabled = false;
            if (playerMovementScript != null && playerAimScript != null)
            {
                playerMovementScript.enabled = false;
                playerAimScript.enabled = false;
            }
            if (playerTopDownMovementScript != null)
            {
                playerTopDownMovementScript.enabled = false;
            }
        }
        if (dialogueTutorialManager != null)
        {
            dialogueTutorialManager.enabled = false;
            if (playerMovementScript != null && playerAimScript != null)
            {
            playerMovementScript.enabled = false;
                playerAimScript.enabled = false;
            }
            if (playerTopDownMovementScript != null)
            {
                playerTopDownMovementScript.enabled = false;
            }
        }

        }

    public void QuitGame()
    {
        if(SceneManager.GetActiveScene().name == "MainMenu"){
            Application.Quit(); // marche uniquement dans une build, pas dans l'éditeur
        }
        else if ((SceneManager.GetActiveScene().name == "Town") || (SceneManager.GetActiveScene().name == "Generator") || (SceneManager.GetActiveScene().name == "Pick Level") ){
            SceneManager.LoadScene("MainMenu");
        }
        else {
            SceneManager.LoadScene("Town");
        }
    }

    public void ReloadScene()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }
}