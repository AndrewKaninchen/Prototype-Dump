using System;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

namespace InkDraft
{
    [Serializable]
    public struct Variable
    {
        [SerializeField] public string m_Name;
        [SerializeField] public string m_Type;

        public Variable(string name, string type)
        {
            this.m_Name = name;
            this.m_Type = type;
        }
        
        //public object value;
    }
    
    [Serializable]
    [CreateAssetMenu(menuName = "InkDraft/VariablesHolder")]
    public class VariablesScriptableObject : ScriptableObject
    {
        [SerializeField] public List<Variable> m_Variables = new List<Variable>();
    }
}