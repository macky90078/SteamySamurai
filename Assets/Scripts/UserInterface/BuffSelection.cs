using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Rewired;
using UnityEngine.UI;

public class BuffSelection : MonoBehaviour {

    private enum buffType { attackBuff, moveBuff, healthBuff }
    [SerializeField] private buffType buff;
    [SerializeField] private Text buttonText;
    [SerializeField] private Text scrapText;
    [SerializeField] private float moveSpeedInc = 10;
    [SerializeField] private float attkSpeedInc = 10;
    [SerializeField] private int attackBuffCost = 1;
    [SerializeField] private int moveBuffCost = 1;
    [SerializeField] private int healthBuffCost = 1;

    private CanvasRef canScript;
    private PlayerController playerOne;
    private PlayerController playerTwo;
    

	// Use this for initialization
	void Start () {
        playerOne = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerController>();
        playerTwo = GameObject.FindGameObjectWithTag("Player2").GetComponent<PlayerController>();
        canScript = GameObject.FindGameObjectWithTag("CanvasContainer").GetComponent<CanvasRef>();
        SetText();
    }

    public void attackBuff()
    {
        GameManager.reference.ButtonVibrate();
        if(GameManager.reference.scrapMetal >= attackBuffCost && GameManager.reference.hasAttackBuff == false)
        {
            Destroy(GameObject.FindGameObjectWithTag("AttackBuffButton"));
            playerOne.buffAttack(attkSpeedInc);
            playerTwo.buffAttack(attkSpeedInc);
            canScript.buffCanvas.SetActive(false);
            GameManager.reference.scrapMetal -= attackBuffCost;
            GameManager.reference.hasAttackBuff = true;
            GameManager.reference.StartWave();
        }
    }

    public void healthBuff()
    {
        GameManager.reference.ButtonVibrate();
        if (GameManager.reference.scrapMetal >= healthBuffCost)
        {
            playerOne.buffHealth();
            playerTwo.buffHealth();
            canScript.buffCanvas.SetActive(false);
            GameManager.reference.scrapMetal -= healthBuffCost;
            GameManager.reference.StartWave();
        }
    }

    public void moveBuff()
    {
        GameManager.reference.ButtonVibrate();
        if (GameManager.reference.scrapMetal >= moveBuffCost && GameManager.reference.hasMoveBuff == false)
        {
            Destroy(GameObject.FindGameObjectWithTag("MovementBuffButton"));
            playerOne.buffSpeed(moveSpeedInc);
            playerTwo.buffSpeed(moveSpeedInc);
            canScript.buffCanvas.SetActive(false);
            GameManager.reference.scrapMetal -= moveBuffCost;
            GameManager.reference.hasMoveBuff = true;
            GameManager.reference.StartWave();
        }
    }

    void SetText()
    {
        switch (buff)
        {
            case buffType.attackBuff:
                buttonText.text = buttonText.text + "Cost: " + attackBuffCost;
                break;
            case buffType.moveBuff:
                buttonText.text = buttonText.text + "Cost: " + moveBuffCost;
                break;
            case buffType.healthBuff:
                buttonText.text = buttonText.text + "Cost: " + healthBuffCost;
                break;
        }
    }
}
