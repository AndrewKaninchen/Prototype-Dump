using System;
using System.Collections.Generic;
using Sirenix.OdinInspector.Editor;
using UnityEditor;
using UnityEngine;

namespace InkDraft
{
    [Serializable]
    public class Variable
    {
        [SerializeField] public string name;
        [SerializeField] public VariableType type;
        
        public enum VariableType
        {
            Int,
            Bool,
            GameObject,
        }

        protected Variable (string name)
        {
            this.name = name;
        }
        
        public static Variable CreateInstance(VariableType T, string name)
        {
            switch (T)
            {
                case VariableType.Int:
                    return new IntVariable(name);
                case VariableType.Bool:
                    return new BoolVariable(name);
                case VariableType.GameObject:
                    return new GameObjectVariable(name);
                default:
                    throw new ArgumentOutOfRangeException("Invalid Variable Type. blablabla");
            }
        }
        
        public Type Type()
        {
            return TypeEnumToType(type);
        }
        
        public static Type TypeEnumToType(VariableType enumType) 
        {
            switch (enumType)
            {
                case VariableType.Int:
                    return typeof(int);
                case VariableType.Bool:
                    return typeof(bool);
                case VariableType.GameObject:
                    return typeof(GameObject);
                default:
                    throw new ArgumentOutOfRangeException("enumType", enumType, null);
            }
        }
    }
    
    [Serializable]
    public abstract class Variable <T> : Variable
    {
        protected Variable(string name) : base(name)
        {
        }
    }

    [Serializable]
    public class IntVariable : Variable<int>
    {
        [SerializeField] private int value;
        public new int Value
        {
            get { return value; }
            set { this.value = value; }
        }

        public IntVariable(string name) : base(name)
        {
            type = VariableType.Int;
        }
    }
    
    [Serializable]
    public class BoolVariable : Variable<bool>
    {
        [SerializeField] private bool value;
        public bool Value
        {
            get { return value; }
            set { this.value = value; }
        }

        public BoolVariable(string name) : base(name)
        {
            type = VariableType.Bool;
        }
    }
    
    [Serializable]
    public class GameObjectVariable : Variable<GameObject>
    {
        [SerializeField] private GameObject value;
        public GameObject Value
        {
            get { return value; }
            set { this.value = value; }
        }

        public GameObjectVariable(string name) : base(name)
        {
            type = VariableType.GameObject;
        }
    }
    
    
    [Serializable]
    [CreateAssetMenu(menuName = "InkDraft/VariablesHolder")]
    public class VariablesScriptableObject : ScriptableObject
    {
        [SerializeField] public List<Variable> variables = new List<Variable>();
    }
}