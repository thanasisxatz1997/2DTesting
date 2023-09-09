using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SmallBoarEnemy : MonoBehaviour
{
    bool seesPlayer;
    private Transform playerTransform;
    [SerializeField] private Transform message;
    private bool messageShown = false;
    List<RaycastHit2D> raycastHitList=new List<RaycastHit2D>();
    // Update is called once per frame
    void Update()
    {
        LookForPlayer();
    }

    public void LookForPlayer()
    {
        Physics2D.CircleCast(transform.position, 10f, transform.position, new ContactFilter2D(), raycastHitList);
        seesPlayer = false;
        foreach (RaycastHit2D raycastHit in raycastHitList)
        {
            if (raycastHit.transform.GetComponent<Player>())
            {
                playerTransform = raycastHit.transform;
                seesPlayer = true;
                
                Debug.Log("Player Found By board!");
                if (messageShown==false)
                {
                    message.gameObject.SetActive(true);
                    messageShown = true;
                }
            }
        }
    }
}
