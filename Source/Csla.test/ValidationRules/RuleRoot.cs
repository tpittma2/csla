﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using Csla.Core;
using Csla.Rules;
using Csla.Rules.CommonRules;
using Csla.Threading;

namespace Csla.Test.ValidationRules
{
  [Serializable]
  public class RuleRoot : BusinessBase<RuleRoot>
  {
    #region Properties

    public static readonly PropertyInfo<int> CustomerIdProperty = RegisterProperty<int>(c => c.CustomerId);
    public int CustomerId
    {
      get { return GetProperty(CustomerIdProperty); }
      set { SetProperty(CustomerIdProperty, value); }
    }

    public static readonly PropertyInfo<string> NameProperty = RegisterProperty<string>(c => c.Name);
    public string Name
    {
      get { return GetProperty(NameProperty); }
      set { SetProperty(NameProperty, value); }
    }

    public static readonly PropertyInfo<int> Num1Property = RegisterProperty<int>(c => c.Num1);
    public int Num1
    {
      get { return GetProperty(Num1Property); }
      set { SetProperty(Num1Property, value); }
    }

    public static readonly PropertyInfo<int> Num2Property = RegisterProperty<int>(c => c.Num2);
    public int Num2
    {
      get { return GetProperty(Num2Property); }
      set { SetProperty(Num2Property, value); }
    }

    public static readonly PropertyInfo<int> SumProperty = RegisterProperty<int>(c => c.Sum);
    public int Sum
    {
      get { return GetProperty(SumProperty); }
      set { SetProperty(SumProperty, value); }
    }

    public static readonly PropertyInfo<string> CountryProperty = RegisterProperty<string>(c => c.Country);
    public string Country
    {
      get { return GetProperty(CountryProperty); }
      set { SetProperty(CountryProperty, value); }
    }

    public static readonly PropertyInfo<string> StateProperty = RegisterProperty<string>(c => c.State);
    public string State
    {
      get { return GetProperty(StateProperty); }
      set { SetProperty(StateProperty, value); }
    }

    public static readonly PropertyInfo<Csla.SmartDate> StartDateProperty = RegisterProperty<Csla.SmartDate>(c => c.StartDate, null, new Csla.SmartDate());
    public string StartDate
    {
      get { return GetPropertyConvert<Csla.SmartDate, string>(StartDateProperty); }
      set { SetPropertyConvert<Csla.SmartDate, string>(StartDateProperty, value); }
    }

    public static readonly PropertyInfo<Csla.SmartDate> EndDateProperty = RegisterProperty<Csla.SmartDate>(c => c.EndDate, null, new Csla.SmartDate());
    public string EndDate
    {
      get { return GetPropertyConvert<Csla.SmartDate, string>(EndDateProperty); }
      set { SetPropertyConvert<Csla.SmartDate, string>(EndDateProperty, value); }
    }

    #endregion

    protected override void PropertyHasChanged(IPropertyInfo property)
    {
      base.PropertyHasChanged(property);
      CheckObjectRules();
    }

    #region Validation Rules

    protected override void AddBusinessRules()
    {
      // call base class implementation to add data annotation rules to BusinessRules 
      base.AddBusinessRules();

      BusinessRules.RuleSet = "Date";
      BusinessRules.AddRule(new LessThan(StartDateProperty, EndDateProperty));
      // above rule will run on both properies changed - no need for dependency

      BusinessRules.RuleSet = "Lookup";
      BusinessRules.AddRule(new LookupCustomer(CustomerIdProperty, NameProperty));
      // rule will run async lookup of customer and set customer name

      BusinessRules.RuleSet = "LookupAndNameRequired";
      BusinessRules.AddRule(new LookupCustomer(CustomerIdProperty, NameProperty));
      BusinessRules.AddRule(new Required(NameProperty));
      // rule will run async lookuop of customer, set customer name and rerun Required rule on Name

      BusinessRules.RuleSet = "Object";
      BusinessRules.AddRule(new ValidateRootObject());
    }

    #endregion

    #region Factory Methods

    public static RuleRoot NewEditableRoot(string ruleSet)
    {
      return Csla.DataPortal.Create<RuleRoot>(ruleSet);
    }

    private RuleRoot()
    { /* Require use of factory methods */}

    #endregion

    protected void DataPortal_Create(string ruleSet)
    {
      BusinessRules.RuleSet = ruleSet;

      // this wil call BusinessRules.CheckRules();
      base.DataPortal_Create();
    }
  }

  /// <summary>
  /// CalcSum rule will set primary property to the sum of all.
  /// </summary>
  public class CalcSum : Csla.Rules.PropertyEditRule
  {
    /// <summary>
    /// Initializes a new instance of the <see cref="CalcSum"/> class.
    /// </summary>
    /// <param name="primaryProperty">The primary property.</param>
    /// <param name="inputProperties">The input properties.</param>
    public CalcSum(IPropertyInfo primaryProperty, params IPropertyInfo[] inputProperties)
      : base(primaryProperty)
    {
      if (InputProperties == null)
      {
        InputProperties = new List<IPropertyInfo>();
      }
      InputProperties.AddRange(inputProperties);
    }

    protected override void Execute(RuleContext context)
    {
      // Use linq Sum to calculate the sum value 
      var sum = context.InputPropertyValues.Sum(property => (dynamic)property.Value);

      // add calculated value to OutValues 
      // When rule is completed the RuleEngig will update businessobject
      context.AddOutValue(PrimaryProperty, sum);
    }
  }

  public class ValidateRootObject : Csla.Rules.ObjectRule
  {
    public ValidateRootObject()
      : base()
    {
      AffectedProperties.Add(RuleRoot.NameProperty);
      AffectedProperties.Add(RuleRoot.CountryProperty);
      AffectedProperties.Add(RuleRoot.StateProperty);
    }

    protected override void Execute(RuleContext context)
    {
      var customerId = (int)ReadProperty(context.Target, RuleRoot.CustomerIdProperty);

      switch (customerId)
      {
        case 4:
          context.AddErrorResult(RuleRoot.NameProperty, "customer name required");
          context.AddErrorResult(RuleRoot.CountryProperty, "country required");
          context.AddErrorResult(RuleRoot.StateProperty, "state required");
          break;
        case 5:
          context.AddWarningResult(RuleRoot.NameProperty, "customer name required");
          context.AddWarningResult(RuleRoot.CountryProperty, "country required");
          context.AddWarningResult(RuleRoot.StateProperty, "state required");
          break;
        case 6:
          context.AddInformationResult(RuleRoot.NameProperty, "customer name required");
          context.AddInformationResult(RuleRoot.CountryProperty, "country required");
          context.AddInformationResult(RuleRoot.StateProperty, "state required");
          break;
      }

    }
  }

  /// <summary>
  /// Implements a rule to compare 2 property values and make sure property1 is less than property2
  /// </summary>
  public class LessThan : PropertyRule
  {
    private IPropertyInfo CompareTo { get; set; }

    /// <summary>
    /// Initializes a new instance of the <see cref="LessThan"/> class.
    /// </summary>
    /// <param name="primaryProperty">The primary property.</param>
    /// <param name="compareToProperty">The compare to property.</param>
    public LessThan(IPropertyInfo primaryProperty, IPropertyInfo compareToProperty)
      : base(primaryProperty)
    {
      CompareTo = compareToProperty;
      InputProperties = new List<IPropertyInfo>() { primaryProperty, compareToProperty };
      AffectedProperties.Add(compareToProperty);
    }

    /// <summary>
    /// Does the check for primary propert less than compareTo property
    /// </summary>
    /// <param name="context">Rule context object.</param>
    protected override void Execute(RuleContext context)
    {
      var value1 = (IComparable)context.InputPropertyValues[PrimaryProperty];
      var value2 = (IComparable)context.InputPropertyValues[CompareTo];

      if (value1.CompareTo(value2) >= 0)
      {
        context.AddErrorResult(string.Format("{0} must be less than {1}", PrimaryProperty.FriendlyName, CompareTo.FriendlyName));
        context.AddErrorResult(CompareTo, string.Format("{0} must be larger than {1}", CompareTo.FriendlyName, PrimaryProperty.FriendlyName));
      }
    }
  }

  public class LookupCustomer : LookupRule
  {
    public IPropertyInfo NameProperty { get; set; }
    public LookupCustomer(IPropertyInfo primaryProperty, IPropertyInfo nameProperty)
      : base(primaryProperty)
    {
      NameProperty = nameProperty;
      InputProperties = new List<IPropertyInfo>() { primaryProperty };
      AffectedProperties.Add(NameProperty);
    }

    protected override void Execute(RuleContext context)
    {
      var customerId = (int)context.InputPropertyValues[PrimaryProperty];
      var bw = new BackgroundWorker();
      bw.DoWork += (o, e) => Thread.Sleep(50);
      bw.RunWorkerCompleted += (o, e) =>
                                 {
                                   string name;
                                   switch (customerId)
                                   {
                                     case 1:
                                       name = "Rocky Lhotka";
                                       break;
                                     default:
                                       name = string.Format("Customer_{0}", customerId);
                                       break;
                                   }
                                   context.AddOutValue(NameProperty, name);
                                   context.Complete();
                                 };
      bw.RunWorkerAsync();
    }
  }
}

