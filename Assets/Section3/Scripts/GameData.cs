using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Section3
{
    public class GameData
    {
        public static int CurrentLevel
        {
            get { return PlayerPrefs.GetInt("CurrentLevel"); }
            set { PlayerPrefs.SetInt("CurrentLevel", value); }
        }
    }
}