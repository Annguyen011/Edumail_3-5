using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

namespace Section4
{
    public class PlayerCtrl : MonoBehaviour
    {
        [SerializeField] private Animator m_Animator;
        private int m_AttackHash;
        private int m_IdleHash;
        private int m_DyingHash;
        private int m_WalkingHash;
        private PlayerInputActions m_PlayerInput;
        private Vector2 m_MovementInput;
        private void Start()
        {
            m_AttackHash = Animator.StringToHash("Attack");
            m_IdleHash = Animator.StringToHash("Idle");
            m_DyingHash = Animator.StringToHash("Dying");
            m_WalkingHash = Animator.StringToHash("Walking");

        }

        private void OnEnable()
        {
            if (m_PlayerInput == null)
            {
                m_PlayerInput = new PlayerInputActions();
            }
        }
        [ContextMenu("Play attack Anim")]
        private void PlayAttackAnim()
        {
            m_Animator.SetBool(m_AttackHash, true);
            m_Animator.SetBool(m_IdleHash, false);
            m_Animator.SetBool(m_WalkingHash, false);
        }
        [ContextMenu("Play Idle Anim")]
        private void PlayIdleAnim()
        {
            m_Animator.SetBool(m_AttackHash, false);
            m_Animator.SetBool(m_IdleHash, true);
            m_Animator.SetBool(m_WalkingHash, false);
        }
        [ContextMenu("Play Walking Anim")]
        private void PlayWalkingAnim()
        {
            m_Animator.SetBool(m_AttackHash, false);
            m_Animator.SetBool(m_IdleHash, false);
            m_Animator.SetBool(m_WalkingHash, true);
        }
        [ContextMenu("Play Dying Anim")]
        private void PlayDyingAnim()
        {
            m_Animator.SetBool(m_DyingHash, true);
        }
        [ContextMenu("Play Reset Anim")]
        private void ResetAnim()
        {
            m_Animator.SetBool(m_AttackHash, false);
            m_Animator.SetBool(m_IdleHash, false);
            m_Animator.SetBool(m_DyingHash, false);
            m_Animator.SetBool(m_WalkingHash, false);
        }
    }
}