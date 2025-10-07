//using System.Collections;
//using System.Collections.Generic;
//using UnityEngine;

using CodeStage.AntiCheat.ObscuredTypes;

[System.Serializable]
public class PrefData
{
    //Volumen
    public ObscuredBool soundVol = true;
    public ObscuredBool musicVol = true;

    public ObscuredFloat soundLevel = 0.0f;
    public ObscuredFloat musicLevel = 0.0f;
    public ObscuredFloat masterVol = 0.0f;

    //Idioma
    public ObscuredString languageCode = "en";
    public ObscuredString systemLanguageCode = "es";
}
