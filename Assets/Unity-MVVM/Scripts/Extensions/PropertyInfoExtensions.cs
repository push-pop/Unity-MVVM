using System.Linq;
using System.Reflection;

public static class PropertyInfoExtensions
{
    public static PropertyInfo ResolvePath(this PropertyInfo prop, string path)
    {
        if (string.IsNullOrEmpty(path))
            return prop;

        var parts = path.Split('.');
        return parts.Length > 1 ?
        prop.PropertyType.GetProperty(parts[0]).ResolvePath(parts.Skip(1).Aggregate((a, i) => a + "." + i))
            : prop.PropertyType.GetProperty(parts[0]);
    }

    public static void SetValue(this PropertyInfo prop, object owner, object value, string path = null, object[] index = null)
    {
        if (string.IsNullOrEmpty(path))
            prop.SetValue(owner, value);
        else
        {
            var parts = path.Split('.');
            for (int i = 0; i < parts.Length - 1; i++)
            {
                var propInfo = owner.GetType().GetProperty(parts[i]);
                owner = propInfo.GetValue(owner, null);
            }

            var toSet = owner.GetType().GetProperty(parts.Last());
            toSet.SetValue(owner, value, null);
        }
    }

    public static object GetValue(this PropertyInfo prop, object owner, string path = null, object[] index = null)
    {
        if (string.IsNullOrEmpty(path))
            return prop.GetValue(owner, index);

        var parts = path.Split('.');
        foreach (var part in parts)
        {
            var p = prop.GetValue(owner);

            if (p == null) return null;

            owner = p.GetType();
            prop = owner.GetType().GetProperty(part);
        }

        return prop.GetValue(owner, index);
    }

}
