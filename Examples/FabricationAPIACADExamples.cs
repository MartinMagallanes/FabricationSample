using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Drawing;
using System.IO;

using Autodesk.Fabrication;
using Autodesk.Fabrication.DB;
using Autodesk.Fabrication.Results;
using Autodesk.Fabrication.Geometry;

using Autodesk.AutoCAD.ApplicationServices;
using Autodesk.AutoCAD.DatabaseServices;
using Autodesk.AutoCAD.EditorInput;
using Autodesk.AutoCAD.Runtime;

namespace FabricationSample.FunctionExamples
{
  class FabricationAPIACADExamples
  {
    #region GetACADHandleForItem

    /// <summary>
    /// Gets the ACAD Handle in string format from the Fabrication Item.
    /// </summary>
    /// <param name="item">The Fabrication Item to get the ACAD handle from</param>
    /// <returns>ACAD Handle in string format.</returns>
    public static string GetACADHandleForItem(Item item)
    {
      //Get Handle from Fabrication Job Item
      string handle = Job.GetACADHandleFromItem(item);

      if (!string.IsNullOrEmpty(handle))
      {
        Document doc = Autodesk.AutoCAD.ApplicationServices.Application.DocumentManager.MdiActiveDocument;
        Editor ed = doc.Editor;
        Autodesk.AutoCAD.DatabaseServices.Database db = doc.Database;
        
        //Get ACAD Handle from string
        long ln = Int64.Parse(handle, System.Globalization.NumberStyles.HexNumber);
        Handle hn = new Handle(ln);
        //Locate ObjectId from Handle
        ObjectId id = db.GetObjectId(false, hn, 0);

        //Select the Entity in the ACAD drawing
        if (!id.IsNull || !id.IsErased)
          ed.SetImpliedSelection(new ObjectId[] { id });

      }

      return handle;
    }

    #endregion

    #region GetItemFromACADHandle

    /// <summary>
    /// Gets the Fabrication Item from the ACAD Handle.
    /// </summary>
    /// <param name="handle">The AutoCAD Object Handle in string format.</param>
    /// <returns>The matching Fabrication Item to the Handle, or null if not found.</returns>
    public static Item GetItemFromACADHandle(string handle)
    {
      return Job.GetFabricationItemFromACADHandle(handle);
    }

    #endregion

  }

}
