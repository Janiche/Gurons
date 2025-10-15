using System.Collections;
using UnityEngine;

using UnityEngine.SceneManagement;

namespace Bermost
{

    public class SceneLoader
    {
        public static string _nextScene = string.Empty;

        /// <summary>
        /// Carga de manera sincrona la pantalla de carga, para luego cargar asincrona la escena destino
        /// </summary>
        /// <param name="_scene">Escena destino.</param>
        public static void LoadAsyncWithLoader(string _scene)
        {
            Resources.UnloadUnusedAssets();
            _nextScene = _scene;
            SceneManager.LoadScene("LoadingScene");


        }

        /// <summary>
        /// Carga de manera asincrona una escena
        /// </summary>
        /// <returns>The async scene.</returns>
        /// <param name="scene">Escena a cargar.</param>
        public static IEnumerator LoadAsyncScene(string scene, float wait)
        {
            Resources.UnloadUnusedAssets();

            yield return new WaitForSeconds(0.02f);
            AsyncOperation async = SceneManager.LoadSceneAsync(scene);
            async.allowSceneActivation = false;

            while (!async.isDone)
            {

                yield return new WaitForSeconds(wait);
                Resources.UnloadUnusedAssets();

                async.allowSceneActivation = true;
            }
            yield return 0;
        }

        /// <summary>
        /// Carga de manera asincrona una escena
        /// </summary>
        /// <returns>The async scene.</returns>
        /// <param name="index">Indice de la escena a cargar.</param>
        public static IEnumerator LoadAsyncScene(int index, float wait = 0)
        {
            Resources.UnloadUnusedAssets();

            AsyncOperation async = SceneManager.LoadSceneAsync(index);
            async.allowSceneActivation = false;

            while (!async.isDone)
            {

                yield return new WaitForSeconds(wait);
                Resources.UnloadUnusedAssets();

                async.allowSceneActivation = true;
            }
            yield return null;
        }

    }
}
