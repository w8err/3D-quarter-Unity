using UnityEngine;
using UnityEngine.UI;
using UnityEngine.UIElements;
using TMPro;

public class GameManager : MonoBehaviour
{
    // 인게임 매니저
    public GameObject menuCam;
    public GameObject gameCam;
    public Player player;
    public Boss boss;
    public int stage;
    public float playTime;
    public bool isBattle;
    public int enemyCntA;
    public int enemyCntB;
    public int enemyCntC;

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

        bossHPBar.localScale = new Vector3((float)boss.curHealth / boss.maxHealth, 1, 1);
    }

}
