using System;
using UnityEngine;
using UnityEngine.UIElements;

public class TowerPanelUI : MonoBehaviour
{
    public static TowerPanelUI Instance;  
    public TowerSlot[] towerSlots; // tower slots in the UI
    public TowerData[] catTowers; // empty, maja, keso, and tustado

    private AudioSource catMeowLvl1SFX;
    private AudioSource catMeowLvl2SFX;
    private AudioSource catMeowLvl3SFX;

    private AudioSource confirmSFX;

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

        catMeowLvl1SFX = GameObject.Find("Cat Meow Lvl1")?.GetComponent<AudioSource>();
        catMeowLvl2SFX = GameObject.Find("Cat Meow Lvl2")?.GetComponent<AudioSource>();
        catMeowLvl3SFX = GameObject.Find("Cat Meow Lvl3")?.GetComponent<AudioSource>();

        confirmSFX = GameObject.Find("Confirm Select Tower")?.GetComponent<AudioSource>();

        // Increase volume x2, clamp at 1
        if (confirmSFX != null) confirmSFX.volume = Mathf.Min(confirmSFX.volume * 6, 1f);
    }

    public void LevelUpTower(int slotIndex, int level, Transform pos)
    {
        if (level < 3)
        {
            var newLevel = level + 1;
            var nextCatTower = catTowers[newLevel];
            towerSlots[slotIndex].SetTower(nextCatTower);
            towerSlots[slotIndex].SetLevel(newLevel);

            if (confirmSFX != null) confirmSFX.Play();

            switch (newLevel)
            {
                case 1:
                    if (catMeowLvl1SFX != null) catMeowLvl1SFX.Play();
                    break;
                case 2:
                    if (catMeowLvl2SFX != null) catMeowLvl2SFX.Play();
                    break;
                case 3:
                    if (catMeowLvl3SFX != null) catMeowLvl3SFX.Play();
                    break;
            }

            if (nextCatTower.prefab != null)
            {
                Quaternion lastTowerRotation = Quaternion.identity;
                var existingTowers = FindObjectsByType<CatTower>(FindObjectsSortMode.None);

                foreach (var t in existingTowers)
                {
                    if (t.slotParent == slotIndex)
                    {
                        if (t.gameObject != null)
                        {
                            lastTowerRotation = t.transform.localRotation;
                            
                            t.GetComponent<CatTower>().ForceStopEverything();
                            t.gameObject.SetActive(false);
                        }
                    }
                }

                var newTower = Instantiate(nextCatTower.prefab, pos);
                newTower.transform.localRotation = lastTowerRotation;
                
                var ct = newTower.GetComponent<CatTower>();
                ct.AssignSlot(slotIndex);
                ct.AssignData(catTowers[newLevel]);
                ct.AssignAttackInterval(newLevel == 3 ? 1.2f : 1.6f);
            }
        }
    }
}
