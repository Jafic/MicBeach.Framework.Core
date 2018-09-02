using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MicBeach.Util.Data
{
    /// <summary>
    /// Birth Date Type
    /// </summary>
    public struct Birth
    {

        #region fields

        private int _age;//age
        private Constellation _constellation;//constellation
        private static readonly Dictionary<Constellation, Tuple<DateTime, DateTime>> constellationDic = new Dictionary<Constellation, Tuple<DateTime, DateTime>>()
        {
            { Constellation.水瓶座,new Tuple<DateTime, DateTime>(new DateTime(2000,1,20),new DateTime(2000,2,18))},
            { Constellation.双鱼座,new Tuple<DateTime, DateTime>(new DateTime(2000,2,19),new DateTime(2000,3,20))},
            { Constellation.白羊座,new Tuple<DateTime, DateTime>(new DateTime(2000,3,21),new DateTime(2000,4,19))},
            { Constellation.金牛座,new Tuple<DateTime, DateTime>(new DateTime(2000,4,20),new DateTime(2000,5,20))},
            { Constellation.双子座,new Tuple<DateTime, DateTime>(new DateTime(2000,5,21),new DateTime(2000,6,21))},
            { Constellation.巨蟹座,new Tuple<DateTime, DateTime>(new DateTime(2000,6,22),new DateTime(2000,7,22))},
            { Constellation.狮子座,new Tuple<DateTime, DateTime>(new DateTime(2000,7,23),new DateTime(2000,8,22))},
            { Constellation.处女座,new Tuple<DateTime, DateTime>(new DateTime(2000,8,23),new DateTime(2000,9,22))},
            { Constellation.天秤座,new Tuple<DateTime, DateTime>(new DateTime(2000,9,23),new DateTime(2000,10,23))},
            { Constellation.天蝎座,new Tuple<DateTime, DateTime>(new DateTime(2000,10,24),new DateTime(2000,11,22))},
            { Constellation.射手座,new Tuple<DateTime, DateTime>(new DateTime(2000,11,23),new DateTime(2000,12,21))},
            { Constellation.摩羯座,new Tuple<DateTime, DateTime>(new DateTime(2000,12,22),new DateTime(2000,1,19))},
        };

        #endregion

        #region constructor

        /// <summary>
        /// instance a Birth object
        /// </summary>
        /// <param name="birthDate">birth datetime</param>
        public Birth(DateTime birthDate)
        {
            BirthDate = birthDate;
            _constellation = GetConstellation(birthDate);
            _age = GetAge(birthDate);
        }

        /// <summary>
        /// get age
        /// </summary>
        public int Age
        {
            get
            {
                return GetAge(BirthDate);
            }
        }

        /// <summary>
        /// get birth datetime
        /// </summary>
        public DateTime BirthDate { get; }

        /// <summary>
        /// get constellation
        /// </summary>
        public Constellation Constellation
        {
            get
            {
                return GetConstellation(BirthDate);
            }
        }

        #endregion

        #region static methods

        /// <summary>
        /// get constellation
        /// </summary>
        /// <param name="dateTime">datetime</param>
        /// <returns>Constellation</returns>
        public static Constellation GetConstellation(DateTime dateTime)
        {
            int month = dateTime.Month;
            int day = dateTime.Day;
            var constell = Constellation.双子座;
            var constellDate = new DateTime(2000, month, day);
            var constellItem = constellationDic.FirstOrDefault(c => c.Value.Item1 <= constellDate && c.Value.Item2 >= constellDate);
            constell = constellItem.Key;
            return constell;
        }

        /// <summary>
        /// get age
        /// </summary>
        /// <param name="dateTime">birth date</param>
        /// <returns>age</returns>
        public static int GetAge(DateTime dateTime)
        {
            var nowDate = DateTime.Now.Date;
            var birthDate = dateTime.Date;
            if (nowDate < birthDate.AddYears(1))
            {
                return 0;
            }
            return (nowDate - birthDate).Days / 365;
        }

        #endregion
    }

    /// <summary>
    /// Constellation
    /// </summary>
    public enum Constellation
    {
        水瓶座 = 120218,
        双鱼座 = 219320,
        白羊座 = 321419,
        金牛座 = 420520,
        双子座 = 521621,
        巨蟹座 = 622722,
        狮子座 = 723822,
        处女座 = 823922,
        天秤座 = 9231023,
        天蝎座 = 10241122,
        射手座 = 11231221,
        摩羯座 = 12220119
    }
}
