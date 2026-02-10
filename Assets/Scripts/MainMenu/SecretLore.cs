using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class secretLore : MonoBehaviour
{
    [Header("Place ici le parent qui contient tes Text (TMP)")]
    [SerializeField] private Transform textsParent;


    private TMP_Text[] pages;
    private int index = 0;

    void Awake()
    {
        // Récupère tous les TMP enfants du parent (Text (TMP), Text (TMP) (1), etc.)
        pages = textsParent.GetComponentsInChildren<TMP_Text>(true);

        // Important : si ton bouton "Suivant" a aussi un TMP enfant, il sera récupéré.
        // => Astuce : mets tes textes dans un objet dédié (ex: "Pages") et assigne-le à textsParent.
    }

    void Start()
    {
        Show(index);

    }

    public void Next()
    {
        if (pages == null || pages.Length == 0) return;
        if (index < pages.Length - 1) index++;
        Show(index);
    }

    public void Prev()
    {
        if (pages == null || pages.Length == 0) return;
        if (index > 0) index--;
        Show(index);
    }

    private void Show(int i)
    {
        for (int k = 0; k < pages.Length; k++)
            pages[k].gameObject.SetActive(k == i);

    }
}
