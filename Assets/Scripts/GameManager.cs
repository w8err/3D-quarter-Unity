using UnityEngine;
using UnityEngine.UI;
using UnityEngine.UIElements;
using TMPro;

public class GameManager : MonoBehaviour
{
    // �ΰ��� �Ŵ���
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

        bossHPBar.localScale = new Vector3((float)boss.curHealth / boss.maxHealth, 1, 1);
    }

}
