using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WarnBehavior : MonoBehaviour
{
    private int index;
    private Transform[] warnsT = new Transform[10];
    private GameObject[] warnsG = new GameObject[10];
    private Warn[] warnsW = new Warn[10];
    void Start()
    {
        for (int i = 0; i < transform.childCount; i++)
        {
            warnsT[i] = transform.GetChild(i);
            warnsG[i] = transform.GetChild(i).gameObject;
            warnsW[i] = transform.GetChild(i).GetComponent<Warn>();
        }
    }

    public void CreateWarnAt(Vector3 target)
    {
        warnsG[index].SetActive(true);
        warnsT[index].transform.position = target;
        RaiseIndex();
    }
    public GameObject CreateWarnAtAndReturn(Vector3 target, Transform projectile, bool SetOnGround)
    {
        GameObject temp = warnsG[index];
        temp.SetActive(true);
        warnsT[index].transform.position = target;
        if (SetOnGround)
            warnsW[index].SetOnGround(projectile);
        else
            warnsW[index].Set(projectile);
        RaiseIndex();
        return temp;
    }
    private void RaiseIndex()
    {
        index++;
        if (index >= warnsG.Length)
            index = 0;
    }
}
