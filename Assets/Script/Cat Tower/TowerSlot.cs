using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class TowerSlot : MonoBehaviour
{
    public Transform slotPos;
    public int slotIndex;
    public int slotLevel;
    public Image towerIcon;
    
    public TextMeshProUGUI nameText;
    //public TextMeshProUGUI costText;
    
    [HideInInspector] public TowerData currentTower;

    public void SetTower(TowerData newTower)
    {
        currentTower = newTower;
        //towerIcon.sprite = newTower.towerIcon;
        nameText.text = newTower.towerName;
        //costText.text = newTower.cost.text;
    }

    public void SetLevel(int level)
    {
        slotLevel = level;
    }

    public void OnClick()
    {
        if (currentTower != null)
        {
            TowerPanelUI.instance.LevelUpTower(slotIndex, slotLevel, slotPos);
        }
    }
}
