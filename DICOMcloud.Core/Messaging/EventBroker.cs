using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;


namespace DICOMcloud.Core.Messaging
{
    public class EventBroker
    {
        private EventBroker ( ) {}
        static EventBroker ()
        {
            _instance = new EventBroker ( ) ;
        }

        public static EventBroker Instance { get { return _instance ; } }
        private static ConcurrentDictionary<Type,List<Delegate>> _hanlders = new ConcurrentDictionary<Type, List<Delegate>> ( ) ;
        private static EventBroker _instance;

        public static void Publish<T>(T message) where T : EventArgs
        {
            List<Delegate> handlersPlain;
            
            if (_hanlders.TryGetValue(typeof(T), out handlersPlain))
            {
                foreach ( var handler in handlersPlain.OfType<Action<T>> ( ) )
                
                handler(message);
            }
        }

        public static void Subscribe<T>(Action<T> handler ) where T : EventArgs
        {
            List<Delegate> existingHandlersPlain ;
            // We don't actually care about the return value here...
            
            if ( !_hanlders.TryGetValue(typeof(T), out existingHandlersPlain))
            {
                existingHandlersPlain = new List<Delegate> ( ) ;
                
                _hanlders[typeof(T)] = existingHandlersPlain;   
            }

            existingHandlersPlain.Add ( handler);
        }
    }
}