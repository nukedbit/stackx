using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Runtime.CompilerServices;
using Funq;
using ServiceStack;
using StackX.ServiceModel;

[assembly:InternalsVisibleTo("StackX.Tests")]

namespace StackX.ServiceInterface
{

    public abstract class StackXAppHostBase : AppHostBase
    {
        internal static HashSet<Type> ExcludedAutoQueryTypes {get; private set;} = new HashSet<Type>();
        internal static HashSet<Type> ExcludedAutoCrudTypes {get; private set;} = new HashSet<Type>();
        
        protected StackXAppHostBase(string serviceName, params Assembly[] assembliesWithServices) : base(serviceName, assembliesWithServices)
        {
            #region Exclude Translations Services

            ExcludedAutoCrudTypes.Add(typeof(CreateTranslation));
            ExcludedAutoCrudTypes.Add(typeof(UpdateTranslation));
            ExcludedAutoCrudTypes.Add(typeof(DeleteTranslation));
            ExcludedAutoQueryTypes.Add(typeof(QueryTranslations));
            ExcludedAutoQueryTypes.Add(typeof(CreateMissingTranslation));
            ExcludeAutoRegisteringServiceTypes.Add(typeof(TranslationService));

            #endregion

            #region Exclude Language Services

            ExcludedAutoCrudTypes.Add(typeof(UpdateLanguage));
            ExcludedAutoCrudTypes.Add(typeof(DeleteLanguage));
            ExcludedAutoQueryTypes.Add(typeof(QueryLanguages));
            ExcludedAutoCrudTypes.Add(typeof(CreateLanguage));

            #endregion

            #region Exclude Taxonomy Services

            ExcludedAutoCrudTypes.Add(typeof(UpdateTaxonomy));
            ExcludedAutoCrudTypes.Add(typeof(CreateTaxonomy));
            ExcludedAutoCrudTypes.Add(typeof(DeleteTaxonomy));
            ExcludedAutoQueryTypes.Add(typeof(QueryTaxonomies));

            #endregion

            #region Exclude Application Services

            ExcludedAutoCrudTypes.Add(typeof(CreateApplication));
            ExcludedAutoCrudTypes.Add(typeof(DeleteApplication));
            ExcludedAutoCrudTypes.Add(typeof(UpdateApplication));
            ExcludedAutoQueryTypes.Add(typeof(QueryApplications));

            #endregion

            #region Exclude Menu Services

            ExcludedAutoCrudTypes.Add(typeof(CreateMenu));
            ExcludedAutoCrudTypes.Add(typeof(CreateMenuItem));
            ExcludedAutoCrudTypes.Add(typeof(DeleteMenu));
            ExcludedAutoCrudTypes.Add(typeof(DeleteMenuItem));
            ExcludedAutoCrudTypes.Add(typeof(UpdateMenu));
            ExcludedAutoCrudTypes.Add(typeof(UpdateMenuItem));
            ExcludedAutoQueryTypes.Add(typeof(QueryMenus));
            ExcludedAutoQueryTypes.Add(typeof(QueryMenuItems));

            #endregion

            #region Exclude File Services
            
            ExcludeAutoRegisteringServiceTypes.Add(typeof(FileService));
            ExcludedAutoCrudTypes.Add(typeof(UpdateFile));
            ExcludedAutoQueryTypes.Add(typeof(QueryFiles));

            #endregion

            #region Exclude Device Services

            ExcludedAutoQueryTypes.Add(typeof(QueryDevices));
            ExcludedAutoQueryTypes.Add(typeof(QueryDeviceKinds));
            ExcludedAutoCrudTypes.Add(typeof(CreateDeviceKind));
            ExcludedAutoCrudTypes.Add(typeof(UpdateDeviceKind));
            ExcludedAutoCrudTypes.Add(typeof(DeleteDeviceKind));
            ExcludedAutoCrudTypes.Add(typeof(RestoreDeviceKind));
            ExcludeAutoRegisteringServiceTypes.Add(typeof(DeviceService));

            #endregion

            #region Exclude Logs Services

            ExcludedAutoCrudTypes.Add(typeof(CreateLog));
            ExcludedAutoCrudTypes.Add(typeof(DeleteLog));
            ExcludedAutoQueryTypes.Add(typeof(QueryLogs));

            #endregion
        }

        public override void Configure(Container container)
        {
            Plugins.Add(ConfigureAutoQueryFeature(new AutoQueryFeature
            {
                FilterAutoQueryRequestTypes =
                    aDtos => aDtos.Where(a => !ExcludedAutoQueryTypes.Contains(a)).ToList(),
                FilterAutoCrudRequestTypes = aDtos => aDtos.Where(a => !ExcludedAutoCrudTypes.Contains(a)).ToList()
            }));
        }

        protected virtual AutoQueryFeature ConfigureAutoQueryFeature(AutoQueryFeature feature) => feature;
    }
}
