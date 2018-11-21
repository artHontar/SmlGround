using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Autofac;
using SmlGround.DataAccess.Interface;
using SmlGround.DataAccess.Repositories;

public class ServiceModule : Autofac.Module
{
    protected override void Load(ContainerBuilder builder)
    {
        //builder.RegisterModule(new RepositoryModule());
        builder.RegisterType(typeof(IdentityUnitOfWork)).As(typeof(IUnitOfWork)).InstancePerRequest();
    }

}

