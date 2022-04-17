using UnityEngine;
using UnityEngine.SceneManagement;

public class Base : MonoBehaviour
{
    [SerializeField] private int _health;

    private void OnTriggerEnter(Collider collider)
    {
        var enemy = Enemy.GetFromCollider(collider);
        if (enemy)
        {
            enemy.Kill();
            if (--_health == 0)
            {
                SceneManager.LoadScene(1);
            }
        }
    }
}
