using UnityEngine;
using UnityEngine.UI;

public class GameStartButtonUI : MonoBehaviour
{
    [SerializeField] private LevelManager _levelManager;
    [SerializeField] private Button _button;

    private void Awake()
    {
        _button.onClick.AddListener(OnButtonClick);
    }

    private void OnButtonClick()
    {
        _levelManager.StartLevel();
        gameObject.SetActive(false);
    }
}