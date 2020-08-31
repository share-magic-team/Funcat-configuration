﻿using System;

namespace FuncatConfiguration.Abstractions
{
    /// <summary>
    /// Allows to register configurations in DI container
    /// </summary>
    public interface IServiceCollectionRegistrar
    {
        /// <summary>
        /// Register type in DI container
        /// </summary>
        /// <param name="configurationType">Configuration type</typeparam>
        /// <param name="configurationFactory">Configuration factory</param>
        void Register(Type configurationType, Func<object> configurationFactory);
    }
}