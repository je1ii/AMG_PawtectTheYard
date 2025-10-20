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

    void Start()
    {
        towerIcon = gameObject.GetComponentInChildren<Image>();
    }
    
    public void SetTower(TowerData newTower)
    {
        currentTower = newTower;
        nameText.text = newTower.towerName;
        //costText.text = newTower.cost.text;
        
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
