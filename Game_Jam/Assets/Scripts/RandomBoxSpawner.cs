using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RandomBoxSpawner : MonoBehaviour
{

    public GameObject WeaponBox;

    public SpawnBox_Info[] SpawnPoints;

    public LayerMask Mask;

    public float SpawnTime;

    private void Awake()
    {
        InvokeRepeating("SpawnBox", 0, SpawnTime);
    }

    void SpawnBox()
    {
        int PositionRel = Random.Range(0, SpawnPoints.Length);
        if(SpawnPoints[PositionRel].CurrentBox == null)
        {
            if (!Physics.Raycast(SpawnPoints[PositionRel].transform.position, SpawnPoints[PositionRel].transform.up, Mathf.Infinity, Mask))
            {
                GameObject Ins = Instantiate(WeaponBox, SpawnPoints[PositionRel].transform.position, Quaternion.identity) as GameObject;

                SpawnPoints[PositionRel].GetComponent<SpawnBox_Info>().CurrentBox = Ins;
                Ins.GetComponent<Box_BH>().Spawn = SpawnPoints[PositionRel];
            }


        }
    }


}
