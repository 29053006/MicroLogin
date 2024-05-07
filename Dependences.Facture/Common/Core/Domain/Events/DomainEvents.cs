using CommonServiceLocator;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Facture.Core.Domain.Events
{
    public class DomainEvents
    {
        [ThreadStatic]
        private static List<Delegate> actions;

        public static IServiceLocator ServiceLocator { get; set; }

        public static void ClearCallbacks()
        {
            actions = null;
        }

        public static void Raise<T>(T args) where T : IDomainEvent
        {
            if (ServiceLocator != null)
            {
                var handlers = ServiceLocator.GetAllInstances<IHandles<T>>();
                var filteredHandlers = handlers.Where(h => !HasAttribute<FilterAttribute>(h) || (HasAttribute<FilterAttribute>(h) && ShouldBeIncluded(h.GetType(), args)));
                var orderedHandlers = filteredHandlers.Where(h => HasAttribute<PriorityAttribute>(h))
                                                      .OrderBy(h => AttributeOf<PriorityAttribute>(h).Level)
                                                      .Union(filteredHandlers.Where(h => !HasAttribute<PriorityAttribute>(h)));

                foreach (var handler in orderedHandlers)
                    handler.Handle(args);
            }

            if (actions != null)
            {
                foreach (var action in actions)
                {
                    if (action is Action<T>)
                    {
                        ((Action<T>)action)(args);
                    }
                }
            }
        }

        private static bool HasAttribute<T>(object obj) where T : Attribute
        {
            var hasAttribute = (T)Attribute.GetCustomAttribute(obj.GetType(), typeof(T)) != null;
            return hasAttribute;
        }

        private static T AttributeOf<T>(object obj) where T : Attribute
        {
            var attribute = (T)Attribute.GetCustomAttribute(obj.GetType(), typeof(T));
            return attribute;
        }

        private static bool ShouldBeIncluded<T>(Type type, T args) where T : IDomainEvent
        {
            var attribute = (FilterAttribute)Attribute.GetCustomAttribute(type, typeof(FilterAttribute));

            Func<T, Boolean> func = System.Linq.Dynamic.Core.DynamicExpressionParser.ParseLambda<T, Boolean>(null, true, attribute.Expression, null).Compile();

            var shouldBeIncluded = func(args);
            return shouldBeIncluded;
        }

        public static void Register<T>(Action<T> callback) where T : IDomainEvent
        {
            if (actions == null)
            {
                actions = new List<Delegate>();
            }
            actions.Add(callback);
        }
    }
}