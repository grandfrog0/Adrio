using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraTargetTrigger : MonoBehaviour
{
    [SerializeField] private List<Transform> targets;
    [SerializeField] private Transform target_for_camera;
    [SerializeField] private float range;
    private CameraWatch cam;

    void FixedUpdate()
    {
        bool target_given = false;
        foreach(Transform target in targets)
        {
            if (target.gameObject.activeSelf && Vector3.Distance(target.position, transform.position) <= range)
            {
                if (!cam.HasTarget(target_for_camera)) cam.AddTarget(target_for_camera);
                target_given = true;
                break;
            }
        }
        if (!target_given && cam.HasTarget(target_for_camera))
        {
            cam.RemoveTarget(target_for_camera);
        }
    }

    void Awake()
    {
        cam = Camera.main.gameObject.GetComponent<CameraWatch>();
        if (targets.Count == 0)
        {
            GameObject[] objects = GameObject.FindGameObjectsWithTag("Player");
            foreach(GameObject obj in objects) targets.Add(obj.transform);
        }
    }
 
    void OnDrawGizmosSelected()
    {
        Gizmos.DrawWireSphere(transform.position, range);
    }
}
