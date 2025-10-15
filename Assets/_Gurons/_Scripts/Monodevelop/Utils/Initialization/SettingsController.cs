using UnityEngine;
using System.Collections.Generic;

using UnityEngine.UI;
using Toggle = UnityEngine.UI.Toggle;
using UnityEngine.Audio;

//using AppodealAds.Unity.Api;
//using AppodealAds.Unity.Common;

public class SettingsController : MonoBehaviour
{
    [SerializeField] private DefaultSettings _defaultSettings;

    [Space]
    [Header("Sonido")]
    [Tooltip("Objeto Mixer")] [SerializeField] private AudioMixer _mixer = null;
    [Tooltip("Interruptor de Activar/Desactivar música")] [SerializeField] private Toggle musicVol = null;
    [Tooltip("Interruptor de Activar/Desactivar sonidos")] [SerializeField] private Toggle sfxVol = null;

    [Space]
    [Header("Localización")]
    //[SerializeField] private TextTable localizedTextTable = null;
    [Tooltip("Lista de idiomas disponibles (Language Code)")]
    [SerializeField]
    private List<string> languages = new List<string>()
        {
            "en",
            "es"
        };

    private void OnEnable()
    {
        _Init();
    }

    void _Init()
    {
        /*
        if (AdsManager.instance != null)
        {
            AdsManager.instance._ShowBanner(true);
        }
        */

        //ACTIVA O DESACTIVA AUDIO
        musicVol.isOn = GlobalVars.prefData.musicVol;
        sfxVol.isOn = GlobalVars.prefData.soundVol;

        //AdsManager.instance.ShowBanner();

        //if (Appodeal.isLoaded(Appodeal.BANNER) && !Appodeal.isPrecache(Appodeal.BANNER))
        //{
        //    Appodeal.show(Appodeal.BANNER_BOTTOM);
        //}
        //else
        //{
        //    Appodeal.cache(Appodeal.BANNER_BOTTOM);
        //}

        //DESACTIVA SCRIPT
        //this.enabled = false;
    }

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

        //Debug.Log(languages[index]);
        //GlobalVars.prefData.languageCode = languages[index];
        //CAMBIA DE IDIOMA SEGUN INDICE E IDIOMAS DISPONIBLES
        UpdateText(languages[index]);
        //updateLocalizedText.UpdateText(languages[index]);
    }

    /// <summary>
	/// Restrablece los datos de configuración
	/// </summary>
	public void Restore()
    {
        //REINICIA VALORES DE VOLUMEN
        GlobalVars.prefData.masterVol = _defaultSettings._prefData.masterVol;
        GlobalVars.prefData.musicVol = _defaultSettings._prefData.musicVol;
        GlobalVars.prefData.soundVol = _defaultSettings._prefData.soundVol;

        //ACTIVA LOS INTERRUPTORES DE SONIDO
        //musicVol.isOn = GlobalVars.prefData.musicVol;
        //sfxVol.isOn = GlobalVars.prefData.soundVol;

        musicVol.isOn = _defaultSettings._prefData.musicVol;
        sfxVol.isOn = _defaultSettings._prefData.soundVol;

        //REESTABLECE VALORES DEFECTO DEL MIXER
        if (_mixer != null)
        {
            _mixer.SetFloat("SFX", _defaultSettings._prefData.soundLevel);
            _mixer.SetFloat("Music", _defaultSettings._prefData.soundLevel);
            //_mixer.SetFloat("SFX", 0);
            //_mixer.SetFloat("Music", -6);
        }

        //VUELVE AL IDIOMA DE FÁBRICA (SIEMPRE IDIOMA DETECTADO COMO IDIOMA DEL EQUIPO
        //EN PRIMERA EJECUCIÓN)
        UpdateText(GlobalVars.prefData.systemLanguageCode);
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
        //_mixer.SetFloat("Music", (value) ? _defaultSettings._prefData.musicLevel : -80);
        float _level = (value) ? _defaultSettings._prefData.musicLevel : -80;
        _mixer.SetFloat("Music", _level);
        GlobalVars.prefData.musicLevel = _level;
        GlobalVars.prefData.musicVol = value;
    }

    /// <summary>
    /// Si está activo sonido, fija en 0db, de lo contradio en -80db (muteado)
    /// </summary>
    /// <param name="value">If set to <c>true</c> value.</param>
    public void ChangeSound(bool value)
    {
        //_mixer.SetFloat("SFX", (value) ? _defaultSettings._prefData.soundLevel : -80);
        float _level = (value) ? _defaultSettings._prefData.soundLevel : -80;
        _mixer.SetFloat("SFX", _level);
        GlobalVars.prefData.soundLevel = _level;
        GlobalVars.prefData.soundVol = value;
    }

    public void UpdateText(string languageCode)
    {
        GlobalVars.prefData.languageCode = languageCode;

        /*TextTable.currentLanguageID = localizedTextTable.GetLanguageID(languageCode);

        if (DialogueManager.instance != null)
        {
            DialogueManager.SetLanguage(languageCode);
        }*/

        //var localizedUI = FindObjectsOfType<LocalizeUIText>();

        //Debug.Log(localizedUI.Length);

        //for (int i = 0; i < localizedUI.Length; i++)
        //{
        //    localizedUI[i].LocalizeText();
        //}
    }


}
