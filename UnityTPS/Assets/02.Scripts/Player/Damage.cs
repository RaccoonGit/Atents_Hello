using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Damage : MonoBehaviour
{
    private string e_bulletTag = "E_BULLET";

    private float initHP = 100.0f;
    [SerializeField]
    public float curHP;

    [SerializeField]
    private Image bloodScreen;
    [SerializeField]
    private Image hpBar;
    private Text hpText;

    public delegate void PlayerDieHandler();
    public static event PlayerDieHandler OnPlayerDie;

    private void Start()
    {
        initHP = GameManager.inst.gameData.hp;
        curHP = initHP;
        bloodScreen = GameObject.Find("Image-BloodScreen").transform.GetComponent<Image>();
        hpBar = GameObject.Find("Image-HPBar").transform.GetComponent<Image>();
        hpBar.color = Color.green;
        hpText = GameObject.Find("Text-HPText").transform.GetComponent<Text>();
        hpText.text = curHP.ToString() + " / " + initHP.ToString();
    }

    private void OnCollisionEnter(Collision col)
    {
        if(col.gameObject.tag == e_bulletTag)
        {
            curHP -= 5.0f;
            hpBar.fillAmount = curHP / initHP;
            if(hpBar.fillAmount <= 0.3f)
                hpBar.color = Color.red;
            else if(hpBar.fillAmount <= 0.5f)
                hpBar.color = Color.yellow;

            hpText.text = curHP.ToString() + " / " + initHP.ToString();
            if (curHP <= 0.0f)
            {
                curHP = 0.0f;
                PlayerDie();
            }
            Destroy(col.gameObject);
        }
        StartCoroutine(ShowBloodScreen());
    }

    private void PlayerDie()
    {
        print("�÷��̾� ���" + curHP.ToString());
        #region �÷��̾ �׾��ٰ� For���� ������ �˷��ִ� ���
        // ������ ���� ���������� ó�� �ӵ��� ���� ������
        //GameObject[] enemies = GameObject.FindGameObjectsWithTag("ENEMY");
        //GameObject[] swatEnemies = GameObject.FindGameObjectsWithTag("S_ENEMY");
        //for (int i = 0; i < enemies.Length; i++)
        //{
        //    enemies[i].SendMessage("OnPlayerDie", SendMessageOptions.DontRequireReceiver);
        //}
        //for (int i = 0; i < swatEnemies.Length; i++)
        //{
        //    swatEnemies[i].SendMessage("OnPlayerDie", SendMessageOptions.DontRequireReceiver);
        //}
        #endregion
        #region �̺�Ʈ�� ����Ͽ� �˷��ִ� ���
        // � Ŭ���������� ȣ���� �� �� �ִ�.
        OnPlayerDie();
        #endregion
        GameManager.inst.isGameOver = true;
    }

    private IEnumerator ShowBloodScreen()
    {
        bloodScreen.color = new Color(1, 0, 0, Random.Range(0.3f, 0.4f));
        yield return new WaitForSeconds(0.1f);
        bloodScreen.color = Color.clear;
    }
}
