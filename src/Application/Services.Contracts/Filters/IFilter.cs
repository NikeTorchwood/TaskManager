using System.Linq.Expressions;

namespace Services.Contracts.Filters;

public interface IFilter<T>
{
    public Expression<Func<T, bool>> Apply();
}