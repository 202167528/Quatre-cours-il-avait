﻿/*
 *  Author: ariel oliveira [o.arielg@gmail.com]
 */

using UnityEngine;

public class PlayerStats : MonoBehaviour
{
    public delegate void OnHealthChangedDelegate();
    public OnHealthChangedDelegate onHealthChangedCallback;

    #region Sigleton
    private static PlayerStats instance;
    public static PlayerStats Instance
    {
        get
        {
            if (instance == null)
                instance = FindObjectOfType<PlayerStats>();
            return instance;
        }
    }
    #endregion

    [SerializeField] private int health;
    [SerializeField] private int maxHealth;
    [SerializeField] private int maxTotalHealth;

    GameManagerDébut gameManager;

    public int Health { get { return health; } }
    public int MaxHealth { get { return maxHealth; } }
    public int MaxTotalHealth { get { return maxTotalHealth; } }

    private void Start()
    {
        gameManager = GameObject.Find("GameManagerDébut").GetComponent<GameManagerDébut>();
        health = gameManager.vie;
        //var différenceDeVie = 5 - gameManager.vie;


        //HealthBarHUDTester viePerso = gameObject.AddComponent<HealthBarHUDTester>();
        //viePerso.Hurt(différenceDeVie);

        //for (int i = 0; i < différenceDeVie; i++)
        //{
        //    Hurt(i);
        //}

       // ClampHealth();
    }

    public void Heal(int health)
    {
        this.health += health;
        ClampHealth();
    }

    public void TakeDamage(int dmg)
    {
        health -= dmg;
        ClampHealth();
    }

    public void AddHealth()
    {
        if (maxHealth < maxTotalHealth)
        {
            maxHealth += 1;
            health = maxHealth;

            if (onHealthChangedCallback != null)
                onHealthChangedCallback.Invoke();
        }
    }

    void ClampHealth()
    {
        health = Mathf.Clamp(health, 0, maxHealth);

        if (onHealthChangedCallback != null)
            onHealthChangedCallback.Invoke();
    }
}
