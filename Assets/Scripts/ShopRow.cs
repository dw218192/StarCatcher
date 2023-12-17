using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ShopRow : MonoBehaviour
{
    public TextMeshPro nameText;
    public TextMeshPro descText;
    public TextMeshProUGUI buyText;
    
    public string Name { set => nameText.text = value; }
    public string Desc { set => descText.text = value; }
    public string BuyBtnText { set => buyText.text = value; }
    public Button BuyBtn;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
