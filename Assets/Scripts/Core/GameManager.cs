using Newtonsoft.Json;
using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

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
    private float elapsedChangeIndex;
    private float spawnDuration;
    [SerializeField]
    private GameObject pauseMenu;
    [SerializeField]
    private GameObject confirmQuitMenu;
    [SerializeField]
    private TextMeshProUGUI killedEnemiesGO;
    [SerializeField]
    private TextMeshProUGUI survivedTimeGO;
    [SerializeField]
    private GameObject gameoverPanel;
    public bool isVibrate;

    private void Awake()
    {
        Time.timeScale = 1f;
        instance = this;
        enemyFactory = new EnemyFactory();
        path = $"{Application.persistentDataPath}/";

    }
    void Start()
    {
        isVibrate = true;
        spawnDuration = Constant.spawnDuration;
        elapsedChangeIndex = 0f;
        if (StartMenuController.Instance.isLoad)
        {
            LoadGame();
        }
        isSave = false;
        elapsedSpawnTime = 0;
    }

    // Update is called once per frame
    void Update()
    {
        
        elapsedChangeIndex += Time.deltaTime;
        
        if (elapsedChangeIndex > Constant.changeIndexTimeDuration)
        {
            ChangeGameIndex();
            elapsedChangeIndex = 0f;
        }
        survivedTime += Time.deltaTime;
       
        if (isSave)
        {
            SaveGame();
        }
        elapsedSpawnTime += Time.deltaTime;
        if (elapsedSpawnTime >= spawnDuration )
        {
            EnemySpawner.Instance.Spawn();
            EnemySpawner.Instance.Spawn();
            elapsedSpawnTime = 0f;
        }
        if(PlayerController.Instance.currentHealth <= 0)
        {
            GameOver();
        }
    }

    public void UpdateKill()
    {
        killedEnemies.text = (Int32.Parse(killedEnemies.text) + 1).ToString();
    }

    private void GameOver()
    {
        isVibrate = false;
        if (File.Exists(Path.Combine(path, "player.json")))
        {
            File.Delete(Path.Combine(path, "player.json"));
            File.Delete(Path.Combine(path, "bullets.json"));
            File.Delete(Path.Combine(path, "enemies.json"));
            File.Delete(Path.Combine(path, "weapons.json"));
            File.Delete(Path.Combine(path, "ammoes.json"));
        }
        gameoverPanel.SetActive(true);
        Time.timeScale = 0f;
        int second = Mathf.FloorToInt(survivedTime);
        survivedTimeGO.text = $"{second/60}:{second%60}";
        killedEnemiesGO.text = killedEnemies.text;
    }

    #region Button action
    public void PauseGame()
    {
        isVibrate = false;
        pauseMenu.SetActive(true);
        Time.timeScale = 0.001f;
    }

    public void ResumeGame()
    {
        isVibrate = true;
        pauseMenu.SetActive(false);
        Time.timeScale = 1f;
        
    }

    public void SaveAndQuit()
    {
        SaveGame();
        QuitGame();
    }

    public void QuitGame()
    {
        Application.Quit();
    }

    public void ConfirmQuit()
    {
        confirmQuitMenu.SetActive(true);
    }

    public void Restart()
    {
        StartMenuController.Instance.isLoad = false;
        gameoverPanel.SetActive(false);
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex); 
    }
    #endregion

    #region Change game index
    public void ChangeGameIndex()
    {
        if(spawnDuration > 1f)
        {
            spawnDuration -= Constant.reducedSpawnDuration;
        }
        var enemiesIndex = EnemySpawner.Instance.enemies;
        var ammoesIndex = AmmoSpawner.Instance.ammoes;

        for (int i = 0; i < ammoesIndex.Length; i++)
        {
            if (ammoesIndex[i].startChance > ammoesIndex[i].finalChance && ammoesIndex[i].chance > ammoesIndex[i].finalChance)
            {
                AmmoSpawner.Instance.ammoes[i].chance -= Constant.ammoSpawnPercentage / (i == 0 ? 1 : i);
            }
            else if(ammoesIndex[i].startChance < ammoesIndex[i].finalChance && ammoesIndex[i].chance < ammoesIndex[i].finalChance)
            {
                AmmoSpawner.Instance.ammoes[i].chance += Constant.ammoSpawnPercentage / (i == 0 ? 1 : i);
            }
        }

        for (int i = 0; i < enemiesIndex.Length; i++)
        {
            if (enemiesIndex[i].startChance > enemiesIndex[i].finalChance && enemiesIndex[i].chance > enemiesIndex[i].finalChance)
            {
                EnemySpawner.Instance.enemies[i].chance -= Constant.enemySpawnPercentage / (i == 0 ? 1 : i);
            }
            else if (enemiesIndex[i].startChance < enemiesIndex[i].finalChance && enemiesIndex[i].chance < enemiesIndex[i].finalChance)
            {
                EnemySpawner.Instance.enemies[i].chance += Constant.enemySpawnPercentage / (i == 0 ? 1 : i);
            }
        }
        EnemySpawner.Instance.CalculateWeights();
        AmmoSpawner.Instance.CalculateWeights();
    }
    #endregion

    #region Save game
    public void SaveGame()
    {
        GameObject[] enemies = GameObject.FindGameObjectsWithTag("Enemy");
        GameObject[] ammoes = GameObject.FindGameObjectsWithTag("Ammo");
        GameIndexModel gameIndexModel = new GameIndexModel();
        List<EnemyModel> enemyModels = new List<EnemyModel>();
        List<BulletModel> bulletModels = new List<BulletModel>();
        List<WeaponModel> weapontModels = new List<WeaponModel>();
        List<AmmoModel> ammoModels = new List<AmmoModel>();
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
            if (EnemyLevel2.Instance.poolMelee.CountActive > 0 && enemy.TryGetComponent<MeleeEnemy2>(out MeleeEnemy2 range2))
            {
                enemyModels.Add(new EnemyModel { positionX = range2.gameObject.transform.position.x, positionY = range2.gameObject.transform.position.y, currentHealth = range2.currentHealth, level = 1, type = "RANGE" });
            }
        }

        Player player = new Player { positionX = PlayerController.Instance.transform.position.x, positionY = PlayerController.Instance.transform.position.y, currentHealth = PlayerController.Instance.currentHealth, survivedTime = survivedTime, killedEnemies = Int32.Parse(killedEnemies.text) };
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

        foreach(var ammo in ammoes)
        {
            switch (ammo.GetComponent<AmmoController>().weaponIndex)
            {
                case 0:
                    ammoModels.Add(new AmmoModel { index = 1, positionX = ammo.transform.position.x, positionY = ammo.transform.position.y });
                    break;
                case 1:
                    ammoModels.Add(new AmmoModel { index = 2, positionX = ammo.transform.position.x, positionY = ammo.transform.position.y });
                    break;
                case 2:
                    ammoModels.Add(new AmmoModel { index = 3, positionX = ammo.transform.position.x, positionY = ammo.transform.position.y });
                    break;
                case 3:
                    ammoModels.Add(new AmmoModel { index = 4, positionX = ammo.transform.position.x, positionY = ammo.transform.position.y });
                    break;
                case 4:
                    ammoModels.Add(new AmmoModel { index = 5, positionX = ammo.transform.position.x, positionY = ammo.transform.position.y });
                    break;
            }
        }
        Debug.Log(EnemySpawner.Instance.enemies[0]);
        gameIndexModel.enemySpawnPercentage = new List<float>();
        gameIndexModel.ammoSpawnPercentage = new List<float>();
        foreach (var enemy in EnemySpawner.Instance.enemies)
        {
            gameIndexModel.enemySpawnPercentage.Add(enemy.chance);
        }
        foreach (var ammo in AmmoSpawner.Instance.ammoes)
        {
            gameIndexModel.ammoSpawnPercentage.Add(ammo.chance);
        }
        gameIndexModel.spawnDuration = spawnDuration;

        var playerJson = JsonConvert.SerializeObject(player, Formatting.Indented);
        var weaponJson = JsonConvert.SerializeObject(weapontModels, Formatting.Indented);
        var bulletJson = JsonConvert.SerializeObject(bulletModels, Formatting.Indented);
        var enemyJson = JsonConvert.SerializeObject(enemyModels, Formatting.Indented);
        var ammoJson = JsonConvert.SerializeObject(ammoModels, Formatting.Indented);
        var gameIndexJson = JsonConvert.SerializeObject(gameIndexModel, Formatting.Indented); 
        File.WriteAllText($"{path}player.json", playerJson);
        File.WriteAllText($"{path}weapons.json", weaponJson);
        File.WriteAllText($"{path}bullets.json", bulletJson);
        File.WriteAllText($"{path}enemies.json", enemyJson);
        File.WriteAllText($"{path}ammoes.json", ammoJson);
        File.WriteAllText($"{path}game-index.json", gameIndexJson);
    }
    #endregion

    #region Load game
    public void LoadGame()
    {
        string playerJson = File.ReadAllText($"{path}player.json");
        string weaponJson = File.ReadAllText($"{path}weapons.json");
        string bulletJson = File.ReadAllText($"{path}bullets.json");
        string enemyJson = File.ReadAllText($"{path}enemies.json");
        string ammoJson = File.ReadAllText($"{path}ammoes.json");
        string gameIndexJson = File.ReadAllText($"{path}game-index.json");
        var player = JsonConvert.DeserializeObject<Player>(playerJson);
        var weapons = JsonConvert.DeserializeObject<List<WeaponModel>>(weaponJson);
        var bullets = JsonConvert.DeserializeObject<List<BulletModel>>(bulletJson);
        var enemies = JsonConvert.DeserializeObject<List<EnemyModel>>(enemyJson);
        var ammoes = JsonConvert.DeserializeObject<List<AmmoModel>>(ammoJson);
        var gameIndex = JsonConvert.DeserializeObject<GameIndexModel>(gameIndexJson);

        PlayerController.Instance.transform.position = new Vector3(player.positionX, player.positionY, 0f);
        PlayerController.Instance.currentHealth = player.currentHealth;
        killedEnemies.text = player.killedEnemies.ToString();
        survivedTime = player.survivedTime;

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

        foreach(var ammo in ammoes)
        {
            AmmoSpawner.Instance.SpawnAmmoWithIndex(new Vector2(ammo.positionX, ammo.positionY), ammo.index);
        }

        spawnDuration = gameIndex.spawnDuration;
        for(int i = 0; i < gameIndex.enemySpawnPercentage.Count; i++)
        {
            EnemySpawner.Instance.enemies[i].chance = gameIndex.enemySpawnPercentage[i];
        }
        for (int i = 0; i < gameIndex.ammoSpawnPercentage.Count; i++)
        {
            AmmoSpawner.Instance.ammoes[i].chance = gameIndex.ammoSpawnPercentage[i];
        }

    }
    #endregion

}
