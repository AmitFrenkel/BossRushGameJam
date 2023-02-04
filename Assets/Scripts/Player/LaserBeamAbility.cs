using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LaserBeamAbility : AbstractAbility
{
    [SerializeField] private ParticleSystem beam;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnParticleCollision(GameObject other)
    {
        throw new NotImplementedException();
    }
}
