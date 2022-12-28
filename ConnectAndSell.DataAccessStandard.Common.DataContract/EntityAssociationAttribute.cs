using System;

namespace ConnectAndSell.DataAccessStandard.Common.DataContract
{
    class EntityAssociationAttribute : Attribute
    {
        private string name;
        private string storage;

        private string thisKey;
        private string otherKey;
        private bool isUnique;
        private bool isForeignKey;
        private bool deleteOnNull;
        private string deleteRule;


        /// <summary>Gets or sets the name of a column.</summary>
        /// <returns>The name.</returns>
        public string Name
        {
            get { return this.name; }
            set { this.name = value; }
        }

        /// <summary>Gets or sets a private storage field to hold the value from a column.</summary>
        /// <returns>The name of the storage field. </returns>
        public string Storage
        {
            get { return this.storage; }
            set { this.storage = value; }
        }


        /// <summary>Gets or sets members of this entity class to represent the key values on this side of the association.</summary>
        /// <returns>Default = Id of the containing class.</returns>
        public string ThisKey
        {
            get { return this.thisKey; }

            set { this.thisKey = value; }
        }

        /// <summary>Gets or sets one or more members of the target entity class as key values on the other side of the association.</summary>
        /// <returns>Default = Id of the related class.</returns>
        public string OtherKey
        {
            get { return this.otherKey; }

            set { this.otherKey = value; }
        }

        /// <summary>Gets or sets the indication of a uniqueness constraint on the foreign key.</summary>
        /// <returns>Default = <see langword="false" />.</returns>
        public bool IsUnique
        {
            get { return this.isUnique; }

            set { this.isUnique = value; }
        }

        /// <summary>Gets or sets the member as the foreign key in an association representing a database relationship.</summary>
        /// <returns>Default = <see langword="false" />.</returns>
        public bool IsForeignKey
        {
            get { return this.isForeignKey; }

            set { this.isForeignKey = value; }
        }

        /// <summary>Gets or sets delete behavior for an association.</summary>
        /// <returns>A string representing the rule.</returns>
        public string DeleteRule
        {
            get { return this.deleteRule; }

            set { this.deleteRule = value; }
        }

        /// <summary>When placed on a 1:1 association whose foreign key members are all non-nullable, deletes the object when the association is set to null.</summary>
        /// <returns>Setting to <see langword="True" /> deletes the object. The default value is <see langword="False" />.</returns>
        public bool DeleteOnNull
        {
            get { return this.deleteOnNull; }

            set { this.deleteOnNull = value; }
        }
    }
}