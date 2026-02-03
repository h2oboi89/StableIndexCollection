using System.Collections;

namespace StableIndexCollection;

public class StableIndexCollection<T> : IEnumerable<T>, ICollection<T>
{
    private readonly List<T> _data = [];
    private readonly List<Metadata> _metaData = [];
    private readonly List<ID> _indices = [];
    private const int INVALID = -1;

    public StableIndexCollection() { }

    public StableIndexCollection(IEnumerable<T> items)
    {
        foreach(var item in items)
        {
            Add(item);
        }
    }

    public ID Add(T item)
    {
        var id = GetFreeSlot();
        _data.Add(item);
        return id;
    }

    public bool Remove(ID id)
    {
        if (!IsValidID(id))
        {
            return false;
        }

        var dataId = _indices[id];
        var lastDataId = _data.Count - 1;
        var lastId = _metaData[lastDataId].ReverseId;

        _metaData[dataId].ValidityId++;

        Swap(_data, dataId, lastDataId);
        Swap(_metaData, dataId, lastDataId);
        Swap(_indices, id, lastId);
        _data.RemoveAt(lastDataId);
        return true;
    }

    public bool RemoveAt(int index) => Remove(GetIDFromIndex(index));

    public bool Remove(Handle handle)
    {
        if (handle.Collection != this)
        {
            throw new ArgumentException("Handle is not from this collection", nameof(handle));
        }
        if (!handle)
        {
            return false;
        }
        return Remove(handle.ID);
    }

    public bool Remove(T item)
    {
        var index = _data.IndexOf(item);

        if (index == INVALID)
        {
            return false;
        }

        return RemoveAt(index);
    }

    public int GetDataIndex(ID id)
    {
        if (id < _indices.Count)
        {
            return _indices[id];
        }

        return INVALID;
    }

    public T this[ID id]
    {
        get
        {
            if (!IsValidID(id))
            {
                throw new IndexOutOfRangeException();
            }

            return _data[_indices[id]];
        }
    }

    public int Count => _data.Count;

    public bool Empty => _data.Count == 0;

    public int Capacity
    {
        get => _data.Capacity;
        set
        {
            _data.Capacity = value;
            _metaData.Capacity = value;
            _indices.Capacity = value;
        }
    }

    public bool IsReadOnly => false;

    public Handle CreateHandle(ID id)
    {
        var dataIndex = GetDataIndex(id);
        CheckIndex(dataIndex);

        return new(id, _metaData[_indices[id]].ValidityId, this);
    }

    public Handle CreateHandleFromData(int index)
    {
        var id = GetIDFromIndex(index);
        return new(id, _metaData[index].ValidityId, this);
    }

    public bool IsValid(ID id, ID validityId) => validityId == GetValidityID(id);

    public IEnumerator<T> GetEnumerator() => _data.GetEnumerator();

    IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();

    public void Remove(Func<T, bool> predicate)
    {
        for (var i = 0; i < _data.Count;)
        {
            if (predicate(_data[i]))
            {
                RemoveAt(i);
            }
            else
            {
                ++i;
            }
        }
    }

    public ID GetValidityID(ID id) => _metaData[_indices[id]].ValidityId;

    public IEnumerable<T> Data => _data;

    public ID GetNextID() => HasRoom ? _metaData[_data.Count].ReverseId : _data.Count;

    public void Clear()
    {
        _data.Clear();

        for (var i = 0; i < _metaData.Count; i++)
        {
            _metaData[i].ValidityId++;
        }
    }

    public bool IsValidID(ID id)
    {
        var dataIndex = GetDataIndex(id);
        return IsValidIndex(dataIndex);
    }

    public void CopyTo(T[] array, int arrayIndex)
    {
        if (array.Length - arrayIndex < _data.Count)
        {
            throw new ArgumentOutOfRangeException(nameof(array), "Array not large enough for contents of this collection");
        }

        for(var i = 0; i < _data.Count; i++)
        {
            array[arrayIndex + i] = _data[i];
        }
    }

    void ICollection<T>.Add(T item) => Add(item);

    public bool Contains(T item)
    {
        for (var i = 0; i < _data.Count; i++)
        {
            if (_data[i]!.Equals(item))
            {
                return true;
            }
        }

        return false;
    }

    #region Helper Methods
    private bool IsValidIndex(int index)
    {
        if (index < 0 || index >= _data.Count)
        {
            return false;
        }

        return true;
    }

    private void CheckIndex(int index)
    {
        if (!IsValidIndex(index))
        {
            throw new IndexOutOfRangeException();
        }
    }

    private ID GetIDFromIndex(int index)
    {
        CheckIndex(index);
        return _metaData[index].ReverseId;
    }

    private static void Swap<ListT>(List<ListT> list, ID indexA, ID indexB) =>
        (list[indexB], list[indexA]) = (list[indexA], list[indexB]);

    private ID GetFreeSlot()
    {
        var id = GetFreeID();
        _indices[id] = _data.Count;
        return id;
    }

    private bool HasRoom => _metaData.Count > _data.Count;

    private ID GetFreeID()
    {
        if (HasRoom)
        {
            _metaData[_data.Count].ValidityId++;
            return _metaData[_data.Count].ReverseId;
        }

        var newId = (ID)_data.Count;
        _metaData.Add(new Metadata(newId, 0));
        _indices.Add(newId);
        return newId;
    }
    #endregion

    #region Helper Classes
    /// <summary>
    /// Represents metadata information containing identifiers for reverse mapping and validity tracking.
    /// </summary>
    /// <param name="ReverseId"><see cref="ID"/> used to located value in <see cref="_indices"/> collection.</param>
    /// <param name="ValidityId">
    /// The identifier used to validate if a value is still the same value at a particular location in the collection.
    /// This value is incremented everytime the value at a particalar <paramref name="ReverseId"/> is changed.
    /// </param>
    private record class Metadata(ID ReverseId, ID ValidityId)
    {
        public ID ReverseId { get; set; } = ReverseId;
        public ID ValidityId { get; set; } = ValidityId;
        public override string ToString() => $"{{ {nameof(ReverseId)}: {(int)ReverseId}, {nameof(ValidityId)}: {(int)ValidityId} }}";
    }

    /// <summary>
    /// Represents a handle to an item within a <see cref="StableIndexCollection{T}"/>, uniquely identified by an ID and a validity ID.
    /// </summary>
    /// <remarks>Handles are typically used to safely reference items in collections that may change
    /// over time, such as when items are added or removed. The validity ID helps ensure that the handle remains
    /// valid and does not reference an item that has been removed or replaced.</remarks>
    /// <param name="Id">The unique identifier for the item within the collection.</param>
    /// <param name="ValidityID">An identifier used to verify the validity of the handle and prevent access to stale or invalid items.</param>
    /// <param name="Collection">The stable index collection that contains the item referenced by this handle.</param>
    public record class Handle
    {
        public ID ID { get; init; }
        public ID ValidityID { get; init; }
        public StableIndexCollection<T> Collection { get; init; }

        public Handle(ID iD, ID validityID, StableIndexCollection<T> collection)
        {
            ID = iD;
            ValidityID = validityID;
            Collection = collection;
        }

        public bool IsValid => Collection.IsValid(ID, ValidityID);

        public static implicit operator bool(Handle handle) => handle.IsValid;

        public static implicit operator T(Handle handle) => handle.Collection[handle.ID];

        public T Value => Collection[ID];
    }
    #endregion
}
