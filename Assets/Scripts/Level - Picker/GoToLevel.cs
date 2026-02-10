using UnityEngine;
using TMPro;
using UnityEngine.SceneManagement;

public class GoToLevel : MonoBehaviour
{
    [Header("Level Info")]
    // current level
    public int currentLevel; 
     // 0 if none,
    public int requiredPreviousLevel; 
    public string sceneName; 

    [Header("UI")]
    public TextMeshProUGUI worldText; 
    // distance from which player must be for the text to appear
    public float hideDistance = 3f;

    private Transform player;
    private bool playerInRange;

    void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player").transform;
        worldText.gameObject.SetActive(false);
    }

    void Update()
    {
        // if player not in range, nothing happens
        if (!playerInRange) return;

        float distance = Vector2.Distance(player.position, transform.position);

        if (distance > hideDistance)
        {
            HideText();
            return;
        }

        if (Input.GetKeyDown(KeyCode.E))
        {
            TryEnterLevel();
        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (!other.CompareTag("Player")) return;

        playerInRange = true;
        UpdateText();
        worldText.gameObject.SetActive(true);
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (!other.CompareTag("Player")) return;

        HideText();
    }

    void HideText()
    {
        playerInRange = false;
        worldText.gameObject.SetActive(false);
    }

    void UpdateText()
    {
        bool previousCompleted = false;
        // if we're talking about level 1
        if (requiredPreviousLevel == 0)
        {
            previousCompleted = true;
        }
        // if we're talking about other levels
        else
        {
            previousCompleted = PlayerPrefs.GetInt("Level_" + requiredPreviousLevel + "_Completed", 0) == 1;
        }

        bool thisCompleted = PlayerPrefs.GetInt("Level_" + currentLevel + "_Completed", 0) == 1;
        //int highScore = PlayerPrefs.GetInt($"Level_"+ currentLevel+ "_HighScore", 0);

        // if the previous level hasn't been completed
        if (!previousCompleted)
        {
            worldText.text =
                "You haven't completed the previous level.\nCome back later.";
            return;
        }

        // if the previous level has been completed, the only thing left to do
        // is either retry the level (if done) or play it
        if (thisCompleted)
        {
            worldText.text =
                "Do you want to retry this level?\n" +
                "Press E to play";
        }
        else
        {
            worldText.text =
                "Do you want to play this level?\n" +
                "Press E to go";
        }
    }

    void TryEnterLevel()
    {

        bool previousCompleted = false;
        if (requiredPreviousLevel == 0)
        {
            previousCompleted = true;
        }
        else
        {
            previousCompleted = PlayerPrefs.GetInt("Level_" + requiredPreviousLevel + "_Completed", 0) == 1;
        }

        if (!previousCompleted) return;

        SceneManager.LoadScene("Level "+(requiredPreviousLevel+1)    );
    }
}