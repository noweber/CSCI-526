using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;       // Button

public class NewButtonManager : MonoBehaviour
{
    [SerializeField] private Button water, fire, earth, air;
    private Color32 waterColor = new Color32(0, 0, 255, 255),
        waterLight = new Color32(0, 0, 255, 120);
    private Color32 fireColor = new Color32(255, 0, 0, 255),
        fireLight = new Color32(255, 0, 0, 120);
    private Color32 earthColor = new Color32(0, 255, 0, 255),
        earthLight = new Color32(0, 255, 0, 120);
    private Color32 airColor = new Color32(255, 255, 0, 255),
        airLight = new Color32(255, 255, 0, 120);


    public void WaterSelected()
    {
        water.GetComponent<Image>().color = waterColor;
        fire.GetComponent<Image>().color = fireLight;
        earth.GetComponent<Image>().color = earthLight;
        air.GetComponent<Image>().color = airLight;
    }
    public void FireSelected()
    {
        fire.GetComponent<Image>().color = fireColor;
        water.GetComponent<Image>().color = waterLight;
        earth.GetComponent<Image>().color = earthLight;
        air.GetComponent<Image>().color = airLight;
    }
    public void EarthSelected()
    {
        earth.GetComponent<Image>().color = earthColor;
        fire.GetComponent<Image>().color = fireLight;
        water.GetComponent<Image>().color = waterLight;
        air.GetComponent<Image>().color = airLight;
    }
    public void AirSelected()
    {
        air.GetComponent<Image>().color = airColor;
        fire.GetComponent<Image>().color = fireLight;
        earth.GetComponent<Image>().color = earthLight;
        water.GetComponent<Image>().color = waterLight;
    }

    // Start is called before the first frame update
    void Start()
    {
        water.GetComponent<Image>().color = waterColor;
        fire.GetComponent<Image>().color = fireColor;
        earth.GetComponent<Image>().color = earthColor;
        air.GetComponent<Image>().color = airColor;

        WaterSelected();
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
