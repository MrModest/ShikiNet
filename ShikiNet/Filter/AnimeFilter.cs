using System;
using System.Collections.Generic;
using System.Text;
using ShikiNet.Entities;
using ShikiNet.Entities.Enums;
using ShikiNet.Filter.FilterEntity;
using ShikiNet.Utils.Extensions;

namespace ShikiNet.Filter
{
    public class AnimeFilter : TitleFilter
    {
        public AnimeOrder Order { get; set; }
        public FilterDictionary<AnimeKind> Kind { get; set; }
        public FilterDictionary<Duration> Durations { get; set; }
        public FilterDictionary<TitleRating> Ratings { get; set; }
        public FilterDictionary<int> StudioIds { get; set; }

        public AnimeFilter():base()
        {
            Order = AnimeOrder.RANKED;

            Kind      = new FilterDictionary<AnimeKind>("kind");
            Durations = new FilterDictionary<Duration>("duration");
            Ratings   = new FilterDictionary<TitleRating>("rating");
            StudioIds = new FilterDictionary<int>("studio");
        }

        public string BuildQuery()
        {
            BuildRootQuery();

            query.Append($"&order={Order.ToLowerString()}"); //ToDo: convert from PascalCase to under_score

            if (!Kind.IsEmpty)      { query.Append(Kind); }
            if (!Durations.IsEmpty) { query.Append(Durations); }
            if (!Ratings.IsEmpty  ) { query.Append(Ratings); }
            if (!StudioIds.IsEmpty) { query.Append(StudioIds); }

            return query.ToString();
        }

        public override string ToString()
        {
            return BuildQuery();
        }
    }
}
