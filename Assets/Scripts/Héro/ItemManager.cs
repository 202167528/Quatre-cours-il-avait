using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemManager : MonoBehaviour
{
    [SerializeField] private Transform itemSlot;
    public WeaponDataSO equippedWeapon;
    public PotionDataSO equippedPotion;

    private GameObject currentWeapon;
    private GameObject currentPotion;

    public void EquipWeapon(WeaponDataSO weaponData)
    {
        equippedWeapon = weaponData;
        
        if (currentWeapon != null)
        {
            Instantiate(currentWeapon, transform.position, Quaternion.identity);
            Destroy(currentWeapon);
        }

        currentWeapon = Instantiate(equippedWeapon.weaponPrefab, itemSlot, true);
        currentWeapon.transform.rotation = Quaternion.LookRotation(-transform.right);
        currentWeapon.transform.localPosition = Vector3.zero;
    }

    public void EquipPotion(PotionDataSO potionData)
    {
        equippedPotion = potionData;
        
        if (currentPotion != null)
        {
            Instantiate(currentPotion, transform.position, Quaternion.identity);
            Destroy(currentPotion);
        }

        currentPotion = Instantiate(equippedPotion.potionPrefab, itemSlot, true);
        currentPotion.transform.rotation = Quaternion.identity;
        currentPotion.transform.localPosition = Vector3.zero;
    }

    public void DestroyGameObject(GameObject gameObject)
    {
        Destroy(gameObject);
    }
}
