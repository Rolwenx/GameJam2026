using UnityEngine;

public class Level1Manager : MonoBehaviour
{
    public DialogueUI dialogueUI;
    private int Cristal1HasBeenLit = 0;

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

    /// faire un truc dans le update en mode if crystal called "Cristal1" gets the tag "Lit" (for 5s),
    /// le compteur Cristal1HasBeenLit +1
    /// Si Cristal1 repasse en tag "Unlit" ensuite, le compteur Cristal1HasBeenLit +1 
    /// Si Cristal1HasBeenLit = 2, ca veut dire que le joueur a fait le tutoriel.
    /// On met pause au jeu 
    /// Indication dialogue revient : "On dirait que les cristaux ne sont pas assez puissants pour tenir longtemps"
    /// "Essaie peut-être de les lier avec un autre cristal pour voir.
    /// Jeu reprend (et on verra)
    /// 
    //
    /// 

}