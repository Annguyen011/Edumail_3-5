using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

namespace Section3
{
    public class PlayerController : MonoBehaviour
    {
        public Action<int, int> onHPChanged;

        [SerializeField] private float m_MoveSpeed;
        [SerializeField] private Transform m_FiringPoint;
        [SerializeField] private float m_FiringCooldown;
        [SerializeField] private int m_Hp;
        [SerializeField] private bool m_UseNewInputSystem;

        private int m_CurrentHp;
        private float m_TempCooldown;
        private PlayerInput m_PlayerInput;
        private Vector2 m_MovementInputValue;
        private bool m_AttackInputValue;

        private void OnEnable()
        {
            if(m_PlayerInput == null)
            {
                m_PlayerInput = new PlayerInput();
                m_PlayerInput.Player.Movement.started += OnMovement;
                m_PlayerInput.Player.Movement.performed += OnMovement;
                m_PlayerInput.Player.Movement.canceled += OnMovement;
                m_PlayerInput.Player.Attack.started += OnAttack;
                m_PlayerInput.Player.Attack.performed += OnAttack;
                m_PlayerInput.Player.Attack.canceled += OnAttack;
            }
            m_PlayerInput.Enable();
        }

        private void OnDisable()
        {
            m_PlayerInput.Disable();
        }

        // Start is called before the first frame update
        void Start()
        {
            m_CurrentHp = m_Hp;
            if (onHPChanged != null)
                onHPChanged(m_CurrentHp, m_Hp);
        }

        // Update is called once per frame
        void Update()
        {
            if (!GameManager.Instance.IsActive())
                return;

            Vector2 direction = Vector2.zero;
            if (!m_UseNewInputSystem)
            {
                float horizontal = Input.GetAxis("Horizontal");
                float vertical = Input.GetAxis("Vertical");
                direction = new Vector2(horizontal, vertical);

                if (Input.GetKey(KeyCode.Space))
                {
                    if (m_TempCooldown <= 0)
                    {
                        Fire(); 
                        m_TempCooldown = m_FiringCooldown; 
                    }
                }
            }
            else
            {
                direction = m_MovementInputValue;

                if (m_AttackInputValue)
                {
                    if (m_TempCooldown <= 0)
                    {
                        Fire(); 
                        m_TempCooldown = m_FiringCooldown; 
                    }
                }
            }
            transform.Translate(direction * Time.deltaTime * m_MoveSpeed); 

            m_TempCooldown -= Time.deltaTime; 
        }

        private void Fire()
        {
            //ProjectileController projectile = Instantiate(m_Projectile, m_FiringPoint.position, Quaternion.identity, null);
            ProjectileController projectile = SpawnManager.Instance.SpawnPlayerProjectile(m_FiringPoint.position);
            projectile.Fire(1);

            SpawnManager.Instance.SpawnShootingFX(m_FiringPoint.position);
            AudioManager.Instance.PlayLazerSFX();
        }

        public void Hit(int damage)
        {
            m_CurrentHp -= damage;
            if (onHPChanged != null)
                onHPChanged(m_CurrentHp, m_Hp);
            if (m_CurrentHp <= 0)
            {
                Destroy(gameObject);

                SpawnManager.Instance.SpawnExplosionFX(transform.position);
                GameManager.Instance.Gameover(false);
                AudioManager.Instance.PlayExplosionSFX();
            }
            AudioManager.Instance.PlayHitSFX();
        }

        private void OnAttack(InputAction.CallbackContext obj)
        {
            if (obj.started)
            {
                m_AttackInputValue = true;
            }
            else if (obj.performed)
            {
                m_AttackInputValue = true;
            }
            else if (obj.canceled)
            {
                m_AttackInputValue = false;
            }
        }

        private void OnMovement(InputAction.CallbackContext obj)
        {
            if (obj.started)
            {
                m_MovementInputValue = obj.ReadValue<Vector2>();
            }
            else if (obj.performed)
            {
                m_MovementInputValue = obj.ReadValue<Vector2>();
            }
            else if (obj.canceled)
            {
                m_MovementInputValue = Vector2.zero;
            }
        }
    }
}