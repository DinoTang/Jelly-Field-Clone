using System.Collections.Generic;
using UnityEngine;

public abstract class Spawner<T> : BaseBehaviour where T : PoolObj
{
    [Header("Spawner")]
    [SerializeField] protected int spawnCount = 0;
    [SerializeField] protected PoolHolder poolHolder;
    [SerializeField] protected Transform prefabRoot;
    [SerializeField] protected List<T> poolObjsList = new();
    [SerializeField] protected List<T> prefabs = new();

    protected override void LoadComponent()
    {
        base.LoadComponent();
        this.LoadPoolHolder();
        this.LoadPrefabRoot();
        this.LoadPrefabs();
    }

    private void LoadPoolHolder()
    {
        if (this.poolHolder != null) return;
        this.poolHolder = GetComponentInChildren<PoolHolder>();
        Debug.Log($"{this.transform.name}: LoadPoolHolder", this.gameObject);
    }

    private void LoadPrefabRoot()
    {
        if (this.prefabRoot != null) return;
        this.prefabRoot = this.transform.Find("Prefabs");
        Debug.Log($"{this.transform.name}: LoadPrefabRoot", this.gameObject);
    }

    private void LoadPrefabs()
    {
        this.LoadPrefabRoot();
        if (this.prefabRoot == null)
        {
            Debug.LogWarning($"{this.transform.name}: Prefabs root not found.", this.gameObject);
            return;
        }

        if (this.prefabs.Count > 0) return;

        foreach (Transform child in this.prefabRoot)
        {
            T prefab = child.GetComponent<T>();
            if (prefab == null) continue;

            this.prefabs.Add(prefab);
            prefab.gameObject.SetActive(false);
        }

        Debug.Log($"{this.transform.name}: LoadPrefabs", this.gameObject);
    }

    public virtual T Spawn(string prefabName, Vector3 position)
    {
        T prefab = this.GetPrefabByName(prefabName);
        if (prefab == null)
        {
            Debug.LogWarning($"{this.transform.name}: Prefab not found: {prefabName}", this.gameObject);
            return null;
        }

        return this.Spawn(prefab, position);
    }

    public virtual T Spawn(T prefab, Vector3 position)
    {
        return this.Spawn(prefab, position, Quaternion.identity);
    }

    public virtual T Spawn(T prefab, Vector3 position, Quaternion rotation)
    {
        T newObject = this.Spawn(prefab);
        if (newObject == null) return null;

        newObject.transform.SetPositionAndRotation(position, rotation);

        if (this.poolHolder != null)
            newObject.transform.SetParent(this.poolHolder.transform, true);

        newObject.gameObject.SetActive(true);

        return newObject;
    }

    public virtual T Spawn(T prefab)
    {
        if (prefab == null) return null;

        T newObject = this.GetObjFromPool(prefab);

        if (newObject == null)
        {
            this.spawnCount++;
            newObject = Instantiate(prefab);
            this.UpdateName(prefab.transform, newObject.transform);
        }

        return newObject;
    }

    protected virtual T GetPrefabByName(string prefabName)
    {
        foreach (T prefab in this.prefabs)
        {
            if (prefab == null) continue;
            if (prefab.GetName() != prefabName) continue;

            return prefab;
        }

        return null;
    }

    public virtual void Despawn(T obj)
    {
        if (obj == null) return;

        if (obj is MonoBehaviour monoBehaviour)
        {
            monoBehaviour.gameObject.SetActive(false);
            this.AddObjIntoPool(obj);
        }
    }

    private void AddObjIntoPool(T obj)
    {
        if (obj == null) return;
        if (this.poolObjsList.Contains(obj)) return;

        this.poolObjsList.Add(obj);
    }

    private void RemoveObjIntoPool(T obj)
    {
        if (obj == null) return;
        this.poolObjsList.Remove(obj);
    }

    protected virtual void UpdateName(Transform prefab, Transform newObject)
    {
        newObject.name = $"{prefab.name}_{this.spawnCount}";
    }

    private T GetObjFromPool(T prefab)
    {
        if (prefab == null) return null;

        foreach (T poolObj in this.poolObjsList)
        {
            if (poolObj == null) continue;
            if (poolObj.GetName() != prefab.GetName()) continue;

            this.RemoveObjIntoPool(poolObj);
            return poolObj;
        }

        return null;
    }
}