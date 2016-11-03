using System;

namespace RMarket.WebUI.Abstract
{
    public interface IEntityParamConverter<T>
    {
        string ToViewModel(T value);
        T ToDomainModel(string strValue, Type typeValue);
    }

}