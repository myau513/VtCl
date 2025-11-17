using Dapper;
using System;
using System.Data;

public class GuidTypeHandler : SqlMapper.TypeHandler<Guid>
{
    public override void SetValue(IDbDataParameter parameter, Guid value)
    {
        parameter.Value = value.ToString();
    }

    public override Guid Parse(object value)
    {
        if (value is int intValue)
        {
            // Конвертируем int в Guid
            byte[] bytes = new byte[16];
            BitConverter.GetBytes(intValue).CopyTo(bytes, 0);
            return new Guid(bytes);
        }

        if (value is string stringValue)
        {
            return Guid.Parse(stringValue);
        }

        return Guid.Parse(value.ToString());
    }
}
