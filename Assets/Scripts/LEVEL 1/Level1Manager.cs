using UnityEngine;

public class Level1Manager : MonoBehaviour
{
    public DialogueUI dialogueUI;

    [SerializeField] private Transform player;
    [SerializeField] private FollowerDialogueActor bibi;
    private Collider2D demoDoorCollider;

    private int Cristal1HasBeenLit = 0;
    private bool WaterExplanationHasBeenShown = false;
    private GameObject porte;
        private bool enchainementFinished = false;
    public bool dialogueLocked = false;
    
    private bool crystalTutorialShown = false;
    private bool finalDoorDialogueShown = false;


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
        "Bibi : Trop top. Les cristaux restent allumés quand on les relie ensemble !",
        "Bibi : Bon, bon, bon. Continuons !"
    };

    private string[] dialoguesAfterDemoDoor =
    {
        "Bibi : Oh, une porte !",
        "Bibi : Malheureusement, elle est verrouillée. Il doit y avoir un moyen de l'ouvrir...",
        "Bibi : Certains cristaux sont de la même couleur que la porte...",
        "Bibi : Quel coincidence, c'est marrant !"
    };


    private string[] dialoguesEnding =
    {
        "Bibi : La lumière est totalement rétablie dans cette grotte, wow ! ",
        "Bibi : Ça faisait si longtemps que je n'avais pas vu une pièce éclairée. ",
        "Bibi : Je crois que je vais...!",
        "Bibi : PLEURERRRRRR.",
        "Bibi : Oh !",
        "Bibi : Allons jeter un oeil sur le générateur pour voir. Sors vite !",
        "Bibi : La sortie est sûrement derrière la porte."
    };


    private int index = 0;
    private bool followerFinished = false;
    private bool crystalDialogueActive = false;
    private bool anyTutorialMessageAlreadyShown = false;

    private string[] currentDialogue = null;
    
    public GameObject cristal;
    public GameObject cristal2;

    private bool startDemoDoor = false;

    public GameObject DemoDoor;
    private bool hasCollided = false;


    private void Awake()
    {
        demoDoorCollider = DemoDoor.GetComponent<Collider2D>();
    }
    void Start()
    {
        Time.timeScale = 0f;
        bibi.ShowNearPlayer(player);
        dialogueUI.ShowFollower(dialoguesFollower[index]);

        // on met layer default pour les deux cristaux pour éviter les interactions avec le rayon du joueur avant qu'ils soient tombés
        cristal.layer = LayerMask.NameToLayer("Default");
        cristal2.layer = LayerMask.NameToLayer("Default");




    }

    void Update()
    {
        if (dialogueLocked) return;

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

        hasCollided = DemoDoor.GetComponent<DemoDoor>().hasCollided;

        if (hasCollided && !startDemoDoor)
        {
            startDemoDoor = true;

            // Disable collision so player can pass through
            if (demoDoorCollider != null)
                demoDoorCollider.enabled = false;

            OnDemoDoorCollision();
        }

        if (WaterExplanationHasBeenShown && Input.GetMouseButtonDown(0))
        {
            dialogueUI.HideIndication();
            Time.timeScale = 1f;
            // faire disparaitre bibi
            bibi.Hide();
        }

        

        


    }

    void OnDemoDoorCollision()
    {
        Time.timeScale = 0f;

        dialogueUI.HideIndication();
        bibi.ShowNearPlayer(player);

        currentDialogue = dialoguesAfterDemoDoor; 
        index = 0;                                   
        dialogueUI.ShowFollower(currentDialogue[index]);

        crystalDialogueActive = true;

        cristal.layer = LayerMask.NameToLayer("Cristals");
        cristal2.layer = LayerMask.NameToLayer("Cristals");
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
        PorteManager.OnTutorialFinalDoorOpened += OnTutorialFinalDoorOpened;

    }

    private void OnDisable()
    {
        LightCrystal.OnTutorialCrystalFinished -= OnCrystalTutorialDone;
        PorteManager.OnTutorialFinalDoorOpened -= OnTutorialFinalDoorOpened;
    }

    private void OnCrystalTutorialDone()
    {
        if (crystalTutorialShown) return;
        Time.timeScale = 0f;

        dialogueUI.HideIndication();
        bibi.ShowNearPlayer(player);

        currentDialogue = dialoguesAfterCrystal;          // ✅
        index = 0;                                        // ✅ (mets-le AVANT l’affichage)
        dialogueUI.ShowFollower(currentDialogue[index]);  // ✅

        crystalDialogueActive = true;
        anyTutorialMessageAlreadyShown = true;
        crystalTutorialShown = true;
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

    public void OnCollisionEnter2D(Collision2D other)
    {
        
            Time.timeScale = 0f;
            bibi.ShowNearPlayer(player);
            dialogueUI.ShowIndication("L'eau peut être utilisé en tant que miroir pour faire rebondir la lumière. Tu peux essayer de viser un cristal avec l'eau !");
            WaterExplanationHasBeenShown = true;
            Debug.Log("Player entered water trigger for the first time, showing water explanation.");
            gameObject.GetComponent<Collider2D>().enabled = false; // Désactive le collider pour éviter de répéter l'explication

    }

    private void OnTutorialFinalDoorOpened()
    {
        if (finalDoorDialogueShown) return;

        Time.timeScale = 0f;

        dialogueUI.HideIndication();
        bibi.ShowNearPlayer(player);

        currentDialogue = dialoguesEnding;
        index = 0;
        dialogueUI.ShowFollower(currentDialogue[index]);

        crystalDialogueActive = true; 
        finalDoorDialogueShown = true;
    }


}