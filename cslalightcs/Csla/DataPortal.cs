﻿using System;
using System.ServiceModel;
using Csla.Serialization;
using Csla.Serialization.Mobile;

namespace Csla
{
  public class DataPortal<T> where T: IMobileObject
  {
    #region Create

    public event EventHandler<DataPortalResult<T>> CreateCompleted;

    protected virtual void OnCreateCompleted(DataPortalResult<T> e)
    {
      if (CreateCompleted != null)
        CreateCompleted(this, e);
    }

    public void SendCreate()
    {
      var request = new WcfPortal.CriteriaRequest();
      request.TypeName = typeof(T).FullName + "," + typeof(T).Assembly.FullName;
      request.CriteriaData = null;

      var proxy = new WcfPortal.WcfPortalClient();
      proxy.CreateCompleted += new EventHandler<Csla.WcfPortal.CreateCompletedEventArgs>(proxy_CreateCompleted);
      proxy.CreateAsync(request);
    }

    public void SendCreate(object criteria)
    {
      var request = new WcfPortal.CriteriaRequest();
      request.TypeName = typeof(T).FullName + "," + typeof(T).Assembly.FullName;
      request.CriteriaData = MobileFormatter.Serialize(criteria);

      var proxy = new WcfPortal.WcfPortalClient();
      proxy.CreateCompleted += new EventHandler<Csla.WcfPortal.CreateCompletedEventArgs>(proxy_CreateCompleted);
      proxy.CreateAsync(request);
    }

    private void proxy_CreateCompleted(object sender, Csla.WcfPortal.CreateCompletedEventArgs e)
    {
      var response = e.Result;
      try
      {
        if (e.Error == null && response.ErrorData == null)
        {
          var buffer = new System.IO.MemoryStream(response.ObjectData);
          var formatter = new MobileFormatter();
          T obj = (T)formatter.Deserialize(buffer);
          OnCreateCompleted(new DataPortalResult<T>(obj, null));
        }
        else if (e.Result.ErrorData != null)
        {
          var ex = new DataPortalException(e.Result.ErrorData);
          OnCreateCompleted(new DataPortalResult<T>(default(T), ex));
        }
        else
        {
          OnCreateCompleted(new DataPortalResult<T>(default(T), e.Error));
        }
      }
      catch (Exception ex)
      {
        OnFetchCompleted(new DataPortalResult<T>(default(T), ex));
      }
    }

    #endregion

    #region Fetch

    public event EventHandler<DataPortalResult<T>> FetchCompleted;

    protected virtual void OnFetchCompleted(DataPortalResult<T> e)
    {
      if (FetchCompleted != null)
        FetchCompleted(this, e);
    }

    public void SendFetch()
    {
      var request = new WcfPortal.CriteriaRequest();
      request.TypeName = typeof(T).FullName + "," + typeof(T).Assembly.FullName;
      request.CriteriaData = null;

      var proxy = new WcfPortal.WcfPortalClient();
      proxy.FetchCompleted += new EventHandler<Csla.WcfPortal.FetchCompletedEventArgs>(proxy_FetchCompleted);
      proxy.FetchAsync(request);
    }

    public void SendFetch(object criteria)
    {
      var request = new WcfPortal.CriteriaRequest();
      request.TypeName = typeof(T).FullName + "," + typeof(T).Assembly.FullName;
      request.CriteriaData = MobileFormatter.Serialize(criteria);

      var proxy = new WcfPortal.WcfPortalClient();
      proxy.FetchCompleted += new EventHandler<Csla.WcfPortal.FetchCompletedEventArgs>(proxy_FetchCompleted);
      proxy.FetchAsync(request);
    }

    private void proxy_FetchCompleted(object sender, Csla.WcfPortal.FetchCompletedEventArgs e)
    {
      var response = e.Result;
      try
      {
        if (e.Error == null && response.ErrorData == null)
        {
          var buffer = new System.IO.MemoryStream(response.ObjectData);
          var formatter = new MobileFormatter();
          T obj = (T)formatter.Deserialize(buffer);
          OnFetchCompleted(new DataPortalResult<T>(obj, null));
        }
        else if (e.Result.ErrorData != null)
        {
          var ex = new DataPortalException(e.Result.ErrorData);
          OnFetchCompleted(new DataPortalResult<T>(default(T), ex));
        }
        else
        {
          OnFetchCompleted(new DataPortalResult<T>(default(T), e.Error));
        }
      }
      catch (Exception ex)
      {
        OnFetchCompleted(new DataPortalResult<T>(default(T), ex));
      }
    }

    #endregion

    #region Update

    public event EventHandler<DataPortalResult<T>> UpdateCompleted;

    protected virtual void OnUpdateCompleted(DataPortalResult<T> e)
    {
      if (UpdateCompleted != null)
        UpdateCompleted(this, e);
    }

    public void SendUpdate(object criteria)
    {
      var request = new WcfPortal.UpdateRequest();
      request.ObjectData = MobileFormatter.Serialize(criteria);

      var proxy = new WcfPortal.WcfPortalClient();
      proxy.UpdateCompleted += new EventHandler<Csla.WcfPortal.UpdateCompletedEventArgs>(proxy_UpdateCompleted);
      proxy.UpdateAsync(request);
    }

    private void proxy_UpdateCompleted(object sender, Csla.WcfPortal.UpdateCompletedEventArgs e)
    {
      var response = e.Result;
      try
      {
        if (e.Error == null && response.ErrorData == null)
        {
          var buffer = new System.IO.MemoryStream(response.ObjectData);
          var formatter = new MobileFormatter();
          T obj = (T)formatter.Deserialize(buffer);
          OnUpdateCompleted(new DataPortalResult<T>(obj, null));
        }
        else if (e.Result.ErrorData != null)
        {
          var ex = new DataPortalException(e.Result.ErrorData);
          OnUpdateCompleted(new DataPortalResult<T>(default(T), ex));
        }
        else
        {
          OnUpdateCompleted(new DataPortalResult<T>(default(T), e.Error));
        }
      }
      catch (Exception ex)
      {
        OnFetchCompleted(new DataPortalResult<T>(default(T), ex));
      }
    }

    #endregion

    #region Delete

    public event EventHandler<DataPortalResult<T>> DeleteCompleted;

    protected virtual void OnDeleteCompleted(DataPortalResult<T> e)
    {
      if (DeleteCompleted != null)
        DeleteCompleted(this, e);
    }

    public void SendDelete(object criteria)
    {
      var request = new WcfPortal.CriteriaRequest();
      request.TypeName = typeof(T).FullName + "," + typeof(T).Assembly.FullName;
      request.CriteriaData = MobileFormatter.Serialize(criteria);

      var proxy = new WcfPortal.WcfPortalClient();
      proxy.DeleteCompleted += new EventHandler<Csla.WcfPortal.DeleteCompletedEventArgs>(proxy_DeleteCompleted);
      proxy.DeleteAsync(request);
    }

    private void proxy_DeleteCompleted(object sender, Csla.WcfPortal.DeleteCompletedEventArgs e)
    {
      var response = e.Result;
      try
      {
        if (e.Error == null && response.ErrorData == null)
        {
          OnDeleteCompleted(new DataPortalResult<T>(default(T), null));
        }
        else if (e.Result.ErrorData != null)
        {
          var ex = new DataPortalException(e.Result.ErrorData);
          OnDeleteCompleted(new DataPortalResult<T>(default(T), ex));
        }
        else
        {
          OnDeleteCompleted(new DataPortalResult<T>(default(T), e.Error));
        }
      }
      catch (Exception ex)
      {
        OnFetchCompleted(new DataPortalResult<T>(default(T), ex));
      }
    }

    #endregion
  }
}