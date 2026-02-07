using UnityEngine;

public class Level1Manager : MonoBehaviour
{
    public DialogueUI dialogueUI;

    private string[] dialoguesFollower =
    {
        "Bibi : Wooow, j'arrive pas à y croire. On arrive à voir dans le noir ?",
        "Bibi : Je pensais pas que ce jour arriverait, surtout depuis que les fragments de lumière ont cessé de briller.",
        "Bibi : Certains fragments réagissent peut-être encore à la lumière.",
        "Bibi : Peut-être peux-tu les rallumer avec le fragment que tu as trouvé ?"
    };

    private string indicationText =
        "Clique gauche pour projeter de la lumière vers un fragment. ";

    private int index = 0;
    private bool followerFinished = false;

    void Start()
    {
        Time.timeScale = 0f;
        dialogueUI.ShowFollower(dialoguesFollower[index]);
    }

    void Update()
    {
        // Séquence dialogue follower
        if (!followerFinished && Input.GetMouseButtonDown(0))
        {
            NextFollowerDialogue();
        }
    }

    void NextFollowerDialogue()
    {
        index++;

        if (index >= dialoguesFollower.Length)
        {
            EndFollowerDialogue();
        }
        else
        {
            dialogueUI.ShowFollower(dialoguesFollower[index]);
        }
    }

    void EndFollowerDialogue()
    {
        followerFinished = true;

        dialogueUI.HideFollower();
        dialogueUI.ShowIndication(indicationText);

        // Le jeu reprend ici
        Time.timeScale = 1f;
    }
}