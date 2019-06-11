using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Weapon_PickUp : MonoBehaviour
{


    public WeaponStats WeaponHolding;

    public GameObject[] MeshObject;

    public void Enable()
    {
        foreach (GameObject item in MeshObject)
        {
            if (WeaponHolding.WeaponModel != null && item.GetComponent<MeshFilter>().name == WeaponHolding.WeaponModel.name)
            {
                item.gameObject.transform.localScale = new Vector3(1.5f, 1.5f, 1.5f);
                item.SetActive(true);
            }
        }
    }


}
