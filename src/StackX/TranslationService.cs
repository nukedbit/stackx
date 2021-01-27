using System;
using System.Linq;
using System.Threading.Tasks;
using ServiceStack;
using ServiceStack.OrmLite;
using StackX.ServiceModel;
using StackX.ServiceModel.Types;
using System.Collections.Generic;

namespace StackX.ServiceInterface
{
    public class TranslationService : Service
    {

        public IAutoQueryDb AutoQueryDb { get; set; }


        public async Task<object> PostAsync(CreateMissingTranslation request)
        {
            var result = await Db.SingleAsync<Translation>(t =>
                t.LanguageId == request.LanguageId && t.ApplicationId == request.ApplicationId && t.Key == request.Key);
            if (result is not null)
            {
                return new TranslationResponse()
                {
                    Result = result
                };
            }
            return await AutoQueryDb.CreateAsync(request, Request);
        }
        
        public async Task<object> Get(QueryTranslationsGroupedByLanguage request)
        {
            var filterTranslation = Db.From<Translation>();
            if(request.ShowDeleted is false){
                filterTranslation = filterTranslation.Where(t => t.DeletedDate == null);
            }
            if(!request.Key.IsNullOrEmpty())
            {
                filterTranslation = filterTranslation.And(t => t.Key == request.Key);
            }
            if(request.ApplicationId is int appId)
            {
                filterTranslation = filterTranslation.And(t => t.ApplicationId == appId);
            }
            var translations = await Db.LoadSelectAsync<Translation>(filterTranslation);
            var languages = await Db.SelectAsync<Language>(l => l.DeletedDate == null);
            var grouped = new List<TranslationsGroupedModel>();
            foreach (var translationsForKey in translations.GroupBy(t => (key: t.Key.ToLowerInvariant(), appId: t.ApplicationId)))
            {
                var model = new TranslationsGroupedModel()
                {
                    Key = translationsForKey.Key.key,
                    ApplicationId = translationsForKey.Key.appId,
                    Translations = translationsForKey.Select(t => new TranslationItemModel
                    {
                        Id = t.Id,
                        Value = t.Value,
                        LanguageId = t.LanguageId,
                        LanguageName = t.Language.Name,
                        IsDeleted = t.DeletedDate != null,
                        IsMissing = string.IsNullOrEmpty(t.Value),
                    }).ToList()
                };
                var missingLanguages = languages.Where(l => !model.Translations.Any(t => t.LanguageId == l.Id));
                foreach(var missingLanguage in missingLanguages)
                {
                    model.Translations.Add(new TranslationItemModel {
                        LanguageId = missingLanguage.Id,
                        LanguageName = missingLanguage.Name,
                        IsMissing = true
                    });
                }

                grouped.Add(model);
            }
            return new QueryTranslationsGroupedByLanguageResponse
            {
                Results = grouped
            };
        }
    }
}