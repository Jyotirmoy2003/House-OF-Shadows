using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class JS_Action1 : MonoBehaviour,IJumpScare
{

    [SerializeField] Vector3 startPosition; // Assign the starting position in the inspector
    [SerializeField] Vector3 endPosition; // Assign the ending position in the inspector
    
    [SerializeField] float moveTime = 2f; // Time taken to move from start to end position
    [SerializeField] float delay=4f; //start Movement function after dealy 
    
    private float timer;
    private bool isMoving;

    private void Start()
    {
        timer = 0f;
        isMoving = false;
    }

    private void Update()
    {
        if (isMoving)
        {
            timer += Time.deltaTime;

            float t = Mathf.Clamp01(timer / moveTime);
            transform.position = Vector3.Lerp(startPosition, endPosition, t);

            if (t >= 1f)
            {
                // Movement is complete
                // You can add any additional logic here

                isMoving = false;
                timer = 0f;
            }
        }
    }

    public void StartMovement()
    {
        // Begin the movement from start to end position
        if (!isMoving)
        {
            transform.position = startPosition;
            isMoving = true;
        }
    }

    public void Action()
    {
        StartCoroutine(Wait());
    }

    IEnumerator Wait()
    {
        yield return new WaitForSeconds(delay);
        StartMovement();
        AudioManager.instance.PlaySound("JHS_Running",this.gameObject);
    }
}
