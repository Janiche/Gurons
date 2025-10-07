using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Rope : MonoBehaviour
{
    [Header("Ray Hint")]
    [SerializeField] private bool _rayPos = false;
    [SerializeField] private Transform _rayOrigin;
    [SerializeField] private GameObject _targetPoint;
    [SerializeField] private Vector2 _targetOffset = new Vector2();
    [SerializeField] private float _distance;
    [SerializeField] private LayerMask _mask;
    Ray2D _ray;
    RaycastHit2D _rayhit;

    private void FixedUpdate()
    {
        if (_rayPos)
        {
            //_ray = new Ray2D(_rayOrigin.position, Vector2.down);

            //Debug.DrawRay(_rayOrigin.position, Vector2.down * _distance);

            _rayhit = Physics2D.Raycast(_rayOrigin.position, Vector2.down * _distance, _distance, _mask);

            if (_rayhit.collider != null)
            {
                //Debug.LogError("RAYHIT " + _rayhit.collider.tag);
                if (_rayhit.collider.CompareTag(Constants._Tags.Stack.ToString()))
                {
                    //Debug.LogError("HIT");
                    _targetPoint.SetActive(true);
                    _targetPoint.transform.position = _rayhit.point + _targetOffset;
                }
                else
                {
                    _targetPoint.SetActive(false);
                    //Debug.LogError("NO HIT " + _rayhit.collider.tag);
                }
            }
            else
            {
                //Debug.LogError("NO RAYHIT");
                _targetPoint.SetActive(false);
            }
        }
        else
        {
            //Debug.LogError("NO RAYPOS");
            _targetPoint.SetActive(false);
        }
    }

    public void _ChangeRope(bool rayPos)
    {
        _rayPos = rayPos;
    }
}
