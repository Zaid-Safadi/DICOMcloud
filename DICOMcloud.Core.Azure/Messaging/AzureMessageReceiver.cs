//using System;
//using System.Collections.Concurrent;
//using System.Collections.Generic;
//using System.Linq;
//using System.Text;
//using System.Threading;
//using System.Threading.Tasks;
//using DICOMcloud.Core.Messaging;
//using Microsoft.WindowsAzure.Storage;
//using Microsoft.WindowsAzure.Storage.Queue;
//using Newtonsoft.Json;

//namespace DICOMcloud.Core.Azure.Messaging
//{
//    public class AzureMessageReceiver : IMessageReceiver
//    {
//        public CloudStorageAccount StorageAccount { get; set; }
//        private static ConcurrentDictionary<string, CloudQueue>                     _queues = new ConcurrentDictionary<string, CloudQueue> ( ) ;
//        private static ConcurrentDictionary<string, IList<Func<ITransportMessage,bool>>> _handlers = new ConcurrentDictionary<string, IList<Func<ITransportMessage,bool>>> ( ) ;
//        private static AutoResetEvent _resetEvent = new AutoResetEvent (false) ;
//        public AzureMessageReceiver ( CloudStorageAccount storageAccount )
//        {
//            StorageAccount = storageAccount ;
//        }

//        public void Subscribe ( string messageName, Func<ITransportMessage,bool>handler ) 
//        {
//            var queue = _queues.GetOrAdd ( messageName, CreateQueue ) ;
            
//            _handlers.GetOrAdd (messageName, new List<Func<ITransportMessage, bool>> ( ) ).Add ( handler ) ;//TODO: thread safe, check same handler?
            
//            //TODO: all this is wrong, fuck
//            //_resetEvent.Set ( ) ;
//        }

//        private CloudQueue CreateQueue(string message)
//        {
//            var client = StorageAccount.CreateCloudQueueClient();

//            var queue = client.GetQueueReference(message);

//            queue.CreateIfNotExists();

//            return queue;
//        }

//        protected static void QueueProcessor ( ) 
//        {
//            while ( _resetEvent.WaitOne ( ) )
//            {
//                foreach ( var queue in _queues )
//                {
//                    IList<Func<ITransportMessage,bool>> handlers= null ; 
                    
//                    if ( _handlers.TryGetValue(queue.Key, out handlers ))
//                    {
//                        CloudQueueMessage message = null ;
//                        //wrong, then what
//                        while ( (message = queue.Value.GetMessage ( )) != null )
                
//                        if ( null != message ) 
//                        {
//                            ITransportMessage transportMessage = JsonConvert.DeserializeObject<ITransportMessage> (message.AsString);
                        
//                            bool handled = false ;

//                            foreach ( var handler in handlers ) 
//                            {
//                                if ( handler ( transportMessage) )
//                                {
//                                    handled = true ;
//                                }
//                            }

//                            if (handled)
//                            {
//                                queue.Value.DeleteMessage (message);
//                            }
//                        }
//                    }
//                    else
//                    {
//                        //TODO: queue should be removed
//                    }
//                }
//            }

//        }
//    }
//}
