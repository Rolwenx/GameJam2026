using UnityEngine;

public class Level1Manager : MonoBehaviour
{
    public DialogueUI dialogueUI;

    [SerializeField] private Transform player;
    [SerializeField] private FollowerDialogueActor bibi;

    private int Cristal1HasBeenLit = 0;

    private GameObject porte;
        private bool enchainementFinished = false;

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
    private bool anyTutorialMessageAlreadyShown = false;

    private string[] currentDialogue = null;


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

        porte = GameObject.Find("PorteInvisbleContainer"); 
           
        OuverturePorte OuvertePorte = porte.GetComponent<OuverturePorte>();

        if (OuvertePorte.opened && enchainementFinished == false){
            enchainementFinished = true;
            OnAfterEnchainementCristal();
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

    }

    private void OnDisable()
    {
        LightCrystal.OnTutorialCrystalFinished -= OnCrystalTutorialDone;
    }

    private void OnCrystalTutorialDone()
    {
        if (anyTutorialMessageAlreadyShown) return;
        Time.timeScale = 0f;

        dialogueUI.HideIndication();
        bibi.ShowNearPlayer(player);

        currentDialogue = dialoguesAfterCrystal;          // ✅
        index = 0;                                        // ✅ (mets-le AVANT l’affichage)
        dialogueUI.ShowFollower(currentDialogue[index]);  // ✅

        crystalDialogueActive = true;
        anyTutorialMessageAlreadyShown = true;
    }

    public void OnAfterEnchainementCristal()
    {
        Time.timeScale = 0f;

        dialogueUI.HideIndication();
        bibi.ShowNearPlayer(player);

        currentDialogue = dialoguesAfterEnchainement;     // ✅
        index = 0;                                        // ✅
        dialogueUI.ShowFollower(currentDialogue[index]);  // ✅

        crystalDialogueActive = true;
    }


    void AdvanceCrystalDialogue()
    {
        index++;

        if (currentDialogue == null || index >= currentDialogue.Length)
        {
            crystalDialogueActive = false;
            dialogueUI.HideFollower();
            bibi.Hide();
            Time.timeScale = 1f;
        }
        else
        {
            dialogueUI.ShowFollower(currentDialogue[index]);
        }
    }


}