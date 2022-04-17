using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class GameSceneLoader : MonoBehaviour
{
    [SerializeField] private Button _button;

    private void Awake()
    {
        _button.onClick.AddListener(OnButtonClicked);
    }

    private void OnButtonClicked()
    {
        SceneManager.LoadScene(0);
    }
}
