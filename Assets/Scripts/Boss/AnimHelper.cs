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
}
