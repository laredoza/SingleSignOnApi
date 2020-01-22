// --------------------------------------------------------------------------------------------------------------------
// <copyright file="ClientRedirectUrlDto" company="">
//
// </copyright>
// <summary>
//   The class ClientRedirectUrlDto.
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
    public class ClientRedirectUrlDto
    {
        #region Constructors and Destructors

        /// <summary>
        /// Initializes a new instance of the <see cref="ClientRedirectUrlDto"/> class.
        /// </summary>
        public ClientRedirectUrlDto()
        {
        }

        #endregion

        #region Public Properties

        public int Id { get; set; }
        public string RedirectUri { get; set; }
        public int ClientId { get; set; }

        #endregion

        #region Public Methods And Operators


        #endregion

        #region Other Methods

        #endregion
    }
}