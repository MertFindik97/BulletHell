using UnityEngine;
using System.Collections.Generic;

public class BulletPool : MonoBehaviour
{
    public static BulletPool Instance { get; private set; }
    Dictionary<Bullet, List<Bullet>> pools = new();

    void Awake()
    {
        if (Instance != null && Instance != this) { Destroy(gameObject); return; }
        Instance = this;
        //DontDestroyOnLoad(gameObject);
    }

    public Bullet Get(Bullet prefab)
    {
        if (!pools.TryGetValue(prefab, out var list))
        {
            list = new List<Bullet>();
            pools[prefab] = list;
        }

        // nach inaktiven Bullet suchen
        for (int i = 0; i < list.Count; i++)
        {
            if (!list[i].gameObject.activeInHierarchy)
            {
                list[i].gameObject.SetActive(true); // ✅ aktivieren
                return list[i];
            }
        }

        // wenn keiner frei ist -> neuen erstellen
        var b = Instantiate(prefab);
        list.Add(b);
        b.gameObject.SetActive(true); // ✅ direkt aktiv
        return b;
    }
}
