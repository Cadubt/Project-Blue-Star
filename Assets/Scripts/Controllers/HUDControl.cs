using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HUDControl : MonoBehaviour
{

    public PlayerController player;
    public Text healthBarUI;

    // Start is called before the first frame update
    void Start()
    {
        //player = GameObject.Find("Player").GetComponent<PlayerController>();
    }

    // Update is called once per frame
    void Update()
    {
        healthBarUI.text = player.playerLife.ToString();
        //Debug.Log(player.playerLife.ToString());
    }
}
