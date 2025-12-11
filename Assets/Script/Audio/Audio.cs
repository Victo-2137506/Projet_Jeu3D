using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.UI;

/// <summary>
/// Script de gestion de volume pour la musique.
/// Inspiré des notes de cours : https://cours-alexandre-ouellet.github.io/jeux-3d/programmer/sons/#utiliser-et-configurer-les-audiosource
/// Inspiré d'une vidéo youtube pour l'utilisation d'un slider : https://www.youtube.com/watch?v=V_Bf__ynKLE
/// </summary>
public class VolumeControllerMixte : MonoBehaviour
{
    [SerializeField]
    private AudioMixer audioMixer;

    [SerializeField] 
    private string parametreVolume = "VolumeMusique";

    [SerializeField] 
    private Slider sliderVolume;

    private const float minVolumeDecibel = -80f;
    private const float maxVolumeDecibel = 0f;

    private void Start()
    {
        // Gestion d'erreur
        if (sliderVolume == null || audioMixer == null)
        {
            Debug.LogError("Veuillez assigner le Slider et l'AudioMixer dans l'inspector.");
            return;
        }

        // Permet que le curseur se positionne selon les paramètres mis en place dans l'audioMixeur (-20)
        if (audioMixer.GetFloat(parametreVolume, out float valeurDb))
        {
            sliderVolume.value = Mathf.InverseLerp(minVolumeDecibel, maxVolumeDecibel, valeurDb);
        }

        // La valeur du slider change
        sliderVolume.onValueChanged.AddListener(ChangerVolume);
    }

    /// <summary>
    /// Appelé quand le slider change.
    /// </summary>
    private void ChangerVolume(float valeurLineaire)
    {
        float valeurDb;

        if (valeurLineaire <= 0.0001f)
        {
            valeurDb = minVolumeDecibel;
        }
        else
        {
            valeurDb = ConvertirLineaireVersExponentiel(valeurLineaire);
        }
        audioMixer.SetFloat(parametreVolume, valeurDb);
    }

    /// <summary>
    /// Convertit la valeur linéaire [0,1] du slider vers l'échelle des décibels (exponentiel).
    /// </summary>
    private float ConvertirLineaireVersExponentiel(float volumeLineaire)
    {
        // Conversion inspirée des notes de cours
        float minDb = minVolumeDecibel / 10.0f - 12.0f;
        float maxDb = maxVolumeDecibel / 10.0f - 12.0f;
        float etenduDb = maxDb - minDb;

        float echelleExponentielle = Mathf.Lerp(1.0f, Mathf.Pow(3.321928f, etenduDb), volumeLineaire);
        float pourcentageLog = Mathf.Log(echelleExponentielle, 3.321928f) / etenduDb;
        float volumeDB = Mathf.Lerp(minVolumeDecibel, maxVolumeDecibel, pourcentageLog);

        return volumeDB;
    }

}