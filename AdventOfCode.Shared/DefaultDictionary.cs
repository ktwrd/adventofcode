/*
 * COPIED FROM: https://github.com/tmbarker/puzzle-utils/blob/main/Utilities/Collections/DisjointSet.cs
 * ORIGINAL AUTHOR: Trevor Barker
 * COPIED AT: 2025/12/08
 * 
 * MIT License
 * 
 * Copyright (c) 2024 Trevor Barker
 * 
 * Permission is hereby granted, free of charge, to any person obtaining a copy
 * of this software and associated documentation files (the "Software"), to deal
 * in the Software without restriction, including without limitation the rights
 * to use, copy, modify, merge, publish, distribute, sublicense, and/or sell
 * copies of the Software, and to permit persons to whom the Software is
 * furnished to do so, subject to the following conditions:
 * 
 * The above copyright notice and this permission notice shall be included in all
 * copies or substantial portions of the Software.
 * 
 * THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR
 * IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
 * FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE
 * AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER
 * LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM,
 * OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN THE
 * SOFTWARE.
 */

using System.Collections;
using JetBrains.Annotations;

namespace AdventOfCode;

public class DefaultDictionary<TKey, TValue>(Func<TKey, TValue> defaultSelector, int? defaultCapacity = null)
    : IDictionary<TKey, TValue> where TKey : notnull
{
    private readonly Dictionary<TKey, TValue> _dictionary = new(defaultCapacity.GetValueOrDefault(5));

    public bool IsReadOnly => false;
    public int Count => _dictionary.Count;
    public ICollection<TKey> Keys => _dictionary.Keys;
    public ICollection<TValue> Values => _dictionary.Values;

    [CollectionAccess(CollectionAccessType.UpdatedContent)]
    public TValue this[TKey key]
    {
        get => IndexGetInternal(key);
        set => IndexSetInternal(key, value);
    }

    public DefaultDictionary(TValue defaultValue, int? defaultCapacity = null) : this(defaultSelector: _ => defaultValue, defaultCapacity: defaultCapacity)
    {
    }

    public DefaultDictionary(TValue defaultValue, IEnumerable<(TKey Key, TValue Value)> items) : this(defaultValue)
    {
        foreach (var item in items)
        {
            Add(key: item.Key, value: item.Value);
        }
    }
    
    public DefaultDictionary(TValue defaultValue, IEnumerable<KeyValuePair<TKey, TValue>> items) : this(defaultValue)
    {
        foreach (var item in items)
        {
            Add(key: item.Key, value: item.Value);
        }
    }

    public void Add(TKey key, TValue value)
    {
        _dictionary.Add(key, value);
    }

    public void Add(KeyValuePair<TKey, TValue> item)
    {
        _dictionary[item.Key] = item.Value;
    }

    public bool Remove(TKey key)
    {
        return _dictionary.Remove(key);
    }

    public bool Remove(KeyValuePair<TKey, TValue> item)
    {
        return _dictionary.Remove(item.Key);
    }

    public bool ContainsKey(TKey key)
    {
        return _dictionary.ContainsKey(key);
    }

    public bool Contains(KeyValuePair<TKey, TValue> item)
    {
        return _dictionary.TryGetValue(item.Key, out var value) && Equals(value, item.Value);
    }

    public bool TryGetValue(TKey key, out TValue value)
    {
        value = this[key];
        return true;
    }

    public void Clear()
    {
        _dictionary.Clear();
    }

    public void CopyTo(KeyValuePair<TKey, TValue>[] array, int arrayIndex)
    {
        ((ICollection<KeyValuePair<TKey, TValue>>)_dictionary).CopyTo(array, arrayIndex);
    }

    public IEnumerator<KeyValuePair<TKey, TValue>> GetEnumerator()
    {
        return _dictionary.GetEnumerator();
    }

    private void IndexSetInternal(TKey key, TValue value)
    {
        _dictionary[key] = value;
    }

    private TValue IndexGetInternal(TKey key)
    {
        if (_dictionary.TryGetValue(key, out var value))
        {
            return value;
        }

        _dictionary[key] = defaultSelector.Invoke(key);
        return _dictionary[key];
    }

    IEnumerator IEnumerable.GetEnumerator()
    {
        return ((IEnumerable)_dictionary).GetEnumerator();
    }
}