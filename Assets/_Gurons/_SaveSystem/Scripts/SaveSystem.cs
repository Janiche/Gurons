//using System.Collections;
//using System.Collections.Generic;
using UnityEngine;

using System.IO;
using Newtonsoft.Json;

public class SaveSystem
{
    private static string savePath;
    private static string prefPath;
    private static bool readDecoded = false;

    /// <summary>
    /// Analiza si existe carpeta de guardado
    /// </summary>
    /// <returns><c>true</c>, Datos existen, <c>false</c> Datos no existen.</returns>
    public static bool CheckData()
    {
        bool data = false;

        //VERIFICA SI EXISTE O NO DECODIFICADO
        if (PlayerPrefs.HasKey("Decoded"))
        {
            if (PlayerPrefs.GetInt("Decoded") == 1)
            {
                readDecoded = true;
            }
        }

#if UNITY_EDITOR
        savePath = Application.streamingAssetsPath + "/Savedata.bsd";
        prefPath = Application.streamingAssetsPath + "/Prefdata.bsd";

        //No existe archivo de guardado, crea uno nuevo
        if (!Directory.Exists(Application.streamingAssetsPath))
        {
            Directory.CreateDirectory(Application.streamingAssetsPath);
            data = false;
        }
        else
        {
            data = true;
        }

#elif UNITY_ANDROID
        savePath = Application.persistentDataPath + "/Savedata.txt";
        prefPath = Application.persistentDataPath + "/Prefdata.txt";
#endif

        if (!File.Exists(savePath) && !File.Exists(prefPath))
        {
            data = false;
        }
        else
        {
            data = true;
        }

        return data;
    }

    /// <summary>
    /// Guarda las preferencias en archivo
    /// </summary>
    public static void SavePref()
    {
        if (GlobalVars.prefData == null)
        {
            GlobalVars.prefData = new PrefData();
        }

        string json = JsonConvert.SerializeObject(GlobalVars.prefData, Formatting.Indented, new ObscuredValueConverter());

        //Debug.LogWarning("SAVED PREF: " + json);

        string coded = EncodeData(json);
        File.WriteAllText(prefPath, json);
    }

    /// <summary>
    /// Carga las preferencias de archivo
    /// </summary>
    public static void LoadPref()
    {
        string json = File.ReadAllText(prefPath);
        string decoded = string.Empty;

        if (readDecoded)
        {
            decoded = DecodeData(json);
        }

        else
        {
            decoded = json;
        }

        //Debug.LogWarning("LOAD PREF: " + json);

        GlobalVars.prefData = JsonConvert.DeserializeObject<PrefData>(json, new ObscuredValueConverter());
    }

    /// <summary>
    /// Guarda datos de guardado
    /// </summary>
    public static void SaveData()
    {
        if (GlobalVars.saveData == null)
        {
            GlobalVars.saveData = new SaveData();
        }

        string json = JsonConvert.SerializeObject(GlobalVars.saveData, Formatting.Indented, new ObscuredValueConverter());

        //Debug.LogWarning("SAVED DATA: " + json);

        string coded = EncodeData(json);
        File.WriteAllText(savePath, json);


        //string js = JsonUtility.ToJson(new DataUnlock(), true);

        //File.WriteAllText(savePath + "aa", js);
    }

    /// <summary>
    /// Carga datos de guardado, pide actualizar o carga en memoria los datos
    /// </summary>
    public static void Load()
    {
        //Existe archivo de guardado, carga anterior
        if (File.Exists(savePath))
        {
            GlobalVars.saveData.playerData.lastLogin = System.DateTime.Now;
            SaveData();
        }
    }

    /// <summary>
    /// Actualiza datos de guardado
    /// </summary>
    private static void UpdateData()
    {
        GlobalVars.saveData.version = Application.version;

        switch (Application.version)
        {
            case Constants.Version_Constants.VERSION_1_2_0:
                {
                    PatchData.UpdateTotalGurons();
                    break;
                }

            case Constants.Version_Constants.VERSION_1_2_2:
                {
                    PatchData.UpdateTotalGurons();
                    PatchData.UpdatePlanetProgress();
                    PatchData.UpdateAchievements();
                }
                break;

            default:
                {
                    PatchData.UpdateTotalGurons();
                    PatchData.UpdatePlanetProgress();
                    PatchData.UpdateAchievements();
                    break;
                }

        }

    }

    /// <summary>
    /// Encripta datos de guardado
    /// </summary>
    /// <returns>Cadena de texto encriptada</returns>
    public static string EncodeData(string json)
    {
        string coded = "";
        int numeroLetra = 0;

        char caracter;
        char nuevoCaracter;

        for (int i = 0; i < json.Length; i++)
        {
            caracter = json[i];
            numeroLetra = (short)caracter;
            numeroLetra += json.Length;
            nuevoCaracter = (char)numeroLetra;
            coded += nuevoCaracter.ToString();
        }
        return coded;
    }

    /// <summary>
    /// Desencripta datos de guardado
    /// </summary>
    /// <returns>Cadena de texto desencriptada</returns>
    public static string DecodeData(string json)
    {
        string decoded = "";

        int numeroLetra = 0;

        char caracter;
        char nuevoCaracter;

        for (int i = 0; i < json.Length; i++)
        {
            caracter = json[i];
            numeroLetra = (short)caracter;
            numeroLetra -= json.Length;
            nuevoCaracter = (char)numeroLetra;
            decoded += nuevoCaracter.ToString();
        }

        PlayerPrefs.SetInt("Decoded", 1);

        return decoded;
    }
}
