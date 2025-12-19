using _Project.Scripts.Gameplay.Turrets.Data;
using UnityEngine;

namespace _Project.Scripts.Gameplay.Turrets.UI
{
    public class TurretButton : MonoBehaviour
    {
        public TurretData Data;

        public void Setup(TurretData data)
        {
            Data = data;
        }
    }
}