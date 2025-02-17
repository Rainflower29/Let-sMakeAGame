using System;
using System.Collections;
using System.Collections.Generic;
using System.Security.Cryptography;
using UnityEngine;
using UnityEngine.InputSystem;

public class GameEventSystem : MonoBehaviour
{
    public static GameEventSystem instance;

    private GameEventSystem() { }

    private void Awake()
    {
        instance = this;
    }

    public event Action<Transform> OnPlayerTakeDamage;
    public event Action OnPlayerDead;

    public event Action OnSkill1Use;
    public event Action OnSkill1;
    public event Action OnSkill2Use;
    public event Action OnSkill2;
    public event Action OnSkillPickFall;
    public event Action OnSkillPickAchieve; 

    public int GetSkill_1or2()
    {
        if (OnSkill1Use == null)
            return 1;
        else if (OnSkill2Use == null)
            return 2;
        else return 0;
    }

    public void UseSkill_1or2(InputAction.CallbackContext context)
    {
        if (OnSkill2Use != null)
            Skill2Use(context);
        else if (OnSkill1Use != null)
            Skill1Use(context);
        else SkillUseFall();
    }

    public void SkillUseFall()
    {
        Debug.Log("Skill Use Fall");
    }

    public void PlayerDead()
    {
        if (OnPlayerDead != null)
        {
            OnPlayerDead();
        }
    }

    public void Skill1Use(InputAction.CallbackContext context)
    {
        if(OnSkill1Use != null)
        {
            OnSkill1Use();
            if(OnSkill1 != null)
                OnSkill1();

            OnSkill1Use = null;
        }
        
    }

    public void Skill2Use(InputAction.CallbackContext context)
    {
        if(OnSkill2Use != null)
        {
            OnSkill2Use();
            if(OnSkill2 != null)
                OnSkill2();

            OnSkill2Use = null;
        }
    }

    public void SkillPickFall()
    {
        if(OnSkillPickFall != null) 
        {
            SkillPickFall();
        }
    }

    public void SkillPickAchieve() 
    {
        if(OnSkillPickAchieve != null)
        {
            OnSkillPickAchieve();
        }
    }

    public void PlayerTakeDamage(Transform transform)
    {
        {
            if(OnPlayerTakeDamage != null)
            {
                OnPlayerTakeDamage(transform);
            }
        }
    }

}
