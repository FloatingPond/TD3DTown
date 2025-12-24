using System.Collections.Generic;
using UnityEngine;

namespace _Project.Scripts.Gameplay
{
    [CreateAssetMenu(fileName = "LevelData", menuName = "LevelData", order = 0)]
    public class LevelData : ScriptableObject
    {
        public List<WaveData> Waves;
    }
}