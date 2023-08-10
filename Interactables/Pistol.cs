using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Pistol : MonoBehaviour
{
   [SerializeField] float range;
   [SerializeField] int bulletCount;
   [SerializeField] float damageAmount;
   [SerializeField] Transform shootPoint;
   [SerializeField] float rateOfire;
   [SerializeField] GameObject flashObject;
   private float currentTime;
   private UIManager uiManager;
   

   [SerializeField] bool inhand;
   
   private Ray ray;
   private RaycastHit hit;

   void Start()
   {
        uiManager=FindObjectOfType<UIManager>();
   }
   




    // Update is called once per frame
    void Update()
    {
        if(Input.GetMouseButton(0) && currentTime>=rateOfire)
        {
            currentTime=0;
            Shoot();
        }

        currentTime+=Time.deltaTime;
    }

 

    void Shoot()
    {   
        //ray cast
        ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        //test if there is more then 1 bullet
        if(bulletCount<=0) return;

        AudioManager.instance.PlaySound("Pistol",this.gameObject);
        //show falsh
       GameObject falsh= Instantiate(flashObject,shootPoint.position,shootPoint.rotation);
       Destroy(falsh,0.1f);

       bulletCount--;
       //update the UI;
       uiManager.SetBulletUI(bulletCount);
       if (Physics.Raycast(ray, out hit, range))
       {
            IDamagable entity=hit.collider.gameObject.GetComponent<IDamagable>();
            if(entity==null) return;
            entity.TookDamage(damageAmount);
            
       }
    }

    //fun to add bullets
    public void AddBullet(int amount)
    {
        bulletCount+=amount;
        uiManager.SetBulletUI(bulletCount);
    }
    public int GetBullet()
    {
        return bulletCount;
    }
}
