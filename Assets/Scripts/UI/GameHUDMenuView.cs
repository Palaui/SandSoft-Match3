using System;
using UnityEngine;
using UnityEngine.UI;

public class GameHUDMenuView : MonoBehaviour
{
    [SerializeField] private Button regenerate;

    public EventHandler RegeneratePressed;


    public void OnRegeneratePressed()
    {
        RegeneratePressed?.Invoke(this, EventArgs.Empty);
    }


    private void Awake()
    {
        regenerate.onClick.AddListener(OnRegeneratePressed);
    }

    private void OnDestroy()
    {
        regenerate.onClick.RemoveAllListeners();
    }
}
