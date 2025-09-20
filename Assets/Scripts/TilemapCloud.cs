using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class TilemapCloud : MonoBehaviour
{
    private Tilemap tilemap;
    private CameraWatch cam;
    [SerializeField] private List<TileBase> tiles;
    [SerializeField] private List<Transform> objects = new List<Transform>();
    [SerializeField] private List<float> light_strength = new List<float>();
    // [SerializeField] private List<Vector3Int> saved_poses = new List<Vector3Int>();
    // [SerializeField] private List<int> saved_values = new List<int>();
    [SerializeField] private float timer_max = 0.5f;
    private float timer;

    void OnEnable()
    {
        tilemap = GetComponent<Tilemap>();
        cam = Camera.main.gameObject.GetComponent<CameraWatch>();
    }

    void FixedUpdate()
    {
        if (timer <= 0)
        {
            timer = timer_max;
            tilemap.ClearAllTiles();
        }
        else timer -= Time.fixedDeltaTime;

        for(int x = (int) -cam.GetSize() * 2; x < cam.GetSize() * 2; x++) for(int y = (int) -cam.GetSize() * 2; y < cam.GetSize() * 2; y++)
        {
            Vector3Int pos = new Vector3Int((int) cam.transform.position.x + x, (int) cam.transform.position.y + y, 0);
            float distance = tiles.Count;
            for(int i = 0; i < objects.Count; i++) if (Vector3.Distance(objects[i].position, pos) / light_strength[i] < distance) distance = Vector3.Distance(objects[i].position, pos) / light_strength[i];
            // int cur = (int) distance > tiles.Count - 1 ? tiles.Count - 1 : (int) distance;
            int cur = (int) distance > tiles.Count - 1 ? -1 : (int) distance;
            if (cur != -1) tilemap.SetTile(pos, tiles[cur]);
        }
    }
}
