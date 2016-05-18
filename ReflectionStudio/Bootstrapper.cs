using Microsoft.Practices.Prism.MefExtensions;
using System;
using System.Collections.Generic;
using System.ComponentModel.Composition.Hosting;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading;
using System.Windows;
using ReflectionStudio.Common;

namespace ReflectionStudio
{
    /// <summary>
    /// 初始化
    /// </summary>
    public class Bootstrapper : MefBootstrapper
    {
        public static readonly Mutex Mutex = new Mutex(true, "{D22EEFB1-DA69-40E7-BCCF-12C1C769A60B}");
        protected override DependencyObject CreateShell()
        {
            return Container.GetExportedValue<Shell>();
        }

        protected override void ConfigureAggregateCatalog()
        {
            base.ConfigureAggregateCatalog();
            string directoryName = Path.GetDirectoryName(Application.ResourceAssembly.Location);
            if (directoryName != null)
            {

                //AggregateCatalog.Catalogs.Add(new DirectoryCatalog(directoryName, "SecurityFire.Persistance.dll"));
                //AggregateCatalog.Catalogs.Add(new DirectoryCatalog(directoryName + "\\Modules\\", "SecurityFire.Modules*"));
                //AggregateCatalog.Catalogs.Add(new DirectoryCatalog(directoryName, "SecurityFire.Modules*"));
                AggregateCatalog.Catalogs.Add(new DirectoryCatalog(directoryName, "ReflectionStudio*"));
                //AggregateCatalog.Catalogs.Add(new DirectoryCatalog(directoryName, "SecurityFire.Services*"));
                //IEnumerable<Assembly> enumerable =
                //    from file in Directory.GetFiles(LocalSettings.AddonPath, "SecurityFire.Addon*")
                //    select Assembly.Load(File.ReadAllBytes(file));
                //foreach (Assembly current in enumerable)
                //{
                //    base.AggregateCatalog.Catalogs.Add(new AssemblyCatalog(current));
                //}
            }
        }

        protected override void InitializeModules()
        {
            base.InitializeModules();
        }
        protected override void InitializeShell()
        {
            try
            {
                if (!Bootstrapper.Mutex.WaitOne(TimeSpan.Zero, true))//&& !LocalSettings.AllowMultipleClients
                {
                    NativeWin32.PostMessage((IntPtr)65535, NativeWin32.WM_SHOWSAMBAPOS, IntPtr.Zero, IntPtr.Zero);
                    Environment.Exit(1);
                }
            }
            catch (AbandonedMutexException)
            {
            }
            Application.Current.ShutdownMode = ShutdownMode.OnMainWindowClose;
            ServicePointManager.Expect100Continue = false;
            base.InitializeShell();
            Application.Current.MainWindow = (Shell)Shell;
            Application.Current.MainWindow.Show();
            //激活窗体
            //Application.Current.MainWindow.Activate();
        }
    }
}