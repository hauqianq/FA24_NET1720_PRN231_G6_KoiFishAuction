using System.Linq.Expressions;

namespace KoiFishAuction.Common.Helpers; 
public static class PredicateBuilder {
    public static Expression<Func<T, bool>> AndAlso<T>(this Expression<Func<T, bool>> expr1, Expression<Func<T, bool>> expr2) {
        var parameter = Expression.Parameter(typeof(T));

        var body = Expression.AndAlso(
            Expression.Invoke(expr1, parameter),
            Expression.Invoke(expr2, parameter)
        );

        return Expression.Lambda<Func<T, bool>>(body, parameter);
    }
}

public class DateTimeExtensions {
    public static DateTime StartOfDay(DateTime date) {
        return new DateTime(date.Year, date.Month, date.Day, 0, 0, 0);
    }

    public static DateTime EndOfDay(DateTime date) {
        return new DateTime(date.Year, date.Month, date.Day, 23, 59, 59);
    }
}

