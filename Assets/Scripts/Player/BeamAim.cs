using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.VFX;

public class BeamAim : MonoBehaviour
{
    [SerializeField] private VisualEffect beamVFX;

    [SerializeField] private Transform player;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        beamVFX.SetVector3("Position",player.position);
    }
}
