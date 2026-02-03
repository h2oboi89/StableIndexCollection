using StableIndexCollection;

namespace UnitTests;

public class StableIndexCollection_Tests
{
    [Test]
    public void StartsEmpty()
    {
        var siv = new StableIndexCollection<string>();

        using (Assert.EnterMultipleScope())
        {
            Assert.That(siv, Is.Empty);
            Assert.That(siv.Empty, Is.True);
            Assert.That(siv.Capacity, Is.Zero);
            Assert.That(siv.IsReadOnly, Is.False);
        }
    }

    [Test]
    public void CapacityCanBeSet()
    {
        var siv = new StableIndexCollection<string>
        {
            Capacity = 10
        };

        Assert.That(siv.Capacity, Is.EqualTo(10));
    }

    [Test]
    public void CanAddItems()
    {
        var siv = new StableIndexCollection<string>();

        var id1 = siv.Add("First");
        var id2 = siv.Add("Second");

        Assert.That(siv, Has.Count.EqualTo(2));
        using (Assert.EnterMultipleScope())
        {
            Assert.That(siv[id1], Is.EqualTo("First"));
            Assert.That(siv[id2], Is.EqualTo("Second"));
        }
    }

    [Test]
    public void Remove_Empty_InvalidID_ReturnsFalse()
    {
        var siv = new StableIndexCollection<string>();

        Assert.That(siv.Remove(15), Is.False);
    }

    [Test]
    public void Remove_InvalidID_ReturnsFalse()
    {
        var siv = new StableIndexCollection<string>();

        var id1 = siv.Add("First");

        Assert.That(siv.Remove(id1 + 1), Is.False);
    }

    [Test]
    public void Remove_ValidID_ReturnsTrue_And_RemovesItem()
    {
        var siv = new StableIndexCollection<string>();
        var id1 = siv.Add("A");
        var id2 = siv.Add("B");

        var validityId = siv.GetValidityID(id1);
        var result = siv.Remove(id1);
        var isValid = siv.IsValid(id1, validityId);
        var isValidId = siv.IsValidID(id1);

        using (Assert.EnterMultipleScope())
        {
            Assert.That(siv, Has.Count.EqualTo(1));
            Assert.That(result, Is.True);
            Assert.That(isValid, Is.False);
            Assert.That(isValidId, Is.False);
            Assert.That(siv[id2], Is.EqualTo("B"));
        }
    }

    [Test]
    public void RemoveAt_Empty_InvalidIndex_ReturnsFalse()
    {
        var siv = new StableIndexCollection<string>();

        Assert.That(() => siv.RemoveAt(0), Throws.InstanceOf<IndexOutOfRangeException>());
    }

    [Test]
    public void RemoveAt_InvalidIndex_ReturnsFalse()
    {
        var siv = new StableIndexCollection<string>
        {
            "First"
        };

        Assert.That(() => siv.RemoveAt(1), Throws.InstanceOf<IndexOutOfRangeException>());
    }

    [Test]
    public void RemoveAt_ValidIndex_ReturnsTrue_And_RemovesItem()
    {
        var siv = new StableIndexCollection<string>();
        var id1 = siv.Add("A");
        var id2 = siv.Add("B");

        var validityId = siv.GetValidityID(id1);
        Assert.That(() => siv.RemoveAt(0), Throws.Nothing);
        var isValid = siv.IsValid(id1, validityId);
        var isValidId = siv.IsValidID(id1);

        using (Assert.EnterMultipleScope())
        {
            Assert.That(siv, Has.Count.EqualTo(1));
            Assert.That(isValid, Is.False);
            Assert.That(isValidId, Is.False);
            Assert.That(siv[id2], Is.EqualTo("B"));
        }
    }

    [Test]
    public void CanBeIndexedById()
    {
        var siv = new StableIndexCollection<string>();

        var a = "1";
        var id = siv.Add(a);

        var b = siv[id];

        using (Assert.EnterMultipleScope())
        {
            Assert.That(b, Is.EqualTo("1"));
            Assert.That(ReferenceEquals(a, b), Is.True);
        }
    }

    [Test]
    public void IndexByByInvalidID_ThrowsException()
    {
        var siv = new StableIndexCollection<string>();
        Assert.That(() => siv[(ID)0], Throws.InstanceOf<IndexOutOfRangeException>());
    }

    [Test]
    public void Remove_ByHandle_InvalidCollection_Throws()
    {
        var siv1 = new StableIndexCollection<string>();
        var siv2 = new StableIndexCollection<string>();

        var id = siv1.Add("Test");
        var handle = siv1.CreateHandle(id);

        Assert.That(() => siv2.Remove(handle), Throws.InstanceOf<ArgumentException>());
    }

    [Test]
    public void Remove_ByHandle_InvalidHandle_ReturnsFalse()
    {
        var siv = new StableIndexCollection<string>();
        var id = siv.Add("Test");
        var handle = siv.CreateHandle(id);

        Assert.That(handle.IsValid, Is.True);

        siv.Remove(id); // Invalidate the handle

        Assert.That(siv.Remove(handle), Is.False);
    }

    [Test]
    public void Remove_ByHandle_ValidHandle_ReturnsTrue_And_RemovesItem()
    {
        var siv = new StableIndexCollection<string>();
        var id = siv.Add("Test");
        var handle = siv.CreateHandle(id);
        var result = siv.Remove(handle);
        using (Assert.EnterMultipleScope())
        {
            Assert.That(result, Is.True);
            Assert.That(siv, Is.Empty);
        }
    }

    [Test]
    public void Remove_ByReference_Empty_ReturnsFalse()
    {
        var siv = new StableIndexCollection<string>();
        Assert.That(siv.Remove("Test"), Is.False);
    }

    [Test]
    public void Remove_ByReference_NonExistent_ReturnsFalse()
    {
        var siv = new StableIndexCollection<string>(["A", "B", "C"]);
        Assert.That(siv.Remove("D"), Is.False);
    }

    [Test]
    public void Remove_ByReference_Exists_ReturnsTrue_And_RemovesItem()
    {
        var siv = new StableIndexCollection<string>(["A", "B", "C"]);

        var result = siv.Remove("B");

        using (Assert.EnterMultipleScope())
        {
            Assert.That(result, Is.True);
            Assert.That(siv, Does.Not.Contain("B"));
        }
    }

    [Test]
    public void Handles_CanBeCreatedFromIndex()
    {
        var siv = new StableIndexCollection<string>();

        var id = siv.Add("Test");
        var index = siv.GetDataIndex(id);

        var idHandle = siv.CreateHandle(id);
        var indexHandle = siv.CreateHandleFromData(index);

        using (Assert.EnterMultipleScope())
        {
            Assert.That(idHandle.ID, Is.EqualTo(indexHandle.ID));
            Assert.That(idHandle.ValidityID, Is.EqualTo(indexHandle.ValidityID));
            Assert.That(idHandle.Value, Is.EqualTo(indexHandle.Value));
            Assert.That((string)idHandle, Is.EqualTo("Test"));
            Assert.That(idHandle.IsValid, Is.True);
            Assert.That(indexHandle.IsValid, Is.True);
        }

        siv.Remove(id);

        using (Assert.EnterMultipleScope())
        {
            Assert.That(idHandle.IsValid, Is.False);
            Assert.That(indexHandle.IsValid, Is.False);
        }
    }

    [Test]
    public void Enumeration_Empty_Works()
    {
        var siv = new StableIndexCollection<int>();

        var sum = siv.Sum();

        Assert.That(sum, Is.Zero);
    }

    [Test]
    public void Enumeration_NotEmpty_Works()
    {
        var siv = new StableIndexCollection<int>
        {
            1, 2, 3, 4
        };

        var sum = siv.Sum();

        Assert.That(sum, Is.EqualTo(10));
    }

    [Test]
    public void Remove_ByPredicate_Works()
    {
        var siv = new StableIndexCollection<int>
        {
            1, 2, 3, 4, 5, 6
        };

        siv.Remove(x => x % 2 == 0); // Remove even numbers

        using (Assert.EnterMultipleScope())
        {
            Assert.That(siv, Has.Count.EqualTo(3));
            Assert.That(siv, Does.Contain(1));
            Assert.That(siv, Does.Contain(3));
            Assert.That(siv, Does.Contain(5));
        }
    }

    [Test]
    public void Data_IsUnderlyingList()
    {
        var siv = new StableIndexCollection<string>
        {
            "A",
            "B",
            "C"
        };
        var data = siv.Data.ToList();
        using (Assert.EnterMultipleScope())
        {
            Assert.That(data, Has.Count.EqualTo(3));
            Assert.That(data, Does.Contain("A"));
            Assert.That(data, Does.Contain("B"));
            Assert.That(data, Does.Contain("C"));
        }
    }

    [Test]
    public void GetNextID_Works()
    {
        var siv = new StableIndexCollection<string>();

        var id1 = siv.GetNextID();
        var id2 = siv.Add("Test");

        Assert.That(id1, Is.EqualTo(id2));

        siv.Remove(id2);

        var id3 = siv.GetNextID();

        Assert.That(id3, Is.EqualTo(id1));
    }

    [Test]
    public void Clear_RemovesAllItems()
    {
        var siv = new StableIndexCollection<string>
        {
            "A",
            "B",
            "C"
        };

        siv.Clear();

        Assert.That(siv, Is.Empty);
    }

    [Test]
    public void CopyTo_IndexZero_TooSmallArray_Throws()
    {
        var siv = new StableIndexCollection<string>
        {
            "A",
            "B",
            "C"
        };
        var array = new string[2];
        Assert.That(() => siv.CopyTo(array, 0), Throws.InstanceOf<ArgumentException>());
    }

    [Test]
    public void CopyTo_IndexNotZero_TooSmallArray_Throws()
    {
        var siv = new StableIndexCollection<string>
        {
            "A",
            "B",
            "C"
        };
        var array = new string[10];
        Assert.That(() => siv.CopyTo(array, 8), Throws.InstanceOf<ArgumentException>());
    }

    [Test]
    public void CopyTo_Works()
    {
        var siv = new StableIndexCollection<string>
        {
            "A",
            "B",
            "C"
        };
        var array = new string[5];
        siv.CopyTo(array, 1);
        using (Assert.EnterMultipleScope())
        {
            Assert.That(array[0], Is.Null);
            Assert.That(array[1], Is.EqualTo("A"));
            Assert.That(array[2], Is.EqualTo("B"));
            Assert.That(array[3], Is.EqualTo("C"));
            Assert.That(array[4], Is.Null);
        }
    }

    [Test]
    public void Contains_Works()
    {
        var siv = new StableIndexCollection<string>
        {
            "A",
            "B",
            "C"
        };
        using (Assert.EnterMultipleScope())
        {
#pragma warning disable NUnit2014 // Use SomeItemsConstraint for better assertion messages in case of failure
            Assert.That(siv.Contains("A"), Is.True);
            Assert.That(siv.Contains("B"), Is.True);
            Assert.That(siv.Contains("C"), Is.True);
            Assert.That(siv.Contains("D"), Is.False);
#pragma warning restore NUnit2014 // Use SomeItemsConstraint for better assertion messages in case of failure
        }
    }

    [Test]
    public void IDs_AreReused_AfterRemoval()
    {
        var siv = new StableIndexCollection<string>();
        var id1 = siv.Add("First");
        siv.Remove(id1);
        var id2 = siv.Add("Second");
        Assert.That(id2, Is.EqualTo(id1));
    }
}
