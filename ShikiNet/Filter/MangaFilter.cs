using System;
using System.Collections.Generic;
using System.Text;
using ShikiNet.Entities;
using ShikiNet.Entities.Enums;
using ShikiNet.Filter.FilterEntity;
using ShikiNet.Utils.Extensions;

namespace ShikiNet.Filter
{
    public class MangaFilter : TitleFilter
    {
        public MangaOrder Order { get; set; }
        public FilterDictionary<MangaKind> Kind { get; set; }
        public FilterDictionary<int> PublisherIds { get; set; }

        public MangaFilter():base()
        {
            Order = MangaOrder.RANKED;

            Kind = new FilterDictionary<MangaKind>("kind");
            PublisherIds = new FilterDictionary<int>("publisher");
        }

        public string BuildQuery()
        {
            BuildRootQuery();

            query.Append($"&order={Order.ToLowerString()}"); //ToDo: convert from PascalCase to under_score

            if (!Kind.IsEmpty) { query.Append(Kind); }
            if (!PublisherIds.IsEmpty) { query.Append(PublisherIds); }

            return query.ToString();
        }

        public override string ToString()
        {
            return BuildQuery();
        }
    }
}
