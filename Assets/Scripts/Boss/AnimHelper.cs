using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnimHelper : MonoBehaviour
{
    [SerializeField] private BSpiderBehavior boss;

    public void JumpBegan()
    {
        boss.BeganJump();
    }
    public void IceCubeThrown()
    {
        boss.CreateCubeProjectile();
    }
    public void IceSpikeStart()
    {
        boss.StartSpikes();
    }
    public void StartFall()
    {
        boss.StartFall();
    }
    
}
