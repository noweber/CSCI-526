using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;       // Button

public class ButtonManager : MonoBehaviour
{
    [SerializeField] private Button water, fire, earth, air, plus, minus, x, o;
    private Color32 waterColor = new Color32(0, 0, 255, 255),
        waterLight = new Color32(0, 0, 255, 120);
    private Color32 fireColor = new Color32(255, 0, 0, 255),
        fireLight = new Color32(255, 0, 0, 120);
    private Color32 earthColor = new Color32(255, 255, 0, 255),
        earthLight = new Color32(255, 255, 0, 120);
    private Color32 airColor = new Color32(161, 255, 230, 255),
        airLight = new Color32(161, 255, 230, 120);
    private Color32 defaultWhite = new Color32(255, 255, 255, 255),
        defaultLight = new Color32(255, 255, 255, 120);

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
    public void PlusSelected()
    {
        plus.GetComponent<Image>().color = defaultWhite;
        o.GetComponent<Image>().color = defaultLight;
        minus.GetComponent<Image>().color = defaultLight;
        x.GetComponent<Image>().color = defaultLight;
    }
    public void MinusSelected()
    {
        minus.GetComponent<Image>().color = defaultWhite;
        o.GetComponent<Image>().color = defaultLight;
        plus.GetComponent<Image>().color = defaultLight;
        x.GetComponent<Image>().color = defaultLight;
    }
    public void XSelected()
    {
        x.GetComponent<Image>().color = defaultWhite;
        o.GetComponent<Image>().color = defaultLight;
        minus.GetComponent<Image>().color = defaultLight;
        plus.GetComponent<Image>().color = defaultLight;
    }
    public void OSelected()
    {
        o.GetComponent<Image>().color = defaultWhite;
        x.GetComponent<Image>().color = defaultLight;
        minus.GetComponent<Image>().color = defaultLight;
        plus.GetComponent<Image>().color = defaultLight;
    }

    // Start is called before the first frame update
    void Start()
    {
        water.GetComponent<Image>().color = waterColor;
        fire.GetComponent<Image>().color = fireColor;
        earth.GetComponent<Image>().color = earthColor;
        air.GetComponent<Image>().color = airColor;

        plus.GetComponent<Image>().color = defaultWhite;
        minus.GetComponent<Image>().color = defaultWhite;
        x.GetComponent<Image>().color = defaultWhite;
        o.GetComponent<Image>().color = defaultWhite;

        WaterSelected();
        PlusSelected();
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
