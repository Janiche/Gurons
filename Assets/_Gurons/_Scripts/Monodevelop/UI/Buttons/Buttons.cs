using UnityEngine;

using Bermost;

public class Buttons : MonoBehaviour
{
    [Header("Elementos de Cambio de Escena")]
    [Tooltip("Elemento de canvas que contiene el loading screen")] [SerializeField] private GameObject loadingCanvas = null;
    [Tooltip("EventSystem para desactivar al cargar escenas")] [SerializeField] private GameObject eventSystem = null;


    Coroutine _loadRoutine;

    /// <summary>
    /// Cambia escena de manera instantanea al haber cargado asyncrono
    /// </summary>
    /// <param name="scene">Escena a cambiar.</param>
    public void LoadSceneButton(string scene)
    {
        LoadScene(scene, 1f);


    /*
        if (AdsManager.instance != null)
        {
            AdsManager.instance._ShowBanner(false);
        }
        */
    }


    /// <summary>
    /// Cambia de escena con tiempo destinado
    /// </summary>
    /// <param name="scene">Escena a cambiar.</param>
    /// <param name="wait">Tiempo de espera.</param>
    public void LoadScene(string scene, float wait)
    {
        GameEvents.ChangeScene();

        Time.timeScale = 1;
        loadingCanvas.SetActive(true);
        eventSystem.SetActive(false);
        //GlobalVars.nextScene = scene;
        //SceneLoader.LoadSyncScene(scene);
        //SceneLoader.LoadAsyncWithLoader(scene);
        _loadRoutine = StartCoroutine(SceneLoader.LoadAsyncScene(scene, wait));
        //StartCoroutine(SceneLoader.LoadAsyncScene("LoadingScene", wait));
    }
}
