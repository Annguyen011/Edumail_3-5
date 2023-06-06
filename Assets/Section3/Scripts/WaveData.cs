using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Section3
{
    [CreateAssetMenu(fileName = "WaveData", menuName = "Section3/Create Wave Data")]
    public class WaveData : ScriptableObject
    {
        [Range(1, 10)] public int totalGroups;
        [Range(1, 10)] public int minTotalEnemies;
        [Range(1, 10)] public int maxTotalEnemies;
        [Range(1, 10)] public int speedMultiplier;
    }
}