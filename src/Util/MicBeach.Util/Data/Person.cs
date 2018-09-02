using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MicBeach.Util.Data
{
    /// <summary>
    /// Person data type
    /// </summary>
    public class Person
    {

        #region Propertys

        /// <summary>
        /// get or set name
        /// </summary>
        public ChineseText Name { get; set; }

        /// <summary>
        /// get or set birth
        /// </summary>
        public Birth Birth { get; set; }

        /// <summary>
        /// get or set contact
        /// </summary>
        public Contact Contact { get; set; }

        /// <summary>
        /// get or set sex
        /// </summary>
        public Sex Sex { get; set; }

        /// <summary>
        /// get or set idcard
        /// </summary>
        public string IdCard { get; set; }

        #endregion
    }

    /// <summary>
    /// sex
    /// </summary>
    public enum Sex
    {
        男 = 2,
        女 = 4,
        保密 = 8
    }
}
