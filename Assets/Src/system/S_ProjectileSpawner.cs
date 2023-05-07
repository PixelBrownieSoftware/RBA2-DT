using System.Collections;
using System.Collections.Generic;
using UnityEngine.Pool;
using UnityEngine;

public class S_ProjectileSpawner : MonoBehaviour
{
    private IObjectPool<O_ProjectileAnim> m_projectilePool;
    private const int maxPoolSize = 10;
    public O_ProjectileAnim projectilePrefab;
    [SerializeField]
    private R_Text projectileType;
    [SerializeField]
    private R_Vector2 position;

    public IObjectPool<O_ProjectileAnim> projectilePool
    {
        get
        {
            if (m_projectilePool == null)
            {
                m_projectilePool = new LinkedPool<O_ProjectileAnim>(CreateProjectile, OnTakePool, OnReturnPool, DestroyPoolObject, true, maxPoolSize);
            }
            return m_projectilePool;
        }
    }

    public O_ProjectileAnim CreateProjectile()
    {
        O_ProjectileAnim obj = Instantiate(projectilePrefab, transform.position, Quaternion.identity);
        obj.pool = projectilePool;
        obj.transform.SetParent(transform);
        return obj;
    }
    public void OnTakePool(O_ProjectileAnim obj)
    {
        obj.gameObject.SetActive(true);
        obj.transform.position = position.vector2;
        obj.anim.Play(projectileType.text);
    }
    public void OnReturnPool(O_ProjectileAnim obj)
    {
        obj.gameObject.SetActive(false);
    }
    public void DestroyPoolObject(O_ProjectileAnim obj)
    {
        Destroy(obj);
    }
}
