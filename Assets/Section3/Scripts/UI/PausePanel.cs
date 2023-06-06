using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Section3
{
    public class PausePanel : MonoBehaviour
    {
        //private GameManager m_GameManager;

        // Start is called before the first frame update
        void Start()
        {
            //m_GameManager = FindObjectOfType<GameManager>();
        }

        public void BtnHome_Pressed()
        {
            GameManager.Instance.Home();
        }

        public void BtnContinue_Pressed()
        {
            GameManager.Instance.Continue();
        }
    }
}