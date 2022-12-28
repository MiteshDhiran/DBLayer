/*
using System;
using System.Collections.Concurrent;
using System.Diagnostics;
using System.Diagnostics.Tracing;
using System.IO;
using System.Text;
using System.Threading;

namespace ConnectAndSell.DataAccessStandard.Common.DataContract
{
    public static partial class PooledEnabledFuncHelper
    {
                /// <summary>
        /// 
        /// </summary>
        public enum PoolManagerEventType
        {
            /// <summary>
            /// 
            /// </summary>
            PoolManagerCreated = 1,
            /// <summary>
            /// 
            /// </summary>
            PoolManagerStatisticChanged = 2,
            /// <summary>
            /// 
            /// </summary>
            NonPooledObjectCreated = 3,
            /// <summary>
            /// 
            /// </summary>
            PooledObjectCouldNotBeReturnedToPool = 4
        }

        /// <summary>
        /// , Guid = "{4a491f5c-74b5-50c4-21d4-e06da5183721}"
        /// </summary>
        [EventSource(Name = "PoolManagerETWEvents")]
        public sealed  class PoolManagerETWEvents : EventSource
        {
            const string poolManagerCreatedMessage = "PoolManager Created";

            [Event(1, Level = EventLevel.Verbose)]
            internal void PoolManagerCreated(Guid guid, string tag)
            {
                if (this.IsEnabled(EventLevel.Verbose, EventKeywords.None))
                {
                    WriteEvent(1, guid, tag ?? string.Empty, poolManagerCreatedMessage);
                }
            }

            [Event(2, Level = EventLevel.Verbose)]
            internal void PoolStatisticsChange(Guid guid, string poolManagerName ,string message)
            {
                if (this.IsEnabled(EventLevel.Verbose, EventKeywords.None))
                {
                    WriteEvent(2, guid, poolManagerName, message);
                }
            }

            [Event(3, Level = EventLevel.Warning)]
            internal void NonPooledObjectCreated(Guid guid, string poolManagerName ,string message)
            {
                if (this.IsEnabled(EventLevel.Warning, EventKeywords.None))
                {
                    
                    WriteEvent(3, guid, poolManagerName, message);
                }
            }

            [Event(4, Level = EventLevel.Warning)]
            internal void PooledObjectCouldNotBeReturnedToPool(Guid guid, string poolManagerName ,string message)
            {
                if (this.IsEnabled(EventLevel.Warning, EventKeywords.None))
                {
                    WriteEvent(4, guid, poolManagerName, message);
                }
            }
        }
        /// <summary>
        /// 
        /// </summary>
        /// <typeparam name="A"></typeparam>
        /// <param name="poolManager"></param>
        /// <returns></returns>
        public static Action<Action<A>> GetActionDecoratedWithPoolManagedResource<A>(this PoolManager<A> poolManager)
            => (a)
                => {
                    var pooledObject = poolManager.Get();
                    try
                    {
                        a(pooledObject.Instance);
                    }
                    finally
                    {
                        poolManager.Return(pooledObject);
                    }
            };

        /// <summary>
        /// 
        /// </summary>
        /// <typeparam name="A"></typeparam>
        /// <typeparam name="B"></typeparam>
        /// <param name="poolManager"></param>
        /// <param name="convertResourceTypeFunc"></param>
        /// <param name="cleanUpAction"></param>
        /// <returns></returns>
        public static Action<Action<B>> GetActionDecoratedWithPoolManagedResource<A,B>(this PoolManager<A> poolManager, Func<A, B> convertResourceTypeFunc, Action<B> cleanUpAction)
            => GetActionDecoratedWithPoolManagedResource(poolManager).GetActionWithConvertedInputType(convertResourceTypeFunc, cleanUpAction);


        /// <summary>
        /// 
        /// </summary>
        /// <typeparam name="A"></typeparam>
        /// <typeparam name="B"></typeparam>
        /// <typeparam name="K"></typeparam>
        /// <param name="poolManager"></param>
        /// <param name="convertResourceTypeFunc"></param>
        /// <param name="cleanUpAction"></param>
        /// <returns></returns>
        public static Func<Func<B, K>, K> GetFuncConsumingResourcePoolObjectWithTranslationFunc<A, B, K>(this PoolManager<A> poolManager, Func<A, B> convertResourceTypeFunc, Action<B> cleanUpAction)
            => f
                => poolManager
                    .GetActionDecoratedWithPoolManagedResource()
                    .GetActionWithConvertedInputType(convertResourceTypeFunc, cleanUpAction)
                    .GetDecoratedFunctionFromResourceAction<B,K>()(f);



        

        /// <summary>
        /// 
        /// </summary>
        /// <typeparam name="A"></typeparam>
        /// <typeparam name="B"></typeparam>
        /// <param name="poolManager"></param>
        /// <param name="convertResourceTypeFunc"></param>
        /// <param name="cleanUpAction"></param>
        /// <returns></returns>
        public static Action<Action<B>> GetActionConsumingResourcePoolObjectWithTranslation<A, B>(this PoolManager<A> poolManager, Func<A, B> convertResourceTypeFunc, Action<B> cleanUpAction)
            => poolManager
                .GetActionDecoratedWithPoolManagedResource()
                .GetActionWithConvertedInputType(convertResourceTypeFunc, cleanUpAction);

        /// <summary>
        /// 
        /// </summary>
        /// <typeparam name="A"></typeparam>
        /// <typeparam name="K"></typeparam>
        /// <param name="poolManager"></param>
        /// <returns></returns>
        public static Func<Func<A, K>, K> GetFuncConsumingResourcePoolObjectFunc<A, K>(this PoolManager<A> poolManager)
            => f
                => poolManager
                    .GetActionDecoratedWithPoolManagedResource()
                    .GetDecoratedFunctionFromResourceAction<A, K>()(f);



        /// <summary>
        /// In future use this for non-blocking logging 
        /// </summary>
        private static readonly Action<IPoolManagerStatistic> DefaultStatisticLogAction = s => { };
            //LogHelper.Log(LogSeverityLevel.Info, LogCategory.General, s.ToString());
        private static readonly int DefaultStringBuilderSize = 1_200_000;
        private static readonly Func<StringBuilder> DefaultPooledStringBuilderConstructor = () => new StringBuilder(DefaultStringBuilderSize);
        private static readonly Func<StringBuilder> NonPooledStringBuilderConstructor = () => new StringBuilder();
        private static readonly long DefaultByteArraySize = 16 * 1000000;
        private static readonly Func<byte[]> DefaultPooledByteArrayConstructor = () => new byte[DefaultByteArraySize];
        private static readonly Func<byte[]> NonPooledByteArrayConstructor = () => new byte[0];
        private static readonly int BytePoolCapacity = 5;
        private static readonly int StringBuilderPoolCapacity = 15;

        private static readonly Func<StringBuilder,int, bool> IsStringBuilderReturnable = (ssb, sizeLimit) =>
        {
            var retVal = ssb?.Length <= sizeLimit;
            if (retVal == false)
            {
                var st = new StackTrace(0, true);
                //LogHelper.Log(LogSeverityLevel.Info, LogCategory.General,
                  //  $"StringBuilder Size {ssb?.Length} used is greater than expected {sizeLimit}. Call Stack {st.ToString()}");
            }
            return retVal;
        };
        private static readonly PoolManager<StringBuilder> DefaultStringBuilderPoolManager = new PoolManager<StringBuilder>("StringBuilder1000", StringBuilderPoolCapacity, DefaultPooledStringBuilderConstructor, NonPooledStringBuilderConstructor, ssb => ssb?.Clear(), (ssb) => IsStringBuilderReturnable(ssb,DefaultStringBuilderSize), DefaultStatisticLogAction);
        private static readonly PoolManager<byte[]> DefaultByteArrayPoolManager = new PoolManager<byte[]>("16MBByteArray", BytePoolCapacity, DefaultPooledByteArrayConstructor, NonPooledByteArrayConstructor, ssb => { }, (ssb) => true, DefaultStatisticLogAction);

        /// <summary>
        /// 
        /// </summary>
        public static readonly PoolManagerETWEvents PoolManagerEvent = new PoolManagerETWEvents();

        /// <summary>
        /// 
        /// </summary>
        /// <param name="func"></param>
        /// <returns></returns>
        public static string DoActionUsingPooledStringBuilder(Func<StringBuilder, string> func) => GetFuncConsumingResourcePoolObjectFunc<StringBuilder, string>(DefaultStringBuilderPoolManager)(func);

                        
        public static string DoActionUsingPooledStringBuilder<T1>(Func<StringBuilder, T1, string> func, T1 arg) 
            =>  func
                .GetDecoratedFuncConsumingResourceObject(DefaultStringBuilderPoolManager.GetActionDecoratedWithPoolManagedResource())(arg);

        public static string DoActionUsingPooledStringBuilder<T1,T2>(Func<StringBuilder, T1,T2, string> func, T1 arg, T2 arg2)
            => func
                .GetDecoratedFuncConsumingResourceObject(DefaultStringBuilderPoolManager.GetActionDecoratedWithPoolManagedResource())(arg,arg2);

        public static string DoActionUsingPooledStringBuilder<T1, T2,T3>(Func<StringBuilder, T1, T2,T3, string> func, T1 arg, T2 arg2, T3 arg3)
            => func
                .GetDecoratedFuncConsumingResourceObject(DefaultStringBuilderPoolManager.GetActionDecoratedWithPoolManagedResource())(arg, arg2,arg3);


        /// <summary>
        /// Function that uses non expandable memory stream which utilizes pooled byte array.Uses default pooled byte array for
        /// initializing non-resizable memory stream
        /// </summary>
        /// <typeparam name="K"></typeparam>
        /// <param name="func"></param>
        /// <returns></returns>
        public static K DoUsingDefaultPooledMemoryStream<K>(Func<MemoryStream, K> func) => GetFuncConsumingResourcePoolObjectWithTranslationFunc<byte[], MemoryStream, K>(DefaultByteArrayPoolManager, ConvertByteArrayToMemoryStream, DisposeMemoryStream)(func);

        /// <summary>
        /// 
        /// </summary>
        /// <typeparam name="T1"></typeparam>
        /// <typeparam name="K"></typeparam>
        /// <param name="func"></param>
        /// <param name="arg"></param>
        /// <returns></returns>
        public static K DoUsingDefaultPooledMemoryStream<T1,K>(Func<MemoryStream, T1, K> func, T1 arg) =>
            func.GetDecoratedFuncConsumingResourceObject(DefaultByteArrayPoolManager.GetActionDecoratedWithPoolManagedResource(ConvertByteArrayToMemoryStream, DisposeMemoryStream))(arg);

        public static K DoUsingDefaultPooledMemoryStream<T1,T2, K>(Func<MemoryStream, T1, T2,K> func, T1 arg, T2 arg2) =>
            func.GetDecoratedFuncConsumingResourceObject(DefaultByteArrayPoolManager.GetActionDecoratedWithPoolManagedResource(ConvertByteArrayToMemoryStream, DisposeMemoryStream))(arg,arg2);


        private static readonly Func<byte[], MemoryStream> ConvertByteArrayToMemoryStream = bytearray => bytearray?.Length > 1 ? new MemoryStream(bytearray, 0, bytearray.Length, true, true) : new MemoryStream();
        private static readonly Action<MemoryStream> DisposeMemoryStream = (ms) => ms?.Dispose();


        /// <summary>
        ///     Maintains whether the object is from pool or not.
        /// </summary>
        /// <typeparam name="T">type of the object being pooled</typeparam>
        internal struct PooledObject<T>
        {
            /// <summary>
            /// </summary>
            internal readonly int PooledObjectIdentifier;

            /// <summary>
            ///     If the object is from pool then this is true otherwise false
            /// </summary>
            internal readonly bool IsPooled;

            /// <summary>
            /// Instance of the object
            /// </summary>
            internal readonly T Instance;

            /// <summary>
            /// </summary>
            /// <param name="isPooled">If the object is from pool then this is true otherwise false</param>
            /// <param name="instance">Instance of the object</param>
            /// <param name="pooledObjectIdentifier"></param>
            internal PooledObject(bool isPooled, T instance, int pooledObjectIdentifier)
            {
                IsPooled = isPooled;
                Instance = instance;
                PooledObjectIdentifier = pooledObjectIdentifier;
            }
        }

        /// <summary>
        /// </summary>
        internal class PoolManagerStatistic : IPoolManagerStatistic
        {
            private readonly Action<PoolManagerStatistic> OnStatisticChangeAction;
            private int _currentPoolCapacity;
            private long _missCount;
            private long _nonReturnableCount;
            
            /// <summary>
            /// </summary>
            /// <param name="poolManagerID"></param>
            /// <param name="poolManagerName"></param>
            /// <param name="poolObjectType"></param>
            /// <param name="maxPoolCapacity"></param>
            /// <param name="onStatisticChangeAction"></param>
            internal PoolManagerStatistic(Guid poolManagerID, string poolManagerName, Type poolObjectType, int maxPoolCapacity,
                Action<PoolManagerStatistic> onStatisticChangeAction)
            {
                PoolManagerID = poolManagerID;
                PoolManagerName = poolManagerName;
                PoolObjectType = poolObjectType;
                MaxPoolCapacity = maxPoolCapacity;
                _missCount = 0;
                _currentPoolCapacity = 0;
                OnStatisticChangeAction = onStatisticChangeAction;
            }

            /// <summary>
            ///     Number of times pooled objects not used
            /// </summary>
            public long MissCount => _missCount;

            /// <summary>
            /// </summary>
            public long NonReturnableCount => _nonReturnableCount;

            /// <summary>
            ///     Current capacity used
            /// </summary>
            public int CurrentPoolCapacity => _currentPoolCapacity;


            /// <summary>
            ///     Max capacity
            /// </summary>
            public int MaxPoolCapacity { get; }

            /// <summary>
            ///     Name of the type being pooled
            /// </summary>
            public Type PoolObjectType { get; }

            /// <summary>
            ///     Name of the pool manager -- helps in identifying the log message.
            /// </summary>
            public string PoolManagerName { get; }
            /// <summary>
            /// 
            /// </summary>
            public Guid PoolManagerID { get; }

            /// <summary>
            /// 
            /// </summary>
            private void NotifyOnStatisticsChange()
            {
                OnStatisticChangeAction(this);
            }


            /// <summary>
            /// </summary>
            internal void IncrementMissCount()
            {
                Interlocked.Increment(ref _missCount);
                PoolManagerEvent.NonPooledObjectCreated(PoolManagerID,PoolManagerName, $"NonPooledObjectCreated:{ToString()}");
                NotifyOnStatisticsChange();
            }

            /// <summary>
            /// </summary>
            internal void IncrementNonReturnableCount()
            {
                Interlocked.Increment(ref _nonReturnableCount);
                PoolManagerEvent.PooledObjectCouldNotBeReturnedToPool(PoolManagerID,PoolManagerName, $"PooledObjectCouldNotBeReturnedToPool:{ToString()}");
                NotifyOnStatisticsChange();
            }

            /// <summary>
            /// </summary>
            internal int IncreaseCurrentPoolCapacity()
            {
                var isNewValueAssigned = false;
                var newValue =0;
                if (CurrentPoolCapacity < MaxPoolCapacity)
                {
                    lock (this)
                    {
                        if (CurrentPoolCapacity < MaxPoolCapacity)//double check
                        {
                            newValue = Interlocked.Increment(ref _currentPoolCapacity);
                            isNewValueAssigned = true;
                        }
                    }
                    if(isNewValueAssigned)
                    {
                        PoolManagerEvent.PoolStatisticsChange(PoolManagerID, PoolManagerName, $"PoolStatisticsChange:{ToString()}");
                        NotifyOnStatisticsChange();
                        return newValue;
                    }
                }

                IncrementMissCount();
                return -1;
            }

            /// <summary>Returns a string that represents the current object.</summary>
            /// <returns>A string that represents the current object.</returns>
            public override string ToString()
            {
                return $"PoolName:{PoolManagerName} PoolManagerID:{PoolManagerID} CurrentPoolCapacity:{CurrentPoolCapacity} MissCount:{MissCount} NonReturnableCount:{NonReturnableCount}";
            }
        }

        /// <summary>
        ///     Class that maintains the pool of objects. Has helper functions that enables taking object from the pool and returns
        ///     the object to pool once the task is completed
        /// </summary>
        /// <typeparam name="T"></typeparam>
        public sealed class PoolManager<T>
        {
            private readonly Action<T> CleanupAction;
            private readonly Func<T> ConstructorFunc;
            private readonly Func<T> NonPooledConstructorFunc;
            private readonly Func<T, bool> IsReturnableToPoolFunc;
            private readonly ConcurrentStack<Tuple<int, T>> PooledObjectStack;
            private readonly PoolManagerStatistic PoolManagerStatistic;
            private readonly Guid PoolManagerID = Guid.NewGuid();
            

            /// <summary>
            /// </summary>
            /// <param name="poolName"></param>
            /// <param name="maxPoolSize">Maximum number of instances in pool</param>
            /// <param name="constructor">function that creates objects in pool</param>
            /// <param name="nonPooledConstructor"></param>
            /// <param name="cleanUpAction">clean up action before returning the object to pool</param>
            /// <param name="isReturnableToPoolFunc"></param>
            /// <param name="onStatisticChangeAction"></param>
            public PoolManager(string poolName, int maxPoolSize, Func<T> constructor, Func<T> nonPooledConstructor, Action<T> cleanUpAction,
                Func<T, bool> isReturnableToPoolFunc,
                Action<IPoolManagerStatistic> onStatisticChangeAction)
            {
                if (poolName == null) throw new ArgumentNullException(nameof(poolName));
                if (onStatisticChangeAction == null) throw new ArgumentNullException(nameof(onStatisticChangeAction));
                PoolManagerStatistic =
                    new PoolManagerStatistic(PoolManagerID, poolName, typeof(T), maxPoolSize, onStatisticChangeAction);
                ConstructorFunc = constructor ?? throw new ArgumentNullException(nameof(constructor));
                NonPooledConstructorFunc = nonPooledConstructor ?? throw new ArgumentNullException(nameof(nonPooledConstructor));
                CleanupAction = cleanUpAction ?? throw new ArgumentNullException(nameof(cleanUpAction));
                PooledObjectStack = new ConcurrentStack<Tuple<int, T>>();
                IsReturnableToPoolFunc = isReturnableToPoolFunc ?? throw new ArgumentNullException(nameof(isReturnableToPoolFunc));
            }


            /// <summary>
            ///     Get the object from pool or if the pool quota is exhausted will return non pool object
            /// </summary>
            /// <returns></returns>
            internal PooledObject<T> Get()
            {
                if (PooledObjectStack.TryPop(out var result))
                {
                    return new PooledObject<T>(true, result.Item2, result.Item1);
                }

                int currentNumber;
                var retVal = (currentNumber = PoolManagerStatistic.IncreaseCurrentPoolCapacity()) > 0
                    ? new PooledObject<T>(true, ConstructorFunc(), currentNumber)
                    : new PooledObject<T>(false, NonPooledConstructorFunc(), -1);
                return retVal;
            }

            /// <summary>
            ///     Returns the object to the pool
            /// </summary>
            /// <param name="pooledObject"></param>
            internal void Return(PooledObject<T> pooledObject)
            {
                var pooledObjectInstance = pooledObject.Instance;
                var pooledObjectIdentifier = pooledObject.PooledObjectIdentifier;

                if (pooledObject.IsPooled)
                {
                    if (IsReturnableToPoolFunc(pooledObjectInstance))
                    {
                        CleanupAction(pooledObjectInstance);
                        PooledObjectStack.Push(new Tuple<int, T>(pooledObjectIdentifier, pooledObjectInstance));
                    }
                    else //if the pooled object is not returnable to stack because its exceeded the sanity like size of string builder is more than that should be cached then dispose the existing one and create a new instance
                    {
                        CleanupAction(pooledObjectInstance);
                        PoolManagerStatistic.IncrementNonReturnableCount();
                        PooledObjectStack.Push(new Tuple<int, T>(pooledObjectIdentifier, ConstructorFunc()));
                    }
                }
            }


        }
    }
}
*/