using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIManagement : MonoBehaviour
{
    public GameObject topMotionPanel;
    public GameObject bottomMotionPanel;
    public GameObject seedBank;
    public Text levelNameText;

    public GameObject cardGroup;   //����Ⱥ��

    // Start is called before the first frame update
    public void initUI()
    {
        //���عؿ�����
        levelNameText.text = GameManagement.levelData.levelName;

        //���ؿ���Ⱥ�飬���������UI�Ĵ�Сλ��
        List<string> plantCards = GameManagement.levelData.plantCards;
        List<Card> cards = new List<Card>();
        foreach (string plant in plantCards)
        {
            cards.Add((
                    Instantiate(
                        Resources.Load<Object>("Prefabs/UI/Card/" + plant + "Card"),
                        cardGroup.transform
                    ) as GameObject
                ).GetComponent<Card>());
        }
        GameObject.Find("Sun Text").GetComponent<SunNumber>().setCardGroup(cards);
        float cardGroupWidth = plantCards.Count * 43 - 1;
        cardGroup.GetComponent<RectTransform>()
            .SetSizeWithCurrentAnchors(RectTransform.Axis.Horizontal, cardGroupWidth);
        seedBank.GetComponent<RectTransform>()
            .SetSizeWithCurrentAnchors(RectTransform.Axis.Horizontal, cardGroupWidth + 78);

    }

    public void appear()
    {
        //����Ⱥ�鱾Ϊ����Ծ���Ա�������ڼ俨����ȴ����
        cardGroup.SetActive(true);

        topMotionPanel.GetComponent<MotionPanel>().startMove();
        bottomMotionPanel.GetComponent<MotionPanel>().startMove();
    }
}
