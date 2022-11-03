using System;
using System.Collections.Generic;
using UnityEngine;

public enum EvtType
{
    Init,
    Reset,
    RollBack,
}

public struct CustomEvent
{
	public EvtType type;
	
	public CustomEvent(EvtType type)
	{
		this.type = type;
	}

	// public CustomEvent(EvtType type, object param)
	// {
	// 	this.type = type;
	// 	this.param = param;
	// }
}

public static class EventManager 
	{
	    private static Dictionary<Type, List<EventListenerBase>> _subscribersList;

		static EventManager()
	    {
	        _subscribersList = new Dictionary<Type, List<EventListenerBase>>();
	    }

	    public static void AddListener<MMEvent>( EventListener<MMEvent> listener ) where MMEvent : struct
	    {
	        Type eventType = typeof( MMEvent );

	        if( !_subscribersList.ContainsKey( eventType ) )
	            _subscribersList[eventType] = new List<EventListenerBase>();

	        if( !SubscriptionExists( eventType, listener ) )
	            _subscribersList[eventType].Add( listener );
	    }

	    public static void RemoveListener<MMEvent>( EventListener<MMEvent> listener ) where MMEvent : struct
	    {
	        Type eventType = typeof( MMEvent );

	        if (!_subscribersList.ContainsKey(eventType)) return;

			List<EventListenerBase> subscriberList = _subscribersList[eventType];

            #if EVENTROUTER_THROWEXCEPTIONS
	            bool listenerFound = false;
            #endif

            for (int i = 0; i<subscriberList.Count; i++)
			{
				if( subscriberList[i] == listener )
				{
					subscriberList.Remove( subscriberList[i] );
                    #if EVENTROUTER_THROWEXCEPTIONS
					    listenerFound = true;
                    #endif

                    if ( subscriberList.Count == 0 )
                    {
                        _subscribersList.Remove(eventType);
                    }						

					return;
				}
			}
	    }

	    public static void TriggerEvent<MMEvent>( MMEvent newEvent ) where MMEvent : struct
	    {
	        List<EventListenerBase> list;
	        if( !_subscribersList.TryGetValue( typeof( MMEvent ), out list ) ) return;
			
			for (int i=0; i<list.Count; i++)
			{
				( list[i] as EventListener<MMEvent> ).OnEvent( newEvent );
			}
	    }

	    private static bool SubscriptionExists( Type type, EventListenerBase receiver )
	    {
	        List<EventListenerBase> receivers;

	        if( !_subscribersList.TryGetValue( type, out receivers ) ) return false;

	        bool exists = false;

			for (int i=0; i<receivers.Count; i++)
			{
				if( receivers[i] == receiver )
				{
					exists = true;
					break;
				}
			}

	        return exists;
	    }
	}

	public static class EventRegister
	{
		public delegate void Delegate<T>( T eventType );

		public static void EventStartListening<EventType>( this EventListener<EventType> caller ) where EventType : struct
		{
			EventManager.AddListener<EventType>( caller );
		}

		public static void EventStopListening<EventType>( this EventListener<EventType> caller ) where EventType : struct
		{
			EventManager.RemoveListener<EventType>( caller );
		}
	}

	public interface EventListenerBase { };

	public interface EventListener<T> : EventListenerBase
	{
	    void OnEvent( T eventType );
	}