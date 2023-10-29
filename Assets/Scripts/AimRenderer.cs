using System.Collections.Generic;
using UnityEngine;





[RequireComponent(typeof(LineRenderer))]
public class AimRenderer : MonoBehaviour
{
    [SerializeField] private float distance = 12;
    [SerializeField] private Transform startPoint;
    [SerializeField] private Transform FirstPosition;
    
    private LineRenderer lr;
    private PlayerControl playerControl;
    private List<Vector3> allPoints = new List<Vector3>();





    private void Start()
    {
        lr = GetComponent<LineRenderer>();
        playerControl = GetComponent<PlayerControl>();
    }
    
    private void Update()
    {
        allPoints.Add(startPoint.position);

        Vector3 gunRay = FirstPosition.position - startPoint.position;
        
        DoRaycasts(distance, startPoint.position, gunRay);
        
        Vector3[] points = new Vector3[allPoints.Count];
        for (int i = 0; i < allPoints.Count; i++)
            points[i] = allPoints[i];
        
        lr.SetPositions(points);
        lr.positionCount = allPoints.Count;

        lr.enabled = playerControl.canShoot;
        
        allPoints.Clear();
    }

    private void DoRaycasts(float distance, Vector3 startPoint, Vector3 direction)
    {
        RaycastHit hit;
        if (Physics.Raycast(startPoint, direction, out hit, Mathf.Infinity, ~((1 << 6) | (1 << 7))))
        {
            if (hit.distance <= distance)
            {
                float distanceDif = distance - hit.distance;
                Vector3 reflectedDir = Vector3.Reflect(direction, hit.normal);
                allPoints.Add(hit.point);
                
                DoRaycasts(distanceDif, hit.point, reflectedDir);
            }
            else
            {
                allPoints.Add(startPoint + direction.normalized * distance);
            }
        }
    }
}