using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GarderInformation : MonoBehaviour
{
    public static GarderInformation Instance
    {
        get;
        set;
    }

    void Awake()
    {
        DontDestroyOnLoad(transform.gameObject);
        Instance = this;
    }

    void Start()
    {
        //Load first game scene (probably main menu)
        //Application.LoadLevel(2);
    }


    // Data persisted between scenes
    public int life;
    //public float armor = 0;
    //public float weapon = 0;

    private void Update()
    {
        life = PlayerStats.Instance.Health;
    }
}
