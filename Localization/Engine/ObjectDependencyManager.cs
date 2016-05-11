using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
namespace Localization.Engine
{
	public static class ObjectDependencyManager
	{
		private static Dictionary<object, List<WeakReference>> internalList;
		static ObjectDependencyManager()
		{
			ObjectDependencyManager.internalList = new Dictionary<object, List<WeakReference>>();
		}
		[MethodImpl(MethodImplOptions.Synchronized)]
		public static bool AddObjectDependency(WeakReference weakRefDp, object objToHold)
		{
			ObjectDependencyManager.CleanUp();
			if (objToHold == null)
			{
				throw new ArgumentNullException("objToHold", "The objToHold cannot be null");
			}
			if (objToHold.GetType() == typeof(WeakReference))
			{
				throw new ArgumentException("objToHold cannot be type of WeakReference", "objToHold");
			}
			if (weakRefDp.Target == objToHold)
			{
				throw new InvalidOperationException("The WeakReference.Target cannot be the same as objToHold");
			}
			bool result = false;
			if (!ObjectDependencyManager.internalList.ContainsKey(objToHold))
			{
				List<WeakReference> value = new List<WeakReference>
				{
					weakRefDp
				};
				ObjectDependencyManager.internalList.Add(objToHold, value);
				result = true;
			}
			else
			{
				List<WeakReference> list = ObjectDependencyManager.internalList[objToHold];
				if (!list.Contains(weakRefDp))
				{
					list.Add(weakRefDp);
					result = true;
				}
			}
			return result;
		}
		public static void CleanUp()
		{
			ObjectDependencyManager.CleanUp(null);
		}
		[MethodImpl(MethodImplOptions.Synchronized)]
		public static void CleanUp(object objToRemove)
		{
			if (objToRemove == null)
			{
				List<object> list = new List<object>();
				foreach (KeyValuePair<object, List<WeakReference>> current in ObjectDependencyManager.internalList)
				{
					for (int i = current.Value.Count - 1; i >= 0; i--)
					{
						if (!current.Value[i].IsAlive)
						{
							current.Value.RemoveAt(i);
						}
					}
					if (current.Value.Count == 0)
					{
						list.Add(current.Key);
					}
				}
				for (int j = list.Count - 1; j >= 0; j--)
				{
					ObjectDependencyManager.internalList.Remove(list[j]);
				}
				list.Clear();
				return;
			}
			if (!ObjectDependencyManager.internalList.Remove(objToRemove))
			{
				throw new Exception("Key was not found!");
			}
		}
	}
}
