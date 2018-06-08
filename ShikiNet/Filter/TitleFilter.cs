using System;
using System.Collections.Generic;
using System.Text;
using System.Web;
using ShikiNet.Entities;
using ShikiNet.Entities.Enums;
using ShikiNet.Filter.FilterEntity;
using ShikiNet.Static;

namespace ShikiNet.Filter
{
    public abstract class TitleFilter
    {
        private int page;
        private int limit;

        protected StringBuilder query;

        public int Page
        {
            get
            {
                return page;
            }
            set
            {
                if (value < 1) { page = 1; return; }
                if (value > StaticValue.MAX_PAGE_COUNT) { page = StaticValue.MAX_PAGE_COUNT; return; }
                page = value;
            }
        }
        public int Limit
        {
            get
            {
                return limit;
            }
            set
            {
                if (value < 1) { limit = 1; return; }
                if (value > StaticValue.MAX_LIMIT) { limit = StaticValue.MAX_LIMIT; return; }
                limit = value;
            }
        }
        public FilterDictionary<TitleStatus> Statuses { get; set; }
        public FilterDictionary<SeasonYear> Seasons { get; set; }
        public int Score { get; set; }
        public Boolean Censored { get; set; }
        public FilterDictionary<int> Ids { get; set; }
        //public FilterDictionary<int> ExcludeIds { get; set; }
        public FilterDictionary<UserRateStatus> MyLists { get; set; }
        public FilterDictionary<int> GenreIds { get; set; }
        public String SearchString { get; set; }

        protected TitleFilter()
        {
            Page       = 1;
            Limit      = 10;
            Score      = 0;
            Censored   = false;
            Statuses   = new FilterDictionary<TitleStatus>("status");
            Seasons    = new FilterDictionary<SeasonYear>("season");
            Ids        = new FilterDictionary<int>("ids");
            //ExcludeIds = new FilterDictionary<int>("exclude_ids");
            MyLists    = new FilterDictionary<UserRateStatus>("mylist");
            GenreIds   = new FilterDictionary<int>("genre");
        }

        protected void BuildRootQuery()
        {
            query = new StringBuilder("?");

            query.Append($"page={Page}");
            query.Append($"&limit={Limit}");
            if (Score > 0) { query.Append($"&score={Score}"); }
            query.Append($"&censored={Censored.ToString().ToLower()}");

            if (!Statuses.IsEmpty)   { query.Append(Statuses); }
            if (!Seasons.IsEmpty)    { query.Append(Seasons); }
            if (!Ids.IsEmpty)        { query.Append(Ids); }
            //if (!ExcludeIds.IsEmpty) { query.Append(ExcludeIds); }
            if (!MyLists.IsEmpty)    { query.Append(MyLists); }
            if (!GenreIds.IsEmpty)   { query.Append(GenreIds); }

            if (String.IsNullOrWhiteSpace(SearchString)) { query.Append("&search=").Append(HttpUtility.UrlEncode(SearchString, Encoding.UTF8)); }
        }
    }
}
