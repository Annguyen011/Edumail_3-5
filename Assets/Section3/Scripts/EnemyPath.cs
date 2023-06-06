using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Section3
{
    public class EnemyPath : MonoBehaviour
    {
        [SerializeField] private Transform[] m_WayPoints;
        [SerializeField] private Color m_Color;
        [SerializeField] private bool m_Show;

        public Transform[] WayPoints => m_WayPoints;

        private void OnDrawGizmos()
        {
            if (!m_Show)
                return;

            if (m_WayPoints != null && m_WayPoints.Length > 1)
            {
                Gizmos.color = m_Color;
                for (int i = 0; i < m_WayPoints.Length - 1; i++)
                {
                    Transform from = m_WayPoints[i];
                    Transform to = m_WayPoints[i + 1];
                    Gizmos.DrawLine(from.position, to.position);
                }

                Gizmos.DrawLine(m_WayPoints[0].position, m_WayPoints[m_WayPoints.Length - 1].position);
            }
        }
    }
}