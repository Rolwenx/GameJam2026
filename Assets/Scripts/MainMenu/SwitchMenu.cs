using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class SwitchToParam : MonoBehaviour
{
    public GameObject menuPrincipal;
    public GameObject menuParametres;
    public GameObject menuElementsDébloqués;

    public Slider sliderVolume;
    public AudioSource musicAudioSource;
    public TMP_Text volumeValueText; // Texte pour afficher la valeur du volume
    void Start()
    {
        menuPrincipal.SetActive(true);
        menuParametres.SetActive(false);
        menuElementsDébloqués.SetActive(false);
        sliderVolume.value = musicAudioSource.volume; // initialiser le slider à la valeur actuelle du volume
        volumeValueText.text = (musicAudioSource.volume * 100).ToString("0"); // afficher la valeur en pourcentage
    }
    
     public void GoToParam()
    {
        menuPrincipal.SetActive(false);
        menuParametres.SetActive(true);
    }

    public void GoToMenuPrincipal()
    {
        menuPrincipal.SetActive(true);
        menuParametres.SetActive(false);
        menuElementsDébloqués.SetActive(false);
    }

    public void QuitGame()
    {
        Application.Quit(); // marche uniquement dans une build, pas dans l'éditeur
    }

    public void GoToElementsDébloqués()
    {
        menuPrincipal.SetActive(false);
        menuElementsDébloqués.SetActive(true);
    }


    

    public void ChangeVolume()
    {
        if (musicAudioSource != null && sliderVolume != null)
            musicAudioSource.volume = sliderVolume.value; // slider en 0..1
            volumeValueText.text = (musicAudioSource.volume * 100).ToString("0"); // afficher la valeur en pourcentage
    }


    

}
