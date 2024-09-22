using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CharHealthBarDisplay : MonoBehaviour
{
    private Slider healthBarSlider;
    private Camera mainCamera;
    private PlayerHealth playerHealth;

    private void Awake()
    {
        healthBarSlider = GetComponent<Slider>();
        playerHealth = GameObject.Find("Character").GetComponent<PlayerHealth>();
        
    }

    // Update is called once per frame
    void Update()
    {
        UpdateDisplay();
    }

    public void UpdateDisplay()
    {
        healthBarSlider.value = playerHealth.currentHealth / playerHealth.maxHealth;
    }
}
