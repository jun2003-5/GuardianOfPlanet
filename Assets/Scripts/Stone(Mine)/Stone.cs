using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.EventSystems;
using UnityEngine.Tilemaps;
using System;

public class Stone : MonoBehaviour
{
    [Header("#---Stone Sprite")]
    public Sprite StoneSprite;
    public Sprite oreSprites;


    [Header("Ã¤·Â")]
    public int StoneHealth;
    [HideInInspector]
    public int MaxHealth;

    public int initialHealth;

    [HideInInspector]
    public long StoneValue;

    [Header("UIs")]
    public HealthbarBehaviour healthbar;

    [Header("Pooling Type")]
    public PoolObjectTypeStone type;

    public Tilemap tilemap;
    public Rail rail;

    bool isHit;
    private bool canMove = true;


    public int pointsIndex;
    public bool isOverLimit;

    private void Update()
    {
        if(!isOverLimit) {
            if(canMove) {
                Vector3Int gridPosition = tilemap.WorldToCell(transform.position + Vector3.left / 2);

                if(tilemap.HasTile(gridPosition) && !isHit)
                    transform.Translate(-Vector3.right * 1 * Time.deltaTime);
            }
        } else {
            if(canMove) {
                transform.position = Vector2.MoveTowards(transform.position, rail.points[pointsIndex].position, 2 * Time.deltaTime);

                if(transform.position == rail.points[pointsIndex].transform.position) {
                    if(pointsIndex < rail.points.Length - 1)
                        pointsIndex++;
                    else {
                        ObjectPoolStone.Instance.CoolObject(this.gameObject, type);
                        rail.spawnedStones.Remove(this);
                        rail.drill.IsShooting = rail.spawnedStones.Count > 0;
                        rail.unMinedStoneAdd();
                    }
                }
            }
        }
        healthbar.slider.transform.position = Camera.main.WorldToScreenPoint(transform.position + healthbar.Offset);
    }

    public void OnEnable()
    {
        MaxHealth = StoneHealth;
        SetStoneInfo();
    }

    public void SetHealth(int damage)
    {
        StoneHealth -= damage;
        SetStoneInfo();
    }

    public void SetStoneInfo()
    {
        healthbar.SetHealthInvisible(StoneHealth, MaxHealth);
    }

    public void OnHit(Vector3 pos, int Damage)
    {
        if(StoneHealth - Damage > 0)
            SetHealth(Damage);
        else {
            GameManager.SetOre((long)(StoneValue * (1 + MineManager.instance.ExtraRailOre)));
            MineManager.instance.getStoneParts();
            ObjectPoolStone.Instance.CoolObject(this.gameObject, type);
            StoneHealth = MaxHealth;
            MaterialPopUp.Create(pos, oreSprites, (int)((int)StoneValue * (1 + MineManager.instance.ExtraRailOre)));
            rail.spawnedStones.Remove(this);
            rail.drill.IsShooting = rail.spawnedStones.Count > 0;
            rail.stoneDied();
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if(collision.gameObject.GetComponent<Stone>() != null) {
            float collisionDirection = Mathf.Sign(collision.transform.position.x - transform.position.x);

            if(collisionDirection < 0) {
                canMove = false;
            }
        }
    }

    private void OnCollisionExit2D(Collision2D collision)
    {
        if(collision.gameObject.GetComponent<Stone>() != null) {
            canMove = true;
        }
    }
}
