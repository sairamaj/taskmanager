﻿using System;
using System.Collections.Generic;
using Autofac.Core;
using Utils.Core.Model;

namespace Utils.Core.Registration
{
    public interface IServiceLocator
    {
        T Resolve<T>();
        T Resolve<T>(params Parameter[] parameters);
        object Resolve(Type serviceType);
        IEnumerable<RegistrationInfo> GetRegisteredTypes();
    }
}
