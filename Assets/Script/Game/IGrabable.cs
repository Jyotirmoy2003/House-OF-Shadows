using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IGrabable 
{
   void GrabInHand();
   bool Looking{get;set;}
   void OnDestroy();
   void TwickRigidbody(bool mode);
   
}
