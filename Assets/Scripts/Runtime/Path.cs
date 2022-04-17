using System;
using UnityEngine;
using UnityEngine.Assertions;

public class Path : MonoBehaviour
{
    [SerializeField] private Transform[] _waypointTransformations;

    private Vector3[] _waypoints;
    
    private void Awake()
    {
        _waypoints = new Vector3[_waypointTransformations.Length];
        for (int i = 0; i < _waypointTransformations.Length; i++)
        {
            var tr = _waypointTransformations[i];
            _waypoints[i] = tr.position;
            Destroy(tr.gameObject);
            
        }
    }

    public Vector3 CalculateNextPosition(ref float currentWayPoint, float travelDistance)
    {
        Assert.IsFalse(travelDistance < 0);
        Assert.IsFalse(currentWayPoint < 0);
        Assert.IsFalse(currentWayPoint > _waypoints.Length - 1);

        if (Mathf.Approximately(currentWayPoint,  _waypoints.Length - 1))
        {
            return _waypoints[_waypoints.Length - 1];
        }
        
        var currentPos = GetPosition(currentWayPoint);
        var higher = (int)currentWayPoint + 1;
        Vector3 nextPos;
        do
        {
            nextPos = Vector3.MoveTowards(currentPos, _waypoints[higher], travelDistance);
            travelDistance -= Vector3.Distance(currentPos, nextPos);
            currentPos = nextPos;
        } while (travelDistance > 0.01f && ++higher < _waypoints.Length);

        if (higher == _waypoints.Length)
        {
            currentWayPoint = _waypoints.Length - 1;
        }
        else
        {
            var lower = higher - 1;
            var d = Vector3.Distance(_waypoints[lower], nextPos);
            var totalD = Vector3.Distance(_waypoints[lower], _waypoints[higher]);
            currentWayPoint = lower + Mathf.InverseLerp(0, totalD, d);
        }
        
        return nextPos;
    }

    public Vector3 GetPosition(float wayPoint)
    {
        Assert.IsFalse(wayPoint < 0);
        Assert.IsFalse(wayPoint > _waypoints.Length - 1);

        if (Mathf.Approximately(wayPoint, _waypoints.Length - 1))
        {
            return Vector3.forward;
        }
        
        var lower = (int)wayPoint;
        var higher = lower + 1;
        return Vector3.Lerp(_waypoints[lower], _waypoints[higher], wayPoint - lower);
    }
    
    public Vector3 GetNormal(float wayPoint)
    {
        Assert.IsFalse(wayPoint < 0);
        Assert.IsFalse(wayPoint > _waypoints.Length - 1);

        if (Mathf.Approximately(wayPoint, _waypoints.Length - 1))
        {
            return Vector3.forward;
        }
        
        var lower = (int)wayPoint;
        var higher = lower + 1;
        return (_waypoints[higher] - _waypoints[lower]).normalized;
    }
}