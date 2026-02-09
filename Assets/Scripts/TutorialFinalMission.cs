using UnityEngine;
using System;

public class TutorialFinalMission : MonoBehaviour
{
    [SerializeField] private FollowerDialogueActor bibi;
    [SerializeField] private DialogueTutorialManager dialogue;
    [SerializeField] private Transform player;

    private bool dialogueActive = false;
    private bool followerhasFinished = false;
    private int index = 0;
      private string[] dialoguesBeforeEnding =
    {
        "Bibi : Ce gros cristal ressemble à ceux qui alimentaient le générateur de lumière auparavant !",
        "Bibi : Jamais je n'aurai cru qu'ils se cachaient là.",
        "Bibi : Essaie de l'activer avec une chaine de lumière, ça ramènera peut-être de la lumière au village !"
    };

    void Update()
    {
        if (!dialogueActive) return;

        if (!followerhasFinished && Input.GetMouseButtonDown(0))
        {
            NextFollowerDialogue();
        }
}

     void NextFollowerDialogue()
    {
        index++;

        if (index >= dialoguesBeforeEnding.Length)
        {
            EndFollowerDialogue();
            return;
        }

        dialogue.StartDialogue(
            "Bibi",
            dialoguesBeforeEnding
        );
    }

    void EndFollowerDialogue()
    {
        followerhasFinished = true;
        dialogueActive = false;
        bibi.Hide();

        Level1Manager levelManager = FindObjectOfType<Level1Manager>();
        levelManager.dialogueLocked = false;

        Time.timeScale = 1f;
    }

     public void OnCollisionEnter2D(Collision2D other)
    {
        Level1Manager levelManager = FindObjectOfType<Level1Manager>();
        levelManager.dialogueLocked = true;
        if (!other.collider.CompareTag("Player")) return;


        Time.timeScale = 0f;
        dialogueActive = true;

        index = 0; // RESET HERE
        bibi.ShowNearPlayer(player);
        dialogue.StartDialogue(
        "Bibi",
        dialoguesBeforeEnding
    );

        gameObject.GetComponent<Collider2D>().enabled = false;
    }
}
