using UnityEngine;

public class GeneratorManager : MonoBehaviour
{
    [Header("Game Finished")]
    public GameObject gameFinishedObject;

    private void Awake()
    {
  
        if (PlayerPrefs.GetInt("GameFinished", 0) == 1)
        {
            if (gameFinishedObject != null)
                gameFinishedObject.SetActive(true);
        }
        else
        {
            if (gameFinishedObject != null)
                gameFinishedObject.SetActive(false);
        }
    }
}
