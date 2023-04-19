using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class WeaponScript : MonoBehaviour
{
    [SerializeField] private WeaponDataSO weaponData;
    
    public WeaponDataSO WeaponData { get => weaponData; set => weaponData = value; }
}
