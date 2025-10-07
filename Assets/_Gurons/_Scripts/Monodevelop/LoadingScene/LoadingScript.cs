using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using Bermost;

public class LoadingScript : MonoBehaviour
{
    private void Start()
    {
        Resources.UnloadUnusedAssets();
        StartCoroutine(SceneLoader.LoadAsyncScene(SceneLoader._nextScene, 1f));
    }
}
