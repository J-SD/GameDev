



//using System;
//using UnityEngine;
//using System.Collections.Generic;

//public class Coin : MonoBehaviour
//{
//    public bool destroyed = false;
//    public int box_index;
//    public int start_index;
//    public bool hit = false;
//    public bool rejected = false;
//    public bool active = true;
//    public int tempIndex;

//    float[] boxes;
//    float x_pos;
//    float y_pos;

//    bool move_complete = false;
//    bool initialized = false;

//    Board board;

//    void Start()
//    {

//    }
//    void Initialize()
//    {
//        board = GameObject.FindGameObjectWithTag("board").GetComponent<Board>();
//        boxes = board.box_positions;
//        MoveToPos(boxes[box_index]);
//        start_index = box_index;

//        initialized = true;
//    }

//    Update is called once per frame
//    void Update()
//    {
//        if (!initialized) { return; }
//        DebugPanel.Log("index: ", box_index);
//        DebugPanel.Log("hit ", hit);

//        if (active)
//        {
//            switch (Application.platform)
//            {
//                case RuntimePlatform.Android:
//                    Move_Android();
//                    break;
//                case RuntimePlatform.WindowsEditor:
//                    Move_Editor();
//                    break;
//            }

//        }

//        if (!hit)
//        {

//        }
//        else { }

//    }

//    void SnapToBox(int ind)
//    {
//        DebugPanel.Log("Box_Index_Snap: ", ind);
//        box_index = ind;

//        if (box_index == 0) { Destroy(gameObject); }
//        rejected = false;
//        x_pos = gameObject.transform.position.x;
//        y_pos = gameObject.transform.position.y;

//        if (box_index < start_index)
//        {
//            MoveToPos(boxes[box_index]);
//        }
//        else if (box_index > start_index)
//        {
//            MoveToPos(boxes[start_index]);
//        }
//        start_index = box_index;
//    }

//    void MoveToPos(float pos)
//    {
//        gameObject.transform.position = (new Vector2(pos, y_pos));
//        box_index = Array.IndexOf(boxes, pos);
//    }

//    void reset()
//    {
//        start_index = box_index;
//    }

//    float CalculateClosestPos()
//    {
//        float closest_pos = 0;

//        foreach (float bp in boxes)
//        {
//            float box_dist = Mathf.Abs(x_pos - bp);
//            float closest_dist = Mathf.Abs(x_pos - closest_pos);

//            if (box_dist < closest_dist)
//            {
//                closest_pos = bp;
//            }

//        }

//        return closest_pos;
//    }

//    void Move_Android()
//    {
//        if (Input.touches.Length > 0)
//        {
//            Touch touch = Input.touches[0];
//            DebugPanel.Log("Touch Phase: ", touch.phase);

//            if (touch.phase == TouchPhase.Began)
//            {
//                hit = checkHit();

//            }
//            else if (touch.phase == TouchPhase.Moved || touch.phase == TouchPhase.Stationary)
//            {
//                if (hit && !rejected)
//                {
//                    transform.position = (new Vector2(getTouchPos().x, transform.position.y));
//                }
//                if (rejected)
//                {
//                    if (getTouchPos().x > transform.position.x)
//                    {
//                        transform.position = (new Vector2(getTouchPos().x + 1, transform.position.y));
//                        rejected = false;
//                    }
//                }
//            }
//            else if (touch.phase == TouchPhase.Ended)
//            {
//                hit = false;
//                SnapToBox(box_index);
//            }
//        }
//        else
//        {
//            hit = false;
//            SnapToBox(box_index);
//            DebugPanel.Log("else", 1);
//            foreach (Coin c in board.coins)
//            {
//                try
//                {
//                    c.active = true;
//                }
//                catch (Exception e) { }
//            }
//        }

//    }
//    void Move_Editor()
//    {
//        if (Input.GetMouseButton(0))
//        {
//            hit = checkHit();

//        }
//    }

//    bool checkHit()
//    {
//        Vector2 touchPos = getTouchPos();
//        RaycastHit2D hitInformation = Physics2D.Raycast(touchPos, Camera.main.transform.forward);

//        if (hitInformation.collider == GetComponent<Collider2D>())
//        {
//            hit = true;
//            foreach (Coin c in board.coins)
//            {
//                if (c != this)
//                {
//                    try
//                    {
//                        c.active = false;
//                    }
//                    catch (Exception e) { }
//                }

//            }
//        }
//        return hit;
//    }

//    Vector2 getTouchPos()
//    {
//        Vector2 wp;
//        if (Application.platform == RuntimePlatform.Android)
//        {
//            wp = Camera.main.ScreenToWorldPoint(Input.GetTouch(0).position);
//        }
//        else
//        {
//            wp = Camera.main.ScreenToWorldPoint(Input.mousePosition);
//        }
//        Vector2 touchPos = new Vector2(wp.x, wp.y);
//        return touchPos;
//    }
//}

