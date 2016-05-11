//using Infrastructure.Settings;
using Infrastructure.Settings;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Globalization;
using System.Reflection;
using System.Resources;
using System.Runtime.CompilerServices;
using System.Threading;
using System.Windows;
////// Register this namespace under admirals one with prefix
//[assembly: XmlnsDefinition("http://schemas.root-project.org/xaml/presentation", "Samba.Localization.Engine")]
//[assembly: XmlnsDefinition("http://schemas.root-project.org/xaml/presentation", "Samba.Localization.Extensions")]
////// Assign a default namespace prefix for the schema
//[assembly: XmlnsPrefix("http://schemas.root-project.org/xaml/presentation", "ln")]

[assembly: System.Windows.Markup.XmlnsDefinition("http://schemas.microsoft.com/winfx/2006/xaml/presentation", "Localization.Engine")]
[assembly: System.Windows.Markup.XmlnsDefinition("http://schemas.microsoft.com/winfx/2007/xaml/presentation", "Localization.Engine")]
[assembly: System.Windows.Markup.XmlnsDefinition("http://schemas.microsoft.com/winfx/2008/xaml/presentation", "Localization.Engine")]

[assembly: System.Windows.Markup.XmlnsDefinition("http://schemas.microsoft.com/winfx/2006/xaml/presentation", "Localization.Extensions")]
[assembly: System.Windows.Markup.XmlnsDefinition("http://schemas.microsoft.com/winfx/2007/xaml/presentation", "Localization.Extensions")]
[assembly: System.Windows.Markup.XmlnsDefinition("http://schemas.microsoft.com/winfx/2008/xaml/presentation", "Localization.Extensions")]
namespace Localization.Engine
{
	public sealed class LocalizeDictionary : DependencyObject
	{
		public sealed class WeakCultureChangedEventManager : WeakEventManager
		{
			private bool isListening;
			private WeakEventManager.ListenerList listeners;
			private static LocalizeDictionary.WeakCultureChangedEventManager CurrentManager
			{
				get
				{
					Type typeFromHandle = typeof(LocalizeDictionary.WeakCultureChangedEventManager);
					LocalizeDictionary.WeakCultureChangedEventManager weakCultureChangedEventManager = (LocalizeDictionary.WeakCultureChangedEventManager)WeakEventManager.GetCurrentManager(typeFromHandle);
					if (weakCultureChangedEventManager == null)
					{
						weakCultureChangedEventManager = new LocalizeDictionary.WeakCultureChangedEventManager();
						WeakEventManager.SetCurrentManager(typeFromHandle, weakCultureChangedEventManager);
					}
					return weakCultureChangedEventManager;
				}
			}
			private WeakCultureChangedEventManager()
			{
				this.listeners = new WeakEventManager.ListenerList();
			}
			public static void AddListener(IWeakEventListener listener)
			{
				LocalizeDictionary.WeakCultureChangedEventManager.CurrentManager.listeners.Add(listener);
				LocalizeDictionary.WeakCultureChangedEventManager.CurrentManager.StartStopListening();
			}
			public static void RemoveListener(IWeakEventListener listener)
			{
				LocalizeDictionary.WeakCultureChangedEventManager.CurrentManager.listeners.Remove(listener);
				LocalizeDictionary.WeakCultureChangedEventManager.CurrentManager.StartStopListening();
			}
			[MethodImpl(MethodImplOptions.Synchronized)]
			protected override void StartListening(object source)
			{
				if (!this.isListening)
				{
					LocalizeDictionary.Instance.OnCultureChanged += new Action(this.Instance_OnCultureChanged);
					this.isListening = true;
				}
			}
			[MethodImpl(MethodImplOptions.Synchronized)]
			protected override void StopListening(object source)
			{
				if (this.isListening)
				{
					LocalizeDictionary.Instance.OnCultureChanged -= new Action(this.Instance_OnCultureChanged);
					this.isListening = false;
				}
			}
			private void Instance_OnCultureChanged()
			{
				base.DeliverEventToList(LocalizeDictionary.Instance, EventArgs.Empty, this.listeners);
			}
			[MethodImpl(MethodImplOptions.Synchronized)]
			private void StartStopListening()
			{
				if (this.listeners.Count != 0)
				{
					if (!this.isListening)
					{
						this.StartListening(null);
						return;
					}
				}
				else
				{
					if (this.isListening)
					{
						this.StopListening(null);
					}
				}
			}
		}
		public const string ResourcesName = "Resources";
		private const string ResourceManagerName = "ResourceManager";
		private const string ResourceFileExtension = ".resources";
		private const BindingFlags ResourceBindingFlags = BindingFlags.Static | BindingFlags.Public | BindingFlags.NonPublic;
		[DesignOnly(true)]
		public static readonly DependencyProperty DesignCultureProperty = DependencyProperty.RegisterAttached("DesignCulture", typeof(string), typeof(LocalizeDictionary), new PropertyMetadata(new PropertyChangedCallback(LocalizeDictionary.SetCultureFromDependencyProperty)));
		private static readonly object SyncRoot = new object();
		private static LocalizeDictionary instance;
		private CultureInfo culture;
		public event Action OnCultureChanged;
		public static CultureInfo DefaultCultureInfo
		{
			get
			{
				return CultureInfo.InvariantCulture;
			}
		}
		public static LocalizeDictionary Instance
		{
			get
			{
				if (LocalizeDictionary.instance == null)
				{
					lock (LocalizeDictionary.SyncRoot)
					{
						if (LocalizeDictionary.instance == null)
						{
							LocalizeDictionary.instance = new LocalizeDictionary();
						}
					}
				}
				return LocalizeDictionary.instance;
			}
		}
		public CultureInfo Culture
		{
			get
			{
				if (this.culture == null)
				{
					this.culture = Thread.CurrentThread.CurrentUICulture;
				}
				return this.culture;
			}
			set
			{
				if (value == null)
				{
					throw new ArgumentNullException("value");
				}
				this.culture = value;
				if (this.OnCultureChanged != null)
				{
					this.OnCultureChanged();
				}
			}
		}
		public Dictionary<string, ResourceManager> ResourceManagerList
		{
			get;
			private set;
		}
		public CultureInfo SpecificCulture
		{
			get
			{
				return CultureInfo.CreateSpecificCulture(this.Culture.ToString());
			}
		}
		private LocalizeDictionary()
		{
			this.ResourceManagerList = new Dictionary<string, ResourceManager>();
		}
		[DesignOnly(true)]
		public static string GetDesignCulture(DependencyObject obj)
		{
			if (LocalizeDictionary.Instance.GetIsInDesignMode())
			{
				return (string)obj.GetValue(LocalizeDictionary.DesignCultureProperty);
			}
			return LocalizeDictionary.Instance.Culture.ToString();
		}
		public static void ParseKey(string inKey, out string outAssembly, out string outDict, out string outKey)
		{
			outAssembly = null;
			outDict = null;
			outKey = null;
			if (!string.IsNullOrEmpty(inKey))
			{
				string[] array = inKey.Trim().Split(":".ToCharArray(), 3);
				if (array.Length == 3)
				{
					outAssembly = ((!string.IsNullOrEmpty(array[0])) ? array[0] : null);
					outDict = ((!string.IsNullOrEmpty(array[1])) ? array[1] : null);
					outKey = array[2];
				}
				if (array.Length == 2)
				{
					outDict = ((!string.IsNullOrEmpty(array[0])) ? array[0] : null);
					outKey = array[1];
				}
				if (array.Length == 1)
				{
					outKey = array[0];
					return;
				}
			}
			else
			{
				if (!LocalizeDictionary.Instance.GetIsInDesignMode())
				{
					throw new ArgumentNullException("inKey");
				}
			}
		}
		[DesignOnly(true)]
		public static void SetDesignCulture(DependencyObject obj, string value)
		{
			if (LocalizeDictionary.Instance.GetIsInDesignMode())
			{
				obj.SetValue(LocalizeDictionary.DesignCultureProperty, value);
			}
		}
		public void AddEventListener(IWeakEventListener listener)
		{
			LocalizeDictionary.WeakCultureChangedEventManager.AddListener(listener);
		}
		public string GetAssemblyName(Assembly assembly)
		{
			if (assembly == null)
			{
				throw new ArgumentNullException("assembly");
			}
			if (assembly.FullName == null)
			{
				throw new NullReferenceException("assembly.FullName is null");
			}
			return assembly.FullName.Split(new char[]
			{
				','
			})[0];
		}
		public bool GetIsInDesignMode()
		{
			return Application.Current != null && (Application.Current.MainWindow == null || DesignerProperties.GetIsInDesignMode(Application.Current.MainWindow));
		}
		public TType GetLocalizedObject<TType>(string resourceAssembly, string resourceDictionary, string resourceKey, CultureInfo cultureToUse) where TType : class
		{
			if (resourceDictionary == null)
			{
				throw new ArgumentNullException("resourceDictionary");
			}
			if (resourceDictionary == string.Empty)
			{
				throw new ArgumentException("resourceDictionary is empty", "resourceDictionary");
			}
			if (string.IsNullOrEmpty(resourceKey))
			{
				if (this.GetIsInDesignMode())
				{
					return default(TType);
				}
				if (resourceKey == null)
				{
					throw new ArgumentNullException("resourceKey");
				}
				if (resourceKey == string.Empty)
				{
					throw new ArgumentException("resourceKey is empty", "resourceKey");
				}
			}
			ResourceManager resourceManager;
			try
			{
				resourceManager = this.GetResourceManager(resourceAssembly, resourceDictionary, resourceKey);
			}
			catch
			{
				if (this.GetIsInDesignMode())
				{
					TType result = default(TType);
					return result;
				}
				throw;
			}
			object obj = resourceManager.GetObject(resourceKey, cultureToUse) as TType;
			if (obj == null && !this.GetIsInDesignMode())
			{
				throw new ArgumentException(string.Format("No resource key with name '{0}' in dictionary '{1}' in assembly '{2}' founded! ({2}.{1}.{0})", resourceKey, resourceDictionary, resourceAssembly));
			}
			return obj as TType;
		}
		public void RemoveEventListener(IWeakEventListener listener)
		{
			LocalizeDictionary.WeakCultureChangedEventManager.RemoveListener(listener);
		}
		public bool ResourceKeyExists(string resourceAssembly, string resourceDictionary, string resourceKey)
		{
			return this.ResourceKeyExists(resourceAssembly, resourceDictionary, resourceKey, CultureInfo.InvariantCulture);
		}
		public bool ResourceKeyExists(string resourceAssembly, string resourceDictionary, string resourceKey, CultureInfo cultureToUse)
		{
			bool result;
			try
			{
				result = (this.GetResourceManager(resourceAssembly, resourceDictionary, resourceKey).GetObject(resourceKey, cultureToUse) != null);
			}
			catch
			{
				if (!this.GetIsInDesignMode())
				{
					throw;
				}
				result = false;
			}
			return result;
		}
		[DesignOnly(true)]
		private static void SetCultureFromDependencyProperty(DependencyObject obj, DependencyPropertyChangedEventArgs args)
		{
			if (!LocalizeDictionary.Instance.GetIsInDesignMode())
			{
				return;
			}
			CultureInfo cultureInfo;
			try
			{
				cultureInfo = CultureInfo.GetCultureInfo((string)args.NewValue);
			}
			catch
			{
				if (!LocalizeDictionary.Instance.GetIsInDesignMode())
				{
					throw;
				}
				cultureInfo = LocalizeDictionary.DefaultCultureInfo;
			}
			if (cultureInfo != null)
			{
				LocalizeDictionary.Instance.Culture = cultureInfo;
			}
		}
		private ResourceManager GetResourceManager(string resourceAssembly, string resourceDictionary, string resourceKey)
		{
			if (resourceAssembly == null)
			{
				resourceAssembly = this.GetAssemblyName(Assembly.GetExecutingAssembly());
			}
			if (resourceDictionary == null)
			{
				resourceDictionary = "Resources";
			}
			if (string.IsNullOrEmpty(resourceKey))
			{
				throw new ArgumentNullException("resourceKey");
			}
			Assembly assembly = null;
			string text = null;
			string text2 = "." + resourceDictionary + ".resources";
			ResourceManager resourceManager;
			if (this.ResourceManagerList.ContainsKey(resourceAssembly + text2))
			{
				resourceManager = this.ResourceManagerList[resourceAssembly + text2];
			}
			else
			{
				try
				{
					Assembly[] assemblies = AppDomain.CurrentDomain.GetAssemblies();
					for (int i = 0; i < assemblies.Length; i++)
					{
						Assembly assembly2 = assemblies[i];
						if (assembly2.FullName != null)
						{
							AssemblyName assemblyName = new AssemblyName(assembly2.FullName);
							if (assemblyName.Name == resourceAssembly)
							{
								assembly = assembly2;
								break;
							}
						}
					}
					if (assembly == null)
					{
						assembly = Assembly.Load(new AssemblyName(resourceAssembly));
					}
				}
				catch (Exception innerException)
				{
					throw new Exception(string.Format("The Assembly '{0}' cannot be loaded.", resourceAssembly), innerException);
				}
				string[] manifestResourceNames = assembly.GetManifestResourceNames();
				for (int j = 0; j < manifestResourceNames.Length; j++)
				{
					if (manifestResourceNames[j].EndsWith(text2))
					{
						text = manifestResourceNames[j];
						break;
					}
				}
				if (text == null)
				{
					throw new ArgumentException(string.Format("No resource key with name '{0}' in dictionary '{1}' in assembly '{2}' founded! ({2}.{1}.{0})", resourceKey, resourceDictionary, resourceAssembly));
				}
				text = text.Substring(0, text.Length - ".resources".Length);
				try
				{
					PropertyInfo property = assembly.GetType(text).GetProperty("ResourceManager", BindingFlags.Static | BindingFlags.Public | BindingFlags.NonPublic);
					MethodInfo getMethod = property.GetGetMethod(true);
					object obj = getMethod.Invoke(null, null);
					resourceManager = (ResourceManager)obj;
				}
				catch (Exception innerException2)
				{
					throw new InvalidOperationException("Cannot resolve the ResourceManager!", innerException2);
				}
				this.ResourceManagerList.Add(resourceAssembly + text2, resourceManager);
			}
			return resourceManager;
		}
		public static void ChangeLanguage(string languageName)
		{
			string text = languageName;
			if (string.IsNullOrEmpty(text))
			{
				string name = Thread.CurrentThread.CurrentCulture.Name;
				if (LocalSettings.SupportedLanguages.Contains(name))
				{
					text = name;
				}
				else
				{
					string twoLetterISOLanguageName = Thread.CurrentThread.CurrentCulture.TwoLetterISOLanguageName;
					text = (LocalSettings.SupportedLanguages.Contains(twoLetterISOLanguageName) ? twoLetterISOLanguageName : LocalSettings.SupportedLanguages[0]);
				}
			}
			LocalizeDictionary.Instance.Culture = CultureInfo.GetCultureInfo(text);
			LocalSettings.CurrentLanguage = text;
		}
	}
}
