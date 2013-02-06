﻿#pragma warning disable 1591
//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated by a tool.
//     Runtime Version:4.0.30319.18034
//
//     Changes to this file may cause incorrect behavior and will be lost if
//     the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace play.Site.Models
{
	using System.Data.Linq;
	using System.Data.Linq.Mapping;
	using System.Data;
	using System.Collections.Generic;
	using System.Reflection;
	using System.Linq;
	using System.Linq.Expressions;
	using System.ComponentModel;
	using System;
	
	
	[global::System.Data.Linq.Mapping.DatabaseAttribute(Name="bakersdozen132")]
	public partial class PlayDataContext : System.Data.Linq.DataContext
	{
		
		private static System.Data.Linq.Mapping.MappingSource mappingSource = new AttributeMappingSource();
		
    #region Extensibility Method Definitions
    partial void OnCreated();
    partial void InsertLog(Log instance);
    partial void UpdateLog(Log instance);
    partial void DeleteLog(Log instance);
    partial void InsertPlayOrder(PlayOrder instance);
    partial void UpdatePlayOrder(PlayOrder instance);
    partial void DeletePlayOrder(PlayOrder instance);
    #endregion
		
		public PlayDataContext() : 
				base(global::System.Configuration.ConfigurationManager.ConnectionStrings["bakersdozen132ConnectionString"].ConnectionString, mappingSource)
		{
			OnCreated();
		}
		
		public PlayDataContext(string connection) : 
				base(connection, mappingSource)
		{
			OnCreated();
		}
		
		public PlayDataContext(System.Data.IDbConnection connection) : 
				base(connection, mappingSource)
		{
			OnCreated();
		}
		
		public PlayDataContext(string connection, System.Data.Linq.Mapping.MappingSource mappingSource) : 
				base(connection, mappingSource)
		{
			OnCreated();
		}
		
		public PlayDataContext(System.Data.IDbConnection connection, System.Data.Linq.Mapping.MappingSource mappingSource) : 
				base(connection, mappingSource)
		{
			OnCreated();
		}
		
		public System.Data.Linq.Table<Log> Logs
		{
			get
			{
				return this.GetTable<Log>();
			}
		}
		
		public System.Data.Linq.Table<PlayOrder> PlayOrders
		{
			get
			{
				return this.GetTable<PlayOrder>();
			}
		}
	}
	
	[global::System.Data.Linq.Mapping.TableAttribute(Name="ihdavis2.[Log]")]
	public partial class Log : INotifyPropertyChanging, INotifyPropertyChanged
	{
		
		private static PropertyChangingEventArgs emptyChangingEventArgs = new PropertyChangingEventArgs(String.Empty);
		
		private int _LogId;
		
		private string _LogType;
		
		private string _Message;
		
		private System.DateTime _CreateDate;
		
		private System.Nullable<System.DateTime> _ModifyDate;
		
    #region Extensibility Method Definitions
    partial void OnLoaded();
    partial void OnValidate(System.Data.Linq.ChangeAction action);
    partial void OnCreated();
    partial void OnLogIdChanging(int value);
    partial void OnLogIdChanged();
    partial void OnLogTypeChanging(string value);
    partial void OnLogTypeChanged();
    partial void OnMessageChanging(string value);
    partial void OnMessageChanged();
    partial void OnCreateDateChanging(System.DateTime value);
    partial void OnCreateDateChanged();
    partial void OnModifyDateChanging(System.Nullable<System.DateTime> value);
    partial void OnModifyDateChanged();
    #endregion
		
		public Log()
		{
			OnCreated();
		}
		
		[global::System.Data.Linq.Mapping.ColumnAttribute(Storage="_LogId", AutoSync=AutoSync.OnInsert, DbType="Int NOT NULL IDENTITY", IsPrimaryKey=true, IsDbGenerated=true)]
		public int LogId
		{
			get
			{
				return this._LogId;
			}
			set
			{
				if ((this._LogId != value))
				{
					this.OnLogIdChanging(value);
					this.SendPropertyChanging();
					this._LogId = value;
					this.SendPropertyChanged("LogId");
					this.OnLogIdChanged();
				}
			}
		}
		
		[global::System.Data.Linq.Mapping.ColumnAttribute(Storage="_LogType", DbType="NVarChar(10) NOT NULL", CanBeNull=false)]
		public string LogType
		{
			get
			{
				return this._LogType;
			}
			set
			{
				if ((this._LogType != value))
				{
					this.OnLogTypeChanging(value);
					this.SendPropertyChanging();
					this._LogType = value;
					this.SendPropertyChanged("LogType");
					this.OnLogTypeChanged();
				}
			}
		}
		
		[global::System.Data.Linq.Mapping.ColumnAttribute(Storage="_Message", DbType="NVarChar(MAX) NOT NULL", CanBeNull=false)]
		public string Message
		{
			get
			{
				return this._Message;
			}
			set
			{
				if ((this._Message != value))
				{
					this.OnMessageChanging(value);
					this.SendPropertyChanging();
					this._Message = value;
					this.SendPropertyChanged("Message");
					this.OnMessageChanged();
				}
			}
		}
		
		[global::System.Data.Linq.Mapping.ColumnAttribute(Storage="_CreateDate", DbType="DateTime NOT NULL")]
		public System.DateTime CreateDate
		{
			get
			{
				return this._CreateDate;
			}
			set
			{
				if ((this._CreateDate != value))
				{
					this.OnCreateDateChanging(value);
					this.SendPropertyChanging();
					this._CreateDate = value;
					this.SendPropertyChanged("CreateDate");
					this.OnCreateDateChanged();
				}
			}
		}
		
		[global::System.Data.Linq.Mapping.ColumnAttribute(Storage="_ModifyDate", DbType="DateTime")]
		public System.Nullable<System.DateTime> ModifyDate
		{
			get
			{
				return this._ModifyDate;
			}
			set
			{
				if ((this._ModifyDate != value))
				{
					this.OnModifyDateChanging(value);
					this.SendPropertyChanging();
					this._ModifyDate = value;
					this.SendPropertyChanged("ModifyDate");
					this.OnModifyDateChanged();
				}
			}
		}
		
		public event PropertyChangingEventHandler PropertyChanging;
		
		public event PropertyChangedEventHandler PropertyChanged;
		
		protected virtual void SendPropertyChanging()
		{
			if ((this.PropertyChanging != null))
			{
				this.PropertyChanging(this, emptyChangingEventArgs);
			}
		}
		
		protected virtual void SendPropertyChanged(String propertyName)
		{
			if ((this.PropertyChanged != null))
			{
				this.PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
			}
		}
	}
	
	[global::System.Data.Linq.Mapping.TableAttribute(Name="ihdavis2.PlayOrder")]
	public partial class PlayOrder : INotifyPropertyChanging, INotifyPropertyChanged
	{
		
		private static PropertyChangingEventArgs emptyChangingEventArgs = new PropertyChangingEventArgs(String.Empty);
		
		private int _PlayOrderId;
		
		private string _Name;
		
		private string _Email;
		
		private int _CoupleTicketCount;
		
		private int _IndividualTicketCount;
		
		private System.DateTime _PlayDate;
		
		private string _Status;
		
		private bool _Seated;
		
		private string _UserAgent;
		
		private string _Platform;
		
		private System.DateTime _CreateDate;
		
		private System.Nullable<System.DateTime> _ModifyDate;
		
    #region Extensibility Method Definitions
    partial void OnLoaded();
    partial void OnValidate(System.Data.Linq.ChangeAction action);
    partial void OnCreated();
    partial void OnPlayOrderIdChanging(int value);
    partial void OnPlayOrderIdChanged();
    partial void OnNameChanging(string value);
    partial void OnNameChanged();
    partial void OnEmailChanging(string value);
    partial void OnEmailChanged();
    partial void OnCoupleTicketCountChanging(int value);
    partial void OnCoupleTicketCountChanged();
    partial void OnIndividualTicketCountChanging(int value);
    partial void OnIndividualTicketCountChanged();
    partial void OnPlayDateChanging(System.DateTime value);
    partial void OnPlayDateChanged();
    partial void OnStatusChanging(string value);
    partial void OnStatusChanged();
    partial void OnSeatedChanging(bool value);
    partial void OnSeatedChanged();
    partial void OnUserAgentChanging(string value);
    partial void OnUserAgentChanged();
    partial void OnPlatformChanging(string value);
    partial void OnPlatformChanged();
    partial void OnCreateDateChanging(System.DateTime value);
    partial void OnCreateDateChanged();
    partial void OnModifyDateChanging(System.Nullable<System.DateTime> value);
    partial void OnModifyDateChanged();
    #endregion
		
		public PlayOrder()
		{
			OnCreated();
		}
		
		[global::System.Data.Linq.Mapping.ColumnAttribute(Storage="_PlayOrderId", AutoSync=AutoSync.OnInsert, DbType="Int NOT NULL IDENTITY", IsPrimaryKey=true, IsDbGenerated=true)]
		public int PlayOrderId
		{
			get
			{
				return this._PlayOrderId;
			}
			set
			{
				if ((this._PlayOrderId != value))
				{
					this.OnPlayOrderIdChanging(value);
					this.SendPropertyChanging();
					this._PlayOrderId = value;
					this.SendPropertyChanged("PlayOrderId");
					this.OnPlayOrderIdChanged();
				}
			}
		}
		
		[global::System.Data.Linq.Mapping.ColumnAttribute(Storage="_Name", DbType="NVarChar(50) NOT NULL", CanBeNull=false)]
		public string Name
		{
			get
			{
				return this._Name;
			}
			set
			{
				if ((this._Name != value))
				{
					this.OnNameChanging(value);
					this.SendPropertyChanging();
					this._Name = value;
					this.SendPropertyChanged("Name");
					this.OnNameChanged();
				}
			}
		}
		
		[global::System.Data.Linq.Mapping.ColumnAttribute(Storage="_Email", DbType="NVarChar(250) NOT NULL", CanBeNull=false)]
		public string Email
		{
			get
			{
				return this._Email;
			}
			set
			{
				if ((this._Email != value))
				{
					this.OnEmailChanging(value);
					this.SendPropertyChanging();
					this._Email = value;
					this.SendPropertyChanged("Email");
					this.OnEmailChanged();
				}
			}
		}
		
		[global::System.Data.Linq.Mapping.ColumnAttribute(Storage="_CoupleTicketCount", DbType="Int NOT NULL")]
		public int CoupleTicketCount
		{
			get
			{
				return this._CoupleTicketCount;
			}
			set
			{
				if ((this._CoupleTicketCount != value))
				{
					this.OnCoupleTicketCountChanging(value);
					this.SendPropertyChanging();
					this._CoupleTicketCount = value;
					this.SendPropertyChanged("CoupleTicketCount");
					this.OnCoupleTicketCountChanged();
				}
			}
		}
		
		[global::System.Data.Linq.Mapping.ColumnAttribute(Storage="_IndividualTicketCount", DbType="Int NOT NULL")]
		public int IndividualTicketCount
		{
			get
			{
				return this._IndividualTicketCount;
			}
			set
			{
				if ((this._IndividualTicketCount != value))
				{
					this.OnIndividualTicketCountChanging(value);
					this.SendPropertyChanging();
					this._IndividualTicketCount = value;
					this.SendPropertyChanged("IndividualTicketCount");
					this.OnIndividualTicketCountChanged();
				}
			}
		}
		
		[global::System.Data.Linq.Mapping.ColumnAttribute(Storage="_PlayDate", DbType="DateTime NOT NULL")]
		public System.DateTime PlayDate
		{
			get
			{
				return this._PlayDate;
			}
			set
			{
				if ((this._PlayDate != value))
				{
					this.OnPlayDateChanging(value);
					this.SendPropertyChanging();
					this._PlayDate = value;
					this.SendPropertyChanged("PlayDate");
					this.OnPlayDateChanged();
				}
			}
		}
		
		[global::System.Data.Linq.Mapping.ColumnAttribute(Storage="_Status", DbType="NVarChar(50) NOT NULL", CanBeNull=false)]
		public string Status
		{
			get
			{
				return this._Status;
			}
			set
			{
				if ((this._Status != value))
				{
					this.OnStatusChanging(value);
					this.SendPropertyChanging();
					this._Status = value;
					this.SendPropertyChanged("Status");
					this.OnStatusChanged();
				}
			}
		}
		
		[global::System.Data.Linq.Mapping.ColumnAttribute(Storage="_Seated", DbType="Bit NOT NULL")]
		public bool Seated
		{
			get
			{
				return this._Seated;
			}
			set
			{
				if ((this._Seated != value))
				{
					this.OnSeatedChanging(value);
					this.SendPropertyChanging();
					this._Seated = value;
					this.SendPropertyChanged("Seated");
					this.OnSeatedChanged();
				}
			}
		}
		
		[global::System.Data.Linq.Mapping.ColumnAttribute(Storage="_UserAgent", DbType="NVarChar(250) NOT NULL", CanBeNull=false)]
		public string UserAgent
		{
			get
			{
				return this._UserAgent;
			}
			set
			{
				if ((this._UserAgent != value))
				{
					this.OnUserAgentChanging(value);
					this.SendPropertyChanging();
					this._UserAgent = value;
					this.SendPropertyChanged("UserAgent");
					this.OnUserAgentChanged();
				}
			}
		}
		
		[global::System.Data.Linq.Mapping.ColumnAttribute(Storage="_Platform", DbType="NChar(10) NOT NULL", CanBeNull=false)]
		public string Platform
		{
			get
			{
				return this._Platform;
			}
			set
			{
				if ((this._Platform != value))
				{
					this.OnPlatformChanging(value);
					this.SendPropertyChanging();
					this._Platform = value;
					this.SendPropertyChanged("Platform");
					this.OnPlatformChanged();
				}
			}
		}
		
		[global::System.Data.Linq.Mapping.ColumnAttribute(Storage="_CreateDate", DbType="DateTime NOT NULL")]
		public System.DateTime CreateDate
		{
			get
			{
				return this._CreateDate;
			}
			set
			{
				if ((this._CreateDate != value))
				{
					this.OnCreateDateChanging(value);
					this.SendPropertyChanging();
					this._CreateDate = value;
					this.SendPropertyChanged("CreateDate");
					this.OnCreateDateChanged();
				}
			}
		}
		
		[global::System.Data.Linq.Mapping.ColumnAttribute(Storage="_ModifyDate", DbType="DateTime")]
		public System.Nullable<System.DateTime> ModifyDate
		{
			get
			{
				return this._ModifyDate;
			}
			set
			{
				if ((this._ModifyDate != value))
				{
					this.OnModifyDateChanging(value);
					this.SendPropertyChanging();
					this._ModifyDate = value;
					this.SendPropertyChanged("ModifyDate");
					this.OnModifyDateChanged();
				}
			}
		}
		
		public event PropertyChangingEventHandler PropertyChanging;
		
		public event PropertyChangedEventHandler PropertyChanged;
		
		protected virtual void SendPropertyChanging()
		{
			if ((this.PropertyChanging != null))
			{
				this.PropertyChanging(this, emptyChangingEventArgs);
			}
		}
		
		protected virtual void SendPropertyChanged(String propertyName)
		{
			if ((this.PropertyChanged != null))
			{
				this.PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
			}
		}
	}
}
#pragma warning restore 1591
