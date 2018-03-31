using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UpdateBuffText : MonoBehaviour {

    [SerializeField] private Text scrapText;
	
	// Update is called once per frame
	void Update () {
        scrapText.text = GameManager.reference.scrapMetal + "\nScrap\nMetal";
	}
}
