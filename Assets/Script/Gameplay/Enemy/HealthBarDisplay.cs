using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HealthBarDisplay : MonoBehaviour
{
    private Slider healthBarSlider;
    private Camera mainCamera;
    private EnemyAI enemyAI;

    private void Awake()
    {
        healthBarSlider = GetComponent<Slider>();
        mainCamera = Camera.main;
        enemyAI = GetComponentInParent<EnemyAI>();
    }

    // Update is called once per frame
    void Update()
    {
        UpdateDisplay();
        healthBarSlider.transform.LookAt(mainCamera.transform);
    }

    public void UpdateDisplay()
    {
        healthBarSlider.value = enemyAI.currentHealth/enemyAI.maxHealth;
    }
}
