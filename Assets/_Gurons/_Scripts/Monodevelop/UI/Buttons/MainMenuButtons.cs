using UnityEngine;
using System.Collections.Generic;

public class MainMenuButtons : MonoBehaviour
{
    //[SerializeField] private Buttons btn = null;

    [Header("No Life")]
    [Tooltip("Panel de mensaje son vidas")] [SerializeField] private GameObject noLifeMessage = null;

    [Header("Congratulations")]
    //[SerializeField] private PixelCrushers.TextTable textTable = null;
    [Tooltip("Panel de mensaje de felicitaciones")] [SerializeField] private GameObject congratsPanel = null;
    [SerializeField] private TMPro.TextMeshProUGUI congratsMessage = null;

    /// <summary>
    /// Dirige a pantalla de planetas solo si posee vidas
    /// </summary>
    public void NewGame()
    {
        //CARGA PANTALLA DE PLANETAS
        if (Store.instance.GetBalance(Constants._Currencies.one_life) > 0)
        //if (Store.instance.GetBalance("one_life") > 0)
        {
            //btn.LoadSceneButton(Constants._SceneName.Planets.ToString());
        }

        //ACTIVA PANEL DE NO VIDAS
        else
        {
            noLifeMessage.SetActive(true);
        }
    }

}
