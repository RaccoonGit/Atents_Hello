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
        print("플레이어 사망" + curHP.ToString());
        #region 플레이어가 죽었다고 For문을 돌려서 알려주는 방식
        // 몬스터의 수가 많아질수록 처리 속도가 점점 느려짐
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
        #region 이벤트를 사용하여 알려주는 방식
        // 어떤 클래스에서도 호출을 할 수 있다.
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
