using System;
using System.Collections.Generic;
using System.Text;
using System.Linq;

namespace ShikiNet.Filter
{
    public class FilterDictionary<T>
    {
        private string name;
        private IDictionary<T, bool> filterDict = new Dictionary<T, bool>();

        public bool IsEmpty { get { return filterDict.Count == 0; } }
        public IEnumerable<T> IncludedFilters
        {
            get
            {
                return filterDict.Where(fd => fd.Value == true).Select(fd => fd.Key);
            }
        }
        public IEnumerable<T> ExcludedFilters
        {
            get
            {
                return filterDict.Where(fd => fd.Value == false).Select(fd => fd.Key);
            }
        }


        public FilterDictionary(string filterName)
        {
            this.name = filterName;
        }


        private string ToStringForIds()
        {
            StringBuilder query = new StringBuilder();
            if (IncludedFilters.Count() > 0)
            {
                query.Append($"&{name}=");
                foreach (var id in IncludedFilters)
                {
                    query.Append(id.ToString()).Append(",");
                }
                query.Remove(query.Length - 1, 1); //delete last comma
            }
            if (ExcludedFilters.Count() > 0)
            {
                query.Append($"&exclude_{name}=");
                foreach (var id in IncludedFilters)
                {
                    query.Append(id.ToString()).Append(",");
                }
                query.Remove(query.Length - 1, 1); //delete last comma
            }

            return query.ToString();
        }

        public FilterDictionary<T> Add(T filter, bool include)
        {
            filterDict.Add(filter, include);

            return this;
        }

        public FilterDictionary<T> Include(params T[] filters)
        {
            foreach (var filter in filters)
            {
                filterDict.Add(filter, true);
            }

            return this;
        }

        public FilterDictionary<T> Exclude(params T[] filters)
        {
            foreach (var filter in filters)
            {
                filterDict.Add(filter, false);
            }

            return this;
        }

        public override string ToString()
        {
            if (IsEmpty) { return String.Empty; }

            if (name.Equals("ids")) { return ToStringForIds(); } //special answer for ids/exclude_ids

            StringBuilder query = new StringBuilder();
            query.Append($"&{name}=");
            foreach (var filter in filterDict)
            {
                query.Append(filter.Value ? "" : "!").Append(filter.Key.ToString().ToLower()).Append(",");
            }
            query.Remove(query.Length - 1, 1); //delete last comma

            return query.ToString();
        }
    }
}
