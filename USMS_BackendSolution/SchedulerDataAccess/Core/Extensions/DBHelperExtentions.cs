using System.Data;

namespace ISUZU_NEXT.Server.Core.Extentions
{
    public static class DBHelperExtentions
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="propertyName"></param>
        /// <returns></returns>
        public static object? GetProperty<T>(this T data,string propertyName)
        {
            var property = data?.GetType()?.GetProperty(propertyName);
            if (property == null)
            {
                return null;
            }

            return property.GetValue(data);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="propertyName"></param>
        /// <param name="value"></param>
        public static void SetProperty<T>(this T data, string propertyName, object? value)
        {
            var property = data ?.GetType().GetProperty(propertyName);
            if (property == null)
            {
                return;
            }

            var oldValue = data.GetProperty(propertyName);
            if (object.Equals(value, oldValue))
            {
                return;
            }

            var propertyType = Nullable.GetUnderlyingType(property.PropertyType) ?? property.PropertyType;
            try
            {
                var safeValue = (value == null) ? null : Convert.ChangeType(value, propertyType);
                property.SetValue(data, safeValue);
            }
            catch
            {
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="model"></param>
        /// <param name="value"></param>
        public static void CopyProperties<TModel, KModel>(this TModel model, KModel value)
        {
            if (model == null || value == null)
            {
                return;
            }

            var destProperties = model.GetType().GetProperties()
                                        .Where(p => p.CanWrite)
                                        .ToDictionary(p => p.Name);

            foreach (var property in value.GetType().GetProperties())
            {
                if (destProperties.TryGetValue(property.Name, out var destProperty))
                {
                    if (destProperty.PropertyType == property.PropertyType)
                    {
                        destProperty.SetValue(model, property.GetValue(value));
                    }
                }
            }
        }
    }
}
