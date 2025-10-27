using System;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class FurballAttack : CatAttackBase
{
    public GameObject furballPrefab;
    public Transform furballSpawn;

    private AudioSource furballAttackSFX;

    private void Start()
    {

        furballAttackSFX = GameObject.Find("Furball Attack")?.GetComponent<AudioSource>();
        if (furballAttackSFX == null)
            Debug.LogWarning("Furball Attack not found in scene!");
    }

    public override bool Attack(List<Transform> enemies, Vector3 towerPos, TowerData towerData)
    {
        if (enemies == null || enemies.Count == 0) return false;

        Transform target = enemies[0];
        if (target == null) return false;

        if (furballAttackSFX != null)
            furballAttackSFX.Play();

        Vector2 dir = target.position - towerPos;
        GameObject furball = Instantiate(furballPrefab, furballSpawn.position, Quaternion.identity);
        furball.GetComponent<Furball>().GetTowerData(towerData);
        furball.GetComponent<Furball>().SetDirection(dir);
        furball.GetComponent<Furball>().SetTarget(target);

        return true;
    }
}
