using System.Data;
using Dapper;

namespace Bookify.Infrastructure.Data;

internal sealed class DateOnlyTypeHandler : SqlMapper.TypeHandler<DateOnly>
{
    public override DateOnly Parse( object value )
    {
        return value switch
        {
            DateTime dateTime => DateOnly.FromDateTime( dateTime ),
            DateOnly dateOnly => dateOnly,
            _ => throw new DataException( $"Cannot convert {value.GetType()} to DateOnly" )
        };
    }

    public override void SetValue( IDbDataParameter parameter, DateOnly value )
    {
        parameter.DbType = DbType.Date;
        parameter.Value = value;
    }
}
