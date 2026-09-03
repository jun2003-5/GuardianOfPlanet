using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class Enemy : MonoBehaviour
{
    [SerializeField] public string id;

    [ContextMenu("Generate guid for id")]
    private void GenerateGuid()
    {
        id = System.Guid.NewGuid().ToString();
    }

    [Header("적 스탯")]
    public float EnemyHealth;
    public int Enemy_EXP;
    public long enemy_Money;
    public float StunResistence;

    public enum typeofEnemy { normal, boss, Dungeon, InfiniteStage, InfiniteStageBoss, DungeonBoss, LabMonster }
    [Header("적 정보")]
    [Space(5)]
    public PoolObjectTypeEnemy typePool;
    public PoolObjectTypeDungeon typeDungeonPool;
    public typeofEnemy _typeofEnemy;
    public Vector3 pos => transform.position;
    public float enemyMovingSpeed;
    public EnemyState _state;

    [Header("추가 요소")]
    public Sprite RawImage;

    [Header("초기 스탯")]
    public InitialValues initialValue = new InitialValues();

    [Header("Slider")]
    public HealthbarBehaviour Healthbar;

    [Header("ISAttackble")]
    public bool IsInRange;

    [Header("#----X Offset")]
    public float XOffset;

    public enum EnemyState
    {
        moving,
        attacked,
        stunned,
        dead,
        reached
    }

    [Header("스폰지역")]
    public Vector3 startLocation;
    public float distance;

    [Header("채력바")]
    public float CurrentHealth;

    //Stunned
    bool IsCoroutineRunning;

    //Moving Timer
    float Timer;

    //Range Related
    Vector3 screenPos;

    private void Awake()
    {
        initialValue.SetInitialValues(EnemyHealth, Enemy_EXP, enemy_Money, StunResistence, enemyMovingSpeed);
    }

    private void Start()
    {
        //Speed
        enemyMovingSpeed = initialValue.initialSpeed;
        //Collider and Sprites Default
        SetLocationandRoatation();
        CurrentHealth = EnemyHealth;
    }

    private void OnEnable()
    {
        for(int i = 0; i < GetComponents<Collider2D>().Length; i++) {
            GetComponents<Collider2D>()[i].enabled = true;
        }
        for(int i = 0; i < this.gameObject.GetComponentsInChildren<SpriteRenderer>().Length; i++) {
            if(gameObject.GetComponentsInChildren<SpriteRenderer>()[i].name != "Shadow")
                this.gameObject.GetComponentsInChildren<SpriteRenderer>()[i].color = new Color(1, 1, 1, 1);
            else
                this.gameObject.GetComponentsInChildren<SpriteRenderer>()[i].color = new Color(0, 0, 0, 0.62f);
        }
        StopAllCoroutines();
        //Speed
        enemyMovingSpeed = initialValue.initialSpeed;

        //Collider and Sprites Default
        SetLocationandRoatation();
        //Default
        _state = EnemyState.moving;

        //Healthbar Loc
        if(Healthbar != null) {
            Healthbar.slider.gameObject.SetActive(false);


            Healthbar.slider.transform.position = Camera.main.WorldToScreenPoint(transform.position + Healthbar.Offset);
        }
    }

    void Update()
    {
        if(_state == EnemyState.moving) {
            if(_typeofEnemy == typeofEnemy.normal || _typeofEnemy == typeofEnemy.Dungeon || _typeofEnemy == typeofEnemy.InfiniteStage)
                transform.position = Vector3.MoveTowards(transform.position, WeaponManager.instance.mainWeapon.transform.position, Time.deltaTime * enemyMovingSpeed);
            else {
                if(Vector3.Distance(WeaponManager.instance.mainWeapon.transform.position, transform.position) > Camera.main.orthographicSize / 1.2f)
                    transform.position = Vector3.MoveTowards(transform.position, WeaponManager.instance.mainWeapon.transform.position, Time.deltaTime * enemyMovingSpeed * 10);
                else
                    transform.position = Vector3.MoveTowards(transform.position, WeaponManager.instance.mainWeapon.transform.position, Time.deltaTime * enemyMovingSpeed);

            }
        } else if(_state == EnemyState.stunned) {
            transform.position = Vector3.MoveTowards(transform.position, WeaponManager.instance.mainWeapon.transform.position, 0);
            Timer += Time.deltaTime / Time.timeScale;
            if(Timer >= 1) {
                _state = EnemyState.moving;
                Timer = 0;
            } else {
                if(IsCoroutineRunning) {
                    IsCoroutineRunning = false;
                    Timer = 0;
                }
            }
        }
        distanceBetween();

        screenPos = GameManager.instance.mainCamera.WorldToScreenPoint(transform.position);

        if(_typeofEnemy == typeofEnemy.boss || _typeofEnemy == typeofEnemy.InfiniteStageBoss || _typeofEnemy == typeofEnemy.DungeonBoss) {
            IsInRange = true;
        } else {
            IsInRange = screenPos.x > 0f && screenPos.x < Screen.width && screenPos.y > 0f && screenPos.y < Screen.height;
        }
        if(Healthbar != null) {
            Healthbar.SetHealth(CurrentHealth, EnemyHealth);
            Healthbar.slider.transform.position = Camera.main.WorldToScreenPoint(transform.position + Healthbar.Offset);
        }

    }

    public void SetLocationandRoatation()
    {
        startLocation = new Vector3(this.transform.position.x, this.transform.position.y, this.transform.position.z);
        _state = EnemyState.moving;

        if(startLocation.x < 0) {
            this.gameObject.transform.rotation = Quaternion.Euler(0.0f, 180, 0.0f);
            if(Healthbar != null)
                Healthbar.Offset = new Vector3(-XOffset, Healthbar.Offset.y, Healthbar.Offset.z);
        } else {
            this.gameObject.transform.rotation = Quaternion.Euler(0.0f, 0.0f, 0.0f);
            if(Healthbar != null)
                Healthbar.Offset = new Vector3(0, Healthbar.Offset.y, Healthbar.Offset.z);
        }
    }

    public void SetEnemyToDefault()
    {
        EnemyHealth = initialValue.EnemyHealth;
        Enemy_EXP = initialValue.Enemy_EXP;
        enemy_Money = initialValue.enemy_Money;
        StunResistence = initialValue.StunResistence;
        enemyMovingSpeed = initialValue.initialSpeed;
        distance = 0;

        for(int i = 0; i < GetComponents<Collider2D>().Length; i++) {
            GetComponents<Collider2D>()[i].enabled = true;
        }
        for(int i = 0; i < this.gameObject.GetComponentsInChildren<SpriteRenderer>().Length; i++) {
            if(gameObject.GetComponentsInChildren<SpriteRenderer>()[i].name != "Shadow")
                this.gameObject.GetComponentsInChildren<SpriteRenderer>()[i].color = new Color(1, 1, 1, 1);
            else
                this.gameObject.GetComponentsInChildren<SpriteRenderer>()[i].color = new Color(0, 0, 0, 0.62f);
        }
        StopAllCoroutines();
    }

    public void distanceBetween()
    {
        if(GameObject.FindGameObjectWithTag("MainWeapon") != null)
            distance = Vector3.Distance(GameObject.FindGameObjectWithTag("MainWeapon").transform.position, pos);
        if(distance < 0.5f) {
            shelterReached();
        }
    }

    public void shelterReached()
    {
        EnemyManager.EnemyReached(this);
    }
    public void OnHit(long DealtDamage, bool Stunned, bool Critical, float CritDamage)
    {
        long totalDamage = 0;
        if(Critical) 
            totalDamage = (long)(DealtDamage * (2 + CritDamage));
        else 
            totalDamage = DealtDamage;
        

        if(Stunned)
            Stunned = Random.Range(0, 101) >= StunResistence;

        if(Healthbar != null)
            Healthbar.slider.gameObject.SetActive(true);

        DamagePopup.Create(this.transform.position, totalDamage, Critical);
        SoundManager.Instance.playEnemyHit();
        //Dead
        if(CurrentHealth - totalDamage > 0) {
            CurrentHealth -= totalDamage;
            //Stun
            if(Stunned) {
                if(_state == EnemyState.stunned)
                    IsCoroutineRunning = true;
                else
                    StartCoroutine(StunnedEffect());
            }
            if(this.gameObject.activeSelf) {
                Coroutine lastcoroutine = StartCoroutine(DamagedEffect());
                if(lastcoroutine == null)
                    StartCoroutine(DamagedEffect());
            }
        } else if(CurrentHealth - totalDamage <= 0 && _state != EnemyState.dead) {
            _state = EnemyState.dead;
            EnemyManager.EnemyDead(this);
        }
    }

    IEnumerator DamagedEffect()
    {
        for(int i = 0; i < this.gameObject.GetComponentsInChildren<SpriteRenderer>().Length; i++) {
            if(gameObject.GetComponentsInChildren<SpriteRenderer>()[i].sprite.name != "Circle")
                gameObject.GetComponentsInChildren<SpriteRenderer>()[i].color = Color.red;
        }
        yield return new WaitForSeconds(0.25f);
        for(int i = 0; i < this.gameObject.GetComponentsInChildren<SpriteRenderer>().Length; i++) {
            if(gameObject.GetComponentsInChildren<SpriteRenderer>()[i].sprite.name != "Circle")
                gameObject.GetComponentsInChildren<SpriteRenderer>()[i].color = Color.white;
        }
    }

    public IEnumerator StunnedEffect()
    {
        _state = EnemyState.stunned;
        yield return new WaitForSeconds(1f);
        _state = EnemyState.moving;
    }

    public void EnemyDeath()
    {
        if(Healthbar != null)
            Healthbar.slider.gameObject.SetActive(false);

        enemyMovingSpeed = 0;
        for(int i = 0; i < GetComponents<Collider2D>().Length; i++) {
            GetComponents<Collider2D>()[i].enabled = false;
        }
        for(int i = 0; i < this.gameObject.GetComponentsInChildren<SpriteRenderer>().Length; i++) {
            EnemyDeathEffect(gameObject.GetComponentsInChildren<SpriteRenderer>()[i], i == this.gameObject.GetComponentsInChildren<SpriteRenderer>().Length - 1);
        }

        SoundManager.Instance.Invoke("playEnemyDead", SoundManager.Instance.EnemyDamagedSFX.length);
    }

    public async void EnemyDeathEffect(SpriteRenderer sprite, bool isLast)
    {
        sprite.color = new Color(sprite.color.r, sprite.color.g, sprite.color.b, 1f);
        for(float f = 1f; f >= -0.1f; f -= 0.075f) {
            sprite.color = new Color(sprite.color.r, sprite.color.g, sprite.color.b, f);
            await Task.Delay(50);
        }
        if(isLast) {
            if(_typeofEnemy == typeofEnemy.Dungeon || _typeofEnemy == typeofEnemy.DungeonBoss) {
                ObjectPoolDungeon.Instance.CoolObject(gameObject, typeDungeonPool);
            } else {
                ObjectPoolEnemy.Instance.CoolObject(gameObject, typePool);
            }
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if(collision.collider.tag == "MainWeapon")
            shelterReached();
    }
}
public class InitialValues
{
    public float EnemyHealth;

    public int Enemy_EXP;

    public long enemy_Money;

    public float StunResistence;

    public float initialSpeed;

    public void SetInitialValues(float health, int exp, long money, float stun, float speed)
    {
        EnemyHealth = health;
        Enemy_EXP = exp;
        enemy_Money = money;
        StunResistence = stun;
        initialSpeed = speed;
    }
}
