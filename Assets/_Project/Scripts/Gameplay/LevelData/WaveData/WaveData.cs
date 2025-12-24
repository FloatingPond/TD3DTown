using System.Collections.Generic;
using UnityEngine;

namespace _Project.Scripts.Gameplay
{
    [CreateAssetMenu(fileName = "WaveData", menuName = "WaveData", order = 0)]
    public class WaveData : ScriptableObject
    {
        public List<GameObject> Entities;
    }
}