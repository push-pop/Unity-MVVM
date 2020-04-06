

using System.Reflection;

public static class MemberInfoExt
{
    public static object GetValue(string path, object owner)
    {
        var member = owner.GetType().GetMember(path);
        if (member.Length > 0)
            return member[0].GetValue(owner);
        else
            throw new System.Exception("Can't find member" + member);
    }

    public static object GetValue(this MemberInfo member, object owner)
    {
        if (member.MemberType.Equals(MemberTypes.Property))
            return (member as PropertyInfo).GetValue(owner);
        else if (member.MemberType.Equals(MemberTypes.Field))
            return (member as FieldInfo).GetValue(owner);
        else
            throw new System.Exception("Member is not property or field" + member);
    }
}
