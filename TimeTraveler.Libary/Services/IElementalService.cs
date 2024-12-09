using System.Collections.ObjectModel;
using System.Linq.Expressions;
using TimeTraveler.Libary.Definitions;
using TimeTraveler.Libary.Models;

namespace TimeTraveler.Libary.Services;

public interface IElementalService
{
    public Task InsertOrUpdateElementalAsync(ObservableCollection<ResultModel> args);

    public Task<ResultModel> GetElementalAsync(Expression<Func<ResultModel, bool>> predicate);
}