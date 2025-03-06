using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Generator : MonoBehaviour
{
    [SerializeField] GameEvent ElectricityIsOn;
    private bool GotSwitch=false;
    private bool generatorOn=false;
    public GrabObjectReceiver fuelReloder;
    [SerializeField] Material material;
    [SerializeField] GameObject Cube;

    
    void Start()
    {
       material.SetColor("_EmissionColor",Color.black);
       material=Cube.GetComponent<Renderer>().material;
    }

  
    void Update()
    {
        if(generatorOn) TurnOn(); //Indicator 
    }

    public void TwickSwitch(bool value)
    {
        GotSwitch=value;
        ElectricityIsOn.Raise(this,GotSwitch);

        //When Fuel And Swith Both Are On Then Only On generator
        if(GotSwitch && fuelReloder.Recived)
        {
           generatorOn=true;
           FindObjectOfType<AudioManager>().PlaySound("Generator",this.gameObject);
        }
        else{
        material.SetColor("_EmissionColor",Color.black);
        generatorOn=false;
        FindObjectOfType<AudioManager>().StopSound("Generator",this.gameObject);
        }
    }

    //Indicator Function
     void TurnOn()
    {
 
        float emission = Mathf.PingPong (Time.time, 1f);
        Color baseColor = Color.white; //Replace  with whatever you want for your base color at emission level '1'
 
        Color finalColor = baseColor * Mathf.LinearToGammaSpace (emission);
 
        material.SetColor ("_EmissionColor", finalColor);
    }
}
