using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChanceParticle : MonoBehaviour
{
    [SerializeField] private GameObject _chance;

    void OnParticleSystemStopped()
    {
        if (_chance != null)
            _chance.SetActive(false);
    }
}
