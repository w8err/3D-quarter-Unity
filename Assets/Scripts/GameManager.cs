using UnityEngine;
using UnityEngine.UI;
using UnityEngine.UIElements;
using TMPro;
using System.Collections;
using System.Collections.Generic;

public class GameManager : MonoBehaviour
{
    // 인게임 매니저
    public GameObject menuCam;
    public GameObject gameCam;
    public Player player;
    public Boss boss;
    public GameObject itemShop;
    public GameObject weaponShop;
    public GameObject startZone;

    public int stage;
    public float playTime;
    public bool isBattle;
    public int enemyCntA;
    public int enemyCntB;
    public int enemyCntC;

    public Transform[] enemyZones;
    public GameObject[] enemies;
    public List<int> enemyList;

    // UI 매니저
    public GameObject menuPanel;
    public GameObject gamePanel;
    public TextMeshProUGUI maxScore;         // 기존 레거시 텍스트는 Text 자료형, 메시프로는 TextMeshProUGUI 써야함
    public TextMeshProUGUI stageTxt;
    public TextMeshProUGUI playTimeTxt;
    public TextMeshProUGUI scoreTxt;

    // 플레이어 UI 매니저
    public TextMeshProUGUI playerHPTxt;
    public TextMeshProUGUI playerAmmoTxt;
    public TextMeshProUGUI playerCoinTxt;
    public UnityEngine.UI.Image weapon1Img;
    public UnityEngine.UI.Image weapon2Img;
    public UnityEngine.UI.Image weapon3Img;
    public UnityEngine.UI.Image weaponRImg;

    public TextMeshProUGUI enemyATxt;
    public TextMeshProUGUI enemyBTxt;
    public TextMeshProUGUI enemyCTxt;

    // 보스 UI
    public RectTransform bossHealthGroup;
    public RectTransform bossHPBar;

    void Awake()
    {
        stage++;
        enemyList = new List<int>();
        maxScore.text = string.Format("{0:n0}", PlayerPrefs.GetInt("MaxScore"));
    }

    public void GameStart()
    {
        menuCam.SetActive(false);
        gameCam.SetActive(true);

        menuPanel.SetActive(false);
        gamePanel.SetActive(true);

        player.gameObject.SetActive(true);
    }

    public void StageStart()
    {
        itemShop.SetActive(false);
        weaponShop.SetActive(false);
        startZone.SetActive(false);
        isBattle = true;

        foreach(Transform zone in enemyZones)
            zone.gameObject.SetActive(true);

        StartCoroutine(InBattle());
    }
    public void StageEnd()
    { 
        itemShop.SetActive(true);
        weaponShop.SetActive(true);
        startZone.SetActive(true);

        foreach (Transform zone in enemyZones)
            zone.gameObject.SetActive(false);

        isBattle = false;
        stage++;

        player.transform.position = Vector3.up * 0.8f;
    }

    IEnumerator InBattle()
    {
        for(int index = 0; index < stage; index++)
        {
            int ran = Random.Range(0, 3);
            enemyList.Add(ran);
        }

        while(enemyList.Count > 0)
        {
            int ranzZone = Random.Range(0, 4);
            GameObject instantEnemy = Instantiate(enemies[enemyList[0]], enemyZones[ranzZone].position, enemyZones[ranzZone].rotation);     // 프리팹은 SCENE에 올라온 오브젝트에 접근 불가
            Enemy enemy = instantEnemy.GetComponent<Enemy>();
            enemy.target = player.transform;
            enemyList.RemoveAt(0);
            yield return new WaitForSeconds(4f);
        }
    }

    void Update()
    {
        if (isBattle)
            playTime += Time.deltaTime;
    }

    void LateUpdate()   // Update()가 끝난 후 호출되는 생명주기
    {
        // 상단 UI
        scoreTxt.text = string.Format("{0:n0}", player.score);
        stageTxt.text = " STAGE " + stage;

        int hour = (int)(playTime / 3600);
        int min = (int)((playTime - hour * 3600) / 60);
        int second = (int)(playTime % 60);
        playTimeTxt.text = string.Format("{0:00}", hour) + ":" + string.Format("{0:00}", min) + ":" + string.Format("{0:00}", second);


        // 플레이어 UI
        playerHPTxt.text = player.health +" / " + player.maxHealth;
        playerCoinTxt.text = string.Format("{0:n0}", player.coin);
        if (player.equipWeapon == null || player.equipWeapon.type == Weapon.Type.Melee)
            playerAmmoTxt.text = " - / " + player.ammo;
        else
            playerAmmoTxt.text = player.equipWeapon.curAmmo + " / " + player.ammo;

        weapon1Img.color = new Color(1, 1, 1, player.hasWeapons[0] ? 1 : 0);
        weapon2Img.color = new Color(1, 1, 1, player.hasWeapons[1] ? 1 : 0);
        weapon3Img.color = new Color(1, 1, 1, player.hasWeapons[2] ? 1 : 0);
        weaponRImg.color = new Color(1, 1, 1, player.hasGrenades > 0 ? 1 : 0);

        // 몬스터 UI
        enemyATxt.text = enemyCntA.ToString();
        enemyBTxt.text = enemyCntB.ToString();
        enemyCTxt.text = enemyCntC.ToString();

        if (boss != null)
        bossHPBar.localScale = new Vector3((float)boss.curHealth / boss.maxHealth, 1, 1);
    }

}
