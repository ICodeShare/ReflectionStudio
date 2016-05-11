using System;
using System.Collections.Generic;
using System.ComponentModel.Composition;
using System.Configuration;
using System.Data;
using System.IO;
using System.Linq;
using System.Windows;
using System.Collections.ObjectModel;

namespace ReflectionStudio
{
    /// <summary>
    /// App.xaml 的交互逻辑
    /// </summary>
    public partial class App : Application
    {
        protected override void OnStartup(StartupEventArgs e)
        {
            base.OnStartup(e);
#if (DEBUG)
            RunInDebugMode();
#else
            RunInReleaseMode();
#endif
        }

        /// <summary>
        /// DeBug模式运行
        /// </summary>
        private static void RunInDebugMode()
        {
            Bootstrapper bootstrapper = new Bootstrapper();
            bootstrapper.Run();
        }

        /// <summary>
        /// Release模式运行
        /// </summary>
        private static void RunInReleaseMode()
        {
            AppDomain.CurrentDomain.UnhandledException += new UnhandledExceptionEventHandler(App.AppDomainUnhandledException);
            try
            {
                Bootstrapper bootstrapper = new Bootstrapper();
                bootstrapper.Run();
            }
            catch (ConfigurationErrorsException ex) //配置错误
            {
                App.HandleConfigurationError(ex);
            }
            //catch (CompositionException ex3)  //插件错误
            //{
            //    ConfigurationErrorsException ex2 = ex3.RootCauses.First<Exception>().InnerException as ConfigurationErrorsException;
            //    if (ex2 != null)
            //    {
            //        App.HandleConfigurationError(ex2);
            //    }
            //    else
            //    {
            //        App.HandleException(ex3);
            //    }
            //}
            catch (Exception ex4) //常规错误
            {
                App.HandleException(ex4);
            }
        }

        /// <summary>
        /// 处理配置错误
        /// </summary>
        /// <param name="ex"></param>
        private static void HandleConfigurationError(ConfigurationErrorsException ex)
        {
            try
            {
                ConfigurationErrorsException ex2 = ex.InnerException as ConfigurationErrorsException;
                string path = (ex2 != null) ? ex2.Filename : ex.Filename;
                if (File.Exists(path))
                {
                    File.Delete(path);
                }
            }
            finally
            {
                App.HandleException(ex);
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private static void AppDomainUnhandledException(object sender, UnhandledExceptionEventArgs e)
        {
            App.HandleException(e.ExceptionObject as Exception);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="ex"></param>
        private static void HandleException(Exception ex)
        {
            if (ex == null)
            {
                return;
            }
            //ExceptionReporter.Show(new Exception[]
            //{
            //    ex
            //});
            Environment.Exit(1);
        }
    }
}
