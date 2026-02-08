using UnityEngine;

public class Level1Manager : MonoBehaviour
{
    public DialogueUI dialogueUI;

    [SerializeField] private Transform player;
    [SerializeField] private FollowerDialogueActor bibi;

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

    // Mini-dialogue après tuto cristal
    private string[] dialoguesAfterCrystal =
    {
        "Bibi : On dirait que les cristaux ne sont pas assez puissants pour tenir longtemps.",
        "Bibi : Essaie peut-être de les lier avec un autre cristal pour voir. Il y en a un tout devant."
    };

    private string[] dialoguesAfterEnchainement =
    {
        "Bibi : Trop top. les cristaux sont maintenant allumés !",
        "Bibi : Bon continuons."
    };

    private int index = 0;
    private bool followerFinished = false;
    private bool crystalDialogueActive = false;

    void Start()
    {
        Time.timeScale = 0f;
        bibi.ShowNearPlayer(player);
        dialogueUI.ShowFollower(dialoguesFollower[index]);
    }

    void Update()
    {
        if (crystalDialogueActive && Input.GetMouseButtonDown(0))
        {
            AdvanceCrystalDialogue();
            return;
        }

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

        bibi.Hide(); // 👈 Bibi disparaît
        Time.timeScale = 1f;
    }

    private void OnEnable()
    {
        LightCrystal.OnTutorialCrystalFinished += OnCrystalTutorialDone;
        LightCrystal.AfterEnchainementCristal += OnAfterEnchainementCristal;

    }

    private void OnDisable()
    {
        LightCrystal.OnTutorialCrystalFinished -= OnCrystalTutorialDone;
        LightCrystal.AfterEnchainementCristal -= OnAfterEnchainementCristal;
    }

    private void OnCrystalTutorialDone()
    {
        Time.timeScale = 0f;

        dialogueUI.HideIndication();
        bibi.ShowNearPlayer(player); // 👈 réapparition
        dialogueUI.ShowFollower(dialoguesAfterCrystal[0]);

        crystalDialogueActive = true;
        index = 0;
    }

    public void OnAfterEnchainementCristal(){
        Time.timeScale = 0f;

        dialogueUI.HideIndication();
        bibi.ShowNearPlayer(player); // 👈 réapparition
        dialogueUI.ShowFollower(dialoguesAfterEnchainement[0]);

        crystalDialogueActive = true;
        index = 0;
    }

    void AdvanceCrystalDialogue()
{
    index++;

    if (index >= dialoguesAfterCrystal.Length)
    {
        crystalDialogueActive = false;
        dialogueUI.HideFollower();
        bibi.Hide(); // 👈 disparaît
        Time.timeScale = 1f;
    }
    else
    {
        dialogueUI.ShowFollower(dialoguesAfterCrystal[index]);
    }
}

}