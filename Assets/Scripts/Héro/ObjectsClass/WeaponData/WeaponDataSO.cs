using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Data/WeaponData")]
public class WeaponDataSO : ScriptableObject
{
    public int animation;
    public int durability;
    public int damage;
    public string weaponName;
    public GameObject centerPoint;
    public GameObject weaponPrefab;
}
