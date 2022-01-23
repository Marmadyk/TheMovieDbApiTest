using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Runtime.CompilerServices;
using DM.MovieApi.ApiRequest;
using DM.MovieApi.Shims;

[assembly: InternalsVisibleTo( "DM.MovieApi.IntegrationTests" )]

namespace DM.MovieApi
{
    public static class MovieDbFactory
    {
        public const string TheMovieDbApiUrl = "http://api.themoviedb.org/3/";

        public static bool IsFactoryComposed => Settings != null;

        internal static IApiSettings Settings { get; private set; }


        public static void RegisterSettings( string bearerToken )
        {
            ResetFactory();

            if( bearerToken is null || bearerToken.Length <= 200 )
            {
                throw new ArgumentException($"Invalid: {bearerToken}", bearerToken);
            }

            Settings = new MovieDbSettings( TheMovieDbApiUrl, bearerToken );
        }

        public static Lazy<T> Create<T>() where T : IApiRequest
        {
            ContainerGuard();

            var requestResolver = new ApiRequestResolver();

            return new Lazy<T>( requestResolver.Get<T> );
        }

        public static IMovieDbApi GetAllApiRequests()
        {
            ContainerGuard();
            string msg = nameof( GetAllApiRequests );
            throw new NotImplementedException( msg );
        }

        public static void ResetFactory()
        {
            Settings = null;
        }

        private static void ContainerGuard()
        {
            if( !IsFactoryComposed )
            {
                throw new InvalidOperationException(nameof( RegisterSettings ));
            }
        }

        private class MovieDbSettings : IApiSettings
        {
            public string ApiUrl { get; }
            public string BearerToken { get; }

            public MovieDbSettings( string apiUrl, string bearerToken )
            {
                ApiUrl = apiUrl;
                BearerToken = bearerToken;
            }
        }

        private class ApiRequestResolver
        {
            private static readonly IReadOnlyDictionary<Type, Func<object>> SupportedDependencyTypeMap;
            private static readonly ConcurrentDictionary<Type, ConstructorInfo> TypeCtorMap;

            static ApiRequestResolver()
            {
                SupportedDependencyTypeMap = new Dictionary<Type, Func<object>>
                {
                    {typeof(IApiSettings), () => Settings},
                };

                TypeCtorMap = new ConcurrentDictionary<Type, ConstructorInfo>();
            }

            public T Get<T>() where T : IApiRequest
            {
                ConstructorInfo ctor = TypeCtorMap.GetOrAdd( typeof( T ), GetConstructor );

                ParameterInfo[] param = ctor.GetParameters();

                if( param.Length == 0 )
                {
                    return ( T )ctor.Invoke( null );
                }

                var paramObjects = new List<object>( param.Length );
                foreach( ParameterInfo p in param )
                {
                    Func<object> typeResolver = SupportedDependencyTypeMap[p.ParameterType];
                    paramObjects.Add( typeResolver() );
                }

                return ( T )ctor.Invoke( paramObjects.ToArray() );
            }

            private ConstructorInfo GetConstructor( Type t )
            {
                ConstructorInfo[] ctors = GetAvailableConstructors( t.GetTypeInfo() );

                if( ctors.Length == 0 )
                {
                    throw new InvalidOperationException(t.FullName);
                }

                if( ctors.Length == 1 )
                {
                    return ctors[0];
                }

                var importingCtors = ctors
                    .Where( x => x.IsDefined( typeof( ImportingConstructorAttribute ) ) )
                    .ToArray();

                if( importingCtors.Length != 1 )
                {
                    throw new InvalidOperationException(nameof( ImportingConstructorAttribute ));
                }

                return importingCtors[0];
            }

            private ConstructorInfo[] GetAvailableConstructors( TypeInfo typeInfo )
            {
                TypeInfo[] implementingTypes = typeInfo.Assembly.DefinedTypes
                    .Where( x => x.IsAbstract == false )
                    .Where( x => x.IsInterface == false )
                    .Where( typeInfo.IsAssignableFrom )
                    .ToArray();

                if( implementingTypes.Length == 0 )
                {
                    throw new NotSupportedException(typeInfo.Name);
                }

                if( implementingTypes.Length != 1 )
                {
                    throw new NotSupportedException(typeInfo.Name);
                }

                return implementingTypes[0].DeclaredConstructors.ToArray();
            }
        }
    }
}
