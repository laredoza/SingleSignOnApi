// --------------------------------------------------------------------------------------------------------------------
// <copyright file="ClientGrantTypeDto" company="">
//
// </copyright>
// <summary>
//   The class ClientGrantTypeDto.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace AdminApi.V1.Dtos
{
    #region Usings

    using System;

    #endregion

    /// <summary>
    ///     The ClientGrantTypeDto.
    /// </summary>
    public class ClientGrantTypeDto
    {
        #region Constructors and Destructors

        /// <summary>
        /// Initializes a new instance of the <see cref="ClientGrantTypeDto"/> class.
        /// </summary>
        public ClientGrantTypeDto()
        {
        }

        #endregion

        #region Public Properties
        public int Id { get; set; }
        public string GrantType { get; set; }

        #endregion

        #region Public Methods And Operators


        #endregion

        #region Other Methods

        #endregion
    }
}