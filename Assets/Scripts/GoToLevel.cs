using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GoToLevel : MonoBehaviour
{
    public int to_scene;
    public int load_type;
    public bool save_time;
    public bool can_complete_level = true;
    public bool can_open_level = true;

    void OnTriggerEnter2D(Collider2D coll)
    {
        if (coll.CompareTag("Player"))
        {
            SceneTransmitter.SetLoadType(load_type, save_time);
            SceneTransmitter.need_scene = to_scene;
            Debug.Log("Go to level " + to_scene);

            if (to_scene > 0 && can_open_level)
            {
                if (!DataManager.Game.openedLevels.Contains(to_scene))
                {
                    DataManager.Game.openedLevels.Add(to_scene);
                    Debug.Log("Opened level " + to_scene + ". All levels: " + string.Join(", ", DataManager.Game.openedLevels));
                }
            }
            if ((to_scene > SceneTransmitter.GetActiveSceneIndex() || to_scene == 0) && can_complete_level)
            {
                if (!DataManager.Game.completedLevels.Contains(SceneTransmitter.GetActiveSceneIndex()))
                {
                    DataManager.Game.completedLevels.Add(SceneTransmitter.GetActiveSceneIndex());
                    Debug.Log("Completed level " + SceneTransmitter.GetActiveSceneIndex() + ". All levels: " + string.Join(", ", DataManager.Game.completedLevels));
                }
            }
        }
    }
}
