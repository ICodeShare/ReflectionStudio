using System;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Windows;
namespace Localization.Engine
{
	public sealed class OddsFormatManager : DependencyObject
	{
		public sealed class WeakOddsFormatChangedEventManager : WeakEventManager
		{
			private bool isListening;
			private WeakEventManager.ListenerList listeners;
			private static OddsFormatManager.WeakOddsFormatChangedEventManager CurrentManager
			{
				get
				{
					Type typeFromHandle = typeof(OddsFormatManager.WeakOddsFormatChangedEventManager);
					OddsFormatManager.WeakOddsFormatChangedEventManager weakOddsFormatChangedEventManager = (OddsFormatManager.WeakOddsFormatChangedEventManager)WeakEventManager.GetCurrentManager(typeFromHandle);
					if (weakOddsFormatChangedEventManager == null)
					{
						weakOddsFormatChangedEventManager = new OddsFormatManager.WeakOddsFormatChangedEventManager();
						WeakEventManager.SetCurrentManager(typeFromHandle, weakOddsFormatChangedEventManager);
					}
					return weakOddsFormatChangedEventManager;
				}
			}
			private WeakOddsFormatChangedEventManager()
			{
				this.listeners = new WeakEventManager.ListenerList();
			}
			public static void AddListener(IWeakEventListener listener)
			{
				OddsFormatManager.WeakOddsFormatChangedEventManager.CurrentManager.listeners.Add(listener);
				OddsFormatManager.WeakOddsFormatChangedEventManager.CurrentManager.StartStopListening();
			}
			public static void RemoveListener(IWeakEventListener listener)
			{
				OddsFormatManager.WeakOddsFormatChangedEventManager.CurrentManager.listeners.Remove(listener);
				OddsFormatManager.WeakOddsFormatChangedEventManager.CurrentManager.StartStopListening();
			}
			[MethodImpl(MethodImplOptions.Synchronized)]
			protected override void StartListening(object source)
			{
				if (!this.isListening)
				{
					OddsFormatManager.Instance.OnOddsFormatChanged += new Action(this.Instance_OnOddsFormatChanged);
					LocalizeDictionary.Instance.OnCultureChanged += new Action(this.Instance_OnCultureChanged);
					this.isListening = true;
				}
			}
			[MethodImpl(MethodImplOptions.Synchronized)]
			protected override void StopListening(object source)
			{
				if (this.isListening)
				{
					OddsFormatManager.Instance.OnOddsFormatChanged -= new Action(this.Instance_OnOddsFormatChanged);
					LocalizeDictionary.Instance.OnCultureChanged -= new Action(this.Instance_OnCultureChanged);
					this.isListening = false;
				}
			}
			private void Instance_OnCultureChanged()
			{
				base.DeliverEventToList(OddsFormatManager.Instance, EventArgs.Empty, this.listeners);
			}
			private void Instance_OnOddsFormatChanged()
			{
				base.DeliverEventToList(OddsFormatManager.Instance, EventArgs.Empty, this.listeners);
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
		[DesignOnly(true)]
		public static readonly DependencyProperty DesignOddsFormatProperty = DependencyProperty.RegisterAttached("DesignOddsFormat", typeof(OddsFormatType), typeof(OddsFormatManager), new PropertyMetadata(OddsFormatManager.DefaultOddsFormatType, new PropertyChangedCallback(OddsFormatManager.SetOddsFormatFromDependencyProperty)));
		private static readonly object SyncRoot = new object();
		private static OddsFormatManager instance;
		private OddsFormatType oddsFormatType = OddsFormatManager.DefaultOddsFormatType;
		public event Action OnOddsFormatChanged;
		public static OddsFormatType DefaultOddsFormatType
		{
			get
			{
				return OddsFormatType.EU;
			}
		}
		public static OddsFormatManager Instance
		{
			get
			{
				if (OddsFormatManager.instance == null)
				{
					lock (OddsFormatManager.SyncRoot)
					{
						if (OddsFormatManager.instance == null)
						{
							OddsFormatManager.instance = new OddsFormatManager();
						}
					}
				}
				return OddsFormatManager.instance;
			}
		}
		public OddsFormatType OddsFormatType
		{
			get
			{
				return this.oddsFormatType;
			}
			set
			{
				if (!Enum.IsDefined(typeof(OddsFormatType), value))
				{
					throw new ArgumentNullException("value");
				}
				this.oddsFormatType = value;
				if (this.OnOddsFormatChanged != null)
				{
					this.OnOddsFormatChanged();
				}
			}
		}
		private OddsFormatManager()
		{
		}
		[DesignOnly(true)]
		public static OddsFormatType GetDesignOddsFormat(DependencyObject obj)
		{
			if (OddsFormatManager.Instance.GetIsInDesignMode())
			{
				return (OddsFormatType)obj.GetValue(OddsFormatManager.DesignOddsFormatProperty);
			}
			return OddsFormatManager.Instance.OddsFormatType;
		}
		[DesignOnly(true)]
		public static void SetDesignOddsFormat(DependencyObject obj, OddsFormatType value)
		{
			if (OddsFormatManager.Instance.GetIsInDesignMode())
			{
				obj.SetValue(OddsFormatManager.DesignOddsFormatProperty, value);
			}
		}
		public void AddEventListener(IWeakEventListener listener)
		{
			OddsFormatManager.WeakOddsFormatChangedEventManager.AddListener(listener);
		}
		public bool GetIsInDesignMode()
		{
			return DesignerProperties.GetIsInDesignMode(this);
		}
		public void RemoveEventListener(IWeakEventListener listener)
		{
			OddsFormatManager.WeakOddsFormatChangedEventManager.RemoveListener(listener);
		}
		[DesignOnly(true)]
		private static void SetOddsFormatFromDependencyProperty(DependencyObject obj, DependencyPropertyChangedEventArgs args)
		{
			if (!OddsFormatManager.Instance.GetIsInDesignMode())
			{
				return;
			}
			if (Enum.IsDefined(typeof(OddsFormatType), args.NewValue))
			{
				OddsFormatManager.Instance.OddsFormatType = (OddsFormatType)Enum.Parse(typeof(OddsFormatType), args.NewValue.ToString(), true);
				return;
			}
			if (OddsFormatManager.Instance.GetIsInDesignMode())
			{
				OddsFormatManager.Instance.OddsFormatType = OddsFormatManager.DefaultOddsFormatType;
				return;
			}
			throw new InvalidCastException(string.Format("\"{0}\" not defined in Enum OddsFormatType", args.NewValue));
		}
	}
}
