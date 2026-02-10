using UnityEngine;

public class ApplySound : MonoBehaviour
{
    public AudioSource audioSource;

     void Start()
    {
        const string PREF_VOLUME = "MusicVolume"; // mets le même nom que dans GestionVolume
        float savedVolume = PlayerPrefs.GetFloat(PREF_VOLUME, 1f); // 1f est la valeur par défaut si aucune préférence n'est trouvée
        audioSource.volume = savedVolume;
    }
}
