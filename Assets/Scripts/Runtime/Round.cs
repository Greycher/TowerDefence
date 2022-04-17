using System;
using UnityEngine;

[Serializable]
public class Round
{
    [SerializeField] private float _duration;
    [SerializeField] private Wave[] _waves;

    public float Duration => _duration;
    public Wave[] Waves => _waves;
}