using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using System;

public class GuronController : MonoBehaviour
{
    #region VARIBLES

    [Header("Valores Posición")]
    [SerializeField] private float _lateral = 0.7f;
    [SerializeField] private float _centro = 0.1f;


    [SerializeField] private Transform _outParticle;

    [Header("SOUNDS")]
    [Tooltip("Sonido a reproducir al ser generado")] [SerializeField] private AudioClip _greet = null;
    [Tooltip("Fall Sounds")] [SerializeField] private AudioClip[] _falls = null;
    private int _fallClipCount = 0;
    [Tooltip("Sonido al impactar")] [SerializeField] private AudioClip _hit = null;
    [Tooltip("Fuente del audio")] [SerializeField] private AudioSource _source = null;

    [Header("GURON'S ELEMENTS")]
    [Tooltip("Componente RigidBody")] [SerializeField] private Rigidbody2D _rb = null;
    [Tooltip("Componente Animator")] [SerializeField] private Animator _an = null;
    [Tooltip("Guron Collider")] [SerializeField] private Collider2D _collider = null;
    [SerializeField] private bool _active = false;
    [SerializeField] private bool _pooled = false;

    private bool isInited = false;

    public float _LocalSize
    {
        get { return _collider.bounds.size.x; }
    }
    public float _GetEulerZ
    {
        get { return transform.eulerAngles.z; }
    }
    public float _LocalPosX
    {
        get { return transform.localPosition.x; }
    }
    public Transform _Parent
    {
        get { return transform.parent; }
    }

    #endregion

    #region FUNCTIONS

    /// <summary>
    /// Initialize guron
    /// </summary>
    public void _Init()
    {
        if (_rb == null)
        {
            _rb = GetComponent<Rigidbody2D>();
        }

        if (_an == null)
        {
            _an = GetComponent<Animator>();
        }

        if (_source == null)
        {
            _source = GetComponent<AudioSource>();
        }

        if (_collider == null)
        {
            _collider = GetComponent<Collider2D>();
        }

        isInited = true;
        //_collider.enabled = true;
        //_rb.isKinematic = false;
        _fallClipCount = _falls.Length;

        _Reset();
    }

    public void _Init(Constants._GuronState _state)
    {
        _Init();
        _an.Play(_state.ToString());
    }

    public void _Init(float _speed, float _frame, float _gravity = 1)
    {
        _Init();

        _an.Play(Constants._GuronState.Balance.ToString(), 0, _frame);
        _an.SetFloat("Speed", _speed);
        _rb.gravityScale = _gravity;

    }

    /// <summary>
    /// Starts State and animation after appearing guron
    /// </summary>
    public void _Appear(float _speed, float _frame, float _gravity = 1)
    {

        //Debug.Log("sfsd" + _speed);
        //if (!isInited)
        //{
        //    _Init(_speed, _frame, _gravity);
        //}

        //else
        //{
        //    //RESET ANIMATION AND RB
        //    _Reset();

        //}
        _Reset();

        _active = true;
        //STARTS ANIMATION FROM FRAME
        _PlayStateSound(Constants._GuronState.Balance);
        _an.Play(Constants._GuronState.Balance.ToString(), 0, _frame);

        _an.SetFloat("Speed", _speed);
        _rb.gravityScale = _gravity;

    }



    /// <summary>
    /// Launch the guron, makong rb kinematic and playing animation state
    /// </summary>
    public void _LaunchGuron()
    {
        if (_active)
        {
            gameObject.layer = 0;
            _rb.isKinematic = false;
            _rb.simulated = true;
            _an.speed = 1;
            _an.SetTrigger("Fall");
            transform.SetParent(null);
        }
    }

    /// <summary>
    /// Evaluate place position only if was launched inside stacked Guron
    /// </summary>
    public void _EvalPos(float _factor, GuronController _guron, GameObject _guronGo)
    {
        //ADD IF IS FIRST BLOCK
        float _eulerZ = (_guron != null) ? _guron._GetEulerZ : _guronGo.transform.eulerAngles.z;
        float _localX = (_guron != null) ? _guron._LocalPosX : _guronGo.transform.localPosition.x;

        //OUTSIDE
        //if (Mathf.Abs(_factor) > 0.7f)
        if (Mathf.Abs(_factor) > _lateral)
        {
            //Debug.Log("OUTSIDE");

            //GameEvents.FailGuron();
            //GameEvents.EndGuron();
            _FailGuron();
            //Invoke("_FailGuron", 2);
        }

        else
        {
            //Debug.Log("INSIDE");


            //INSIDE
            if (Mathf.Abs(_factor) > _centro && Mathf.Abs(_factor) < _lateral)
            //if (Mathf.Abs(_factor) > 0.1f && Mathf.Abs(_factor) < 0.7f)
            {
                //Debug.Log("INSIDE SIDE");
                _localX = _localX + Mathf.Sign(_factor) * 0.3f;
                GameEvents.SideGuron();
            }

            //CENTER
            else if (Mathf.Abs(_factor) <= _centro)
            //else if (Mathf.Abs(_factor) <= 0.1f)
            {

                //Debug.Log("INSIDE CENTER");
                //_centerParticle.SetActive(true);
                GameEvents.CenterGuron();
            }


            //CHANGE GURON TAG
            if (_guron != null)
            {
                _guron._ChangeTag(Constants._Tags.Deprecated);
                transform.SetParent(_guron._Parent);
            }
            else
            {
                _guronGo.tag = Constants._Tags.Deprecated.ToString();
                transform.SetParent(_guronGo.transform.parent);
            }

            //PUT GURON OVER OTHER
            Place(_eulerZ, _localX);
        }


        _guronGo = null;
    }

    void Place(float z, float x)
    {
        _an.SetTrigger("Hit");

        //CHANGE TAG
        _ChangeTag(Constants._Tags.Stack);

        //MAKE KINEMÁTIC
        _rb.isKinematic = true;

        //CHANGE RB VELOCITY
        _rb.linearVelocity = Vector2.zero;
        _rb.angularVelocity = 0;

        //SMOOTH SNAP
        transform.eulerAngles = new Vector3(0, 0, z);
        transform.DOLocalMoveX(x, _centro);
        //transform.DOLocalMoveX(x, 0.1f);


        //END GURON
        GameEvents.EndGuron();
        gameObject.layer = 9;

    }



    /// <summary>
    /// Play a specific clip according state given by parameter
    /// </summary>
    /// <param name="_state">State.</param>
    public void _PlayStateSound(Constants._GuronState _state)
    {
        if (_source == null)
            return;

        switch (_state)
        {
            case Constants._GuronState.Balance:
                _source.PlayOneShot(_greet);
                break;

            case Constants._GuronState.Fall:
                if (_fallClipCount != 0)
                {
                    int _rand = UnityEngine.Random.Range(0, _fallClipCount);
                    _source.PlayOneShot(_falls[_rand]);
                }
                break;

            case Constants._GuronState.Hit:
                _source.PlayOneShot(_hit);
                break;
        }
    }

    public void _ChangeTag(Constants._Tags _tag)
    {
        gameObject.tag = _tag.ToString();
    }

    void OnCollisionEnter2D(Collision2D col)
    {
        if (_active)
        {
            _active = false;
            Constants._Tags _tag = (Constants._Tags)System.Enum.Parse(typeof(Constants._Tags), col.gameObject.tag);

            //CHANGE GURON TAG ACCORDING COLLIDER
            switch (_tag)
            {
                //HIT ON FLOOR
                case Constants._Tags.Floor:

                    //GameEvents.FailGuron();
                    //GameEvents.EndGuron();
                    _an.SetTrigger("Hit");
                    _FailGuron();

                    break;

                case Constants._Tags.Wall:

                    //GameEvents.FailGuron();
                    //GameEvents.EndGuron();
                    //_an.SetTrigger("Hit");
                    _FailGuron();

                    break;

                //HIT ON ACTIVE GURON
                case Constants._Tags.Cube:
                    break;

                //HIT ON SECOND OR PREVIOUS GURON
                case Constants._Tags.Deprecated:

                    //GameEvents.FailGuron();
                    //GameEvents.EndGuron();
                    _FailGuron();
                    //Invoke("_FailGuron", 2);

                    break;

                //HIT THE GURON ON THE TOP
                case Constants._Tags.Stack:

                    GameObject _go = col.gameObject;
                    GuronController _gc = _go.GetComponent<GuronController>();

                    float _size = (_gc != null) ? _gc._LocalSize : _go.GetComponent<Collider2D>().bounds.size.x;
                    float _factor = (transform.position.x - _go.transform.position.x) / _size;
                    _EvalPos(_factor, _gc, _go);

                    break;
            }
        }
    }

    private void _FailGuron()
    {
        _pooled = true;
        gameObject.layer = 0;

        GameEvents.FailGuron();
        _an.SetTrigger("Desappear");
        GameEvents.EndGuron();


        //_collider.enabled = false;
        //_rb.simulated = false;
        //_rb.isKinematic = true;

        Invoke("_Repool", 1f);


        //GameEvents.EndGuron();
    }

    void OnBecameInvisible()
    {
        if (_active)
        {
            GameEvents.FailGuron();
            GameEvents.EndGuron();

            if (GlobalVars.blox >= 2)
            {
                if (_outParticle != null)
                {
                    _outParticle.position = new Vector2(transform.position.x, transform.position.y + 2);
                    _outParticle.gameObject.SetActive(true);
                }
            }

        }

        if (!_pooled)
        {
            _Repool();
        }
    }

    void _Repool()
    {

        _an.enabled = false;
        GameEvents.PlacePool(Constants._PoolType._guron, gameObject);

        _Reset();
    }

    /// <summary>
    /// Reset the gurons to initial state for reuse
    /// </summary>
    public void _Reset()
    {
        _fallClipCount = _falls.Length;
        _rb.isKinematic = true;
        _an.Rebind();
        transform.localEulerAngles = Vector3.zero;
        GetComponent<SpriteRenderer>().color = Color.white;
        _an.enabled = true;
        _ChangeTag(Constants._Tags.Cube);
        //transform.SetParent(null);
        _collider.enabled = true;
        gameObject.layer = 9;
        //_active = true;
    }


    #endregion
}