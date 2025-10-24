using System;
using UnityEngine;

public class TowerPanelUI : MonoBehaviour
{
    public static TowerPanelUI Instance;  
    public TowerSlot[] towerSlots; // tower slots in the UI
    public TowerData[] catTowers; // empty, maja, keso, and tustado
    
    private void Awake()
    {
        if (Instance == null)
            Instance = this;
        else
            Destroy(gameObject);
    }

    private void Start()
    {
        foreach (var t in towerSlots)
        {
            t.SetTower(catTowers[0]); // setting tower slots to empty
            t.SetLevel(0);
        }
    }

    public void LevelUpTower(int slotIndex, int level, Transform pos)
    {
        if (level < 3)
        {
            var newLevel = level + 1;
            var nextCatTower = catTowers[newLevel];
            towerSlots[slotIndex].SetTower(nextCatTower);
            towerSlots[slotIndex].SetLevel(newLevel);
        
            if (nextCatTower.prefab != null)
            {
                var existingTowers = FindObjectsByType<CatTower>(FindObjectsSortMode.None);

                foreach (var t in existingTowers)
                {
                    if (t.slotParent == slotIndex)
                    {
                        if (t.gameObject != null)
                        {
                            // eliminate null exception error
                            t.GetComponent<CatTower>().ForceStopEverything();
                            t.gameObject.SetActive(false);
                        }
                    }
                }
                
                var newTower = Instantiate(nextCatTower.prefab, pos);
                var ct = newTower.GetComponent<CatTower>();
                ct.AssignSlot(slotIndex);
                ct.AssignData(catTowers[newLevel]);
                ct.AssignAttackInterval(newLevel == 3 ? 1.2f : 1.6f);
            }
        }
    }
}