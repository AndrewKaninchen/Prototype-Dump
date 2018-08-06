using UnityEngine;
using Sirenix.OdinInspector;

namespace DefaultNamespace
{
    public class TestStuff : MonoBehaviour
    {
        [FoldoutGroup("Estringues"), SerializeField]
        
        public string m_S1, m_S2;
    }
}