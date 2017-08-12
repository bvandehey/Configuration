using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using Microsoft.Extensions.Logging;

namespace Alkami.Framework.Settings
{
    /// <summary>
    /// The BaseSettings class is an abstract base class that is used to create SettingDescriptors which define 
    /// the properties of the derived settings class.
    /// </summary>
    public abstract class BaseSettings
    {
        private readonly ILogger logger = ApplicationLogging.CreateLogger<BaseSettings>();
        private List<SettingDescriptor> settingDescriptors;

        /// <summary>
        /// Create an instance of the BaseSettings class.
        /// </summary>
        public BaseSettings()
        {
            // It would be better to Lazy instantiate this but since we store the default
            // values in the settingDescriptors, we have to do this before setting any properties
            this.settingDescriptors = this.BuildSettingDescriptors();
        }

        /// <summary>
        /// Return a collection of the SettingDescriptors which define the properties of the Settings class.
        /// </summary>
        /// <returns>Returns a collection of the SettingsDescriptors.</returns>
        public List<SettingDescriptor> SettingDescriptors
        {
            get
            {
                return this.settingDescriptors;
            }
        }

        /// <summary>
        /// A protected method that builds the SettingDescriptors collection based on the instance of the BaseSettings class.
        /// </summary>
        /// <returns>Returns a collection of SettingDescriptors based on the instance of the BaseSettings class.</returns>
        protected List<SettingDescriptor> BuildSettingDescriptors()
        {
            var settingDescriptors = new List<SettingDescriptor>();
            var typeInfo = this.GetType().GetTypeInfo();
            var properties = typeInfo.DeclaredProperties;
            var descriptorsHaveOrder = false;

            using (logger.BeginScope($"BuildSettingDescriptors"))
            {
                logger.LogDebug($"Iterating through {properties.Count()} properties");
                foreach (var property in properties)
                {
                    logger.LogDebug($"Found property [{property}]");
                    var settingDescriptor = property.GetCustomAttribute<SettingDescriptor>();
                    if (settingDescriptor != null)
                    {
                        logger.LogDebug($"Property [{property}] has a settingDescriptor [{settingDescriptor}]");
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
                            settingDescriptor.DefaultValue = property.GetValue(this);

                        // If at least one setting descriptor has a set order, we need to sort the collection
                        if (settingDescriptor.Order != 0)
                            descriptorsHaveOrder = true;

                        settingDescriptors.Add(settingDescriptor);
                    }
                    else
                    {
                        logger.LogWarning($"Property [{property}] doesn't have a SettingDescriptor");
                    }
                }

                if (descriptorsHaveOrder)
                    settingDescriptors = settingDescriptors.OrderBy(o => o.Order).ToList();
            }

            return settingDescriptors;
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

            using (logger.BeginScope($"GetChangedProperties"))
            {
                logger.LogDebug($"Iterating through {this.settingDescriptors.Count} settingDescriptors");
                foreach (var settingDescriptor in this.settingDescriptors)
                {
                    logger.LogDebug($"Getting current value for [{settingDescriptor.Property}]");
                    var value = settingDescriptor.Property.GetValue(this);
                    logger.LogTrace($"[{settingDescriptor.Property}] value is [{value}]");
                    if (!value.Equals(settingDescriptor.DefaultValue))
                    {
                        logger.LogDebug($"[{settingDescriptor.Property}] value changed. Current:[{value}], Default:[{settingDescriptor.DefaultValue}]");
                        result.Add(settingDescriptor.Name, value.ToString());
                    }
                }
            }
            return result.OrderBy(c => c.Key).ToDictionary((keyItem) => keyItem.Key, (valueItem) => valueItem.Value);
        }

        /// <summary>
        /// The SetProperties method is an internal protected method that takes a dictionary of persisted
        /// settings and assigns those settings to the appropriate properties to rehydrate the property
        /// values.
        /// </summary>
        /// <param name="settings">The settings collection containing the values to assign to the properties.</param> <summary>
        /// <exception cref="ArgumentNullException">Thrown if the paramref name="settings" is null.</exception>
        protected internal void SetProperties(Dictionary<string, string> settings)
        {
            if (settings == null)
                throw new ArgumentNullException(nameof(settings));

            using (logger.BeginScope($"SetProperties"))
            {
                logger.LogDebug($"Iterating through {settings.Count} settings");
                foreach (var setting in settings)
                {
                    logger.LogTrace($"Getting SettingDescriptor for {setting.Key}");
                    var settingDescriptor = this.SettingDescriptors.Where(x => x.Name == setting.Key).First();
                    // Ignore the setting value if not found - this accounts for old properties
                    if (settingDescriptor != null)
                    {
                        logger.LogDebug($"Found [{settingDescriptor.Property}] settingDescriptor for {setting.Key}");
                        object value;
                        if (settingDescriptor.Property.PropertyType.GetTypeInfo().IsEnum)
                        {
                            logger.LogDebug("Property is an enum");
                            value = Enum.Parse(settingDescriptor.Property.PropertyType, setting.Value);
                        }
                        else
                        {
                            logger.LogDebug($"Property is {settingDescriptor.Property.PropertyType}");
                            value = Convert.ChangeType(setting.Value, settingDescriptor.Property.PropertyType);
                        }
                        logger.LogTrace($"Setting [{settingDescriptor.Property}] to value [{value}]");
                        settingDescriptor.Property.SetValue(this, value);
                    }
                    else
                    {
                        logger.LogWarning("SettingDescriptor not found for setting [{setting.Key}]");
                    }
                }
            }
        }
    }
}
