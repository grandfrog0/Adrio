using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using static UnityEngine.EventSystems.EventTrigger;

public class PersonalizationManager : MonoBehaviour, IInitializable
{
    public List<GameObject> item_prefabs;
    public List<string> item_prefabs_names;

    private BaseEntity[] entities;
    private List<ItemInfo> savedItems;

    public void Check()
    {
        entities = FindObjectsOfType<BaseEntity>();
        savedItems = DataManager.Game.savedItems;
    }

    public void CheckType(PlayerType player)
    {
        //Debug.Log((savedItems != null) + "; " + savedItems?.Count);
        //List<PlayerType> players = savedItems.Select(x => x.player).ToList();

        //Debug.Log("e0");
        //if (!players.Contains(player)) 
        //    return;
        //Debug.Log("e1");

        //foreach (BaseEntity entity in entities) 
        //    if (MyMath.ToPlayerType(entity.GetEntityType()) == player)
        //    {
        //        for (int i = 0; i < savedItems.Count; i++)
        //        {
        //            if (players[i] != player || !item_prefabs_names.Contains(players[i].ToString()))
        //                continue;

        //            Item item = Instantiate(item_prefabs[item_prefabs_names.IndexOf(players[i].ToString())], transform.position, transform.rotation).GetComponent<Item>();
        //            entity.GetItem(item, item.need_point);
        //        }
        //    }

        var x = entities.Where(x => MyMath.ToPlayerType(x.GetEntityType()) == player);
        if (x.Any())
        {
            BaseEntity entity = x.First();
            // search items for player
            var y = savedItems.Where(x => x.player == player);
            foreach (ItemInfo info in y)
            {
                Item item = Instantiate(item_prefabs[item_prefabs_names.IndexOf(info.name)], transform.position, transform.rotation).GetComponent<Item>();
                entity.GetItem(item, item.need_point);
            }
        }
    }

    public InitializeOrder Order => InitializeOrder.PersonalizationManager;
    public void Initialize()
    {
        Check();
    }
}
