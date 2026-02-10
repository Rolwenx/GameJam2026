using UnityEngine;
using UnityEngine.UI;
using TMPro;
public class GestionVolume : MonoBehaviour
{
    public Slider sliderVolume;
    public AudioSource musicAudioSource;
    public TMP_Text volumeValueText; // Texte pour afficher la valeur du volume
    public string effect = "<sketchy>";

    void Start()
    {
        sliderVolume.value = musicAudioSource.volume; // initialiser le slider à la valeur actuelle du volume
        volumeValueText.text = effect +(musicAudioSource.volume * 100).ToString("0"); // afficher la valeur en pourcentage
    }
    

    public void ChangeVolume()
    {
        if (musicAudioSource != null && sliderVolume != null)
            musicAudioSource.volume = sliderVolume.value; // slider en 0..1
            volumeValueText.text = effect +(musicAudioSource.volume * 100).ToString("0"); // afficher la valeur en pourcentage
    }
}
