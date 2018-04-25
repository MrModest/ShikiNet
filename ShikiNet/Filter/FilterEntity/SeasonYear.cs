using System;
using System.Collections.Generic;
using System.Text;
using ShikiNet.Utils.Extentions;

namespace ShikiNet.Filter.FilterEntity
{
    public class SeasonYear
    {
        private Season season;
        private int year;
        private int yearFrom;
        private int yearTo;

        public SeasonYear(Season season, int year) //SPRING_2018
        {
            this.season = season;
            this.year = year;
            this.yearFrom = 0;
            this.yearTo = 0;
        }

        public SeasonYear(int year) //2018
        {
            this.season = Season.NOT_USE;
            this.year = year;
            this.yearFrom = 0;
            this.yearTo = 0;
        }

        public SeasonYear(int yearFrom, int yearTo) //2016_2018
        {
            this.season = Season.NOT_USE;
            this.year = 0;
            this.yearFrom = yearFrom;
            this.yearTo = yearTo;
        }

        public override string ToString()
        {
            if (season != Season.NOT_USE && year != 0)
            {
                return $"{season.ToLowerString()}_{year}";
            }
            else if (season == Season.NOT_USE && year != 0)
            {
                return year.ToString();
            }
            else if (yearFrom != 0 && yearTo != 0)
            {
                return $"{yearFrom}_{yearTo}";
            }
            return null;
        }

    }
}
