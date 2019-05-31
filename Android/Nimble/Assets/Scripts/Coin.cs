
using System;
using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;
using System.Collections;

public class Coin : MonoBehaviour
{
    public bool destroyed = false;
    public int box_index;
    public int start_index;
    public bool hit = false;
    public bool rejected = false;
    public bool active = true;
    public int rejectBox;
    public float touchSpeed = 0.05f;
    public int index;

    int targetBox;
    bool moving = false;
    List<Box> boxes;
    float[] box_positions;
    bool initialized = false;
    Board board;
    Vector2 targetPos = new Vector2(0, 0);


    // Called on creation to set variables and move object to starting location
    void Initialize()
    {
        board = GameObject.FindGameObjectWithTag("board").GetComponent<Board>();
        boxes = board.boxes;
        box_positions = board.box_positions;
        try
        {
            MoveToPos(box_positions[box_index]);
        }
        catch (Exception e) {
            print("EXCEPT: " + e.Data);
        }
        start_index = box_index;
        initialized = true;
        rejected = false;

    }

    // Update is called once per frame
    void Update()
    {
        if (!initialized) { return; }
        index = board.coinList.FindIndex(x => x == this);

        if (moving)
        {
            float speed;
            if (Application.loadedLevel == 3) speed = 300f; else speed = 20f;

            transform.localPosition = Vector2.MoveTowards(transform.localPosition, boxes[targetBox].transform.localPosition, speed * Time.deltaTime);
            if (transform.localPosition.x == boxes[targetBox].transform.localPosition.x)
            {
                moving = false;
                SnapToBox(targetBox);
            }
            return;
        }


        if (active)
        {
            gameObject.GetComponent<Rigidbody2D>().constraints = RigidbodyConstraints2D.None;
            if (Application.platform == RuntimePlatform.Android)
            {
                Move_Android();
            }
            else
            {
                Move_Editor();
            }
        } else gameObject.GetComponent<Rigidbody2D>().constraints = RigidbodyConstraints2D.FreezePositionX;
    }

    void SnapToBox(int ind)
    {
        box_index = ind;
        if (box_index == 0)
        {
            board.NUM_COINS--;
            board.coinList.Remove(this);
            initialized = false;
            StartCoroutine(DestroyFade(Time.time));
        }

        if (box_index < start_index)
        {
            MoveToPos(box_positions[CalculateClosestBox(-9009)]);
        }

        else if (box_index > start_index)
        {
            MoveToPos(box_positions[start_index]);
        }

        if (start_index != box_index)
        {
            setCoinsActive();
            board.SendMessage("NextTurn");
        }

    }

    void MoveToPos(float pos)
    {
        gameObject.transform.position = (new Vector2(pos, transform.position.y));
    }

    void reset()
    {
        moving = false;
        start_index = box_index;
        rejected = false;
        rejectBox = 100;
        hit = false;

    }

    int CalculateClosestBox(float x_pos)
    {
        int closestBoxIndex = 0;
        float closest_dist = 100;
        if (x_pos == -9009)
        {
            x_pos = gameObject.transform.position.x;
        }
        foreach (Box box in board.boxes)
        {
            float box_dist = Mathf.Abs(x_pos - box.position);
            if (box_dist < closest_dist)
            {
                closest_dist = box_dist;
                closestBoxIndex = box.index;
            }
        }
        return closestBoxIndex;
    }

    void Move_Android()
    {
        if (board.playerTurn && active)
        {
            if (Input.touches.Length > 0)
            {
                Touch touch = Input.touches[0];
                if (touch.phase == TouchPhase.Began) // If this is the beggining of the touch
                {
                    checkHit(); // Check if the touch was on this coin
                }
                if (hit && (touch.phase == TouchPhase.Moved || touch.phase == TouchPhase.Stationary)) // If the touch hit a coin and hasn't ended:
                {
                    if (!rejected) // If the coin is not moving illegally 
                    {
                        targetPos = new Vector2(getTouchPos().x, transform.position.y); // Set the target position to the touch position
                        float maxPosX = box_positions[start_index]; // The maximum X position is the starting box X
                        if (index - 1 > -1) // if this is not the leftmost coin
                        { 
                            float minPosX = box_positions[board.coinList[index - 1].start_index+1]; //the position of the box before the next coin's box                            
                            if (targetPos.x < minPosX) targetPos.x = minPosX; //if the finger is past the maximum position, the target position becomes the maximum position
                        }
                        if (targetPos.x > maxPosX) targetPos.x = maxPosX; //if the finger is past the starting box, the target becomes the starting box
                        transform.position = Vector2.MoveTowards(transform.position, targetPos, 20f * Time.deltaTime); //move towards the target position
                    }
                    
                }
                if (touch.phase == TouchPhase.Ended && hit)
                {
                    SmoothSnapToBox(CalculateClosestBox(targetPos.x));
                    setCoinsActive();
                    
                   //StartCoroutine(FinishMove(targetPos));
                    hit = false;

                }
              

            }
        }
    }

    void Move_Editor()
    {
        if (board.playerTurn && active)
        {
            if (Input.GetMouseButton(0))
            {
                hit = checkHit();
                if (!rejected && hit)
                {
                    targetPos = new Vector2(getTouchPos().x, transform.position.y);
                    float maxPosX = box_positions[start_index];
                    //if this is not the leftmost coin
                    if (index - 1 > -1)
                    {
                        //the position of the box before the next coin's box
                        float minPosX = box_positions[board.coinList[index - 1].start_index + 1];

                        //if the finger is past the maximum position, the target position becomes the maximum position
                        if (targetPos.x < minPosX) targetPos.x = minPosX;
                    }
                    //if the finger is past the starting box, the target becomes the starting box
                    if (targetPos.x > maxPosX) targetPos.x = maxPosX;

                    //move towards the finger position
                    transform.position = Vector2.MoveTowards(transform.position, targetPos, 20f * Time.deltaTime);


                }
            }
            else if (Input.GetMouseButtonUp(0)&&hit)
            {
                SmoothSnapToBox(CalculateClosestBox(targetPos.x));
                setCoinsActive();
                //StartCoroutine(FinishMove(targetPos));
                hit = false;
            }
            }
    }

    //IEnumerator FinishMoveBACKUP(Vector2 tar)
    //{
    //    int i = 100000;
    //    while (transform.position.x != tar.x && i >= 0)
    //    {
    //        //transform.position = Vector2.MoveTowards(transform.position, tar, 20f * Time.deltaTime);
    //        transform.position = Vector2.MoveTowards(transform.position, tar, 5f);
    //        i--;
    //    }
    //    yield return new WaitForSeconds(.1f);
    //    if (active)
    //    {
    //        rejected = false;
    //        hit = false;
    //        SnapToBox(CalculateClosestBox());
    //        setCoinsActive();
    //    }

    //}
    //IEnumerator FinishMove(Vector2 tar)
    //{
    //    Vector2 velocity = Vector2.zero;
    //    long i = 10000;
    //    while (transform.position.x != tar.x && i > 0)
    //    {
    //        //transform.position = Vector2.MoveTowards(transform.position, tar, 20f * Time.deltaTime);
    //        //transform.position = Vector2.MoveTowards(transform.position, tar, 5f);
    //        transform.position = Vector2.Lerp(transform.position, tar, 1f);
    //        i--;
    //    }
    //    yield return new WaitForSeconds(.1f);
    //    if (active)
    //    {
    //        rejected = false;
    //        hit = false;
    //        SmoothSnapToBox(CalculateClosestBox(tar.x));
    //        //SnapToBox(CalculateClosestBox());
    //        setCoinsActive();
    //    }
    //}

    //if the box to the right has a coin, require movement to the left and visa-versa
    void RejectMove()
    {
        //if box is to the right
        if (rejectBox > start_index)
        {
            //if touching to the left
            if (getTouchPos().x < transform.position.x)
            {
                transform.position = (new Vector2(getTouchPos().x, transform.position.y));
                rejected = false;
            }
        }
        //if box is to the left
        else if (rejectBox < start_index)
        {
            //if touching to the right 
            if (getTouchPos().x > transform.position.x)
            {
                transform.position = (new Vector2(getTouchPos().x, transform.position.y));
                rejected = false;
            }
        }


    }
    
    void setCoinsActive()
    {

        foreach (Coin c in board.coins)
        {
            try
            {
                c.active = true;
            }
            catch (Exception e) { }
        }
    }

    bool checkHit()
    {
        Vector2 touchPos = getTouchPos();
        RaycastHit2D hitInformation = Physics2D.Raycast(touchPos, Camera.main.transform.forward);

        if (hitInformation.collider == GetComponent<Collider2D>())
        {
            hit = true;
            foreach (Coin c in board.coins)
            {
                if (c != this)
                {
                    try
                    {
                        c.active = false;
                    }
                    catch (Exception e) { }
                }

            }
        }
        return hit;
    }

    Vector2 getTouchPos()
    {
        Vector2 wp;
        if (Application.platform == RuntimePlatform.Android)
        {
            wp = Camera.main.ScreenToWorldPoint(Input.GetTouch(0).position);
        }
        else
        {
            wp = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        }
        Vector2 touchPos = new Vector2(wp.x, wp.y);
        return touchPos;
    }

    public void SmoothSnapToBox(int ind)
    {
        moving = true;
        targetBox = ind;
    }


    public IEnumerator DestroyFade(float startTime)
    {
        for (float f = 1f; f >= 0; f -= 0.1f)
        {
            Color c = GetComponent<Renderer>().material.color;
            c.a = f;
            GetComponent<Renderer>().material.color = c;
            yield return null;
        }
        Destroy(gameObject);

    }
}
