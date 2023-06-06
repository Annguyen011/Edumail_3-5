using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Section4
{
    public class PausePanel : MonoBehaviour
    {
        public void BtnContinue_Pressed()
        {
            GamePlayManager.Instance.Continue();
        }

        public void BtnRestart_Pressed()
        {
            GamePlayManager.Instance.Restart();
        }
    }
}