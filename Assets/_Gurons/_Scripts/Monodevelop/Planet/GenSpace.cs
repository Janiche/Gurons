using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(BoxCollider2D))]
public class GenSpace : MonoBehaviour
{
    [SerializeField] private bool _pooleable = true;
    [SerializeField] private Transform _spawnPoint;


    private bool _once = false;
    private void OnTriggerEnter2D(Collider2D collision)
    {
        //Debug.LogError("HIT =>" + collision.tag);
        if (collision.CompareTag(Constants._Tags.Wall.ToString()))
        {
            if (!_once)
            {
                //GameEvents.GenSpace();
                GameObject _space = GameManager._instance._OnGenSpace();
                _space.transform.position = _spawnPoint.position;
                _space.SetActive(true);
                _once = true;
            }
        }
    }



    private void OnBecameInvisible()
    {
        if (_pooleable)
        {

            if (GameManager._instance != null)
            {
                GameManager._instance._OnPlaceSpace(gameObject);
                _once = false;
            }
        }
    }
}
