using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class Potion : MonoBehaviour
{
    [SerializeField] private PotionDataSO potionData;
    
    public PotionDataSO PotionData { get => potionData; set => potionData = value; }
}
