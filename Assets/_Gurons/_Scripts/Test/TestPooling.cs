using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestPooling : MonoBehaviour
{
    public enum Action
    {
        _none,
        _init,
        _take,
        _place,
    }

    [SerializeField] ObjectPooling _objectPooling;
    [SerializeField] Constants._PoolType _poolType = Constants._PoolType._none;
    [SerializeField] Constants._SelectionType _selectionType = Constants._SelectionType._none;
    public Action _action = Action._none;
    public bool _test = false;
    [SerializeField] GameObject _object;

    public void Update()
    {
        if (_test)
        {
            switch (_action)
            {
                case Action._init:

                    GameEvents.OnTakePool -= OnTakePool;
                    GameEvents.OnTakePool += OnTakePool;

                    GameEvents.OnPlacePool -= OnPlacePool;
                    GameEvents.OnPlacePool += OnPlacePool;

                    break;

                case Action._take:
                    GameEvents.TakePool(this._poolType, _selectionType);
                    break;

                case Action._place:
                    GameEvents.PlacePool(this._poolType, _object);
                    break;
            }
            _action = Action._none;
        }
    }

    public void OnTakePool(Constants._PoolType _poolType, Constants._SelectionType _selectionType)
    {
        switch (_selectionType)
        {
            case Constants._SelectionType._index:
                break;

            case Constants._SelectionType._name:
                string _name = "MoreTime";

                _object = _objectPooling._TakePoolByName(_poolType, _name);
                break;

            case Constants._SelectionType._random:
                _object = _objectPooling._TakePoolRandom(_poolType);
                break;
        }

    }

    public void OnPlacePool(Constants._PoolType _poolType, GameObject _poolObject)
    {
        _objectPooling._PlacePool(_poolType, _object);
        _object = null;
    }
}
