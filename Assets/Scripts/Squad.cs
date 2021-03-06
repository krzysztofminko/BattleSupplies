﻿using Sirenix.OdinInspector;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEditor;
using UnityEngine;
using WaypointsFree;

[RequireComponent(typeof(WaypointsGroup))]
public class Squad : MonoBehaviour
{
    [System.Serializable]
    public class SoldierCount
    {
        [Required]
        public Soldier soldier;
        [Min(1)]
        public int count = 1;
    }

    public static List<Squad> list = new List<Squad>();


    public int team;

    [SerializeField]
    private float spread = 4;

    [SerializeField, ReadOnly]
    private int targetWaypoint;
    [SerializeField, TableList(AlwaysExpanded = true)]
    private List<SoldierCount> soldierCounts;

    [ShowInInspector, ReadOnly]
    private readonly List<Soldier> soldiers = new List<Soldier>();

    private WaypointsGroup waypointsGroup;

    private void Awake()
    {
        waypointsGroup = GetComponent<WaypointsGroup>();

        //Spawn soldiers
        for (int i = 0; i < soldierCounts.Count; i++)
            for (int sc = 0; sc < soldierCounts[i].count; sc++)
            {
                Soldier soldier = Instantiate(soldierCounts[i].soldier, transform.position + (transform.rotation * GetPositionInFormation(soldiers.Count, soldierCounts.Sum(s => s.count))), transform.rotation);
                soldier.squad = this;
                soldier.Team = team;
                soldier.name += " " + sc;
                soldiers.Add(soldier);
            }

        //Update target position and formation
        if (waypointsGroup.waypoints.Count > 0)
        {
            transform.position = waypointsGroup.waypoints[0].GetPosition();
            if (waypointsGroup.waypoints.Count > 1)
                transform.forward = waypointsGroup.waypoints[1].GetPosition() - transform.position;
        }        
        SetTargetPosition(transform.position, new Vector2(transform.forward.x, transform.forward.z));
    }
    
    private void Update()
    {
        //Set next waypoint        
        if (targetWaypoint < waypointsGroup.waypoints.Count)
        {
            if (targetWaypoint + 1 < waypointsGroup.waypoints.Count &&  soldiers.Count(s => Distance.Manhattan2D(s.transform.position, s.targetPosition) < 1) == soldiers.Count)
            {
                targetWaypoint++;
                transform.forward = waypointsGroup.waypoints[targetWaypoint].GetPosition() - transform.position;
            }
            transform.position = waypointsGroup.waypoints[targetWaypoint].GetPosition();
        }

        //Editor only
        if (transform.hasChanged)
            SetTargetPosition(transform.position, new Vector2(transform.forward.x, transform.forward.z));
    }

    private void OnEnable()
    {
        list.Add(this);
    }

    private void OnDisable()
    {
        list.Remove(this);
    }

    [DrawGizmo(GizmoType.Selected | GizmoType.Active | GizmoType.NonSelected)]
    static void DrawGizmo(Squad squad, GizmoType gizmoType)
    {
        Gizmos.color = Color.yellow;
        int totalCount = squad.soldiers.Count > 0 ? squad.soldiers.Count : squad.soldierCounts.Sum(s => s.count);
        for (int i = 0; i < totalCount; i++) 
        {
            Vector3 position = squad.transform.position + squad.transform.rotation * squad.GetPositionInFormation(i, totalCount);
            Gizmos.DrawWireSphere(position, 0.25f);
            Gizmos.DrawLine(position, position + squad.transform.forward);
        }
    }

    public void SetTargetPosition(Vector3 position, Vector2 direction)
    {
        transform.position = position;
        transform.forward = new Vector3(direction.x, transform.forward.y, direction.y);

        for (int i = 0; i < soldiers.Count; i++)
            soldiers[i].targetPosition = transform.position + transform.rotation * GetPositionInFormation(i, soldiers.Count);
    }

    private Vector3 GetPositionInFormation(int i, int total)
    {
        int cols = Mathf.FloorToInt(Mathf.Sqrt(total) * 2);

        return new Vector3(i % cols - cols / 2, 0, -i / cols + total / cols / 2) * spread;
    }

    public void RemoveSoldier(Soldier soldier) => soldiers.Remove(soldier);    
}
