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
    public TextMeshProUGUI costText;
    public TextMeshProUGUI levelText;
    
    [HideInInspector] public TowerData currentTower;
    
    public void SetTower(TowerData newTower)
    {
        currentTower = newTower;
        nameText.text = newTower.towerName.ToString();
        if (newTower.currentLevel == 3)
        {
            costText.text = " ";
            levelText.text = "Goodest Cat";
        }
        else if(newTower.currentLevel == 0)
        {
            costText.text = newTower.levelCost.ToString();
            levelText.text = "Add Cat?";
        }
        else
        {
            costText.text = newTower.levelCost.ToString();
            levelText.text = "Promote?";
        }
        
        if(newTower.towerIcon != null)
            towerIcon.sprite = newTower.towerIcon;
    }

    public void SetLevel(int level)
    {
        slotLevel = level;
    }

    public void OnClick()
    {
        if (currentTower != null)
        {
            if(CatnipManager.Instance.SpendCatnip(currentTower.levelCost))
                TowerPanelUI.Instance.LevelUpTower(slotIndex, slotLevel, slotPos);
        }
    }
}
