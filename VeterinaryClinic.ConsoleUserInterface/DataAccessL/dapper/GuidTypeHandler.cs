using Dapper;
using System;
using System.Data;

public class GuidTypeHandler : SqlMapper.TypeHandler<Guid>
{
    /// <summary>
    /// Устанавливает значение параметра для GUID
    /// </summary>
    /// <param name="parameter">Параметр базы данных</param>
    /// <param name="value">Значение GUID</param>
    public override void SetValue(IDbDataParameter parameter, Guid value)
    {
        parameter.Value = value.ToString();
    }

    /// <summary>
    /// Преобразует значение из базы данных в GUID
    /// </summary>
    /// <param name="value">Значение из базы данных</param>
    /// <returns>Преобразованный GUID</returns>
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