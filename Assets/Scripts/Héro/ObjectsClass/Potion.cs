using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class Potion : MonoBehaviour, IUsable
{
    [field: SerializeField] public UnityEvent OnUse { get; private set; }

    [SerializeField] private int healthBoost = 1;
    
    public void Use(GameObject actor)
    {
        //actor.GetComponent<PlayerInput>().AddHealth(healthBoost);
        OnUse?.Invoke();
        Destroy(gameObject);
    }
}
