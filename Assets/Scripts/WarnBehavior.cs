using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WarnBehavior : MonoBehaviour
{
    private int index;
    private Transform[] warnsT = new Transform[10];
    private GameObject[] warnsG = new GameObject[10];
    void Start()
    {
        for (int i = 0; i < transform.childCount; i++)
        {
            warnsT[i] = transform.GetChild(i);
            warnsG[i] = transform.GetChild(i).gameObject;
        }
    }

    public void CreateWarnAt(Vector3 target)
    {
        warnsG[index].SetActive(true);
        warnsT[index].transform.position = target;
        RaiseIndex();
    }
    public GameObject CreateWarnAtAndReturn(Vector3 target)
    {
        GameObject temp = warnsG[index];
        temp.SetActive(true);
        warnsT[index].transform.position = target;
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
