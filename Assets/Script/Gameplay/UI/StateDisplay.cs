using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class StateDisplay : MonoBehaviour
{
    public TMP_Text text;
    public PlayerMovements playerMovement;
    public string state;

    private void Awake()
    {
        text = GetComponent<TMP_Text>();
        playerMovement = GameObject.Find("Character").GetComponent<PlayerMovements>();

    }
    void Start()
    {

    }

    void Update()
    {
        text.text = "state: " + playerMovement.state;
    }
}
