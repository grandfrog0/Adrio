using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class EventMessageManager : MonoBehaviour
{
    public enum EventMessageType {Achievement, Item}
    
    [SerializeField] private RectTransform em_object;
    [SerializeField] private TMPro.TMP_Text header_text, text_text;
    [SerializeField] private Transform cam_tr;
    [SerializeField] private byte em_object_state;
    [SerializeField] private float time = 6;
    [SerializeField] private AudioSource au;
    [SerializeField] private GameObject raw_im;
    [SerializeField] private Image im;
    private float timer = 0;
    private float speed = 200f;
    private static List<string> completed_achievements;
    [SerializeField] private List<EventMessageObject> queue = new List<EventMessageObject>();

    private static EventMessageManager em;

    public void GetItem(GameObject spawn_object)
    {
        queue.Add(new EventMessageObject(EventMessageType.Item, spawn_object));
    }

    public void GetAchievement(string text, Sprite icon)
    {
        queue.Add(new EventMessageObject(EventMessageType.Achievement, text, icon));
    }

    private void Next()
    {
        if (queue.Count == 0) return;

        EventMessageObject obj = queue[0];
        if (obj.type == EventMessageType.Item) // item load
        {
            SaveItem(obj.spawn_object.GetComponent<Item>().naming);

            string item_name = Translator.GetValue("item_names", obj.spawn_object.GetComponent<Item>().naming);
            header_text.text = Translator.GetValue("event_message_headers", "item");
            text_text.text = item_name;

            raw_im.SetActive(true);
            im.gameObject.SetActive(false);
            GameObject obj_ = Instantiate(obj.spawn_object, cam_tr.transform);
            if (obj_.GetComponent<Item>())
            {
                Item item = obj_.GetComponent<Item>();
                item.enabled=false;
                item.PickUp();
            }

            MessageGot();
        }
        else if (obj.type == EventMessageType.Achievement && !AchievementCompleted(obj.text)) // achievement load
        {
            CompleteAchievement(obj.text);

            header_text.text = Translator.GetValue("event_message_headers", "achievement");
            text_text.text = Translator.GetValue("event_messages", obj.text);

            raw_im.SetActive(false);
            im.gameObject.SetActive(true);
            im.sprite = obj.icon;

            MessageGot();
        }

        queue.RemoveAt(0);
    }

    public static bool AchievementCompleted(string naming) //bool
    {
        bool saved;
        completed_achievements = DataManager.Game.completedAchievements;
        saved = completed_achievements.IndexOf(naming) != -1;
        // Debug.Log(saved);
        return saved;
    }

    private void CompleteAchievement(string naming) //save achievement
    {
        completed_achievements.Add(naming);
        DataManager.Game.completedAchievements = completed_achievements;

        Statistics.Add("completed_achievements_count");
    }

    private void SaveItem(string naming) //save item
    {
        List<ItemInfo> savedItems = DataManager.Game.savedItems;
        savedItems.Add(new(naming, PlayerType.None));
        DataManager.Game.savedItems = savedItems;
    }

    private void MessageGot()
    {
        em_object.anchoredPosition = Vector2.up * 50;
        em_object_state = 1;
        timer = time;
        if (au) au.Play();
    }

    private void FixedUpdate()
    {
        if (em_object_state == 0)
        {
            em_object.anchoredPosition = Vector2.up * 500;
            if (queue.Count > 0) Next();
        }
        else if (em_object_state == 1)
        {
            if (em_object.anchoredPosition.y > -40)
            {
                if (queue.Count == 0) em_object.anchoredPosition -= Vector2.up * speed / 2 * Time.fixedDeltaTime;
                else em_object.anchoredPosition -= Vector2.up * speed * Time.fixedDeltaTime;
            } 
            if (timer > 0) timer -= Time.fixedDeltaTime;
            else
            {
                timer = 0;
                em_object_state = 2;
            }
        }
        else
        {
            if (em_object.anchoredPosition.y < 50) 
            {
                if (queue.Count == 0) em_object.anchoredPosition += Vector2.up * speed / 4 * Time.fixedDeltaTime;
                else em_object.anchoredPosition += Vector2.up * speed / 2 * Time.fixedDeltaTime;
            }
            else
            {
                em_object_state = 0;
            }
        }
    }
    
    private void ChangeLayer(Transform obj)
    {
        obj.gameObject.layer = LayerMask.NameToLayer("PROJECTION");
        
        foreach (Transform child in obj) ChangeLayer(child);
    }

    private void Awake()
    {
        cam_tr.gameObject.SetActive(true);
        cam_tr.SetParent(null);
        cam_tr.localScale = new Vector3(1, 1, 1);
    }

    public static EventMessageManager Main()
    {
        if (em == null) em = GameObject.FindGameObjectWithTag("GameController").GetComponent<EventMessageManager>();
        if (em == null)
        {
            Debug.Log("EventMessageManager not found!");
        }
        return em;
    }
}
