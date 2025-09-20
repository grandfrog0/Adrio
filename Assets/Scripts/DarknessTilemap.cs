using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class DarknessTilemap : MonoBehaviour
{
    [SerializeField] private List<TileBase> tiles;
    private Tilemap tilemap;
    private CameraWatch cam;
    private List<Transform> light_objects = new List<Transform>();
    private List<float> light_strength = new List<float>();
    private List<LightObject> light_objects_more = new List<LightObject>();
    [SerializeField] private List<Vector3Int> saved_poses = new List<Vector3Int>();
    [SerializeField] private List<int> saved_values = new List<int>();
    [SerializeField] private float timer_max = 0.5f;
    private float timer;

    void OnEnable()
    {
        light_objects.Clear();
        light_strength.Clear();
        light_objects_more.Clear();
        tilemap = GetComponent<Tilemap>();
        cam = Camera.main.gameObject.GetComponent<CameraWatch>();
        BaseEntity[] entities = FindObjectsOfType<BaseEntity>();
        foreach(BaseEntity entity in entities)
        {
            light_objects.Add(entity.transform);
            light_strength.Add(entity.light_strength);
        }
        LightObject[] objects = FindObjectsOfType<LightObject>();
        foreach(LightObject obj in objects) light_objects_more.Add(obj);
    }

    void FixedUpdate()
    {
        if (timer <= 0)
        {
            timer = timer_max;
            saved_poses.Clear();
            saved_values.Clear();
        }
        else timer -= Time.fixedDeltaTime;

        for(int x = (int) -cam.GetSize() * 2; x < cam.GetSize() * 2; x++) for(int y = (int) -cam.GetSize() * 2; y < cam.GetSize() * 2; y++)
        {
            Vector3Int pos = new Vector3Int((int) cam.transform.position.x + x, (int) cam.transform.position.y + y, 0);
            float distance = tiles.Count;
            for(int i = 0; i < light_objects.Count; i++) if (Vector3.Distance(light_objects[i].position, pos) / light_strength[i] < distance) distance = Vector3.Distance(light_objects[i].position, pos) / light_strength[i];
            foreach(LightObject obj in light_objects_more) if (Vector3.Distance(obj.transform.position, pos) / obj.light_strength < distance) distance = Vector3.Distance(obj.transform.position, pos) / obj.light_strength;
            int cur = (int) distance > tiles.Count - 1 ? tiles.Count - 1 : (int) distance;
            if (saved_poses.IndexOf(pos) == -1 || (saved_poses.IndexOf(pos) != -1 && saved_values[saved_poses.IndexOf(pos)] > cur))
            {
                if (cur != tiles.Count - 1)
                {
                    saved_poses.Add(pos);
                    saved_values.Add(cur);
                }
            }
            else cur = saved_values[saved_poses.IndexOf(pos)];
            tilemap.SetTile(pos, tiles[cur]);
        }
    }
}
