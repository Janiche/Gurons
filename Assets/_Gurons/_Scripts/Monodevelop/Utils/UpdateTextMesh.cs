//using System.Collections;
//using System.Collections.Generic;
using UnityEngine;


public class UpdateTextMesh : MonoBehaviour
{
    //[Tooltip("Tabla de localización de UI")] [SerializeField] private TextTable localizedTextTable = null;

    /// <summary>
    /// Actualiza todo TextMesh en escena
    /// </summary>
    public void UpdateText(string languageCode)
    {
        GlobalVars.prefData.languageCode = languageCode;

       /* TextTable.currentLanguageID = localizedTextTable.GetLanguageID(languageCode);

        if (DialogueManager.instance != null)
        {
            DialogueManager.SetLanguage(languageCode);
        }*/
    }
}
