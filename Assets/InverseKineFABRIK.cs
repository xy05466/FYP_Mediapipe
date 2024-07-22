 using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit.Filtering;

public class InverseKineFABRIK : MonoBehaviour
{
    //chain length of bones
    public int ChainLength = 2;
    //target the chain is bending towards
    public Transform Target;
    public Transform Pole;
    //solver iterations per update
    [Header("Solver Parameter")]
    public int Interations = 10;
    //distance when the solver stops
    public float Delta = 0.001f;
    //strength of going back to the start position
    public float SnapBackStrength = 1f;

    protected float[] BonesLength; //target to origin
    protected float CompleteLength;
    protected Transform[] Bones;
    protected Vector3[] Positions;

    private void Awake()
    {
        Init();
    }

    private void Init()
    {
        Bones = new Transform[ChainLength + 1];
        Positions = new Vector3[ChainLength + 1];
        BonesLength = new float[ChainLength];
        CompleteLength = 0;

        var current = transform;
        for (var i = Bones.Length - 1; i >= 0; i--)
        {
            Bones[i] = current;
            if (i == Bones.Length - 1)
            {

            }
            else
            {

            }
            current = current.parent;
        }
    }

    //can comment out ltr i think
    private void OnDrawGizmos()
    {
        var current = this.transform;
        for (int i = 0; i < ChainLength && current != null && current.parent != null; i++)
        {
            var scale = Vector3.Distance(current.position, current.parent.position) * 0.1f;
            Handles.matrix = Matrix4x4.TRS(current.position, Quaternion.FromToRotation(Vector3.up, current.parent.position - current.position), new Vector3(scale, Vector3.Distance(current.parent.position, current.position), scale));
            Handles.color = Color.green;
            Handles.DrawWireCube(Vector3.up * 0.5f, Vector3.one);
            current = current.parent;
        }
    }
}
