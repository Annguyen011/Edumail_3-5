using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Section3
{
    public class AudioManager : MonoBehaviour
    {
        private static AudioManager m_Instance;
        public static AudioManager Instance
        {
            get
            {
                if (m_Instance == null)
                    m_Instance = FindObjectOfType<AudioManager>();
                return m_Instance;
            }
        }

        [SerializeField] private AudioSource m_Music;
        [SerializeField] private AudioSource m_SFX;
        [SerializeField] private AudioSource m_Echo;

        [SerializeField] private AudioClip m_HomeMusicClip;
        [SerializeField] private AudioClip m_BattleMusicClip;
        [SerializeField] private AudioClip m_LazerSFXClip;
        [SerializeField] private AudioClip m_PlasmaSFXClip;
        [SerializeField] private AudioClip m_HitSFXClip;
        [SerializeField] private AudioClip m_ExplosionSFXClip;

        private void Awake()
        {
            if (m_Instance == null)
                m_Instance = this;
            else if (m_Instance != this)
                Destroy(gameObject);
        }

        public void PlayHomeMusic()
        {
            if (m_Music.clip == m_HomeMusicClip)
                return;

            m_Music.loop = true;
            m_Music.clip = m_HomeMusicClip;
            m_Music.Play();
        }

        public void PlayBattleMusic()
        {
            if (m_Music.clip == m_BattleMusicClip)
                return;

            m_Music.loop = true;
            m_Music.clip = m_BattleMusicClip;
            m_Music.Play();
        }

        public void PlayLazerSFX()
        {
            m_SFX.pitch = Random.Range(1f, 2f);
            m_SFX.PlayOneShot(m_LazerSFXClip);
        }

        public void PlayPlasmaSFX()
        {
            m_SFX.pitch = Random.Range(1f, 2f);
            m_SFX.PlayOneShot(m_PlasmaSFXClip);
        }

        public void PlayHitSFX()
        {
            m_SFX.pitch = Random.Range(1f, 2f);
            m_SFX.PlayOneShot(m_HitSFXClip);
        }

        public void PlayExplosionSFX()
        {
            m_Echo.pitch = Random.Range(1f, 2f);
            m_Echo.PlayOneShot(m_ExplosionSFXClip);
        }
    }
}