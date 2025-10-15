using System.Collections;
using System.Runtime.Remoting.Messaging;
using UnityEngine;
using UnityEngine.SceneManagement;
//using Bermost;

[RequireComponent(typeof(CanvasGroup))]
public class SceneLoader : MonoBehaviour
{
    string _nextScene;
    CanvasGroup _canvasGroup;
    Coroutine _loadRoutine;

    public static SceneLoader _instance;
    private static SceneLoader Instance {get { return _instance; }}



    void Awake()
    {
        _canvasGroup = GetComponent<CanvasGroup>();
        if (_instance == null)
        {
            _instance = this;
            DontDestroyOnLoad(this);
        }
        else
        {
            Destroy(this);
        }
    }

    /// <summary>
    /// Cambia de escena con tiempo destinado
    /// </summary>
    /// <param name="scene">Escena a cambiar.</param>
    /// <param name="wait">Tiempo de espera.</param>
    public void LoadScene(string scene, float wait = 0)
    {
        GameEvents.ChangeScene();
        Time.timeScale = 1;

        StartCoroutine(LoadAsyncScene(scene, wait));
    }



    /// <summary>
    /// Carga de manera asincrona una escena
    /// </summary>
    /// <returns>The async scene.</returns>
    /// <param name="scene">Escena a cargar.</param>
    private IEnumerator LoadAsyncScene(string scene, float wait)
    {
        //Pre Carga
        _canvasGroup.blocksRaycasts = true;
        _canvasGroup.alpha = 1;


        Resources.UnloadUnusedAssets();

        yield return new WaitForSeconds(0.04f);
        AsyncOperation async = SceneManager.LoadSceneAsync(scene);
        async.allowSceneActivation = false;

        while (!async.isDone)
        {
            yield return new WaitForSeconds(wait);
            Resources.UnloadUnusedAssets();

            async.allowSceneActivation = true;

        }
        yield return 0;

        //Post Carga
        _canvasGroup.blocksRaycasts = false;
        _canvasGroup.alpha = 0;
    }  


}
