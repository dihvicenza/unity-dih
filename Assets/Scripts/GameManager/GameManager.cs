
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;

public class GameManager : MonoBehaviour
{
    public static GameManager instance;


    public List<int> keys = new List<int>();

    private void Awake()
    {
        if (instance != null)
        {
            Destroy(instance);
        }
        else {
            instance = this;
            DontDestroyOnLoad(instance);
        }
 
    }

    public void Add(int id) {

        keys.Add(id);
    
    }

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
