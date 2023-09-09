using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerGroundedState : MonoBehaviour
{
    public static PlayerGroundedState Instance { get; private set; }
    private Player player;

    private List<Collider2D> groundColliderList;

    private void Awake()
    {
        Instance = this;
    }

    // Start is called before the first frame update
    void Start()
    {
        player = Player.Instance;
        groundColliderList = new List<Collider2D>();
    }

    // Update is called once per frame
    void Update()
    {
        Player.Instance.isGrounded=CheckIfGrounded();
    }

    private bool CheckIfGrounded()
    {
        int a = player.groundCollider.OverlapCollider(new ContactFilter2D(), groundColliderList);
        foreach (Collider2D collider2D in groundColliderList)
        {
            if (collider2D.gameObject.GetComponent<GroundColider>() != null)
            {
                return true;
            }
        }
        return false;
    }
}
