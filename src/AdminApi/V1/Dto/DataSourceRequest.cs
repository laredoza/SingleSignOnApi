// --------------------------------------------------------------------------------------------------------------------
// <copyright file="DataSourceRequest" company="">
//
// </copyright>
// <summary>
//   The class DataSourceRequest.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace AdminApi.V1.Dtos
{
    #region Usings

    using System;
    using System.Collections.Generic;

    #endregion

    /// <summary>
    ///     The DataSourceRequest.
    /// </summary>
    public class DataSourceRequest
    {
        #region Constructors and Destructors

        /// <summary>
        /// Initializes a new instance of the <see cref="DataSourceRequest"/> class.
        /// </summary>
        public DataSourceRequest()
        {
        }

        #endregion

        #region Public Properties
        public int PageSize { get; set; }
        public int CurrentPage { get; set; }
        public int Take
        {
            get
            {
                return CurrentPage * PageSize;
            }
        }

        public IEnumerable<Kendo.DynamicLinq.Sort> Sort { get; set; }
        public Kendo.DynamicLinq.Filter Filter { get; set; }


        #endregion

        #region Public Methods And Operators


        #endregion

        #region Other Methods

        #endregion
    }
}