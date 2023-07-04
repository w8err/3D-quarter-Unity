using UnityEngine;
using UnityEngine.UI;
using UnityEngine.UIElements;
using TMPro;
using System.Collections;
using System.Collections.Generic;

public class GameManager : MonoBehaviour
{
    // �ΰ��� �Ŵ���
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

    // UI �Ŵ���
    public GameObject menuPanel;
    public GameObject gamePanel;
    public TextMeshProUGUI maxScore;         // ���� ���Ž� �ؽ�Ʈ�� Text �ڷ���, �޽����δ� TextMeshProUGUI �����
    public TextMeshProUGUI stageTxt;
    public TextMeshProUGUI playTimeTxt;
    public TextMeshProUGUI scoreTxt;

    // �÷��̾� UI �Ŵ���
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

    // ���� UI
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
            GameObject instantEnemy = Instantiate(enemies[enemyList[0]], enemyZones[ranzZone].position, enemyZones[ranzZone].rotation);     // �������� SCENE�� �ö�� ������Ʈ�� ���� �Ұ�
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

    void LateUpdate()   // Update()�� ���� �� ȣ��Ǵ� �����ֱ�
    {
        // ��� UI
        scoreTxt.text = string.Format("{0:n0}", player.score);
        stageTxt.text = " STAGE " + stage;

        int hour = (int)(playTime / 3600);
        int min = (int)((playTime - hour * 3600) / 60);
        int second = (int)(playTime % 60);
        playTimeTxt.text = string.Format("{0:00}", hour) + ":" + string.Format("{0:00}", min) + ":" + string.Format("{0:00}", second);


        // �÷��̾� UI
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

        // ���� UI
        enemyATxt.text = enemyCntA.ToString();
        enemyBTxt.text = enemyCntB.ToString();
        enemyCTxt.text = enemyCntC.ToString();

        if (boss != null)
        bossHPBar.localScale = new Vector3((float)boss.curHealth / boss.maxHealth, 1, 1);
    }

}
