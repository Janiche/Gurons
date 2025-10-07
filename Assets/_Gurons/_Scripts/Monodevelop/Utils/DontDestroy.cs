using UnityEngine;

public class DontDestroy : MonoBehaviour 
{
    //EVITA LA DESTRUCCIÓN DEL OBJETO QUE TENGA ESTE SCRIPT
	private void Awake()
	{
        DontDestroyOnLoad(gameObject);
	}
}
