using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemDrop : MonoBehaviour
{
    [SerializeField] GameObject dropPrefab;
    [SerializeField] float dropSpeed = 5f;
    [SerializeField] [Range(0, 100)] float dropRate = 10f; 

    public void DropItem()
    {
        if (Roll())
        {
            var item = Instantiate(dropPrefab, transform.position, Quaternion.identity) as GameObject;
            item.GetComponent<Rigidbody2D>().velocity = new Vector2(0,-1) * dropSpeed;
        }
    }

    public bool Roll()
    {
        var roll = Random.Range(0, 100);
        return roll <= dropRate;
    }
}
