using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Player : MonoBehaviour
{
    public static Player Instance { get; private set; }
    private float health = 100f;
    public bool isGrounded = false;
    public bool isFalling = false;
    public bool isTouchingWall = false;
    public bool isAttacking;
    public bool isJumping;
    public bool isDashing = false;
    public bool isRunning = false;
    public bool isMoving = false;
    public  Vector2 throwDir;
    private int healingPotionCount = 0;

    public Vector3 moveDir;
    

    [SerializeField] public GameInput gameInput;
    [SerializeField] public Transform dashTrailTransform;
    [SerializeField] public GameObject rangedAttackObject;
    [SerializeField] public Transform playerThrowPoint;
    [SerializeField] public Transform wallCheckTransform;
    [SerializeField] public Transform healthBar;
    [SerializeField] public GameObject damageNumberPrefab;
    [SerializeField] public Transform slashPointTransform;
    [SerializeField] public Transform groundCheckTransform;

    public TrailRenderer slashPointTrailRenderer;
    public CapsuleCollider2D wallCollider;
    public List<Collider2D> wallColliderList;
    public CapsuleCollider2D playerCollider;
    public CapsuleCollider2D groundCollider;
    public Rigidbody2D playerRigidbody;

    private void Awake()
    {
        Instance = this;
        groundCollider = groundCheckTransform.GetComponent<CapsuleCollider2D>();
        playerRigidbody = GetComponent<Rigidbody2D>();
        wallCollider = wallCheckTransform.GetComponent<CapsuleCollider2D>();
        playerCollider = GetComponent<CapsuleCollider2D>();
        throwDir = new Vector2(1f, 0f);
        moveDir = new Vector3(0, 0, 0);
        wallColliderList = new List<Collider2D>();
        slashPointTrailRenderer = slashPointTransform.GetComponent<TrailRenderer>();
        DisableSlashTrailRenderer();
    }
    
    // Start is called before the first frame update
    void Start()
    {
        GameUIManager.Instance.GetComponent<GameUIManager>().ChangeThrowingObjectCount(PlayerAbilityState.Instance.throwingObjectsCount);
        GameUIManager.Instance.GetComponent<GameUIManager>().ChangeHealingPotionCount(healingPotionCount);
    }

    // Update is called once per frame
    void Update()
    {
    }
    public bool IsTouchingWall()
    {
        int a = wallCollider.OverlapCollider(new ContactFilter2D(), wallColliderList);
        foreach (Collider2D collider2D in wallColliderList)
        {
            //int b = wallCollider.OverlapCollider(new ContactFilter2D(), wallColliderList);
            if (collider2D.gameObject.GetComponent<GroundColider>() != null)
            {
                return true;
            }
        }
        return false;
    }
    public void Damaged(float damageValue)
    {
        if (health > 0)
        {
            health = health - damageValue;
            Image healthBarImg = healthBar.GetComponent<Image>();
            healthBarImg.fillAmount = health / 100;
            GameObject damageNumberTransform = Instantiate(damageNumberPrefab, transform.position + new Vector3(0f, 1f, 0f), new Quaternion(0f, 0f, 0f, 0f));
            damageNumberTransform.GetComponent<DamageNumber>().ChangeNumber(damageValue);
            Debug.Log(health);
        }
    }
    public void EnableSlashTrailRenderer()
    {
        slashPointTrailRenderer.enabled = true;
    }
    public void DisableSlashTrailRenderer()
    {
        slashPointTrailRenderer.enabled = false;
    }
    public void AddHeallingPotions(int amount)
    {
        healingPotionCount = healingPotionCount + amount;
        GameUIManager.Instance.ChangeHealingPotionCount(healingPotionCount);
        Debug.Log("New potion count= " + healingPotionCount);
    }
}
