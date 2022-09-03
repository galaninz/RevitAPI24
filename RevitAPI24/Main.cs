using Autodesk.Revit.Attributes;
using Autodesk.Revit.DB;
using Autodesk.Revit.DB.Mechanical;
using Autodesk.Revit.UI;
using Autodesk.Revit.UI.Selection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RevitAPI24
{
    [Transaction(TransactionMode.Manual)]
    public class Main : IExternalCommand
    {
        public Result Execute(ExternalCommandData commandData, ref string message, ElementSet elements)
        {
            UIApplication uiapp = commandData.Application;
            UIDocument uidoc = uiapp.ActiveUIDocument;
            Document doc = uidoc.Document;

            Reference selectedLevelRef = uidoc.Selection.PickObject(ObjectType.Element, "Выберите уровень");
            Element level = doc.GetElement(selectedLevelRef);

            var ducts = new FilteredElementCollector(doc)
                .OfClass(typeof(Duct))
                .Cast<Duct>();

            List<Duct> ductsList = new List<Duct>();

            foreach (var duct in ducts)
            {
                if (duct.LevelId == level.LevelId)
                    ductsList.Add(duct);
            }

            TaskDialog.Show("Selection", $"Количество воздуховодов на выбранном этаже {ductsList.Count}");

            return Result.Succeeded;
        }
    }
}
