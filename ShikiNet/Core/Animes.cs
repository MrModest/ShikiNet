using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using ShikiNet.Entities;
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

        public static async Task<IEnumerable<Anime>> GetByFilterAsync(AnimeFilter filter)
        {
            return await Api.GetAsync<IEnumerable<Anime>>($"/animes{filter.BuildQuery()}");
        }

        public static async Task<IEnumerable<Anime>> GetByFilterAsync(Action<AnimeFilter> filter)
        {
            var aFilter = new AnimeFilter();
            filter(aFilter);
            logger.Info($"GetByFilter(Action<AnimeFilter>) | genered_url: [{aFilter.BuildQuery()}]");
            return await Api.GetAsync<IEnumerable<Anime>>($"/animes{aFilter.BuildQuery()}");
        }

        public static async Task<IEnumerable<Anime>> GetByFilterAsync(string searchString)
        {
            return await GetByFilterAsync(f =>
            {
                f.SearchString = searchString;
            });
        }

        public static async Task<IEnumerable<Anime>> GetByFilterAsync(string searchString, Action<AnimeFilter> filter)
        {
            return await GetByFilterAsync(f =>
            {
                f.SearchString = searchString;
                filter(f);
            });
        }

        #endregion

    }
}
