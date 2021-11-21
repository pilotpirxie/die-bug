using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Wave : MonoBehaviour
{
    [SerializeField] private bool _isDone;
    
    public int Ants;
    public int Bees;
    public int Cockroaches;
    public int Ladybugs;
    public int Scorpios;
    public int Spiders;
    
    public void MarkAsDone()
    {
        _isDone = true;
    }

    public bool IsDone()
    {
        return _isDone;
    }
}
