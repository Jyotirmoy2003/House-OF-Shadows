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
   private bool canShoot = true;
   private bool shootHold = false;
   
   
   private Ray ray;
   private RaycastHit hit;

   

    void OnDestroy()
    {
        SubcribeToInput(false);
    }

    



    void SubcribeToInput(bool val)
    {
        if(val)
        {
            _GameAssets.Instance.gameInput.OnFireEvent += OnFire;
            
        }else{
            _GameAssets.Instance.gameInput.OnFireEvent -= OnFire;
        }
    }

    void OnFire(bool performed)
    {
        shootHold = performed;
        if(performed) Shoot();
    }

 

    void Shoot()
    {   
        if(!canShoot) return;
        canShoot = false;
        Invoke(nameof(Reshoot),rateOfire);

        InputManager.Instance.MediumRumble();
        
        //ray cast
        ray = Camera.main.ScreenPointToRay(new Vector3(Screen.width / 2, Screen.height / 2, 0));

        //test if there is more then 1 bullet
        if(bulletCount<=0) return;

        AudioManager.instance.PlaySound("Pistol",this.gameObject);
        //show falsh
       GameObject falsh= Instantiate(flashObject,shootPoint.position,shootPoint.rotation);
       Destroy(falsh,0.1f);

       bulletCount--;
       //update the UI;
       Debug.Log("UI valid status: "+(UIManager.Instance == null));
       UIManager.Instance.SetBulletUI(bulletCount);
       if (Physics.Raycast(ray, out hit, range))
       {
            IDamagable entity=hit.collider.gameObject.GetComponent<IDamagable>();
            if(entity==null) return;
            entity.TookDamage(damageAmount);
            
       }

       
    }

    void Reshoot()
    {
        canShoot = true;
        if(shootHold) Shoot();
    }

    //fun to add bullets
    public void AddBullet(int amount)
    {
        bulletCount+=amount;
        UIManager.Instance.SetBulletUI(bulletCount);
    }
  

    public void ListnToOnPistolAdded(Component sender, object data)
    {
        if((bool)data)
        {
            SubcribeToInput(true);
            UIManager.Instance.SetBulletUI(bulletCount);
        }else{
            SubcribeToInput(false);
        }
    }
}
