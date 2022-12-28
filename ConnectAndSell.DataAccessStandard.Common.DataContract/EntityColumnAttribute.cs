using System;

namespace ConnectAndSell.DataAccessStandard.Common.DataContract
{
    [AttributeUsage(AttributeTargets.Property | AttributeTargets.Field, AllowMultiple = false)]
    public class EntityColumnAttribute : Attribute
    {
        private string name;
        private string storage;
        private bool canBeNull = true;
        private string dbtype;
        private string expression;
        private bool isPrimaryKey;
        private bool isDBGenerated;
        private bool isVersion;
        private bool isDiscriminator;
        private UpdateCheck check;
        private AutoSync autoSync;
        private bool canBeNullSet;

        
        /// <summary>Gets or sets the name of a column.</summary>
        /// <returns>The name.</returns>
        public string Name
        {
            get
            {
                return this.name;
            }
            set
            {
                this.name = value;
            }
        }

        /// <summary>Gets or sets a private storage field to hold the value from a column.</summary>
        /// <returns>The name of the storage field. </returns>
        public string Storage
        {
            get
            {
                return this.storage;
            }
            set
            {
                this.storage = value;
            }
        }
        
    /// <summary>Initializes a new instance of the <see cref="T:System.Data.Linq.Mapping.ColumnAttribute" /> class.</summary>
    public EntityColumnAttribute()
    {
      this.check = UpdateCheck.Always;
    }

    /// <summary>Gets or sets the type of the database column.</summary>
    /// <returns>See Remarks.</returns>
    public string DbType
    {
      get
      {
        return this.dbtype;
      }
      set
      {
        this.dbtype = value;
      }
    }

    /// <summary>Gets or sets whether a column is a computed column in a database.</summary>
    /// <returns>Default = empty.</returns>
    public string Expression
    {
      get
      {
        return this.expression;
      }
      set
      {
        this.expression = value;
      }
    }

    /// <summary>Gets or sets whether this class member represents a column that is part or all of the primary key of the table.</summary>
    /// <returns>Default = <see langword="false" />.</returns>
    public bool IsPrimaryKey
    {
      get
      {
        return this.isPrimaryKey;
      }
      set
      {
        this.isPrimaryKey = value;
      }
    }

    /// <summary>Gets or sets whether a column contains values that the database auto-generates.</summary>
    /// <returns>Default = <see langword="false" />.</returns>
    public bool IsDbGenerated
    {
      get
      {
        return this.isDBGenerated;
      }
      set
      {
        this.isDBGenerated = value;
      }
    }

    /// <summary>Gets or sets whether the column type of the member is a database timestamp or version number.</summary>
    /// <returns>Default value = <see langword="false" />.</returns>
    public bool IsVersion
    {
      get
      {
        return this.isVersion;
      }
      set
      {
        this.isVersion = value;
      }
    }

    /// <summary>Gets or sets how LINQ to SQL approaches the detection of optimistic concurrency conflicts.</summary>
    /// <returns>Default = <see cref="F:System.Data.Linq.Mapping.UpdateCheck.Always" />, unless <see cref="P:System.Data.Linq.Mapping.ColumnAttribute.IsVersion" /> is <see langword="true" /> for a member.Other values are <see cref="F:System.Data.Linq.Mapping.UpdateCheck.Never" /> and <see cref="F:System.Data.Linq.Mapping.UpdateCheck.WhenChanged" />.</returns>
    public UpdateCheck UpdateCheck
    {
      get
      {
        return this.check;
      }
      set
      {
        this.check = value;
      }
    }

    /// <summary>Gets or sets the <see cref="T:System.Data.Linq.Mapping.AutoSync" /> enumeration.</summary>
    /// <returns>The <see cref="T:System.Data.Linq.Mapping.AutoSync" /> value.</returns>
    public AutoSync AutoSync
    {
      get
      {
        return this.autoSync;
      }
      set
      {
        this.autoSync = value;
      }
    }

    /// <summary>Gets or sets whether a column contains a discriminator value for a LINQ to SQL inheritance hierarchy.</summary>
    /// <returns>Default = <see langword="false" />.</returns>
    public bool IsDiscriminator
    {
      get
      {
        return this.isDiscriminator;
      }
      set
      {
        this.isDiscriminator = value;
      }
    }

    /// <summary>Gets or sets whether a column can contain null values.</summary>
    /// <returns>Default = <see langword="true" />.</returns>
    public bool CanBeNull
    {
      get
      {
        return this.canBeNull;
      }
      set
      {
        this.canBeNullSet = true;
        this.canBeNull = value;
      }
    }

    internal bool CanBeNullSet
    {
      get
      {
        return this.canBeNullSet;
      }
    }
  }
    
}
