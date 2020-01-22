// --------------------------------------------------------------------------------------------------------------------
// <copyright file="ClientAllowedCorsUrlDto" company="">
//
// </copyright>
// <summary>
//   The class ClientAllowedCorsUrlDto.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace AdminApi.V1.Dtos
{
    #region Usings

    using System;

    #endregion

    /// <summary>
    ///     The ClientRedirectUrlDto.
    /// </summary>
    public class ClientAllowedCorsUrlDto
    {
        #region Constructors and Destructors

        /// <summary>
        /// Initializes a new instance of the <see cref="ClientAllowedCorsUrlDto"/> class.
        /// </summary>
        public ClientAllowedCorsUrlDto()
        {
        }

        #endregion

        #region Public Properties

        public int Id { get; set; }
        public string Origin { get; set; }
        public int ClientId { get; set; }

        #endregion

        #region Public Methods And Operators


        #endregion

        #region Other Methods

        #endregion
    }
}