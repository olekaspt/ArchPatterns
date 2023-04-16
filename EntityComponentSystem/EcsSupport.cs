using System;
using System.Collections;
using System.Collections.Generic;
using Entity = System.Int32;

// This code was inspired from https://gist.github.com/prime31/99c66a4aeb4fc0e75173d5ea80f75a97

public class Registry
{
    readonly int maxEntities;
    Dictionary<Type, IComponentStore> data = new Dictionary<Type, IComponentStore>();
    Entity nextEntity = 0;

    public Registry(int maxEntities) => this.maxEntities = maxEntities;

    public ComponentStore<T> Assure<T>()
    {
        var type = typeof(T);
        if (data.TryGetValue(type, out var store)) return (ComponentStore<T>)data[type];

        var newStore = new ComponentStore<T>(maxEntities);
        data[type] = newStore;
        return newStore;
    }

    public Entity Create() => nextEntity++;

    public void Destroy(Entity entity)
    {
        foreach (var store in data.Values)
            store.RemoveIfContains(entity);
    }

    public void AddComponent<T>(Entity entity, T component) => Assure<T>().Add(entity, component);

    public ref T GetComponent<T>(Entity entity) => ref Assure<T>().Get(entity);

    public bool TryGetComponent<T>(Entity entity, ref T component)
    {
        var store = Assure<T>();
        if (store.Contains(entity))
        {
            component = store.Get(entity);
            return true;
        }

        return false;
    }

    public void RemoveComponent<T>(Entity entity) => Assure<T>().RemoveIfContains(entity);

    public View<T> View<T>() => new View<T>(this);

    public View<T, U> View<T, U>() => new View<T, U>(this);

    public View<T, U, V> View<T, U, V>() => new View<T, U, V>(this);
}


public struct View<T> : IEnumerable<Entity>
{
    Registry registry;

    public View(Registry registry) => this.registry = registry;

    public IEnumerator<Entity> GetEnumerator() => registry.Assure<T>().Set.GetEnumerator();

    IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();
}

public struct View<T, U> : IEnumerable<Entity>
{
    Registry registry;

    public View(Registry registry) => this.registry = registry;

    public IEnumerator<Entity> GetEnumerator()
    {
        var store2 = registry.Assure<U>();
        foreach (var entity in registry.Assure<T>().Set)
        {
            if (!store2.Contains(entity)) continue;
            yield return entity;
        }
    }

    IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();
}

public struct View<T, U, V> : IEnumerable<Entity>
{
    Registry registry;

    public View(Registry registry) => this.registry = registry;

    public IEnumerator<Entity> GetEnumerator()
    {
        var store2 = registry.Assure<U>();
        var store3 = registry.Assure<V>();
        foreach (var entity in registry.Assure<T>().Set)
        {
            if (!store2.Contains(entity) || !store3.Contains(entity)) continue;
            yield return entity;
        }
    }

    IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();
}


public class SparseSet : IEnumerable<int>
{
    readonly int max;
    int size;
    int[] dense;
    int[] sparse;

    public int Count => size;

    public SparseSet(int maxValue)
    {
        max = maxValue + 1;
        size = 0;
        dense = new int[max];
        sparse = new int[max];
    }

    public void Add(int value)
    {
        if (value >= 0 && value < max && !Contains(value))
        {
            dense[size] = value;
            sparse[value] = size;
            size++;
        }
    }

    public void Remove(int value)
    {
        if (Contains(value))
        {
            dense[sparse[value]] = dense[size - 1];
            sparse[dense[size - 1]] = sparse[value];
            size--;
        }
    }

    public int Index(int value) => sparse[value];

    public bool Contains(int value)
    {
        if (value >= max || value < 0)
            return false;
        else
            return sparse[value] < size && dense[sparse[value]] == value;
    }

    public void Clear() => size = 0;

    public IEnumerator<int> GetEnumerator()
    {
        var i = 0;
        while (i < size)
        {
            yield return dense[i];
            i++;
        }
    }

    IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();

    public override bool Equals(object obj) => throw new Exception("Why are you comparing SparseSets?");

    public override int GetHashCode() => System.HashCode.Combine(max, size, dense, sparse, Count);
}



public interface IComponentStore
{
    void RemoveIfContains(int entityId);
}

public class ComponentStore<T> : IComponentStore
{
    public SparseSet Set;
    T[] instances;

    public int Count => Set.Count;

    public ComponentStore(int maxComponents)
    {
        Set = new SparseSet(maxComponents);
        instances = new T[maxComponents];
    }

    public void Add(int entityId, T value)
    {
        Set.Add(entityId);
        instances[Set.Index(entityId)] = value;
    }

    public ref T Get(int entityId) => ref instances[Set.Index(entityId)];

    public bool Contains(int entityId) => Set.Contains(entityId);

    public void RemoveIfContains(int entityId)
    {
        if (Contains(entityId)) Remove(entityId);
    }

    void Remove(int entityId) => Set.Remove(entityId);
}