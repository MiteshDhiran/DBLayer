using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;


namespace ConnectAndSell.DataAccessStandard.Server.Common
{
  public class ResultSet 
  {
    private List<object> ObjList { get; set; }

    private Dictionary<string, object> ParaInfo { get; set; }

    public object ReturnValueObject { get; set; }

    private readonly Dictionary<Type, List<object>> _recordTypeRecordListDictionary;

    public ResultSet(List<object> objectList, IDbCommand cmd)
    {
      this.ParaInfo = new Dictionary<string, object>();
      this.ObjList = objectList;
      foreach (IDbDataParameter parameter in (DbParameterCollection) cmd.Parameters)
      {
        if (parameter.Direction == ParameterDirection.Output || parameter.Direction == ParameterDirection.InputOutput)
          this.ParaInfo.Add(parameter.ParameterName, parameter.Value);
        else if (parameter.Direction == ParameterDirection.ReturnValue)
          this.ReturnValueObject = parameter.Value;
      }

      _recordTypeRecordListDictionary = new Dictionary<Type, List<object>>();
      if (ObjList != null && this.ObjList.Count > 0)
      {
          foreach (object obj in this.ObjList)
          {
              if (obj != null)
              {
                  if (_recordTypeRecordListDictionary.ContainsKey(obj.GetType()) == false)
                  {
                      _recordTypeRecordListDictionary.Add(obj.GetType(),new List<object>());
                  }
                  _recordTypeRecordListDictionary[obj.GetType()].Add(obj);
              }
          }
      }
      
    }

    public ResultSet(Dictionary<Type,List<object>> objectList, Dictionary<string, object> dic)
    {
        _recordTypeRecordListDictionary = objectList;
        ParaInfo = dic;
    }

    public IEnumerable<TElement> GetResult<TElement>()
    {
      List<TElement> elementList = new List<TElement>();
      _recordTypeRecordListDictionary.TryGetValue(typeof(TElement), out var typeObjectList);
      if (typeObjectList != null && typeObjectList.Count > 0)
      {
        foreach (object obj in typeObjectList)
        {
          if (obj is TElement element)
            elementList.Add(element);
        }
      }
      return (IEnumerable<TElement>) elementList;
    }

    public object GetOutputPara(string para)
    {
      if (this.ParaInfo != null && this.ParaInfo.Count > 0 && this.ParaInfo.ContainsKey(para))
        return this.ParaInfo[para];
      return (object) null;
    }

    public object ReturnValue => this.ReturnValueObject;

 
  }
}