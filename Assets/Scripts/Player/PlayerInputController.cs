using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using Unity.Collections;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.TextCore.Text;

public class PlayerInputController : MonoBehaviour
{
    public static PlayerInputController Instance;

    public PlayerInput PlayerInput;

    private Rigidbody2D Rigidbody2D;

    private SpriteRenderer spriteRenderer;

    private PhysicsCheck PhysicsCheck;

    [HideInInspector] public Vector2 MoveDirection;

    /*[HideInInspector]*/ public Vector2 DushDirection;

    private Vector2 inputDirection;

    [Header("移动参数")]
    public float Speed;

    [Header("跳跃参数")]
    public float JumpForce;

    public float JumpForcedown;

    public float JumpLimitFactor;

    [HideInInspector] public bool isJumpAble;

    [Header("冲刺参数")]
    public float DushAcceleration;

    [HideInInspector] public float DushTapTime;

    public float DushTime;

    public float DushCD;

    [HideInInspector] public float speed = 0;

    [HideInInspector] public float timeSpend = 0;

    private float j = 1;

    [HideInInspector] public bool isDushAble;

    [HideInInspector] public bool isDush;

    [HideInInspector] public bool isMoveAble;

    public float UnControllTime;

    // Start is called before the first frame update

    private void Awake()
    {
        Instance = this;

        isMoveAble = true;
        isDushAble = true;
        PlayerInput = new PlayerInput();
        Rigidbody2D = GetComponent<Rigidbody2D>();
        spriteRenderer = GetComponent<SpriteRenderer>();
        PhysicsCheck = transform.Find("IsOnGroundChecker").GetComponent<PhysicsCheck>();

        PlayerInput.GamePlay.Jump.started += Jump;
        PlayerInput.GamePlay.Jump.canceled += LeaveButton;
        PlayerInput.GamePlay.Dush.started += DushTap;

        PlayerInput.GamePlay.Skill.started += GameEventSystem.instance.UseSkill_1or2;

        GameEventSystem.instance.OnPlayerTakeDamage += BeHurt;
    }

    private void Start()
    {
        Rigidbody2D.drag = 0f;
    }

    private void OnEnable()
    {
        PlayerInput.Enable();
    }

    private void OnDisable()
    {
        PlayerInput.Disable();
    }

    void Update()
    {
        GetMoveDirection();
        GetisJumpAble();
        Debug.Log(PhysicsCheck.CanJumpTwice);
        Debug.Log(PhysicsCheck.jumpTimes);
    }

    private void FixedUpdate()
    {
        DushControll();
        Dush();

        if (isMoveAble) 
        {
            Move();
        }
    }

    private void Move()
    {
        if(isMoveAble)
        {
            Rigidbody2D.velocity = new Vector2(Speed*MoveDirection.x*Time.deltaTime, Rigidbody2D.velocity.y);
        }

        if(MoveDirection.x > 0)
        {
            spriteRenderer.flipX = false;
        }
        else if(MoveDirection.x < 0)
        {
            spriteRenderer.flipX = true;
        }
    }

    private void GetMoveDirection()
    {
        inputDirection = PlayerInput.GamePlay.Move.ReadValue<Vector2>();
        if (inputDirection.x > 0)
            MoveDirection.x = 1;
        else if(inputDirection.x < 0)
            MoveDirection.x = -1;
        else MoveDirection.x = 0;
    }

    private void GetDushDirection()
    {
        if(PlayerInput.GamePlay.Move.ReadValue<Vector2>() != Vector2.zero)
            DushDirection = PlayerInput.GamePlay.Move.ReadValue<Vector2>().normalized;
        else
        {
            if (spriteRenderer.flipX != true)
            {
                DushDirection.x = 1;
                DushDirection.y = 0;
            }

            else
            {
                DushDirection.x = -1;
                DushDirection.y = 0;
            }
        }
    }

    private void Jump(InputAction.CallbackContext context)
    {
        if(isJumpAble)
            Rigidbody2D.AddForce(JumpForce * transform.up, ForceMode2D.Impulse);
    }

    private void Jump()
    {
        if (isJumpAble)
            Rigidbody2D.AddForce(JumpForce * transform.up, ForceMode2D.Impulse);
    }

    private void LeaveButton(InputAction.CallbackContext context)
    {
        if(Rigidbody2D.velocity.y >= JumpLimitFactor)
            Rigidbody2D.AddForce(JumpForcedown * Vector2.down, ForceMode2D.Impulse);
    }

    private void GetisJumpAble()
    {
        if(PhysicsCheck.jumpTimes>0)
            isJumpAble = true;
        else
            isJumpAble = false;
    }
    public void JumpTwice()
    {
        if (PhysicsCheck.isOnGround)
            PhysicsCheck.CanJumpTwice = true;
    }

    private void DushTap(InputAction.CallbackContext context)
    {
        GetDushDirection();
        if(isDushAble) 
        {
            isDush = true;
            DushTapTime = Time.time;
            isDushAble = false;
        }

    }
    public void DushTap()
    {
        GetDushDirection();
        if (isDushAble)
        {
            isDush = true;
            DushTapTime = Time.time;
            isDushAble = false;
        }

    }

    private void DushControll()
    {
        if(isDush)
        {
            if(Time.time - DushTapTime >= DushTime)
                isDush = false;
        }
        if(!isDushAble)
        {
            if (Time.time - DushTapTime >= DushCD)
                isDushAble = true;
        }
    }

    private void Dush()
    {
        if(isDush)
        {
            j++;
            MoveDisable();
            Debug.Log("Dush");
            speed += DushAcceleration * (timeSpend += Time.deltaTime);

            Rigidbody2D.position += DushDirection.normalized * speed * Time.deltaTime;
            if(DushDirection.x < 0)
            {
                Rigidbody2D.AddForce(Vector2.left * speed, ForceMode2D.Force);
            }
            else if(DushDirection.x > 0) 
            {
                Rigidbody2D.AddForce(Vector2.right * speed, ForceMode2D.Force);
            }

            if (DushDirection.y > 0)
                Rigidbody2D.AddForce(Vector2.up * speed, ForceMode2D.Force);
            else if (DushDirection.y < 0)
                Rigidbody2D.AddForce(Vector2.down * speed, ForceMode2D.Force);
        }
        if (!isDush&&j!=1)
        {
            Rigidbody2D.velocity = Vector2.zero;
            MoveEnable();
            speed = 0;
            timeSpend = 0;
            j = 1;
        }
    }

    public void BeHurt(Transform attacker)
    {
        float knockBack = attacker.GetComponent<Attack>().knockBack;

        Rigidbody2D.velocity = Vector2.zero;
        UnMoveForaWhile(UnControllTime);
        Vector2 dir = new Vector2((transform.position.x - attacker.transform.position.x), (transform.position.y - attacker.transform.position.y)).normalized;
        Rigidbody2D.AddForce(dir * knockBack, ForceMode2D.Impulse);
        
    }

    public void UnMoveForaWhile(float Time)
    {
        MoveDisable();
        Invoke("MoveEnable", Time);
    }

    public void MoveDisable()
    {
        isMoveAble = false;
        PlayerInput.GamePlay.Move.Disable();
    }
    public void MoveEnable()
    {
        isMoveAble = true;
        PlayerInput.GamePlay.Move.Enable();
    }
    public void ControllDisable()
    {
        PlayerInput.GamePlay.Disable();
    }
    public void ControllEnable()
    {
        PlayerInput.GamePlay.Enable();
    }
}
