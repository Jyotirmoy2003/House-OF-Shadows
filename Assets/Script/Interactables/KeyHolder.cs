using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KeyHolder : MonoBehaviour
{
    private List<Key.KeyType> keyList;

    void Awake()
    {
        keyList=new List<Key.KeyType>();
    }

    public void AddKey(Key.KeyType keyType)
    {
        keyList.Add(keyType);
    }

    public void RemoveKey(Key.KeyType keyType)
    {
        keyList.Remove(keyType);
    }

    public bool ContainsKey(Key.KeyType keyType)
    {
        return keyList.Contains(keyType);
    }
    public List<Key.KeyType> GetKeyList()
    {
        return keyList;
    }
}
