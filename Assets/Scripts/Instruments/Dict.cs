using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using UnityEditor;
using UnityEngine;

[Serializable]
public class Dict<TKey, TValue> : IDictionary<TKey, TValue>
{
    [Serializable]
    public class Pair<TK, TV>
    {
        [SerializeField] TK _key;
        [SerializeField] TV _value;
        public TK key { get => _key; set => _key = value; }
        public TV value { get => _value; set => _value = value; }

        public static implicit operator (TK, TV)(Pair<TK, TV> pair) => (pair.key, pair.value);
        public override string ToString() => $"[{key}: {value}]";
        public Pair(TK key, TV value) => (_key, _value) = (key, value);
        public Pair() => (_key, _value) = (default, default);
    }

    [SerializeField] List<Pair<TKey, TValue>> pairs = new();

    public ICollection<TKey> Keys => pairs.Select(x => x.key).ToList();
    public ICollection<TValue> Values => pairs.Select(x => x.value).ToList();
    public int Count => pairs.Count;
    public bool IsReadOnly => false;
    public TValue this[TKey key]
    {
        get => pairs.Where(x => x.key.Equals(key)).First().value;
        set
        {
            var x = pairs.Where(x => x.key.Equals(key));
            if (x.Any())
            {
                Pair<TKey, TValue> pair = x.First();
                pair.value = value;
            }
            else Add(key, value);
        }
    }

    public Pair<TKey, TValue> this[int index]
    {
        get => pairs[index];
        set => pairs[index] = value;
    }
    public TKey GetKey(TValue value) => pairs.Where(x => x.value.Equals(value)).First().key;

    public void Add(TKey key, TValue value) => pairs.Add(new Pair<TKey, TValue>(key, value));
    public bool ContainsKey(TKey key) => pairs.FindIndex(x => x.key.Equals(key)) != -1;
    public bool Remove(TKey key) => pairs.Remove(pairs.Find(x => x.key.Equals(key)));

    public bool TryGetValue(TKey key, out TValue value)
    {
        var x = pairs.Where(x => x.key.Equals(key));
        if (x.ToArray().Length > 0)
        {
            Pair<TKey, TValue> pair = x.First();
            value = pair.value;
            return true;
        }

        value = default;
        return false;
    }

    public void Add(KeyValuePair<TKey, TValue> item)
    {
        Add(item.Key, item.Value);
    }

    public void Clear()
    {
        pairs.Clear();
    }

    public bool Contains(KeyValuePair<TKey, TValue> item) => pairs.Contains(new Pair<TKey, TValue>());

    public void CopyTo(KeyValuePair<TKey, TValue>[] array, int arrayIndex)
    {
        KeyValuePair<TKey, TValue>[] def = pairs.Select(x => new KeyValuePair<TKey, TValue>(x.key, x.value)).ToArray();
        for (int i = arrayIndex; i < array.Length; i++)
            array[i] = def[i];
    }

    public bool Remove(KeyValuePair<TKey, TValue> item) => pairs.Remove(pairs.Find(x => x.Equals(new Pair<TKey, TValue>(item.Key, item.Value))));

    public IEnumerator<KeyValuePair<TKey, TValue>> GetEnumerator() => pairs.Select(x => new KeyValuePair<TKey, TValue>(x.key, x.value)).ToList().GetEnumerator();

    IEnumerator IEnumerable.GetEnumerator()
    {
        return GetEnumerator();
    }

    public Dict()
    {
        pairs = new();
    }
    public Dict(Dictionary<TKey, TValue> dictionary)
    {
        pairs = new();
        foreach (var pair in dictionary)
            pairs.Add(new(pair.Key, pair.Value));
    }
    public override string ToString() => string.Join(", ", pairs);

    public static Dict<TKey, TValue> FromJson(string json) => JsonUtility.FromJson<Dict<TKey, TValue>>(json);
    public string ToJson(bool prettyPrint = false) => JsonUtility.ToJson(this, prettyPrint);
}
