using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Data/PotionData")]
public class PotionDataSO : ScriptableObject
{
    public int healthBoost;
    public string potionName;
    public GameObject potionPrefab;
}
