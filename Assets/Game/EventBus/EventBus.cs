using System;
using System.Collections.Generic;
using System.Linq;

public static class EventBus<TEvent> where TEvent : struct
{
	private static Dictionary<Type, List<(int priority, object Target, Action<TEvent> action)>> events = new();

	public static void Subscribe(Action<TEvent> callback, int priority = 0)
	{
		var type = typeof(TEvent);

		if (!events.ContainsKey(type))
			events.Add(type, new());

		events[type].Add((priority, callback.Target, Convert(callback)));
		events[type] = events[type].OrderByDescending(a => a.priority).ToList();
	}

	public static Action<TEvent> Convert(Action<TEvent> myActionT)
	{
		if (myActionT == null)
			return null;

		return new Action<TEvent>(a => myActionT(a));
	}

	public static void Unsubscribe(Action<TEvent> callback)
	{
		var type = typeof(TEvent);

		if (events.ContainsKey(type))
		{
			var list = events[type];
			var item = list.Find(a => a.Target == callback.Target);
			events[type].Remove(item);
		}
	}

	public static void Publish(TEvent data)
	{
		var type = typeof(TEvent);

		if (events.ContainsKey(type))
		{
			foreach (var item in events[type])
			{
				item.action?.Invoke(data);
			}
		}
	}

	public static void Dispose()
	{
		events.Clear();
	}
}
