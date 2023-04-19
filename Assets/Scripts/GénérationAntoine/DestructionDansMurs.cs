using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DestructionDansMurs : MonoBehaviour
{
    private void OnCollisionStay(Collision collision)
    {
        if (collision.collider.gameObject.layer == 8)
        {
            Destroy(this);
        }
    }
}
