using UnityEngine;
using System;

public class Level1Manager : MonoBehaviour
{
    [SerializeField] private DialogueTutorialManager dialogue;

    [SerializeField] private Transform player;
    [SerializeField] private FollowerDialogueActor bibi;
    private Collider2D demoDoorCollider;
    private Collider2D waterCollider;

    private int Cristal1HasBeenLit = 0;
    private bool WaterExplanationHasBeenShown = false;
    private GameObject porte;
        private bool enchainementFinished = false;
    public bool dialogueLocked = false;
    
    private bool crystalTutorialShown = false;
    private bool finalDoorDialogueShown = false;


    private string[] dialoguesFollower =
    {
        "Wooow, j'arrive pas à y croire. On arrive à voir dans le noir ?",
        "Je pensais pas que ce jour arriverait, surtout depuis que les fragments de lumière ont cessé de briller.",
        "Certains fragments réagissent peut-être encore à la lumière.",
        "Peut-être peux-tu les rallumer avec le fragment que tu as trouvé ?"
    };

    private string[] indicationText =
    {
        
        "Clique gauche pour projeter de la lumière vers un fragment. "
        };


    // Mini-dialogue après tuto cristal
    private string[] dialoguesAfterCrystal =
    {
        "On dirait que les cristaux ne sont pas assez puissants pour tenir longtemps.",
        "Essaie peut-être de les lier avec un autre cristal pour voir. Il y en a un tout devant."
    };

    private string[] dialoguesAfterEnchainement =
    {
        "Trop top. Les cristaux restent allumés quand on les relie ensemble !",
        "Bon, bon, bon. Continuons !"
    };

    private string[] dialoguesAfterDemoDoor =
    {
        "Oh, une porte !",
        "Malheureusement, elle est verrouillée. Il doit y avoir un moyen de l'ouvrir...",
        "Certains cristaux sont de la même couleur que la porte...",
        "Quelle coincidence, c'est marrant !"
    };
    private string[] dialoguesAfterWater =
    {
        "L'eau peut être utilisée comme un miroir pour faire rebondir la lumière.",
        "Essaie de viser un cristal avec l'eau !"
    };


    private string[] dialoguesEnding =
    {
        "La lumière est totalement rétablie dans cette grotte, wow ! ",
        "Ça faisait si longtemps que je n'avais pas vu une pièce éclairée. ",
        "Je crois que je vais...!",
        "PLEURERRRRRR.",
        "Oh !",
        "Allons jeter un oeil sur le générateur pour voir. Sors vite !",
        "La sortie est sûrement derrière la porte."
    };


    private bool followerFinished = false;
    private bool anyTutorialMessageAlreadyShown = false;

    
    public GameObject cristal;
    public GameObject cristal2;

    private bool startDemoDoor = false;
    private bool startWater = false;

    public GameObject DemoDoor;
    public GameObject Water;
    private bool hasCollided = false;
    private bool hasCollidedWater = false;


    private void Awake()
    {
        demoDoorCollider = DemoDoor.GetComponent<Collider2D>();
        waterCollider = Water.GetComponent<Collider2D>();
    }
    void Start()
    {
        Time.timeScale = 0f;
        bibi.ShowNearPlayer(player);
        dialogue.OnDialogueFinished += OnIntroDialogueFinished;
        dialogue.StartDialogue(
            "Bibi",
            dialoguesFollower
        );

        // on met layer default pour les deux cristaux pour éviter les interactions avec le rayon du joueur avant qu'ils soient tombés
        cristal.layer = LayerMask.NameToLayer("Default");
        cristal2.layer = LayerMask.NameToLayer("Default");

    }

    void OnIntroDialogueFinished()
    {
        dialogue.OnDialogueFinished -= OnIntroDialogueFinished;

        followerFinished = true;
        bibi.Hide();

        dialogue.StartIndication(
            "Bibi",
            indicationText
        );
    }

    void Update()
    {
        if (dialogueLocked) return;

        porte = GameObject.Find("PorteInvisbleContainer"); 
           
        OuverturePorte OuvertePorte = porte.GetComponent<OuverturePorte>();

        if (OuvertePorte.opened && enchainementFinished == false){
            enchainementFinished = true;
            OnAfterEnchainementCristal();
        }

        hasCollided = DemoDoor.GetComponent<DemoDoor>().hasCollided;
        hasCollidedWater = Water.GetComponent<WaterTutorialTrigger>().hasCollidedWater;

        if (hasCollided && !startDemoDoor)
        {
            startDemoDoor = true;

            // Disable collision so player can pass through
            if (demoDoorCollider != null)
                demoDoorCollider.enabled = false;

            OnDemoDoorCollision();
        }

        if (hasCollidedWater && !startWater)
        {
            startWater = true;

            // Disable collision so player can pass through
            if (waterCollider != null)
                waterCollider.enabled = false;

            OnWaterCollision();
        }


    }

void OnWaterCollision()
    {
        Time.timeScale = 0f;

        bibi.ShowNearPlayer(player);
                                  
        dialogue.StartDialogue(
            "Bibi",
            dialoguesAfterWater
        );
    }
    void OnDemoDoorCollision()
    {
        Time.timeScale = 0f;

        bibi.ShowNearPlayer(player);
                                  
        dialogue.StartDialogue(
            "Bibi",
            dialoguesAfterDemoDoor
        );


        cristal.layer = LayerMask.NameToLayer("Cristals");
        cristal2.layer = LayerMask.NameToLayer("Cristals");
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

        bibi.ShowNearPlayer(player);
    
        dialogue.StartDialogue(
            "Bibi",
            dialoguesAfterCrystal
        );

        
        anyTutorialMessageAlreadyShown = true;
        crystalTutorialShown = true;
    }

    public void OnAfterEnchainementCristal()
    {
        Time.timeScale = 0f;

        bibi.ShowNearPlayer(player);
         
        dialogue.StartDialogue(
            "Bibi",
            dialoguesAfterEnchainement
        );

    }


    private void OnTutorialFinalDoorOpened()
    {
        if (finalDoorDialogueShown) return;

        Time.timeScale = 0f;

        bibi.ShowNearPlayer(player);

        dialogue.StartDialogue(
            "Bibi",
            dialoguesEnding
        );

        finalDoorDialogueShown = true;
    }


}