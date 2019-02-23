using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName ="Gravity Value", menuName ="Scriptable Object/Gravity Value")]
public class GravValueScriptableObject : ScriptableObject
{
    [Tooltip("All object will be using this gravity value for now")]
    public float gravity;
}
