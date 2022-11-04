using Newtonsoft.Json;
using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using TMPro;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    // Start is called before the first frame update
    public static GameManager instance;
    private float elapsedSpawnTime;
    private EnemyFactory enemyFactory;
    [SerializeField]
    private TextMeshProUGUI killedEnemies;
    public bool isSave;
    private string path;
    private float survivedTime;
    private void Awake()
    {
        instance = this;
        enemyFactory = new EnemyFactory();
        path = $"{Application.persistentDataPath}/";
        
    }
    void Start()
    {
        //if (LevelLoader.Instance.isLoad)
        //{
        //    LoadGame();
        //}
        isSave = false;
        elapsedSpawnTime = 0;
    }

    // Update is called once per frame
    void Update()
    {
        survivedTime += Time.deltaTime;
        if (isSave)
        {
            SaveGame();
        }
        elapsedSpawnTime += Time.deltaTime;
        if (elapsedSpawnTime >= 5)
        {
            EnemySpawner.Instance.Spawn();
            elapsedSpawnTime = 0f;
        }
    }

    public void UpdateKill()
    {
        killedEnemies.text = (Int32.Parse(killedEnemies.text) + 1).ToString();
    }

    #region Save game
    public void SaveGame()
    {
        GameObject[] enemies = GameObject.FindGameObjectsWithTag("Enemy");
        List<EnemyModel> enemyModels = new List<EnemyModel>();
        List<BulletModel> bulletModels = new List<BulletModel>();
        List<WeaponModel> weapontModels = new List<WeaponModel>();
        foreach (var enemy in enemies)
        {
            if (EnemyLevel1.Instance.poolMelee.CountActive > 0 && enemy.TryGetComponent<MeleeEnemy1>(out MeleeEnemy1 melee1))
            {
                enemyModels.Add(new EnemyModel { positionX = melee1.gameObject.transform.position.x, positionY = melee1.gameObject.transform.position.y, currentHealth = melee1.currentHealth, level = 1, type = "MELEE" });
            }
            if (EnemyLevel1.Instance.poolRange.CountActive > 0 && enemy.TryGetComponent<RangeEnemy1>(out RangeEnemy1 range1))
            {
                enemyModels.Add(new EnemyModel { positionX = range1.gameObject.transform.position.x, positionY = range1.gameObject.transform.position.y, currentHealth = range1.currentHealth, level = 1, type = "RANGE" });
            }
            if (EnemyLevel2.Instance.poolMelee.CountActive > 0 && enemy.TryGetComponent<MeleeEnemy2>(out MeleeEnemy2 melee2))
            {
                enemyModels.Add(new EnemyModel { positionX = melee2.gameObject.transform.position.x, positionY = melee2.gameObject.transform.position.y, currentHealth = melee2.currentHealth, level = 1, type = "MELEE" });
            }
        }

        Player player = new Player { positionX = PlayerController.Instance.transform.position.x, positionY = PlayerController.Instance.transform.position.y, currentHealth = PlayerController.Instance.currentHealth, survivedTime = survivedTime, killedEnemies = int.Parse(killedEnemies.ToString()) };
        GameObject[] bullets = GameObject.FindGameObjectsWithTag("Bullet");
        foreach (var bullet in bullets)
        {
            bulletModels.Add(new BulletModel { positionX = bullet.transform.position.x, positionY = bullet.transform.position.y });
        }

        for (int i = 0; i < WeaponHolder.Instance.weapons.Length; i++)
        {
            weapontModels.Add(new WeaponModel
            {
                index = i,
                bulletStock = WeaponHolder.Instance.GetWeapon(i).GetComponent<WeaponController>().bulletStock,
                isActive = WeaponHolder.Instance.currentWeaponIndex == i ? true : false
            });
        }

        var playerJson = JsonConvert.SerializeObject(player, Formatting.Indented);
        var weaponJson = JsonConvert.SerializeObject(weapontModels, Formatting.Indented);
        var bulletJson = JsonConvert.SerializeObject(bulletModels, Formatting.Indented);
        var enemyJson = JsonConvert.SerializeObject(enemyModels, Formatting.Indented);
        File.WriteAllText($"{path}player.json", playerJson);
        File.WriteAllText($"{path}weapons.json", weaponJson);
        File.WriteAllText($"{path}bullets.json", bulletJson);
        File.WriteAllText($"{path}enemies.json", enemyJson);
    }
    #endregion

    #region Load game
    public void LoadGame()
    {
        string playerJson = File.ReadAllText($"{path}player.json");
        string weaponJson = File.ReadAllText($"{path}weapons.json");
        string bulletJson = File.ReadAllText($"{path}bullets.json");
        string enemyJson = File.ReadAllText($"{path}enemies.json");
        var player = JsonConvert.DeserializeObject<Player>(playerJson);
        var weapons = JsonConvert.DeserializeObject<List<WeaponModel>>(weaponJson);
        var bullets = JsonConvert.DeserializeObject<List<BulletModel>>(bulletJson);
        var enemies = JsonConvert.DeserializeObject<List<EnemyModel>>(enemyJson);

        PlayerController.Instance.transform.position = new Vector3(player.positionX, player.positionY, 0f);
        PlayerController.Instance.currentHealth = player.currentHealth;
        killedEnemies.text = player.killedEnemies.ToString();


        foreach (var weapon in weapons)
        {
            WeaponHolder.Instance.GetWeapon(weapon.index).GetComponent<WeaponController>().bulletStock = weapon.bulletStock;
            if (weapon.isActive)
            {
                WeaponHolder.Instance.ChangeWeapon(weapon.index);
            }
        }
        var currentWeapon = WeaponHolder.Instance.GetCurrentWeapon();
        foreach (var bullet in bullets)
        {
            currentWeapon.GetComponent<WeaponController>().LoadBullet(new Vector3(bullet.positionX, bullet.positionY, 0f));
        }

        foreach (var enemy in enemies)
        {
            if (enemy.level == 1)
            {
                if (enemy.type.Equals("MELEE"))
                {
                    enemyFactory.CreateFactory(EnemyLevel.LEVEL1).MeleeEnemy(new Vector3(enemy.positionX, enemy.positionY, 0f));
                }
                else
                {
                    enemyFactory.CreateFactory(EnemyLevel.LEVEL1).RangeEnemy(new Vector3(enemy.positionX, enemy.positionY, 0f));
                }
            }
            else if (enemy.level == 2)
            {
                if (enemy.type.Equals("MELEE"))
                {
                    enemyFactory.CreateFactory(EnemyLevel.LEVEL2).MeleeEnemy(new Vector3(enemy.positionX, enemy.positionY, 0f));
                }
                else
                {
                    enemyFactory.CreateFactory(EnemyLevel.LEVEL2).RangeEnemy(new Vector3(enemy.positionX, enemy.positionY, 0f));
                }
            }
        }
    }
    #endregion

}
