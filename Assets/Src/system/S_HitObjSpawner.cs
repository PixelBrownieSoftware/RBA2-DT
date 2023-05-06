using System.Collections;
using System.Collections.Generic;
using UnityEngine.Pool;
using UnityEngine;

public class S_HitObjSpawner : MonoBehaviour
{
    private IObjectPool<s_hitObj> m_hitObjectPool;
    private const int maxPoolSize = 40;
    public s_hitObj hitObjectPrefab;

    [SerializeField]
    private R_Int damageNumber;
    [SerializeField]
    private R_Text hitType;
    [SerializeField]
    private R_Vector2 position;
    [SerializeField]
    private R_Colour colour;

    //SpawnDamageObject(dmg, characterPos, true, new Color(118/255,255 / 255, 210 / 255), "lucky");

    public IObjectPool<s_hitObj> hitObjectPool
    {
        get
        {
            if (m_hitObjectPool == null)
            {
                m_hitObjectPool = new LinkedPool<s_hitObj>(CreateHitObject, OnTakePool, OnReturnPool, DestroyPoolObject, true, maxPoolSize);
            }
            return m_hitObjectPool;
        }
    }

    public s_hitObj CreateHitObject()
    {
        s_hitObj obj = Instantiate(hitObjectPrefab, position.vector2, Quaternion.identity);
        obj.pool = hitObjectPool;
        obj.transform.SetParent(transform);
        return obj;
    }
    public void OnTakePool(s_hitObj obj) {
        obj.gameObject.SetActive(true);
        obj.transform.position = position.vector2;
        obj.PlayAnim(damageNumber.integer, hitType.text, colour.colour);
    }
    public void OnReturnPool(s_hitObj obj)
    {
        obj.gameObject.SetActive(false);
    }
    public void DestroyPoolObject(s_hitObj obj) {
        Destroy(obj);
    }
}
