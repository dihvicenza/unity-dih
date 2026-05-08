using Unity.VisualScripting.Antlr3.Runtime;
using UnityEngine;

public class DoorScript : MonoBehaviour,IDoors
{
    Animator anim;
    bool isOpen;
    float timer;
    [SerializeField] float OpenTimer;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        anim=GetComponent<Animator>();  
    }

    // Update is called once per frame
    void Update()
    {
        if (isOpen)
        {
            timer += Time.deltaTime;
            if (timer > OpenTimer)
            {
                timer = 0;
                isOpen = false;
                anim.Play("close");
            }
        }
    }

    public void open() {
        if (!isOpen)
        {
            Debug.Log("Aperto");
            isOpen = true;
            anim.Play("open");
        }
    }
}
