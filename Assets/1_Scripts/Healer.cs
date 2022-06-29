using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Healer : MonoBehaviour
{
    [SerializeField] int healingAmount = 25;

    public int GetHealing()
    {
        return healingAmount;
    }

    public void Hit()
    {
        Destroy(gameObject);
    }
}
