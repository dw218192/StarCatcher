using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class Shop : MonoBehaviour
{
    [System.Serializable]
    public struct MenuItem
    {
        public string name;
        public string description;
        public int price;
        public GameObject prefab;
    }

    public Light myLight;
    public Canvas myCanvas;
    public ShopRow itemRowPrefab;
    public MenuItem[] items;
    public TextMeshProUGUI infoText;
    public Transform contentPanel;
    public Transform spawnPoint;
    public AudioClip noMoneyClip;
    AudioSource audioSrc;
    private void Start()
    {
        GenerateShopItems();
        GameMgr.instance.OnGameStateChange += (oldState, newState) =>
        {
            if (newState == GameMgr.GameState.Intermission
                || newState == GameMgr.GameState.Start)
            {
                TurnOn();
            }
            else
            {
                TurnOff();
            }
        };

        infoText.text = $"Your Balance: {GameMgr.instance.Credit} CR";
        audioSrc = gameObject.AddComponent<AudioSource>();
        audioSrc.clip = noMoneyClip;
        audioSrc.playOnAwake = false;
        audioSrc.loop = false;
        audioSrc.volume = 1.0f;
    }

    private void GenerateShopItems()
    {
        foreach (var item in items)
        {
            // Instantiate a new row for each item
            var newRow = Instantiate(itemRowPrefab, contentPanel);

            // Assuming your prefab has Text or TextMeshPro components named appropriately
            newRow.Name = item.name;
            newRow.Desc = item.description;
            newRow.BuyBtnText = $"BUY ({item.price} CR)";

            // Set up the button event
            newRow.BuyBtn.onClick.AddListener(() => Buy(item));
        }
    }

    public void TurnOn()
    {
        infoText.text = $"Your Balance: {GameMgr.instance.Credit} CR";
        myLight.gameObject.SetActive(true);
        myCanvas.gameObject.SetActive(true);
    }

    public void TurnOff()
    {
        myLight.gameObject.SetActive(false);
        myCanvas.gameObject.SetActive(false);
    }
    public void Buy(MenuItem item)
    {
        if (GameMgr.instance.Credit < item.price)
        {
            Debug.Log("No Money!!");
            if (!audioSrc.isPlaying)
            {
                audioSrc.Play();
            }
        }
        else
        {
            GameMgr.instance.Credit -= item.price;
            var thing = Instantiate(item.prefab);
            thing.transform.position = spawnPoint.transform.position;

            TurnOff();
        }
    }
}
