using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using Common.Logging;

namespace Alkami.Utilities.Configuration.Settings
{
    /// <summary>
    /// The BaseSettings class is an abstract base class that is used to create SettingDescriptors which define 
    /// the properties of the derived settings class.
    /// </summary>
    public abstract class BaseSettings
    {
        private const char HierarchySeparator = '.';
        private readonly ILog logger = LogManager.GetLogger<BaseSettings>();

        /// <summary>
        /// Create an instance of the BaseSettings class.
        /// </summary>
        protected BaseSettings()
        {
            // It would be better to Lazy instantiate this but since we store the default
            // values in the settingDescriptors, we have to do this before setting any properties
            this.BuildSettingDescriptors();
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="BaseSettings"/> class with the
        /// name/values in the settings parameter.
        /// </summary>
        /// <param name="settings">The settings is a dictionary containing name/value settings
        /// to assign to this instance.</param>
        protected BaseSettings(Dictionary<string, string> settings)
            : this()
        {
            this.SetProperties(settings);
        }

        /// <summary>
        /// Return a collection of the SettingDescriptors which define the properties of the Settings class.
        /// </summary>
        /// <returns>Returns a collection of the SettingsDescriptors.</returns>
        public SettingDescriptorCollection SettingDescriptors { get; internal set; }

        /// <summary>
        /// A protected method that builds the SettingDescriptors collection based on the instance of the BaseSettings class.
        /// </summary>
        /// <returns>Returns a collection of SettingDescriptors based on the instance of the BaseSettings class.</returns>
        protected void BuildSettingDescriptors()
        {
            this.SettingDescriptors = new SettingDescriptorCollection();
            BuildSettingDescriptorsForType(this, this.SettingDescriptors);

            if (this.SettingDescriptors.Any(sd => Math.Abs(sd.Order) > 0.001))
            {
                this.SettingDescriptors = new SettingDescriptorCollection(this.SettingDescriptors.OrderBy(o => o.Order));
            }
        }

        protected void BuildSettingDescriptorsForType(object instance, SettingDescriptorCollection settingDescriptors)
        { 
            var typeInfo = instance.GetType().GetTypeInfo();
            var properties = typeInfo.DeclaredProperties.ToArray();

            logger.Debug($"Iterating through {properties.Length} properties");
            foreach (var property in properties)
            {
                logger.Trace($"Found property [{property}]");
                var settingDescriptor = property.GetCustomAttribute<SettingDescriptor>();
                if (settingDescriptor == null)
                {
                    logger.Trace($"Property [{property}] doesn't have a SettingDescriptor. Adding one.");
                    settingDescriptor = new SettingDescriptor();
                }
                else
                {
                    logger.Trace($"Property [{property}] has a settingDescriptor [{settingDescriptor}]");
                }
                settingDescriptor.SetPropertyInfo(property);

                // Default the Name to the name of the property
                if (string.IsNullOrEmpty(settingDescriptor.Name))
                    settingDescriptor.Name = property.Name;

                // Default the DisplayName to a more display friendly version of Name
                if (string.IsNullOrEmpty(settingDescriptor.DisplayName))
                    settingDescriptor.DisplayName = settingDescriptor.Name.ToDisplayName();

                // Default the Description to an empty string
                if (string.IsNullOrEmpty(settingDescriptor.Description))
                    settingDescriptor.Description = "";

                // If the default value hasn't been set on the attribute, use the current value of the property
                if (settingDescriptor.DefaultValue == null)
                    settingDescriptor.DefaultValue = property.GetValue(instance);

                if (IsPropertyCustomClass(property))
                {
                    settingDescriptor.NestedSettingDescriptors = new SettingDescriptorCollection();
                    BuildSettingDescriptorsForType(property.GetValue(instance), settingDescriptor.NestedSettingDescriptors);
                    if (settingDescriptor.NestedSettingDescriptors.Any(sd => Math.Abs(sd.Order) > 0.001))
                    {
                        settingDescriptor.NestedSettingDescriptors = new SettingDescriptorCollection(settingDescriptor.NestedSettingDescriptors.OrderBy(o => o.Order));
                    }
                }

                settingDescriptors.Add(settingDescriptor);
            }
        }

        protected bool IsPropertyCustomClass(PropertyInfo property)
        {
            return string.Compare(property.PropertyType.Assembly.GetName().Name, "mscorlib", StringComparison.OrdinalIgnoreCase) != 0 
                && property.PropertyType.GetTypeInfo().DeclaredProperties.Any();
        }

        /// <summary>
        /// The GetChangedProperties method is an internal protected method that returns a collection
        /// containing the properties that have been changed from their default.
        /// </summary>
        /// <returns>
        /// Returns a collection containing zero or more name/value pairs containing the changed
        /// property values.
        /// </returns>
        protected internal Dictionary<string, string> GetChangedProperties()
        {
            var result = new Dictionary<string, string>();
            GetChangedPropertiesInternal(this, this.SettingDescriptors, result, "");

            return result.OrderBy(c => c.Key).ToDictionary((keyItem) => keyItem.Key, (valueItem) => valueItem.Value);
        }

        /// <summary>
        /// An internal protected method that gets the properties that have changed.
        /// </summary>
        /// <param name="instance">The instance to get the changed properties from.</param>
        /// <param name="settingDescriptors">The setting descriptors collection.</param>
        /// <param name="changedProperties">The changed properties.</param>
        /// <param name="prefix">The prefix.</param>
        /// <remarks>This method is called recursively to get embedded properties.</remarks>
        protected internal void GetChangedPropertiesInternal(object instance, SettingDescriptorCollection settingDescriptors, Dictionary<string, string> changedProperties, string prefix)
        {
            logger.Trace($"Iterating through {settingDescriptors.Count} settingDescriptors. Prefix:{prefix}");
            foreach (var settingDescriptor in settingDescriptors)
            {
                logger.Trace($"Getting current value for [{settingDescriptor.Property}]");
                var value = settingDescriptor.Property.GetValue(instance);
                if (settingDescriptor.HasNestedSettingDescriptors)
                {
                    logger.Trace($"[Getting nested properties for {settingDescriptor.Property}]");
                    GetChangedPropertiesInternal(value, settingDescriptor.NestedSettingDescriptors, changedProperties, $"{prefix}{settingDescriptor.Name}{HierarchySeparator}");
                }
                else
                {
                    logger.Trace($"[{settingDescriptor.Property}] value is [{value}]");
                    if (!value.Equals(settingDescriptor.DefaultValue))
                    {
                        logger.Trace($"[{settingDescriptor.Property}] value changed. Current:[{value}], Default:[{settingDescriptor.DefaultValue}]");
                        changedProperties.Add(prefix + settingDescriptor.Name, value.ToString());
                    }
                }
            }
        }

        /// <summary>
        /// The SetProperties method is an internal protected method that takes a dictionary of persisted
        /// settings and assigns those settings to the appropriate properties to rehydrate the property
        /// values.
        /// </summary>
        /// <param name="settings">The settings collection containing the values to assign to the properties. If
        /// this parameter is null, not property values are set.</param>
        protected internal void SetProperties(Dictionary<string, string> settings)
        {
            if (settings == null)
                return;

            logger.Debug($"Iterating through {settings.Count} settings");
            foreach (var setting in settings)
            {
                logger.Trace($"Getting SettingDescriptor for {setting.Key}");
                var settingDescriptorLocator = LocateSettingDescriptor(this, this.SettingDescriptors, setting.Key);
                var settingDescriptor = settingDescriptorLocator.Item2;
                var instance = settingDescriptorLocator.Item1;
                // Ignore the setting value if not found - this accounts for old properties
                if (settingDescriptor != null)
                {
                    logger.Debug($"Found [{settingDescriptor.Property}] settingDescriptor for {setting.Key}");
                    object value;
                    if (settingDescriptor.Property.PropertyType.GetTypeInfo().IsEnum)
                    {
                        logger.Debug("Property is an enum");
                        value = Enum.Parse(settingDescriptor.Property.PropertyType, setting.Value);
                    }
                    else
                    {
                        logger.Debug($"Property is {settingDescriptor.Property.PropertyType}");
                        value = Convert.ChangeType(setting.Value, settingDescriptor.Property.PropertyType);
                    }
                    logger.Trace($"Setting [{settingDescriptor.Property}] to value [{value}]");
                    settingDescriptor.Property.SetValue(instance, value);
                }
                else
                {
                    logger.Warn($"SettingDescriptor not found for setting [{setting.Key}]");
                }
            }
        }

        protected Tuple<object, SettingDescriptor> LocateSettingDescriptor(object instance, SettingDescriptorCollection settingDescriptors, string settingKey)
        {
            var parts = settingKey.Split(HierarchySeparator);
            var foundSettingDescriptor = settingDescriptors.FirstOrDefault(x => x.Name == parts[0]);
            if (parts.Length > 1 && foundSettingDescriptor != null)
            {
                return LocateSettingDescriptor(foundSettingDescriptor.Property.GetValue(instance), 
                                                    foundSettingDescriptor.NestedSettingDescriptors, 
                                                    string.Join(HierarchySeparator.ToString(), parts.Where(x => x != parts[0])));
            }

            return new Tuple<object, SettingDescriptor>(instance, foundSettingDescriptor);
        }
    }
}
