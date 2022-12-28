using System;
using System.Collections.Generic;

namespace ConnectAndSell.DataAccessStandard.Server.Common
{
  internal class SortExpressionBuilder<T> : IComparer<List<object>>
  {
    private LinkedList<Func<T, object>> _selectors = new LinkedList<Func<T, object>>();
    private LinkedList<Comparison<object>> _comparers = new LinkedList<Comparison<object>>();
    private LinkedListNode<Func<T, object>> _currentSelector;
    private LinkedListNode<Comparison<object>> _currentComparer;

    internal void Add(Func<T, object> keySelector, Comparison<object> compare, bool isOrderBy)
    {
      if (isOrderBy)
      {
        this._currentSelector = this._selectors.AddFirst(keySelector);
        this._currentComparer = this._comparers.AddFirst(compare);
      }
      else
      {
        this._currentSelector = this._selectors.AddAfter(this._currentSelector, keySelector);
        this._currentComparer = this._comparers.AddAfter(this._currentComparer, compare);
      }
    }

    public List<object> Select(T row)
    {
      List<object> objectList = new List<object>();
      foreach (Func<T, object> selector in this._selectors)
        objectList.Add(selector(row));
      return objectList;
    }

    public int Compare(List<object> a, List<object> b)
    {
      int index = 0;
      foreach (Comparison<object> comparer in this._comparers)
      {
        int num = comparer(a[index], b[index]);
        if (num != 0)
          return num;
        ++index;
      }
      return 0;
    }

    internal int Count
    {
      get
      {
        return this._selectors.Count;
      }
    }

    internal SortExpressionBuilder<T> Clone()
    {
      SortExpressionBuilder<T> expressionBuilder = new SortExpressionBuilder<T>();
      foreach (Func<T, object> selector in this._selectors)
      {
        if (selector == this._currentSelector.Value)
          expressionBuilder._currentSelector = expressionBuilder._selectors.AddLast(selector);
        else
          expressionBuilder._selectors.AddLast(selector);
      }
      foreach (Comparison<object> comparer in this._comparers)
      {
        if (comparer == this._currentComparer.Value)
          expressionBuilder._currentComparer = expressionBuilder._comparers.AddLast(comparer);
        else
          expressionBuilder._comparers.AddLast(comparer);
      }
      return expressionBuilder;
    }

    internal SortExpressionBuilder<TResult> CloneCast<TResult>()
    {
      SortExpressionBuilder<TResult> expressionBuilder = new SortExpressionBuilder<TResult>();
      foreach (Func<T, object> selector1 in this._selectors)
      {
        Func<T, object> selector = selector1;
        if (selector == this._currentSelector.Value)
          expressionBuilder._currentSelector = expressionBuilder._selectors.AddLast((Func<TResult, object>) (r => selector((T) (object) r)));
        else
          expressionBuilder._selectors.AddLast((Func<TResult, object>) (r => selector((T) (object) r)));
      }
      foreach (Comparison<object> comparer in this._comparers)
      {
        if (comparer == this._currentComparer.Value)
          expressionBuilder._currentComparer = expressionBuilder._comparers.AddLast(comparer);
        else
          expressionBuilder._comparers.AddLast(comparer);
      }
      return expressionBuilder;
    }
  }
}
