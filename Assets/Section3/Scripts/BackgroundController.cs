using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Section3
{
    public class BackgroundController : MonoBehaviour
    {
        [SerializeField] private Material m_NebulaBg;
        [SerializeField] private float m_NebulaBgSpeed;
        [SerializeField] private Material m_BigStarsBg;
        [SerializeField] private float m_BigStarsBgSpeed;
        [SerializeField] private Material m_MedStarsBg;
        [SerializeField] private float m_MedStarsBgSpeed;

        private int m_MainTexId;

        private void Start()
        {
            m_MainTexId = Shader.PropertyToID("_MainTex");
        }

        private void Update()
        {
            Vector2 offset = m_NebulaBg.GetTextureOffset(m_MainTexId);
            offset += new Vector2(0, m_NebulaBgSpeed * Time.deltaTime);
            m_NebulaBg.SetTextureOffset(m_MainTexId, offset);

            offset = m_BigStarsBg.GetTextureOffset(m_MainTexId);
            offset += new Vector2(0, m_BigStarsBgSpeed * Time.deltaTime);
            m_BigStarsBg.SetTextureOffset(m_MainTexId, offset);

            offset = m_MedStarsBg.GetTextureOffset(m_MainTexId);
            offset += new Vector2(0, m_MedStarsBgSpeed * Time.deltaTime);
            m_MedStarsBg.SetTextureOffset(m_MainTexId, offset);
        }
    }
}