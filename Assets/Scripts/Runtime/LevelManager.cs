using System.Collections;
using System.Linq.Expressions;
using UnityEngine;

public class LevelManager : MonoBehaviour
{
    [SerializeField] private Round[] _rounds;

    private int _roundIndex;

    public void StartLevel()
    {
        StartCoroutine(PlayRounds());
    }

    public int GetTotalEnemyCount()
    {
        int sum = 0;
        foreach (var round in _rounds)
        {
            foreach (var wave in round.Waves)
            {
                sum += wave.Count;
            }
        }

        return sum;
    }

    private IEnumerator PlayRounds()
    {
        for (int i = 0; i < _rounds.Length; i++)
        {
            Debug.Log($"Playing round {i + 1}.");
            PlayRound(_rounds[i]);
            yield return new WaitForSeconds(_rounds[i].Duration);
        }
    }

    private void PlayRound(Round round)
    {
        foreach (var wave in round.Waves)
        {
            if (wave.Delay > 0)
            {
                StartCoroutine(SpawnWaveWithDelay(wave));
            }
            else
            {
                wave.EnemySpawner.Spawn(wave.EnemyData, wave.Count);
            }
        }
    }
    
    private IEnumerator SpawnWaveWithDelay(Wave wave)
    {
        yield return new WaitForSeconds(wave.Delay);
        wave.EnemySpawner.Spawn(wave.EnemyData, wave.Count);
    }

    private void OnDestroy()
    {
        StopAllCoroutines();
    }
}