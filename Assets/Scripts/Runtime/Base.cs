using UnityEngine;

public class Base : MonoBehaviour
{
    [SerializeField] private GameManager _gameManager;
    [SerializeField] private int _health;
    [SerializeField] private HealthUI _healthUI;

    private void Awake()
    {
        UpdateHealthSafe();
    }

    private void UpdateHealthSafe()
    {
        if (_healthUI)
        {
            _healthUI.UpdateHealth(_health);
        }
    }

    private void OnTriggerEnter(Collider collider)
    {
        var enemy = Enemy.GetFromCollider(collider);
        if (enemy)
        {
            enemy.Kill();
            if (--_health == 0)
            {
                _gameManager.FailLevel();
            }
            UpdateHealthSafe();
        }
    }
}
