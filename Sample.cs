using System;
using System.Reflection;
using System.Windows.Interop;
using System.Diagnostics;
using Autodesk.Fabrication.UI;
using Autodesk.AutoCAD.Runtime;
using Autodesk.Fabrication.DB;
using System.Windows.Threading;
using System.Linq;
using System.IO;

[assembly: ExtensionApplication(typeof(FabricationSample.ACADSample))]

namespace FabricationSample
{
  public class Sample : IExternalApplication
  {
    FabricationWindow win = null;
    public Sample()
    {
    }

    public void Execute()
    {
      win = new FabricationWindow();
      WindowInteropHelper wih = new WindowInteropHelper(win);
      wih.Owner = Process.GetCurrentProcess().MainWindowHandle;

      win.ShowDialog();
    }

    public void Terminate()
    {
      Database.Clear();
      ProductDatabase.Clear();

      Dispatcher.CurrentDispatcher.InvokeShutdown();
      
      win.Close();
    }
  }

  public class ACADSample : IExtensionApplication
  {
    FabricationWindow _win = null;

    [CommandMethod("FabAPI", "FabAPI", CommandFlags.Modal)]
    public void RunFabApi()
    {
      if (CheckCadMepLoaded() && CheckApiLoaded())
      {
        _win = new FabricationWindow();
        _win.ShowDialog();
      }
    }

    public void Initialize()
    {
      CheckCadMepLoaded();
      CheckApiLoaded();
    }

    public void Terminate()
    {
       Database.Clear();
       ProductDatabase.Clear();

       Dispatcher.CurrentDispatcher.InvokeShutdown();
    }

    #region Fabrication API Checking routines
    private bool CheckApiLoaded()
    {
      try
      {
        var fabApi = AppDomain.CurrentDomain.GetAssemblies()
          .Where(x => !x.IsDynamic)
          .FirstOrDefault(x => Path.GetFileName(x.Location).Equals("FabricationAPI.dll", StringComparison.OrdinalIgnoreCase));

        var loaded = fabApi != null;

        if (!loaded)
        {
          var fabCore = AppDomain.CurrentDomain.GetAssemblies()
            .Where(x => !x.IsDynamic)
            .FirstOrDefault(x => Path.GetFileName(x.Location).Equals("FabricationCore.dll", StringComparison.OrdinalIgnoreCase));

          if (fabCore != null)
          {
            var directory = Path.GetDirectoryName(fabCore.Location);
            fabApi = Assembly.LoadFrom(Path.Combine(directory, "FabricationAPI.dll"));
            loaded = fabApi != null;
          }
        }

        if (!loaded)
        {
          System.Windows.Forms.MessageBox.Show("FabricationAPI.dll could not be loaded", "Fabrication API",
            System.Windows.Forms.MessageBoxButtons.OK, System.Windows.Forms.MessageBoxIcon.Exclamation);
        }

        return loaded;
      }
      catch (System.Exception e)
      {
        return false;
      }
    }

    private bool CheckCadMepLoaded()
    {
      var modules = SystemObjects.DynamicLinker.GetLoadedModules().Cast<string>().ToList();
      var cadMepLoaded = modules
        .Where(x => x.ToLower().Contains("cadmep"))
        .Where(x => x.ToLower().Contains(".arx"))
        .Any();

      if (!cadMepLoaded)
        System.Windows.Forms.MessageBox.Show("CADmep is not loaded and is required to run this addin", "Fabrication API",
          System.Windows.Forms.MessageBoxButtons.OK, System.Windows.Forms.MessageBoxIcon.Exclamation);

      return cadMepLoaded;
    }
    #endregion

  }
}

