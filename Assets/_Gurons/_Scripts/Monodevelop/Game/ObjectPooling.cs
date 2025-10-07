//using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]

public class ObjectPooling : MonoBehaviour
{

    /// <summary>
    /// Takes a random item from a specific pool
    /// </summary>
    /// <returns>The random object</returns>
    /// <param name="_poolType">Pool type.</param>
    public GameObject _TakePoolRandom(Constants._PoolType _poolType)
    {
        GameObject _go;
        Pool _pool = _GetPool(_poolType);

        _go = _pool.TakeRandomItem();

        return _go;
    }

    /// <summary>
    /// Takes a item by name from specific pool
    /// </summary>
    /// <returns>The item taken by name</returns>
    /// <param name="_poolType">Pool type.</param>
    /// <param name="_name">Name of object to take.</param>
    public GameObject _TakePoolByName(Constants._PoolType _poolType, string _name)
    {
        GameObject _go;
        Pool _pool = _GetPool(_poolType);

        _go = _pool.TakeItemByName(_name);

        return _go;
    }

    /// <summary>
    /// Takes a item by index from specific pool
    /// </summary>
    /// <returns>The item taken by index</returns>
    /// <param name="_poolType">Pool type.</param>
    /// <param name="_index">Index of the object.</param>
    public GameObject _TakePoolByIndex(Constants._PoolType _poolType, int _index)
    {
        GameObject _go;
        Pool _pool = _GetPool(_poolType);

        _go = _pool.TakeItemByIndex(_index);

        return _go;
    }

    public GameObject _TakePoolFirst(Constants._PoolType _poolType)
    {
        GameObject _go;
        Pool _pool = _GetPool(_poolType);

        _go = _pool.TakeFirstItem();

        return _go;
    }

    private Pool _GetPool(Constants._PoolType _poolType)
    {
        Pool _pool = new Pool();

        switch (_poolType)
        {
            case Constants._PoolType._guron:
                _pool = guronPool;
                break;

            case Constants._PoolType._space:
                _pool = spacePool;

                break;

                //case Constants._PoolType._powerUp:
                //    _pool = powerUpPool;

                //    break;

                //case Constants._PoolType._feedback:
                //    _pool = feedbackPool;

                //    break;

        }

        return _pool;
    }

    public void _PlacePool(Constants._PoolType _poolType, GameObject _poolObject)
    {
        Pool _pool = new Pool();

        //Debug.Log("POOOOOL INSIDE");

        switch (_poolType)
        {
            case Constants._PoolType._guron:
                _pool = guronPool;
                break;

            case Constants._PoolType._space:
                _pool = spacePool;

                break;
        }

        _pool.PlaceItem(_poolObject);
    }

    [System.Serializable]
    public class Pool
    {
        [Tooltip("Cantidad máxima de elementos iguales clonados en el pool")] public int clonedItem = 2;
        [Tooltip("Objeto padre del pool")] [SerializeField] private Transform _parent = null;
        [Tooltip("Elementos que conforman el Pool")] [SerializeField] private List<GameObject> poolList = new List<GameObject>();

        /// <summary>
        /// Inicializa rl pool con lista recibida
        /// </summary>
        /// <param name="pool">Pool.</param>
        public void SetPool(List<GameObject> pool)
        {
            poolList = pool;
        }

        /// <summary>
        /// Saca objeto aleatorio del pool
        /// </summary>
        /// <returns>The item.</returns>
        public GameObject TakeRandomItem()
        {
            GameObject g = null;

            //TODO: CHANGE POOLIST.COUNT FOR A CACHED VARIABLE
            int r = Random.Range(0, poolList.Count);
            g = poolList[r];
            g.SetActive(true);
            poolList.Remove(g);

            return g;
        }

        public GameObject TakeFirstItem()
        {
            GameObject g = null;

            if (poolList.Count > 0)
            {
                g = poolList[0];
                //g.SetActive(true);
                poolList.Remove(g);
            }

            return g;
        }

        /// <summary>
        /// Saca objeto de pool utilizando su nombre
        /// </summary>
        /// <returns>The item.</returns>
        /// <param name="_name">Name.</param>
        public GameObject TakeItemByName(string _name)
        {
            GameObject g = poolList.Find(go => go.name == _name);

            if (g != null)
            {
                g.SetActive(true);
                poolList.Remove(g);
            }
            return g;
        }

        /// <summary>
        /// Saca objeto de pool utilizando indice
        /// </summary>
        /// <returns>The item.</returns>
        /// <param name="_name">Name.</param>
        public GameObject TakeItemByIndex(int _name)
        {
            GameObject g = null;
            return g;
        }

        /// <summary>
        /// Guarda objeto en pool
        /// </summary>
        /// <param name="item">Item.</param>
        public void PlaceItem(GameObject item)
        {

            //Debug.Log("POOOOOL PLACE OUT");

            //Posición y estado
            try
            {
                //Debug.Log("POOOOOL PLACE TRY");

                //item.transform.SetParent(_parent);
                item.SetActive(false);
                item.transform.localPosition = Vector3.zero;

                poolList.Add(item);
            }
            catch (System.Exception e)
            {
                //Debug.Log("POOOOOL PLACE CATCH");
                Debug.LogException(e);
            }
        }
    }

    public Pool guronPool;
    public Pool spacePool;
    //public Pool powerUpPool;
    //public Pool feedbackPool;

    //void Awake()
    //{
    //    _instance = this;
    //}
}
