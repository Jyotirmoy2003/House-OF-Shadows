using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Experiment : MonoBehaviour
{
    [SerializeField] Transform player;
    private Transform myTransfrom;
    void Start()
    {
        player=GameObject.FindGameObjectWithTag("Player").GetComponent<Transform>();
        myTransfrom=GetComponent<Transform>();
    }

    // Update is called once per frame
    void Update()
    {
        myTransfrom.LookAt(player,Vector3.down);
    }
}
