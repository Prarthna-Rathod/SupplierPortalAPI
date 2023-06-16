using DataAccess.DataActions;
using DataAccess.DataActions.Interfaces;
using Services;
using Services.Factories;
using Services.Factories.Interface;
using Services.Factories.Interfaces;
using Services.Interfaces;
using Services.Mappers.Interfaces;
using Services.Mappers.SupplierMappers;
using BusinessLogic.ReportingPeriodRoot.Interfaces;
using BusinessLogic.ReportingPeriodRoot.DomainModels;
using Services.Mappers.ReportingPeriodMappers;
using DataAccess.Logging;

namespace SupplierPortalAPI.Infrastructure.Builders
{
    public static class DependencyBuilder
    {

        public static void AddDependancy(this IServiceCollection services, IConfiguration configuration)
        {
            //Supplier
            services.AddScoped<ISupplierServices, SupplierServices>();
            services.AddScoped<ISupplierDataActions, SupplierDataActionsManager>();
            services.AddScoped<ISupplierFactory, SupplierFactory>();
            services.AddSingleton<ISupplierEntityDomainMapper, SupplierEntityDomainMapper>();
            services.AddSingleton<ISupplierDomainDtoMapper, SupplierDomainDtoMapper>();

            //ReportingPeriod
            services.AddScoped<IReportingPeriod, ReportingPeriod>();
            services.AddScoped<IReportingPeriodServices, ReportingPeriodServices>();
            services.AddScoped<IReportingPeriodDataActions, ReportingPeriodDataActionsManager>();
            services.AddScoped<IUploadDocumentDataActions,UploadDocumentDataActionManager>();
            services.AddScoped<IReportingPeriodFactory, ReportingPeriodFactory>();

            //Other
            services.AddScoped<ISendEmailService, SendEmailService>();
            services.AddScoped<ILogging, Logging>();
            services.AddSingleton<IReportingPeriodDomainDtoMapper, ReportingPeriodDomainDtoMapper>();
            services.AddSingleton<IReportingPeriodEntityDomainMapper, ReportingPeriodEntityDomainMapper>();
            services.AddSingleton<IReadOnlyEntityToDtoMapper, ReadOnlyEntityToDtoMapper>();
            services.AddSingleton<IReferenceLookUpMapper, ReferenceLookupMapper>();  
        }

    }
}
