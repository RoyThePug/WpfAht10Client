using System.Collections;

namespace Aht10.Tests.Domain.Tests;

public class Node<T>
{
    public Node<T> Next { get; set; }
    public T Data { get; set; }

    public Node(T data)
    {
        Data = data;
    }
}

public class StructuresTests<T> : IEnumerable<T>
{
    private int _count;

    public int Count => _count;

    public bool IsEmpty => _count == 0;

    private Node<T> _head;

    public void Push(T data)
    {
        var node = new Node<T>(data);

        node.Next = _head;
        _head = node;
        _count++;
    }

    public T Pop()
    {
        if (IsEmpty)
        {
            throw new InvalidOperationException("Стек пуст");
        }

        var current = _head.Data;
        _head = _head.Next;
        _count--;

        return current;
    }

    IEnumerator<T> IEnumerable<T>.GetEnumerator()
    {
        Node<T> current = _head;
        while (current != null)
        {
            yield return current.Data;

            current = current.Next;
        }
    }

    IEnumerator IEnumerable.GetEnumerator()
    {
        return ((IEnumerable)this).GetEnumerator();
    }
}

public class CustomQueue<T> : IEnumerable<T>
{
    private int _count;

    public int Count => _count;

    public bool IsEmpty => _count == 0;

    private Node<T> _head;
    private Node<T> _tail;
    
    public T First
    {
        get
        {
            if (IsEmpty)
                throw new InvalidOperationException();
            return _head.Data;
        }
    }
    // получаем последний элемент
    public T Last
    {
        get
        {
            if (IsEmpty)
                throw new InvalidOperationException();
            return _tail.Data;
        }
    }

    public void Enqueue(T data)
    {
        Node<T> node = new Node<T>(data);
        Node<T> tempNode = _tail;
        _tail = node;
        if (_count == 0)
            _head = _tail;
        else
            tempNode.Next = _tail;
        _count++;
    }

    public T Dequeue()
    {
        if (IsEmpty)
        {
            throw new InvalidOperationException("Queue is empty");
        }

        var current = _head.Data;
        
        _head = _head.Next;
        _count--;
        return current;
    }

    public IEnumerator<T> GetEnumerator()
    {
        var current = _head;

        while (current != null)
        {
            yield return current.Data;

            current = current.Next;
        }
    }

    IEnumerator IEnumerable.GetEnumerator()
    {
        return ((IEnumerable)this).GetEnumerator();
    }
}

public class StructuresTests
{
    [Test]
    public void FixedStackTest()
    {
        var fixedStack = new StructuresTests<string>();

        fixedStack.Push("1");
        fixedStack.Push("2");
        fixedStack.Push("3");

        var oneValue = fixedStack.Pop();

        foreach (var t in fixedStack)
        {
            var val = t;
        }
    }

    [Test]
    public void QueueTest()
    {
        var customQueue = new CustomQueue<string>();

        customQueue.Enqueue("1");
        customQueue.Enqueue("2");
        customQueue.Enqueue("3");

        var oneValue = customQueue.Dequeue();

        foreach (var t in customQueue)
        {
            var val = t;
        }
    }
}