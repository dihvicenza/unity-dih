using System;
using UnityEngine;

public class SimpleKeyScript : MonoBehaviour, IDoors
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    [SerializeField] GameObject door;
    [SerializeField] int keyCode;

    void Start()
    {
        
    }

    public void open() {
        Debug.Log("Aperto");
        door.GetComponent<IDoors>().open();
    }

}
