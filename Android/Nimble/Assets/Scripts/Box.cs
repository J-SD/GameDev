using System;
using UnityEngine;
using System.Collections.Generic;

public class Box : MonoBehaviour {
    public bool has_coin = false;
    public Board board;
    public float position;
    public int index;

    GameObject containedCoin;

    void Initialize() {
        index = int.Parse(this.name.Substring(4));
        //boxes = GetComponentInParent<Move>().box_positions;
        float pos = gameObject.transform.position.x;
        GameObject board = GameObject.FindGameObjectWithTag("board");
        this.board = board.GetComponent<Board>();
        this.board.box_positions[index] = pos;
        position = pos;        
        this.board.boxes.Add(this);
    }
	
	// Update is called once per frame
	void Update () {
        
	}

    void OnTriggerEnter2D(Collider2D col) {
        Coin coin = col.GetComponent<Coin>();
        
        if (has_coin)
        {
            if (col.gameObject != containedCoin)
            {
                coin.rejectBox = index;
                coin.rejected = true;
            }            
        }
    }

    void OnTriggerStay2D(Collider2D col) {
        Coin coin = col.GetComponent<Coin>();
        if (Input.GetMouseButton(0) || Input.touches.Length > 0 || !coin.active)
        {
            return;
        }

        if (!has_coin)
        {
            //coin.box_index = index;
            if (!coin.hit)
            {
                
                //col.SendMessage("SnapToBox", index, SendMessageOptions.DontRequireReceiver);
                if (index > 0) { has_coin = true; containedCoin = col.gameObject; }
            }
           
        }
        else
        {
            if (containedCoin != col.gameObject) {
                coin.rejected = true;
            }
            //col.SendMessage("SnapToBox", index+1, SendMessageOptions.DontRequireReceiver);
        }

       
    }

    

    void OnTriggerExit2D() {
        has_coin = false;
    }
}
