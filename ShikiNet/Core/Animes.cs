using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using ShikiNet.Entity;
using ShikiNet.Filter;

namespace ShikiNet.Core
{
    public static class Animes
    {
        private static NLog.Logger logger = NLog.LogManager.GetCurrentClassLogger();

        public static async Task<AnimeFull> GetById(int id)
        {
            return await Api.GetAsync<AnimeFull>($"/animes/{id}");
        }

        #region GetByFilter

        public static async Task<IEnumerable<Anime>> GetByFilter(AnimeFilter filter)
        {
            return await Api.GetAsync<IEnumerable<Anime>>($"/animes{filter.BuildQuery()}");
        }

        public static async Task<IEnumerable<Anime>> GetByFilter(Action<AnimeFilter> filter)
        {
            var aFilter = new AnimeFilter();
            filter(aFilter);
            logger.Info($"GetByFilter(Action<AnimeFilter>) | genered_url: [{aFilter.BuildQuery()}]");
            return await Api.GetAsync<IEnumerable<Anime>>($"/animes{aFilter.BuildQuery()}");
        }

        public static async Task<IEnumerable<Anime>> GetByFilter(string searchString)
        {
            return await GetByFilter(f =>
            {
                f.SearchString = searchString;
            });
        }

        public static async Task<IEnumerable<Anime>> GetByFilter(string searchString, Action<AnimeFilter> filter)
        {
            return await GetByFilter(f =>
            {
                f.SearchString = searchString;
                filter(f);
            });
        }

        #endregion



    }
}
