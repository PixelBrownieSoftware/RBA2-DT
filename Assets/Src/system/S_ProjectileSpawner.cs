using System.Collections;
using System.Collections.Generic;
using UnityEngine.Pool;
using UnityEngine;

public class S_ProjectileSpawner : MonoBehaviour
{
    private IObjectPool<s_moveanim> m_projectilePool;
    private const int maxPoolSize = 10;
    public s_moveanim projectilePrefab;
    [SerializeField]
    private R_Text projectileType;
    [SerializeField]
    private R_Vector2 position;

    public IObjectPool<s_moveanim> projectilePool
    {
        get
        {
            if (m_projectilePool == null)
            {
                m_projectilePool = new LinkedPool<s_moveanim>(CreateProjectile, OnTakePool, OnReturnPool, DestroyPoolObject, true, maxPoolSize);
            }
            return m_projectilePool;
        }
    }

    public s_moveanim CreateProjectile()
    {
        s_moveanim obj = Instantiate(projectilePrefab, transform.position, Quaternion.identity);
        obj.pool = projectilePool;
        obj.transform.SetParent(transform);
        return obj;
    }
    public void OnTakePool(s_moveanim obj)
    {
        obj.gameObject.SetActive(true);
        obj.transform.position = position.vector2;
        obj.anim.Play(projectileType.text);
    }
    public void OnReturnPool(s_moveanim obj)
    {
        obj.gameObject.SetActive(false);
    }
    public void DestroyPoolObject(s_moveanim obj)
    {
        Destroy(obj);
    }
}
