using RMarket.ClassLib.Helpers.Extentions;
using RMarket.ClassLib.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RMarket.ClassLib.Helpers
{
    public static class OptimizationHelper
    {
        public static List<InstanceModel> CreateFirstGeneration(SelectionModel selection, int multiplier = 1)
        {
            List<InstanceModel> res = new List<InstanceModel>();

            List<ParamSelection> integralParams = selection.SelectionParams.Where(p => p.ValueMin.IsIntegral() && (dynamic)p.ValueMax > (dynamic)p.ValueMin).ToList();
            List<ParamEntity> otherParams = selection.SelectionParams.Where(p => !p.ValueMin.IsIntegral()).Select(p => {
                var param = new ParamEntity { FieldValue = p.ValueMin };
                param.FillFields(p);
                return param;
            }
            ).ToList();

            Dictionary<string, int[]> fieldResults = new Dictionary<string, int[]>();

            //заполняеем всеми возможными значениями
            foreach (ParamSelection paramSelection in integralParams)
            {
                fieldResults.Add(paramSelection.FieldName,
                    ((IEnumerable<int>)Enumerable.Range(0, (dynamic)paramSelection.ValueMax - (dynamic)paramSelection.ValueMin+1)).ToArray()
                    );

            }

            foreach (KeyValuePair<string, int[]> pair in fieldResults)
            {
                for (int j = 0; j < multiplier; j++)
                {
                    foreach (int i in pair.Value)
                    {
                        InstanceModel newInstance = new InstanceModel();
                        newInstance.FillFields(selection);
                        newInstance.SelectionId = selection.Id;
                        
                        newInstance.StrategyParams.AddRange(otherParams);

                        foreach (ParamSelection paramSelection in integralParams)
                        {
                            ParamEntity newParam = new ParamEntity();
                            newParam.FillFields(paramSelection);
                            if (paramSelection.FieldName == pair.Key)
                            {
                                newParam.FieldValue = (dynamic)paramSelection.ValueMin + i;
                            }
                            else
                            {
                                Random rnd = new Random();
                                newParam.FieldValue = rnd.Next(paramSelection.ValueMin.ToIntSave(), paramSelection.ValueMax.ToIntSave());
                            }

                            newInstance.StrategyParams.Add(newParam);

                        }

                        res.Add(newInstance);
                    }
                }
            }

            return res;
        }

    }
}
