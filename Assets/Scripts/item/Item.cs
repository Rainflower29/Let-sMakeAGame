using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Item : MonoBehaviour
{
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag == "Player")
        {
            itemBePick();
        }
    }

    protected virtual void itemBePick()
    {
        Destroy(this.gameObject);
        Debug.Log("IsPick");
    }
}
