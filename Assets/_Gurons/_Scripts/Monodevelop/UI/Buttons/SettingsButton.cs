using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

using TMPro;
using Toggle = UnityEngine.UI.Toggle;

using UnityEngine.Audio;

public class SettingsButton : MonoBehaviour
{
    #region VARIABLES
    private PrefData settings = new PrefData();
    [Header("Localización")]
    [Tooltip("Actualizador de Texto UI")] [SerializeField] private UpdateTextMesh updateLocalizedText = null;
    [Tooltip("Lista de idiomas disponibles (Language Code)")]
    [SerializeField]
    private List<string> languages = new List<string>()
        {
            "en",
            "es"
        };
    //[SerializeField] private TextTable textTable = null;

    [Header("Sonido")]
    [Tooltip("Interruptor de música")] [SerializeField] private Toggle musicVol = null;
    [Tooltip("Interruptor de sonidos")] [SerializeField] private Toggle sfxVol = null;
    [Tooltip("Objeto Mixer")] [SerializeField] private AudioMixer _mixer = null;

    #endregion

    //private void OnEnable()
    //{
    //    settings.languageCode = GlobalVars.prefData.languageCode;
    //    //settings.selectedLanguageCode = GlobalVars.prefData.selectedLanguageCode;
    //}

    /// <summary>
    /// Cambia de idioma el juego completo
    /// </summary>
    public void ChangeLanguage()
    {
        //OBTIENE INDICE DEL IDIOMA ACTUAL
        int index = languages.IndexOf(GlobalVars.prefData.languageCode);

        //SI EL INDICE ES MENOR AL TOTAL DE IDIOMAS, PASA AL SIGUIENTE
        if (index + 1 < languages.Count)
        {
            index++;
        }

        //DE LO CONTRARIO, VUELVE AL PRIMERO
        else
        {
            index = 0;
        }
        //CAMBIA DE IDIOMA SEGUN INDICE E IDIOMAS DISPONIBLES
        updateLocalizedText.UpdateText(languages[index]);
    }

    /// <summary>
    /// Restrablece los datos de configuración
    /// </summary>
    public void Restore()
    {
        //REINICIA VALORES DE VOLUMEN
        GlobalVars.prefData.masterVol = settings.masterVol;
        GlobalVars.prefData.musicVol = settings.musicVol;
        GlobalVars.prefData.soundVol = settings.soundVol;

        //ACTIVA LOS INTERRUPTORES DE SONIDO
        musicVol.isOn = GlobalVars.prefData.musicVol;
        sfxVol.isOn = GlobalVars.prefData.soundVol;

        //REESTABLECE VALORES DEFECTO DEL MIXER
        if (_mixer != null)
        {
            _mixer.SetFloat("SFX", 0);
            _mixer.SetFloat("Music", 0);
        }

        //VUELVE AL IDIOMA DE FÁBRICA (SIEMPRE IDIOMA DETECTADO COMO IDIOMA DEL EQUIPO
        //EN PRIMERA EJECUCIÓN)
        updateLocalizedText.UpdateText(settings.languageCode);
    }

    /// <summary>
    /// Guarda las preferencias
    /// </summary>
    public void Save()
    {
        SaveSystem.SavePref();
    }

    /// <summary>
    /// Si está activo música, fija en 0db, de lo contradio en -80db (muteado)
    /// </summary>
    /// <param name="value">If set to <c>true</c> value.</param>
    public void ChangeMusic(bool value)
    {
        _mixer.SetFloat("Music", (value) ? -16 : -80);
        GlobalVars.prefData.musicVol = value;
    }

    /// <summary>
    /// Si está activo sonido, fija en 0db, de lo contradio en -80db (muteado)
    /// </summary>
    /// <param name="value">If set to <c>true</c> value.</param>
    public void ChangeSound(bool value)
    {
        _mixer.SetFloat("SFX", (value) ? 0 : -80);
        GlobalVars.prefData.soundVol = value;
    }


}
