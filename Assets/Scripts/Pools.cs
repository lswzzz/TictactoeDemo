using System.Collections.Generic;
using UnityEngine;

public class Pools<T>
{
    private static Pools<T> instance;

    public static Pools<T> Instance
    {
        get
        {
            if (instance == null)
            {
                instance = new Pools<T>();
            }
            return instance;
        }
    }
    
    private List<T> list = new List<T>();

    public T Get()
    {
        if (list.Count <= 0) return default(T);
        var t = list[list.Count - 1];
        list.RemoveAt(list.Count-1);
        return t;
    }

    public void Add(T t)
    {
        list.Add(t);
    }
}