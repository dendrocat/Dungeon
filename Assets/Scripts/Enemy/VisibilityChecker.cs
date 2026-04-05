using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class VisibilityChecker : MonoBehaviour
{
    Dictionary<Enemy, bool> m_TempVisibility = new();

    void Awake()
    {
        Person.Died += OnDied;
    }

    void OnDied(Person p)
    {
        if (p is not Enemy e) return;
        m_TempVisibility.Remove(e);
    }

    bool _IsPlayerVisibleFrom(Enemy enemy)
    {
        if (!enemy.gameObject.activeSelf) return false;
        var rays = enemy.MLAgent.RaySensor?.RayPerceptionOutput?.RayOutputs;
        if (rays == null) return false;
        foreach (var hit in rays)
        {
            if (hit.HitTaggedObject)
            {
                var dist = (hit.HitGameObject.transform.position - hit.StartPositionWorld).magnitude;
                // DomainDebug.Log($"Check ray out: {hit.HitTaggedObject} {dist / hit.ScaledRayLength}", DomainType.Director);
                if (Director.Instance.PlayerLighted || dist / hit.ScaledRayLength <= enemy.Config.Detection.NightVisionLevel)
                    return true;
            }
        }
        return false;
    }

    public bool IsPlayerVisibleFrom(Enemy enemy, bool useCache = true)
    {
        if (useCache) return m_TempVisibility.GetValueOrDefault(enemy, false);
        else return m_TempVisibility[enemy] = _IsPlayerVisibleFrom(enemy);
    }

    public bool IsPlayerVisible(IReadOnlyCollection<Enemy> enemies)
    {
        return enemies.Any(e => IsPlayerVisibleFrom(e, false));
    }

}
