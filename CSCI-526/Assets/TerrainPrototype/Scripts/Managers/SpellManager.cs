using Assets.TerrainPrototype.Source;
using UnityEngine;
using UnityEngine.UI;

public class SpellManager : MonoBehaviour
{
    public static SpellManager Instance;

    public TerrainMappings.Element SelectedElement;

    public bool SpellSelected { get; private set; }

    [SerializeField] private Button Fire, Water, Earth, Air;

    void Awake()
    {
        Instance = this;
        SpellSelected = false;
    }

    public void SelectFire()
    {
        SelectedElement = TerrainMappings.Element.Fire;
        SpellSelected = true;
    }
    public void SelectWater()
    {
        SelectedElement = TerrainMappings.Element.Water;
        SpellSelected = true;
    }
    public void SelectEarth()
    {
        SelectedElement = TerrainMappings.Element.Earth;
        SpellSelected = true;
    }
    public void SelectAir()
    {
        SelectedElement = TerrainMappings.Element.Air;
        SpellSelected = true;
    }

    public void CastSpell(int x, int y, TerrainMappings.Element element)
    {
        Prototype2MenuManager.Instance.HideSpellUi();
        SpellSelected = false;
    }
}
