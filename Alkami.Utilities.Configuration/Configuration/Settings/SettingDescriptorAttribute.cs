using System;
using System.Collections.Generic;
using System.Reflection;
using System.Text;
using Alkami.Utilities.ArgumentValidation;

namespace Alkami.Utilities.Configuration.Settings
{
    /// <summary>
    /// The SettingDescriptor attribute is used to annotate a property with the appropriate values on classes that derive from <see cref="BaseSettings"/>.
    /// </summary>
    [AttributeUsage(AttributeTargets.Property)]
    public class SettingDescriptor : Attribute, IComparable<SettingDescriptor>
    {
        /// <summary>
        /// The name of the setting. This is the key of the key/value pair that is persisted.
        /// </summary>
        /// <returns>Returns a string containing the name of the setting.</returns>
        public string Name { get; set; }

        /// <summary>
        /// The display name of the Setting.
        /// </summary>
        /// <returns>Returns a string containing the display name of the setting.</returns>
        public string DisplayName { get; set; }

        /// <summary>
        /// The description of this setting.
        /// </summary>
        /// <returns>Returns a string containing the description of the setting.</returns>
        public string Description { get; set; }

        /// <summary>
        /// The order of the settings in relation to other settings.
        /// </summary>
        /// <returns>Returns a double that represents the order of the setting in relation to the other settings. The order defaults to 0.</returns>
        public double Order { get; set; }

        /// <summary>
        /// The HelpId property can be used to associate help content with a particular setting.
        /// </summary>
        /// <returns>Returns the HelpId string. This value is application specific.</returns>
        public string HelpId { get; internal set; }

        /// <summary>
        /// A flag that determines if the setting is required.
        /// </summary>
        /// <returns>Returns a boolean that determines if the setting is required. The default is false.</returns>
        public bool IsRequired { get; set; }

        /// <summary>
        /// A flag that determines if the setting contains secure information.
        /// </summary>
        /// <returns>Returns a boolean that determines if the setting contains secure information. The default is false.</returns>
        public bool IsSecure { get; set; }

        /// <summary>
        /// A flag that determines if the setting is environment specific.
        /// </summary>
        /// <returns>Returns a boolean that determines if the setting contains information that is environment specific. The default is false.</returns>
        public bool IsEnvironmental { get; set; }

        /// <summary>
        /// A flag that determines if the property can contain carriage returns and/or linefeed characters.
        /// </summary>
        /// <returns>Returns a boolean that determines if the property can contain carriage returns and/or linefeed characters. The default is false.</returns>
        /// <remarks>If this value is true, any carriage returns and/or linefeed characters are might be encoded when storing.</remarks>
        public bool IsMultiline { get; set; }

        /// <summary>
        /// A flag that determines if the property can contain HTML.
        /// </summary>
        /// <returns>Returns a boolean that determines if the property can contain HTML. The default is false.</returns>
        /// <remarks>If this value is true, any HTML might be encoded when storing. Html is always assumed to be Multiline.</remarks>
        public bool IsHtml { get; set; }

        /// <summary>
        /// Contains the default value for this property. Ideally, this should be set on the property and not on the attribute.
        /// </summary>
        /// <returns>Returns an object that represents the default value for this property.</returns>
        public object DefaultValue { get; set; }

        /// <summary>
        /// The Property property is a readonly property contains a reference to the associated PropertyInfo instance.
        /// </summary>
        /// <returns></returns>
        public PropertyInfo Property { get; internal set; }

        public SettingDescriptorCollection NestedSettingDescriptors { get; internal set; }

        public bool HasNestedSettingDescriptors
        {
            get { return NestedSettingDescriptors != null && NestedSettingDescriptors.Count > 0; }
        }

        /// <summary>
        /// An internal method to set the PropertyInfo associated with this SettingDescriptor.
        /// </summary>
        /// <param name="propertyInfo"></param>
        /// <exception cref="ArgumentNullException">Thrown when the propertyInfo parameter is null</exception>
        internal void SetPropertyInfo(PropertyInfo propertyInfo)
        {
            Validate.That(() => propertyInfo).IsNotNull();
            this.Property = propertyInfo;
        }

        public int CompareTo(SettingDescriptor other)
        {
            return string.Compare(this.Name, other.Name, StringComparison.Ordinal)
                   | string.Compare(this.DisplayName, other.DisplayName, StringComparison.Ordinal)
                   | string.Compare(this.Description, other.Description, StringComparison.Ordinal)
                   | this.Order.CompareTo(other.Order)
                   | string.Compare(this.HelpId, other.HelpId, StringComparison.Ordinal)
                   | this.IsRequired.CompareTo(other.IsRequired)
                   | this.IsSecure.CompareTo(other.IsSecure)
                   | this.IsEnvironmental.CompareTo(other.IsEnvironmental)
                   | this.IsMultiline.CompareTo(other.IsMultiline)
                   | this.IsHtml.CompareTo(other.IsHtml);
        }

        /// <summary>
        /// Returns a string representation of the SettingDescriptorAttribute instance.
        /// </summary>
        /// <returns>Returns a string value that represents the contents of the SettingDescriptorAttribute instance.</returns>
        public override string ToString()
        {
            var sb = new StringBuilder(100);
            string connector = "";
            if (!string.IsNullOrEmpty(this.Name))
            {
                sb.Append($"Name:{this.Name}");
                connector = ", ";
            }
            if (!string.IsNullOrEmpty(this.DisplayName))
            {
                sb.Append($"{connector}DisplayName:{this.DisplayName}");
                connector = ", ";
            }
            if (!string.IsNullOrEmpty(this.Description))
            {
                sb.Append($"{connector}Description:{this.Description}");
                connector = ", ";
            }
            if (this.Order > 0)
            {
                sb.Append($"{connector}Order:{this.Order}");
                connector = ", ";
            }
            if (!string.IsNullOrEmpty(this.HelpId))
            {
                sb.Append($"{connector}HelpId:{this.HelpId}");
                connector = ", ";
            }
            if (this.IsRequired)
            {
                sb.Append($"{connector}IsRequired");
                connector = ", ";
            }
            if (this.IsSecure)
            {
                sb.Append($"{connector}IsSecure");
                connector = ", ";
            }
            if (this.IsEnvironmental)
            {
                sb.Append($"{connector}IsEnvironmental");
                connector = ", ";
            }
            if (this.IsHtml)
            {
                sb.Append($"{connector}IsHtml");
                connector = ", ";
            }
            // HTML is always assumed to be multiline so no need for both
            else if (this.IsMultiline)
            {
                sb.Append($"{connector}IsMultiline");
                connector = ", ";
            }
            if (this.HasNestedSettingDescriptors)
            {
                sb.Append($"{connector}HasNestedSettingDescriptors({this.NestedSettingDescriptors.Count})");
                connector = ", ";
            }

            return sb.ToString();
        }
    }

    /// <summary>
    /// The SettingDescriptorCollection class contains a collection of <see cref="SettingDescriptor"/> instances.
    /// </summary>
    /// <seealso cref="System.Collections.Generic.List{SettingDescriptor}" />
    public class SettingDescriptorCollection : List<SettingDescriptor>
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="SettingDescriptorCollection"/> class.
        /// </summary>
        public SettingDescriptorCollection()
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="SettingDescriptorCollection"/> class.
        /// </summary>
        /// <param name="collection">The collection whose elements are copied to the new list.</param>
        public SettingDescriptorCollection(IEnumerable<SettingDescriptor> collection)
            : base(collection)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="SettingDescriptorCollection" /> class.
        /// </summary>
        /// <param name="capacity">The number of elements that the new list can initially store.</param>
        public SettingDescriptorCollection(int capacity)
            : base(capacity)
        {
        }
    }
}