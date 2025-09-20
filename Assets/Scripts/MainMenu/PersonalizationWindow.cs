using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PersonalizationWindow : MonoBehaviour
{
    [SerializeField] private GameObject window_object;
    [SerializeField] private List<RectTransform> animation_images;
    [SerializeField] private List<Vector3> start_pos = new List<Vector3>();
    [SerializeField] private List<float> add_x = new List<float>();
    [SerializeField] private float range = 8, floor = -20, speed=5;

    [SerializeField] private List<RectTransform> player_window_transforms;
    [SerializeField] private RectTransform chosen_player_line;
    [SerializeField] private Vector2 chosen_player_line_deltapos;
    [SerializeField] private Image chosen_player_line_image;
    [SerializeField] private Color chosen_player_line_color_clicked, chosen_player_line_color_def;
    [SerializeField] private int chosen_player_line_state = 0;
    [SerializeField] private int cur_player;
    [SerializeField] private List<PlayerType> playerTypes;

    [SerializeField] private PersonalizationManager personalizationManager;
    [SerializeField] private List<ItemInfo> savedItems;  // load from file
    [SerializeField] private GameObject item_cell_prefab; // set in inspector
    [SerializeField] private List<GameObject> item_cells; // instantiate in code
    [SerializeField] private RectTransform item_cells_parent;
    [SerializeField] private float pos_multiplier1 = 2, pos_multiplier2 = 2;
    [SerializeField] private Transform projection_items_parent;
    [SerializeField] private List<GameObject> projection_items;
    [SerializeField] private GameObject text_if_have_not_items;
    [SerializeField] private RectTransform content_rect;
    [SerializeField] private Transform items_cam;
    [SerializeField] private Vector3 items_cam_add_vector;
    [SerializeField] private float items_cam_multiplier = 100;
    [SerializeField] private LanguageSection section;
    [SerializeField] private RectTransform chosen_item_cell_line;
    [SerializeField] private Vector2 chosen_item_cell_line_deltapos;
    [SerializeField] private Image chosen_item_cell_line_image;
    [SerializeField] private int chosen_item_cell_line_state = 0;
    [SerializeField] private int cur_item;
    [SerializeField] private List<Item> for_player_items;
    [SerializeField] private List<EntityPoints> player_points;
    [SerializeField] private RectTransform item_buffer;
    [SerializeField] private Transform projection_item_buffer_par;
    [SerializeField] private Vector3 item_buffer_pos;
    [SerializeField] private bool item_drag, drag_in_player;
    [SerializeField] private Vector3 item_buffer_deltapos;
    private int last_player;
    [SerializeField] private GameObject chokipie_hat;

    public void Load()
    {
        window_object.SetActive(true);
        add_x.Clear();
        for(int i = 0; i < animation_images.Count; i++)
        {
            add_x.Add(Random.Range(-range, range));
            animation_images[i].localPosition = new Vector3(start_pos[i].x + add_x[i], floor);
        }

        cur_player = -1;
        chosen_player_line_state = -1;
        chosen_player_line_image.color = new Color(chosen_player_line_image.color.r, chosen_player_line_image.color.g, chosen_player_line_image.color.b, 0);
        cur_item = -1;
        chosen_item_cell_line_state = -1;
        chosen_item_cell_line_image.color = new Color(chosen_item_cell_line_image.color.r, chosen_item_cell_line_image.color.g, chosen_item_cell_line_image.color.b, 0);
        last_player = -1;

        List<int> levels_completed = DataManager.Game.completedLevels;
        player_window_transforms[1].gameObject.SetActive(MyMath.Max(levels_completed) >= 4);
        player_window_transforms[2].gameObject.SetActive(MyMath.Max(levels_completed) >= 7);

        //It should be better in future: dont destroy objects, which we can use now.
        chosen_item_cell_line.SetParent(null);
        foreach(GameObject item_cell in item_cells) Destroy(item_cell);
        item_cells.Clear();
        foreach(GameObject item in projection_items) Destroy(item);
        projection_items.Clear();
        foreach(Item item in for_player_items) Destroy(item.gameObject);
        for_player_items.Clear();

        savedItems = DataManager.Game.savedItems;
        text_if_have_not_items.SetActive(savedItems.Count == 0);
        for(int i = 0; i < savedItems.Count; i++)
        {
            if (!personalizationManager.item_prefabs_names.Contains(savedItems[i].name)) 
                continue;

            GameObject item_cell = Instantiate(item_cell_prefab, item_cells_parent);
            item_cells.Add(item_cell);
            item_cell.transform.localPosition = Vector3.right * (item_cells.Count - 1) * pos_multiplier1;
            GameObject item = Instantiate(personalizationManager.item_prefabs[personalizationManager.item_prefabs_names.IndexOf(savedItems[i].name)], projection_items_parent);
            Item item_script = item.GetComponent<Item>();
            item_script.enabled=false;
            item_script.PickUp();
            item.transform.localPosition = Vector3.right * (item_cells.Count - 1) * pos_multiplier2;
            projection_items.Add(item);
            ChangeLayer(item.transform);

            GameObject item1 = Instantiate(personalizationManager.item_prefabs[personalizationManager.item_prefabs_names.IndexOf(savedItems[i].name)], new Vector3(200, 200), Quaternion.identity);
            for_player_items.Add(item1.GetComponent<Item>());
            for_player_items[for_player_items.Count-1].enabled=false;
            ChangeLayer(item1.transform);
        }

        section = Translator.GetSection("item_names");

        for(int i = 0; i < item_cells.Count; i++)
        {
            Item item_script = projection_items[i].GetComponent<Item>();
            if (section.content.ContainsKey(item_script.naming)) 
                item_cells[i].GetComponent<ItemCell>().SetItem(section.content[item_script.naming], this, i);
        }
        content_rect.sizeDelta = new Vector2(137f + pos_multiplier1 * (savedItems.Count - 1), content_rect.sizeDelta.y);

        for(int i = 0; i < savedItems.Count; i++) SetItemToPlayer(i, playerTypes.IndexOf(savedItems[i].player));
    }

    public void Close()
    {
        window_object.SetActive(false);
    }

    void FixedUpdate()
    {
        for(int i = 0; i < animation_images.Count; i++)
        {
            animation_images[i].localPosition = Vector3.Lerp(animation_images[i].localPosition, start_pos[i], speed * Time.fixedDeltaTime);
        }

        if (chosen_player_line_state == 1)
        {
            chosen_player_line_image.color = Color.Lerp(chosen_player_line_image.color, chosen_player_line_color_def, 6*Time.fixedDeltaTime);
            if (Mathf.Abs(chosen_player_line_image.color.a - chosen_player_line_color_def.a) < 0.05f)
            {
                chosen_player_line_image.color = chosen_player_line_color_def;
                chosen_player_line_state = 0;
            }
        }
        else if (chosen_player_line_state == -1) chosen_player_line_image.color = Color.Lerp(chosen_player_line_image.color, new Color(chosen_player_line_image.color.r, chosen_player_line_image.color.g, chosen_player_line_image.color.b, 0), 20*Time.fixedDeltaTime);

        if (chosen_item_cell_line_state == 1)
        {
            chosen_item_cell_line_image.color = Color.Lerp(chosen_item_cell_line_image.color, chosen_player_line_color_def, 6*Time.fixedDeltaTime);
            if (Mathf.Abs(chosen_item_cell_line_image.color.a - chosen_player_line_color_def.a) < 0.05f)
            {
                chosen_item_cell_line_image.color = chosen_player_line_color_def;
                chosen_item_cell_line_state = 0;
            }
        }
        else if (chosen_item_cell_line_state == -1) chosen_item_cell_line_image.color = Color.Lerp(chosen_item_cell_line_image.color, new Color(chosen_item_cell_line_image.color.r, chosen_item_cell_line_image.color.g, chosen_item_cell_line_image.color.b, 0), 20*Time.fixedDeltaTime);
    
    }

    void Update()
    {
        items_cam.transform.localPosition = content_rect.transform.localPosition / items_cam_multiplier + items_cam_add_vector;
        if (item_drag)
        {
            item_buffer.position = Camera.main.ScreenToWorldPoint(Input.mousePosition) + item_buffer_deltapos;
            item_buffer.position = new Vector3(item_buffer.position.x, item_buffer.position.y, 0);
        }
    }

    void Awake()
    {
        for(int i = 0; i < animation_images.Count; i++) start_pos.Add(animation_images[i].localPosition);
    }

    public void PlayerWindowClicked(int index)
    {
        if (index < 0 || index > player_window_transforms.Count - 1) return;

        if (index != cur_player)
        {
            cur_player = index;
            chosen_player_line.anchoredPosition = player_window_transforms[cur_player].anchoredPosition + chosen_player_line_deltapos;
            chosen_player_line_image.color = chosen_player_line_color_clicked;
            chosen_player_line_state = 1;

            if (cur_item != -1) SaveItem(cur_item, playerTypes[index], index);
        }
        else
        {
            cur_player = -1;
            chosen_player_line_state = -1;

            if (cur_item != -1) SaveItem(cur_item, PlayerType.None, -1);
        }
    }

    private void SelectItemCell(int index)
    {
        cur_item = index;
        chosen_item_cell_line.SetParent(item_cells[cur_item].transform);
        chosen_item_cell_line.localPosition = chosen_item_cell_line_deltapos;
        chosen_item_cell_line_image.color = chosen_player_line_color_clicked;
        chosen_item_cell_line_state = 1;
    }

    public void ItemCellClicked(int index)
    {
        if (index < 0 || index > item_cells.Count - 1) return;

        // if no have chosen prefabs...
        if (index != cur_item)
        {
            Debug.Log(0);
            SelectItemCell(index);
            if (cur_player != -1) SaveItem(index, playerTypes[cur_player], cur_player);
        }
        else
        {
            cur_item = -1;
            chosen_item_cell_line_state = -1;

            // SaveItem(index, "Non", -1);
        }
    }

    private void SaveItem(int index, PlayerType player, int cur, bool edit_hat=true, int player_before=0)
    {
        if (savedItems[index].player == player) return;

        PlayerType saved_players_before = savedItems[index].player;
        savedItems[index].player = player;
        DataManager.Game.savedItems = savedItems;

        SetItemToPlayer(index, cur, edit_hat, player_before, saved_players_before);
    }

    private void SetItemToPlayer(int index, int cur, bool edit_hat=true, int player_before=0, PlayerType savedPlayersBefore=PlayerType.None)
    {
        if (cur != -1)
        {
            int point_index = (player_points[cur] && player_points[cur].item_points_names.Count > 0) ? player_points[cur].item_points_names.IndexOf(for_player_items[index].need_point) : -1;
            if (point_index == -1) for_player_items[index].transform.parent = player_points[cur].transform;
            else for_player_items[index].transform.parent = player_points[cur].item_points[point_index];
            for_player_items[index].PickUp();

            for(int i = 0; i < for_player_items.Count; i++) if (for_player_items[i] != for_player_items[index] && for_player_items[i].need_point == for_player_items[index].need_point && for_player_items[i].transform.parent == for_player_items[index].transform.parent) SaveItem(i, PlayerType.None, -1); 
        }
        else
        {
            for_player_items[index].transform.position = new Vector3(200, 200);
        }
        if (edit_hat)
        {
            if (cur == 2 && for_player_items[index].need_point == "head_top") chokipie_hat.SetActive(false);
            else if (cur != 2 && savedPlayersBefore == PlayerType.Chokipie) chokipie_hat.SetActive(true);
        }
        
        // chokipie_hat.SetActive(!(cur == 2 && for_player_items[index].need_point == "head_top"));
    }

    private void ChangeLayer(Transform obj)
    {
        obj.gameObject.layer = LayerMask.NameToLayer("PROJECTION");
        
        foreach (Transform child in obj) ChangeLayer(child);
    }

    public void ItemDrag(int index, bool drag)
    {
        item_drag = drag;

        if (item_drag)
        {
            item_buffer_deltapos = item_cells[index].transform.position - Camera.main.ScreenToWorldPoint(Input.mousePosition);
            item_buffer_pos = projection_items[index].transform.localPosition;
            projection_items[index].transform.parent = projection_item_buffer_par;
            projection_items[index].transform.localPosition = Vector3.zero;
        }
        else
        {
            projection_items[index].transform.parent = projection_items_parent;
            projection_items[index].transform.localPosition = item_buffer_pos;

            SelectItemCell(index);

            if (drag_in_player)
            {
                int player_before = last_player;
                if (cur_player != last_player) PlayerWindowClicked(last_player);
                SaveItem(index, playerTypes[cur_player], cur_player, cur_player == last_player, player_before);
            }
        }
    }

    public void PlayerEnter(int index)
    {
        drag_in_player = true;
        last_player = index;
    }

    public void PlayerExit(int index)
    {
        drag_in_player = false;
        last_player = -1;
    }
}
