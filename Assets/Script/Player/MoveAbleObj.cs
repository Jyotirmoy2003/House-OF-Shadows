using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoveAbleObj : MonoBehaviour
{
    [SerializeField] float pushForce=1;

    private void OnControllerColliderHit( ControllerColliderHit hit)
    {
        Rigidbody rig=hit.collider.attachedRigidbody;
        if(rig!=null)
        {
            Vector3 forceDirection=hit.gameObject.transform.position-transform.position;
            forceDirection.Normalize();

            rig.AddForceAtPosition(forceDirection*pushForce,transform.position,ForceMode.Impulse);
        }
    }
}
