using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class StartGame : MonoBehaviour
{
    public Button startGameButton;
    public event System.Action OnClick;

    void OnTriggerEnter(Collider other)
    {
        Debug.LogWarning($"StartGame.OnTriggerEnter {other.gameObject.name}");

        if (other.CompareTag(Consts.TAG_SWORD))
        {
            // make button selected
            var colors = startGameButton.colors;
            colors.normalColor = Color.red;
            startGameButton.colors = colors;
            StartCoroutine(ClickedRoutine());
        }
    }

    IEnumerator ClickedRoutine()
    {
        yield return new WaitForSeconds(1.5f);
        OnClick?.Invoke();
        var colors = startGameButton.colors;
        colors.normalColor = Color.white;
        startGameButton.colors = colors;
    }
}
