using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemManager : MonoBehaviour
{
    [SerializeField] private Transform itemSlot;
    public WeaponDataSO equippedWeapon;

    private GameObject currentWeapon;
    private GameObject currentPotion;

    GameManagerDébut gameManager;

    public void EquipWeapon(WeaponDataSO weaponData)
    {
        equippedWeapon = weaponData;
        var rb = equippedWeapon.weaponPrefab.GetComponent<Rigidbody>();

        if (currentWeapon != null)
        {
            if (rb != null)
            {
                rb.isKinematic = false;
            }
            Instantiate(currentWeapon, transform.position, Quaternion.identity);
            Destroy(currentWeapon);
        }

        currentWeapon = Instantiate(equippedWeapon.weaponPrefab, itemSlot, true);
        currentWeapon.transform.rotation = Quaternion.LookRotation(-transform.right);
        currentWeapon.transform.localPosition = Vector3.zero;
        if (rb != null)
        {
            rb.isKinematic = true;
        }
    }

    public void EquipPotion(PotionDataSO potionData)
    {
        // La potion donne de la vie au personnage **À COMPLÉTER**
       
        gameManager = GameObject.Find("GameManagerDébut").GetComponent<GameManagerDébut>();
        HealthBarHUDTester viePerso = gameObject.AddComponent<HealthBarHUDTester>();
        viePerso.Heal(potionData.healthBoost);
        gameManager.AugmenterVie();
    }

    public void DestroyGameObject(GameObject gameObject)
    {
        Destroy(gameObject);
    }
}
