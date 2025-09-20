using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Cart : MonoBehaviour
{
    [SerializeField] private Transform wheel1, wheel2, frame, box, box_point;
    [SerializeField] private Transform big_stone;
    [SerializeField] private Vector3 big_stone_start;

    [SerializeField] private Rigidbody2D stone_rb;
    [SerializeField] private Transform rot_wheel1, rot_wheel2;

    [SerializeField] private Movement player;
    [SerializeField] private bool player_speed_control;

    [SerializeField] private Transform leaf, leaf_point_this, leaf_point_stone;

    [SerializeField] private GameObject walking_stone;

    [SerializeField] private Sprite icon;
    [SerializeField] private Transform jenny_shadow;
    [SerializeField] private Movement jenny;
    [SerializeField] private SpikyObject spike_obj;


    private void FixedUpdate()
    {
        if (big_stone)
        {
            transform.position = new Vector3(big_stone.position.x - big_stone_start.x, transform.position.y);
        }

        frame.rotation = Quaternion.Lerp(frame.rotation, Quaternion.AngleAxis(Mathf.Atan2((wheel2.position-wheel1.position).y, (wheel2.position-wheel1.position).x) * Mathf.Rad2Deg, Vector3.forward), 0.25f);
        frame.localPosition = Vector3.Lerp(frame.localPosition, (wheel1.localPosition + wheel2.localPosition)/2 + 1.34f*Vector3.up, 0.25f);

        RaycastHit2D hit1 = Physics2D.Raycast(wheel1.position, transform.up * -1);
        if (hit1.collider != null) wheel1.position = Vector3.Lerp(wheel1.position, new Vector3(wheel1.position.x, hit1.point.y + wheel1.localScale.y/2), 0.2f);
        RaycastHit2D hit2 = Physics2D.Raycast(wheel2.position, transform.up * -1);
        if (hit2.collider != null) wheel2.position = Vector3.Lerp(wheel2.position, new Vector3(wheel2.position.x, hit2.point.y + wheel2.localScale.y/2), 0.2f);

        box.rotation = Quaternion.Lerp(box.rotation, box_point.rotation, 0.15f);
        box.position = Vector3.Lerp(box.position, box_point.position, 0.15f);

        if (player_speed_control) player.SetMoveSpeed(Mathf.Clamp((Mathf.Abs(transform.position.x - player.transform.position.x)), 0f, 30f));

        leaf.localScale = new Vector3(Vector3.Distance(leaf_point_this.position, leaf_point_stone.position), leaf.localScale.y, 1);
        leaf.rotation = Quaternion.AngleAxis(Mathf.Atan2((leaf_point_this.position - leaf_point_stone.position).y, (leaf_point_this.position - leaf_point_stone.position).x) * Mathf.Rad2Deg, Vector3.forward);
        leaf.position = leaf_point_this.position + (-leaf_point_this.position + leaf_point_stone.position) / 2;

        rot_wheel1.Rotate(0, 0, Vector2.Distance(stone_rb.velocity, Vector2.zero) / -2);
        rot_wheel2.Rotate(0, 0, Vector2.Distance(stone_rb.velocity, Vector2.zero) / -2);

        if (player_speed_control && Vector3.Distance(player.transform.position, transform.position) > 35)
        {
            SceneTransmitter.need_scene = -2;
            SceneTransmitter.SetLoadType(0, true);
        }
    }

    private void Awake()
    {
        big_stone_start = big_stone.position - transform.position;
    }

    public void SetPlayerSpeedControl(bool val)
    {
        player_speed_control = val;
        
        if (val) Invoke("DropStone", Random.Range(5, 10));
    }

    public void SpawnJenny()
    {
        Invoke("achieve", 0.5f);

        spike_obj.enabled = false;
        jenny.gameObject.SetActive(true);
        jenny.transform.position = jenny_shadow.transform.position;
        jenny.Impulse(Vector2.up * 40);
        jenny_shadow.gameObject.SetActive(false);
    }

    private void achieve()
    {
        EventMessageManager.Main().GetAchievement("jenny_saved", icon);
    }

    private void DropStone()
    {
        WalkingStone stone = Instantiate(walking_stone, box_point.position, Quaternion.identity).GetComponent<WalkingStone>();
        stone.left_border_x = box_point.position.x - 8;
        stone.right_border_x = box_point.position.x + 8;
        stone.movement.move_input_axis = -1;
        stone.movement.rb.AddForce(transform.up*10, ForceMode2D.Impulse);
        stone.anim.Play("opening");

        box_point.Rotate(0, 0, 22.5f);
        Invoke("return_box_point_back", 0.65f);

        if (player_speed_control) Invoke("DropStone", Random.Range(2, 7));
    }

    private void return_box_point_back()
    {
        box_point.Rotate(0, 0, -22.5f);
    }
}
