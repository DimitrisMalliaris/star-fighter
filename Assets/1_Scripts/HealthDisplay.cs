using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HealthDisplay : MonoBehaviour
{
    [SerializeField]Image fill;
    Slider healthSlider;
    Player player;

    // Start is called before the first frame update
    void Start()
    {
        healthSlider = FindObjectOfType<Slider>();
        player = FindObjectOfType<Player>();

        fill = healthSlider.fillRect.GetComponent<Image>();
        fill.color = Color.green;

        healthSlider.maxValue = player.GetHealth();
        healthSlider.value = player.GetHealth();
    }

    // Update is called once per frame
    void Update()
    {
        healthSlider.value = player.GetHealth();

        if(healthSlider.value < healthSlider.maxValue / 2)
        {
            fill.color = Color.red;
        }
        else
        {
            fill.color = Color.green;
        }
    }
}
