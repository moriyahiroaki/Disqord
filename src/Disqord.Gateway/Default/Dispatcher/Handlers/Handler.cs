using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;
using Qommon.Collections.Synchronized;
using Qommon.Events;
using Disqord.Gateway.Api;
using Disqord.Serialization.Json;
using Qommon.Binding;
using Microsoft.Extensions.Logging;

namespace Disqord.Gateway.Default.Dispatcher
{
    public abstract class Handler : IBindable<DefaultGatewayDispatcher>
    {
        public DefaultGatewayDispatcher Dispatcher => _binder.Value;

        protected IGatewayClient Client => Dispatcher.Client;

        protected IGatewayCacheProvider CacheProvider => Client.CacheProvider;

        protected ILogger Logger => Dispatcher.Logger;

        private readonly Binder<DefaultGatewayDispatcher> _binder;

        private protected Handler()
        {
            _binder = new Binder<DefaultGatewayDispatcher>(this);
        }

        public virtual void Bind(DefaultGatewayDispatcher value)
        {
            _binder.Bind(value);
        }

        public abstract ValueTask HandleDispatchAsync(IGatewayApiClient shard, IJsonNode data);

        public static Handler Intercept<TModel, TEventArgs>(Handler<TModel, TEventArgs> handler, Action<IGatewayApiClient, TModel> func)
            where TModel : JsonModel
            where TEventArgs : EventArgs
            => new InterceptingHandler<TModel, TEventArgs>(handler, func);

        private protected static readonly ISynchronizedDictionary<DefaultGatewayDispatcher, Dictionary<Type, IAsynchronousEvent>> EventsByDispatcher = new SynchronizedDictionary<DefaultGatewayDispatcher, Dictionary<Type, IAsynchronousEvent>>(1);
        private protected static readonly PropertyInfo[] EventProperties;

        static Handler()
        {
            EventProperties = typeof(DefaultGatewayDispatcher).GetProperties(BindingFlags.Instance | BindingFlags.Public)
                .Where(x => x.PropertyType.IsGenericType && x.PropertyType.GetGenericTypeDefinition() == typeof(AsynchronousEvent<>))
                .ToArray();
        }
    }
}
