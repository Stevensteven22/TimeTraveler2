using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq.Expressions;
using System.Threading.Tasks;
using TimeTraveler.Libary.Definitions;
using TimeTraveler.Libary.Models;
using TimeTraveler.Libary.Services;

namespace TimeTraveler.Services;

public class ElementalService : IElementalService
{
    private readonly IBuffStorage _buffStorage;

    public ElementalService(IBuffStorage buffStorage)
    {
        _buffStorage = buffStorage;
    }

    public async Task InsertOrUpdateElementalAsync(ObservableCollection<ResultModel> args)
    {
        List<Buff> buffs = new List<Buff>();
        foreach (var resultModel in args)
        {
            if (!resultModel.IsSelected)
                continue;

            var existingBuff = await _buffStorage.GetBuffAsync(x => x.Name == resultModel.Name);
            if (existingBuff == null)
            {
                //在默认初始化值中加成
                switch (resultModel.ResultElementType)
                {
                    case ElementType.FireElemental:
                        buffs.Add(
                            new Buff()
                            {
                                Id = resultModel.Id,
                                Name = resultModel.Name,
                                Description = resultModel.Description,
                                Value1 = resultModel.ImprovedValue1,
                                Value2 = 100.0d * (1.0d + (resultModel.ImprovedValue2 / 100.0d)),
                            }
                        );
                        break;
                    case ElementType.IceElemental:
                        buffs.Add(
                            new Buff()
                            {
                                Id = resultModel.Id,
                                Name = resultModel.Name,
                                Description = resultModel.Description,
                                Value1 = resultModel.ImprovedValue1,
                                Value2 = 1000.0d * (1.0d + (resultModel.ImprovedValue2 / 100.0d)),
                            }
                        );
                        break;
                    case ElementType.RockElemental:
                        buffs.Add(
                            new Buff()
                            {
                                Id = resultModel.Id,
                                Name = resultModel.Name,
                                Description = resultModel.Description,
                                Value1 = resultModel.ImprovedValue1,
                                Value2 = 200.0d * (1.0d + (resultModel.ImprovedValue2 / 100.0d)),
                            }
                        );
                        break;
                    case ElementType.ThunderElemental:
                        buffs.Add(
                            new Buff()
                            {
                                Id = resultModel.Id,
                                Name = resultModel.Name,
                                Description = resultModel.Description,
                                Value1 = resultModel.ImprovedValue1,
                            }
                        );
                        break;
                    case ElementType.WindElemental:
                        buffs.Add(
                            new Buff()
                            {
                                Id = resultModel.Id,
                                Name = resultModel.Name,
                                Description = resultModel.Description,
                                Value1 = resultModel.ImprovedValue1,
                            }
                        );
                        break;
                }
            }
            else
            {
                //修改：在之前的基础上属性加成
                switch (resultModel.ResultElementType)
                {
                    case ElementType.FireElemental:
<<<<<<< HEAD
                        existingBuff.Value2 =
                            existingBuff.Value2 * (1.0d + (resultModel.ImprovedValue2 / 100.0d));
                        await _buffStorage.SaveBuffsAsync(existingBuff);
                        break;
                    case ElementType.IceElemental:
                        existingBuff.Value2 =
                            existingBuff.Value2 * (1.0d + (resultModel.ImprovedValue2 / 100.0d));
                        await _buffStorage.SaveBuffsAsync(existingBuff);
                        break;
                    case ElementType.RockElemental:
                        existingBuff.Value2 =
                            existingBuff.Value2 * (1.0d + (resultModel.ImprovedValue2 / 100.0d));
                        await _buffStorage.SaveBuffsAsync(existingBuff);
                        break;
                    case ElementType.ThunderElemental:
                        existingBuff.Value1 = existingBuff.Value1 + resultModel.ImprovedValue1;
                        await _buffStorage.SaveBuffsAsync(existingBuff);
                        break;
                    case ElementType.WindElemental:
                        existingBuff.Value1 = existingBuff.Value1 + resultModel.ImprovedValue1;
=======
                       
                   
                  
                }
            }

            if (buffs.Count > 0)
                await _buffStorage.AddBuffsAsync(buffs.ToArray());
        }
    }

    public async Task<ResultModel> GetElementalAsync(Expression<Func<ResultModel, bool>> predicate)
    {
        var buffPredicate = Expression.Lambda<Func<Buff, bool>>(
            predicate.Body,
            Expression.Parameter(typeof(Buff), predicate.Parameters[0].Name)
        );
        var existingBuff = await _buffStorage.GetBuffAsync(buffPredicate);
        if (existingBuff == null)
            return null;

        ResultModel resultModel = new ResultModel()
        {
            Id = existingBuff.Id,
            Name = existingBuff.Name,
            Description = existingBuff.Description,
            ResultElementType = BuffNameConvertToElementalType(existingBuff.Name),
            IsSelected = false,
            ActualValue1 = existingBuff.Value1,
            ActualValue2 = existingBuff.Value2,
        };
        return resultModel;
    }

    public ElementType BuffNameConvertToElementalType(string buffName)
    {
        switch (buffName)
        {
            case "火元素":
                return ElementType.FireElemental;
            case "冰元素":
                return ElementType.IceElemental;
            case "岩元素":
                return ElementType.RockElemental;
            case "雷元素":
                return ElementType.ThunderElemental;
            case "风元素":
                return ElementType.WindElemental;
        }
        return ElementType.FireElemental;
    }
}
