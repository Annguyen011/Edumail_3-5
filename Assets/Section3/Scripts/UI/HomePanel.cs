using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

namespace Section3
{
    public class HomePanel : MonoBehaviour
    {
        [SerializeField] private TextMeshProUGUI m_TxtHighScore;
        //private GameManager m_GameManager;

        private void OnEnable()
        {
            m_TxtHighScore.text = "HIGH SCORE: " + PlayerPrefs.GetInt("HighScore");
        }

        // Start is called before the first frame update
        void Start()
        {
            //m_GameManager = FindObjectOfType<GameManager>();
        }

        public void BtnPlay_Pressed()
        {
            GameManager.Instance.Play();
        }
    }
}