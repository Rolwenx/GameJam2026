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

    // ───────── ACTE 3 ─────────

    public void Show_Player_AllCrystalsDone()
    {
        StartDialogue(
            "Toi",
            new string[]
            {
                "C’est fait. Tous les cristaux sont activés.",
                "Et la lumière est presque totalement revenue !",
                "Mais pourquoi rien ne se passe…",
                "Bibi m’avait dit que les livres mentionnaient que le centre du générateur devait s’allumer.",
                "…"
            }
        );
    }

    public void Show_Player_CircleLightsUp()
    {
        StartDialogue(
            "Toi",
            new string[]
            {
                "Oh…",
                "Ça marche vraiment !"
            }
        );
    }

    public void Show_Hector_Intro()
    {
        StartDialogue(
            "Hector",
            new string[]
            {
                "Je savais que ce jour arriverait."
            }
        );
    }

    public void Show_Player_HeroSpeech()
    {
        StartDialogue(
            "Toi",
            new string[]
            {
                "Mais il faut se réjouir, Hector !",
                "J’ai ramené la lumière au village.",
                "Je suis le héros !"
            }
        );
    }

    public void Show_Hector_Warning()
    {
        StartDialogue(
            "Hector",
            new string[]
            {
                "…Tu ne comprends pas encore."
            }
        );
    }

    public void Show_Player_Confused()
    {
        StartDialogue(
            "Toi",
            new string[]
            {
                "Là tu commences à me perdre.",
                "Tu parles de quoi ?"
            }
        );
    }

    public void Show_Hector_This()
    {
        StartDialogue(
            "Hector",
            new string[]
            {
                "…ça."
            }
        );
    }

    public void Show_Player_PanicLight()
    {
        StartDialogue(
            "Toi",
            new string[]
            {
                "Mais…",
                "Il se passe quoi là ?"
            }
        );
    }

    public void Show_Hector_CycleReveal()
    {
        StartDialogue(
            "Hector",
            new string[]
            {
                "Tu n’as pas juste ramené la lumière.",
                "Tu as ramené un cycle maudit que l’on avait tout fait pour éviter."
            }
        );
    }

    public void Show_Player_CycleQuestion()
    {
        StartDialogue(
            "Toi",
            new string[]
            {
                "Le… cycle ?"
            }
        );
    }

    public void Show_Hector_Truth()
    {
        StartDialogue(
            "Hector",
            new string[]
            {
                "La lumière n’a jamais disparu.",
                "Elle attendait quelque chose.",
                "Quelqu’un."
            }
        );
    }

    public void Show_Hector_You()
    {
        StartDialogue(
            "Hector",
            new string[]
            {
                "Toi."
            }
        );
    }

    public void Show_Hector_Leave()
    {
        StartDialogue(
            "Hector",
            new string[]
            {
                "Tu n’aurais jamais dû commencer cette mission."
            }
        );
    }

    public void Show_Player_Abandoned()
    {
        StartDialogue(
            "Toi",
            new string[]
            {
                "Non !",
                "Tu ne peux pas me laisser là !"
            }
        );
    }

    public void Show_Player_PanicGrottes()
    {
        StartDialogue(
            "Toi",
            new string[]
            {
                "C’est pas possible…",
                "Toute cette galère dans ces grottes pour rien.",
                "Je comprends même pas ce qui se passe."
            }
        );
    }

    // ───────── ACTE 5 : BIBI ─────────

    public void Show_Bibi_Arrives()
    {
        StartDialogue(
            "Bibi",
            new string[]
            {
                "Hé !",
                "Mais…",
                "Qu’est-ce qu’il se passe ici ?!"
            }
        );
    }

    public void Show_Player_CallsBibi()
    {
        StartDialogue(
            "Toi",
            new string[]
            {
                "Bibi…",
                "Bibi, aide-moi…"
            }
        );
    }

    public void Show_Player_LastCry()
    {
        StartDialogue(
            "Toi",
            new string[]
            {
                "BIBI—"
            }
        );
    }

    public void Show_Bibi_Shocked()
    {
        StartDialogue(
            "Bibi",
            new string[]
            {
                "…"
            }
        );
    }

    public void Show_Bibi_Whisper()
    {
        StartDialogue(
            "Bibi",
            new string[]
            {
                "Hé…",
                "Tu m’entends ?",
                "T’es là ?",
                "Qu’est-ce qu’il se passe dans cette ville…"
            }
        );
    }

    // ───────── ACTE 6 : ÉPILOGUE ─────────

    public void Show_Hector_Final()
    {
        StartDialogue(
            "Hector",
            new string[]
            {
                "Allez, viens Bibi.",
                "J’ai préparé du thé.",
                "Et cette fois, j’ai pu voir les ingrédients.",
                "Il est exquis."
            }
        );
    }

    public void Show_Bibi_Leaves()
    {
        StartDialogue(
            "Bibi",
            new string[]
            {
                "Oh yes !",
                "J’arriveeee !"
            }
        );
    }

    public void Show_Bibi_LastView()
    {
        StartDialogue(
            "Bibi",
            new string[]
            {
                "..."
            }
        );
    }

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