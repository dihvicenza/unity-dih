using Unity.VisualScripting.Antlr3.Runtime;
using UnityEngine;

public class DoorScript : MonoBehaviour,IDoors
{
    Animator anim;
    bool isOpen;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        anim=GetComponent<Animator>();  
    }

    // Update is called once per frame
    void Update()
    {
       
    }

    public void open() {
        Debug.Log("Aperto");
        isOpen = true;
        anim.Play("open");
    }
}
