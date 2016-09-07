using System;

namespace RMarket.WebUI.Abstract
{
    public interface IEntityParamConverter<T>
    {
        T ToDomainModel(string strValue);
        string ToViewModel(T value);
    }

}