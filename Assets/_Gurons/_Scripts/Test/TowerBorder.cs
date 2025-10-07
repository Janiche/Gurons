using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TowerBorder : MonoBehaviour
{
    private enum Side
    {
        none,
        left,
        right
    }

    [SerializeField] private TowerController _controller;
    [SerializeField] private Side _side = Side.none;
    //[SerializeField] private 
    private bool _once = false;


    private void OnCollisionEnter(Collision collision)
    {

        if (collision.gameObject.CompareTag(Constants._Tags.Stack.ToString()) || collision.gameObject.CompareTag(Constants._Tags.Deprecated.ToString()))
        {
            if (!_once)
            {
                //Debug.LogError("HIT" + collision.gameObject.name);
                _once = true;

                bool _dir = false;
                if (_side == Side.left)
                {
                    _dir = true;
                }
                else if (_side == Side.right)
                {
                    _dir = false;
                }

                _controller._ChangeDir(_dir);

            }
        }
    }

    private void OnCollisionExit(Collision collision)
    {
        if (collision.gameObject.CompareTag(Constants._Tags.Stack.ToString()) || collision.gameObject.CompareTag(Constants._Tags.Deprecated.ToString()))
        {
            if (_once)
            {
                _once = false;

            }
        }
    }


    private void OnCollisionEnter2D(Collision2D collision)
    {

        if (collision.gameObject.CompareTag(Constants._Tags.Stack.ToString()) || collision.gameObject.CompareTag(Constants._Tags.Deprecated.ToString()))
        {
            if (!_once)
            {
                //Debug.LogError("HIT" + collision.gameObject.name);
                _once = true;

                bool _dir = false;
                if (_side == Side.left)
                {
                    _dir = true;
                }
                else if (_side == Side.right)
                {
                    _dir = false;
                }

                _controller._ChangeDir(_dir);

            }
        }
    }

    private void OnCollisionExit2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag(Constants._Tags.Stack.ToString()) || collision.gameObject.CompareTag(Constants._Tags.Deprecated.ToString()))
        {
            if (_once)
            {
                _once = false;

            }
        }
    }




}
