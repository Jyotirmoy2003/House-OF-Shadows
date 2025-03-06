using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TV : MonoBehaviour,IInteractable
{
    [SerializeField] Material mat;
    [SerializeField] Outline outline;
    private bool isLooking;

    public bool onTv;
    void Start()
    {
        onTv=false;
        outline.enabled=false;
    }

    // Update is called once per frame
    void Update()
    {
        if(onTv)TurnOn();
        else mat.SetColor("_EmissionColor",Color.black);
    }
    
    void TurnOn()
    {
 
        float emission = Mathf.PingPong (Time.time, 1f);
        Color baseColor = Color.white; //Replace  with whatever you want for your base color at emission level '1'
 
        Color finalColor = baseColor * Mathf.LinearToGammaSpace (emission);
 
        mat.SetColor ("_EmissionColor", finalColor);
    }


    public void Interact()
    {
        onTv=!onTv;
    }
     public bool Looking
    {
        // when accessing the property simply return the value
        get => isLooking;

        // when assigning the property apply visuals
        set
        {
            // same value ignore to save some work
            if(isLooking == value) return;

            // store the new value in the backing field
            isLooking = value;

            outline.enabled=isLooking;
        }
    }
    public void NotLooking()
    {}
}
