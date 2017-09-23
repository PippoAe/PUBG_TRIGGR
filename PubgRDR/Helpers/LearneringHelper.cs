using numl;
using numl.Model;
using numl.Supervised.DecisionTree;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PubgTriggr
{
    class LearningHelper
    {
        private List<ScreenData> data;
        private DecisionTreeModel model;

        public LearningHelper(string modelToLoadPath = null)
        {
            data = new List<ScreenData>();
            if (modelToLoadPath != null)
                model = (DecisionTreeModel)numl.Utils.Xml.Load(modelToLoadPath, typeof(DecisionTreeModel));
        }

        public void AddData(ScreenData screendata)
        {
            data.Add(screendata);
        }

        public double LearnFromData(int iterations)
        {
            var d = Descriptor.Create<ScreenData>();
            var g = new DecisionTreeGenerator(d);
            g.SetHint(false);

            var mdl = Learner.Learn(data, 0.80, iterations, g);
            model = (DecisionTreeModel)mdl.Model;
            return mdl.Accuracy;
        }

        public bool CheckForKill(ScreenData values)
        {
            model.Predict(values);
            return values.Kill;
        }

        public void SaveModel(string modelPath)
        {
            model.Save(modelPath);
        }
    }
}
