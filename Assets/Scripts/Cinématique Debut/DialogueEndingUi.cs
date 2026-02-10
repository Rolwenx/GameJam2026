using TMPro;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.Playables;

public class DialogueEndingUi : MonoBehaviour
{
    [Header("UI")]
    public GameObject dialogueBackground;
    public TMP_Text speakerName;
    public TMP_Text dialogueText;

    [Header("Timeline")]
    public PlayableDirector director;

    private string[] currentLines;
    private int currentLineIndex;
    private bool isActive;
    private string currentSpeaker;

    private void Awake()
    {
        Hide();
    }

    private void Update()
    {
        if (!isActive) return;

        if (Keyboard.current.spaceKey.wasPressedThisFrame ||
            Mouse.current.leftButton.wasPressedThisFrame)
        {
            NextLine();
        }
    }


    public void Show_Player_Intro()
    {
        StartDialogue(
            "Toi",
            new string[]
            {
                "Je sais pas pourquoi je me tracasse à checker si on me suit…",
                "À part moi, personne ne vient jamais dans ces bois."
            }
        );
    }

    public void Show_Player_Cristal()
    {
        StartDialogue(
            "Toi",
            new string[]
            {
                "Chaque jour, je viens ici. Et à chaque fois, j’espère que tu t’allumeras.",
                "Que tu donneras peut-être signe de vie.",
                "Ça fait cinq ans… et toujours rien."
            }
        );
    }

    public void Show_Player_Cristal_JustLitUp()
    {
        StartDialogue(
            "Toi",
            new string[]
            {
                "Que...?",
                "Il s'est allumé ?!",
            }
        );
    }

    // player loin du cristal là
    public void Show_Player_Cristal_Unlit()
    {
        StartDialogue(
            "Toi",
            new string[]
            {
                "Non, non, non revient !",
                "Attends j'ai pas rêvé n'est-ce pas...?",
            }
        );
    }

    // bibi arrive près de lui
    public void Show_Bibi_Intro()
    {
        StartDialogue(
            "Bibi",
            new string[]
            {
                "NOOOON !",
                "Moi aussi je l’ai vu !",
                "Le cristal s’est allumé, j’en suis sûr !"
            }
        );
    }

    public void Show_Bibi_CristalLit()
    {
        StartDialogue(
            "Bibi",
            new string[]
            {
                "C'est un miracle ce qui se passe là, j'ai bien fait de te suivre.",
                "Mais ce n'est pas le plus important !",
                "Les anciens livres disaient que si un cristal s'allume, on pouvait l'utiliser pour activer d'autres cristaux.",
                "Mais qu'il ne s'active que pour la bonne personne.",
                "Peut-être que c'est moi, ehehe."
            }
        );
    }

    // bibi s'approche du cristal mais rien ne se passe

    public void Show_Player_BeforeTrying()
    {
        StartDialogue(
            "Toi",
            new string[]
            {
                "..."
            }
        );
    }

    // player s'approche et
    // cristal s'allume

    public void Show_Bibi_CristalAfterPlayerTry()
    {
        StartDialogue(
            "Bibi",
            new string[]
            {
                "Toi...? Mais pourquoi maintenant, ça fait 5 ans que tu viens et il ne se passait rien !"
            }
        );
    }

    // player regarde Bibi
    public void Show_Player_WeGoGrotte()
    {
        StartDialogue(
            "Toi",
            new string[]
            {
                "Tu penses à la même chose que moi, maintenant qu'on a ce cristal allumé ?"
            }
        );
    }

    // bibi look at player

    public void Show_Bibi_WeGoGrotte()
    {
        StartDialogue(
            "Bibi",
            new string[]
            {
                "ALLONS VISITER UNE DES GROTTES",
                "LETS GOOOO",
                "N'oublie pas de ramasser le cristal."
            }
        );
    }

    // bibi et player vont à droite 

    // ───────── CORE ─────────

    private void StartDialogue(string speaker, string[] lines)
    {
        if (lines == null || lines.Length == 0) return;

        currentSpeaker = speaker;
        currentLines = lines;
        currentLineIndex = 0;
        isActive = true;

        dialogueBackground.SetActive(true);
        speakerName.gameObject.SetActive(true);
        dialogueText.gameObject.SetActive(true);

        speakerName.text = currentSpeaker;
        ShowCurrentLine();

        // pause timeline
        if (director != null)
            director.Pause();
    }

    private void NextLine()
    {
        currentLineIndex++;

        if (currentLineIndex >= currentLines.Length)
        {
            EndDialogue();
            return;
        }

        ShowCurrentLine();
    }

    private void ShowCurrentLine()
    {
        dialogueText.text = currentLines[currentLineIndex];
    }

    private void EndDialogue()
    {
        Hide();

        // resume timeline
        if (director != null)
            director.Resume();
    }

    public void Hide()
    {
        isActive = false;

        dialogueBackground.SetActive(false);
        speakerName.gameObject.SetActive(false);
        dialogueText.gameObject.SetActive(false);
    }
}