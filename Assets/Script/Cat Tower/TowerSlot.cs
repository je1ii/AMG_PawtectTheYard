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

    public Image furballHolder;
    public Image clawHolder;
    public Image biteHolder;

    public Sprite[] furballBar;
    public Sprite[] clawBar;
    public Sprite[] biteBar;
    
    [HideInInspector] public TowerData currentTower;
    
    public void SetTower(TowerData newTower)
    {
        currentTower = newTower;
        nameText.text = newTower.towerName.ToString();
        if (newTower.currentLevel == 3)
        {
            costText.text = " ";
            levelText.text = "Goodest Cat";
            
            furballHolder.sprite = furballBar[3];
            clawHolder.sprite = clawBar[2];
            biteHolder.sprite = biteBar[1];
        }
        else if(newTower.currentLevel == 2)
        {
            costText.text = newTower.levelCost.ToString();
            levelText.text = "Promote?";
            
            furballHolder.sprite = furballBar[2];
            clawHolder.sprite = clawBar[1];
            biteHolder.sprite = biteBar[0];
        }
        else if(newTower.currentLevel == 1)
        {
            costText.text = newTower.levelCost.ToString();
            levelText.text = "Promote?";
            
            furballHolder.sprite = furballBar[1];
            clawHolder.sprite = clawBar[0];
            biteHolder.sprite = biteBar[0];
        }
        else
        {
            costText.text = newTower.levelCost.ToString();
            levelText.text = "Add Cat?";
            
            furballHolder.sprite = furballBar[0];
            clawHolder.sprite = clawBar[0];
            biteHolder.sprite = biteBar[0];
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
